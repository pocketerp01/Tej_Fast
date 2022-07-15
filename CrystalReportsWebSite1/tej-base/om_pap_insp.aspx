<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="om_pap_insp" CodeFile="om_pap_insp.aspx.cs" %>
<%--  --%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../tej-base/Scripts/gridviewScroll.min.js" type="text/javascript"></script>
   <script type="text/javascript">
       $(document).ready(function () {
           gridviewScroll('#<%=sg1.ClientID%>', gridDiv);

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
                        <asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large">Reel/Roll Inspection Sheet</asp:Label></td>
                    <td style="text-align:right">
                        <button type="submit" id="btnnew" class="btn btn-info" style="width: 100px;" runat="server" accesskey="N" onserverclick="btnnew_ServerClick"><u>N</u>ew</button>
                        <button type="submit" id="btnedit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="i" onserverclick="btnedit_ServerClick">Ed<u>i</u>t</button>
                        <button type="submit" id="btnsave" class="btn btn-info" style="width: 100px;" runat="server" accesskey="s" onserverclick="btnsave_ServerClick"><u>S</u>ave</button>
                        <button type="submit" id="btnvalidat" class="btn btn-info" style="width: 100px;" runat="server" accesskey="X" onserverclick="btnvalidat_ServerClick">Vali<u>d</u>ate</button>
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
                                <label id="lbl1" runat="server" class="col-sm-2 control-label">Insp. No.</label>
                                <div class="col-sm-1">
                                    <asp:Label ID="lbl1a" runat="server" Text="TC" Style="width: 22px; float: right;" CssClass="col-sm-1 control-label"></asp:Label>
                                </div>                                
                                <div class="col-sm-3">
                                     <asp:TextBox ID="txtvchnum" runat="server" Width="100px" placeholder="Insp No." ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                </div>
                                 <label id="Label1" runat="server" class="col-sm-2 control-label">Date</label>
                           <div class="col-sm-4">
                                            <asp:TextBox ID="txtvchdate" placeholder="Insp Date" runat="server"  CssClass="form-control"></asp:TextBox>
                                            <asp:CalendarExtender ID="txtvchdate_CalendarExtender" runat="server"
                                                Enabled="True" TargetControlID="txtvchdate"
                                                Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="Maskedit1" runat="server" Mask="99/99/9999"
                                                MaskType="Date" TargetControlID="txtvchdate" />
                                          </div>
                            </div>
                            <div class="form-group">
                                <label id="Label2" runat="server" class="col-sm-2 control-label">MRR No.</label>
                                <div class="col-sm-1">
                                     <asp:ImageButton ID="btnlbl4" runat="server"  ImageUrl="../tej-base/css/images/bdsearch5.png" ToolTip="Select MRR" Style="width: 22px; float: right;" OnClick="btnlbl4_Click" />
                                </div>
                                
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtlbl4" runat="server" Width="100px" placeholder="MRR No." ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                </div>
                                <label id="Label" runat="server" class="col-sm-2 control-label">Date</label>                            
                           <div class="col-sm-4">
                                           <asp:TextBox ID="txtlbl4a" runat="server"  CssClass="form-control" placeholder="MRR Date"></asp:TextBox>
                               <asp:CalendarExtender ID="txtlbl4a_CalendarExtender" runat="server"
                                                Enabled="True" TargetControlID="txtlbl4a"
                                                Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="MaskedEditExtender1" runat="server" Mask="99/99/9999"
                                                MaskType="Date" TargetControlID="txtlbl4a" />
                                          </div> 
                            </div>
                            <div class="form-group">
                                <label id ="Label8" runat="server" class="col-sm-3 control-label">Target B.F</label>
                                <div class="col-sm-5">
                                    <asp:TextBox ID="txtlbl4b" runat="server" Width="100%" ReadOnly="false" MaxLength="8" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div></div></div>
                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <label id="Label7" runat="server" class="col-sm-2 control-label">Supplier's</label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtlbl7" runat="server" Width="100%" placeholder="Supplier Code" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                </div>
                                 <div class="col-sm-6">
                                            <asp:TextBox ID="txtlbl7a" runat="server" ReadOnly="true" placeholder="Supplier Name" CssClass="form-control"></asp:TextBox>
                                        </div>
                            </div>

                            <div class="form-group">
                                <label id="Label3" runat="server" class="col-sm-2 control-label">Chl/Inv No.</label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtlbl2" runat="server" CssClass="form-control" MaxLength="20" Width="100%" placeholder="Chl/Inv No" ReadOnly="true"></asp:TextBox>
                                </div>
                                <label id="Label4" runat="server" class="col-sm-2 control-label">Date</label>
                                <div class="col-sm-4">
                                     <asp:TextBox ID="txtlbl3" runat="server" CssClass="form-control" Width="100%" placeholder="Date" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            
                                <div class="form-group">
                                    <label  runat="server" class="col-sm-8 control-label" style="text-align:left; font-weight: 700; font-size:medium; color:red">Reel Wise Data Filled Into Tab2</label>
                                     <div class="col-sm-4" style="visibility:hidden;">
                                         <asp:Button ID="btnvalidate" runat="server" Text="Validate" Height="32px" Width="100px" OnClick="btnvalidate_Click" Font-Bold="true" Font-Size="14" />
                                </div>
                                    </div>

                        </div></div></div>

                <div class="col-md-6" style="display:none;">
                    <div>
                         <%--<div class="box-body">--%>
                         <div class="box-body">
                            <table style="width: 100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl7" runat="server" Text="lbl7" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>
                                        <asp:ImageButton ID="btnlbl7" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl7_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl101" runat="server" Text="lbl101" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>
                                        <asp:ImageButton ID="btnlbl101" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" Visible="false" OnClick="btnlbl101_Click"/>
                                            </td>
                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl101" runat="server" Width="80px" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl101a" runat="server" Width="350px" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>

                            </table>
                        </div>
                    </div>
                </div>

                <div class="col-md-6" style="display:none;">
                    <div>
                         <%--<div class="box-body">--%>
                         <div class="box-body">
                            <table style="width: 100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl8" runat="server" Text="lbl8" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl8" runat="server"  EnableViewState="true" Width="200px" CssClass="form-control" ></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl9" runat="server" Text="lbl9" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl9" runat="server"  EnableViewState="true" Width="200px" CssClass="form-control" ></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>                                
                            </table>
                        </div>
                    </div>
                </div>
        
                <section class="col-lg-12 connectedSortable">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">
                                <li><a href="#DescTab" id="tab1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab">Form Details</a></li>
                                <li><a href="#DescTab2" id="tab2" runat="server" aria-controls="DescTab2" role="tab" data-toggle="tab">Other.Terms</a></li>
                                <li><a href="#DescTab3" id="tab3" runat="server" aria-controls="DescTab3" role="tab" data-toggle="tab">Reel Wise Data</a></li>
                                <li><a href="#DescTab4" id="tab4" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Delv.Sch</a></li>
                                <li><a href="#DescTab5" id="tab5" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Inv.Dtl</a></li>
                            </ul>

                            <div class="tab-content" >
                                <div role="tabpanel" class="tab-pane active" id="DescTab">
                                    <div class="lbBody" id="gridDiv" style="color: White; height: 300px; overflow: hidden; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <asp:GridView ID="sg1" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="1500px" Font-Size ="13px" 
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
                                                        <asp:ImageButton ID="sg1_btnadd" runat="server" CommandName="SG1_ROW_ADD" ImageAlign="Middle" ImageUrl="../tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Del</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btnrmv" runat="server" CommandName="SG1_RMV" ImageUrl="../tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                                                                
                                                <asp:BoundField DataField="sg1_f15" HeaderText="Co_Reel"/>
                                                <asp:BoundField DataField="sg1_f1" HeaderText="Std.GSM"  ItemStyle-Width="80px"/>
                                                <asp:BoundField DataField="sg1_f2" HeaderText="Std_Size" ItemStyle-Width="80px"/>   
                                                <asp:BoundField DataField="sg1_f4" HeaderText="Icode" ItemStyle-Width="80px"/>                                             
                                                <asp:BoundField DataField="sg1_srno" HeaderText="Sr_No." />
                                                <asp:BoundField DataField="sg1_f3" HeaderText="Reel_No." ItemStyle-Width="80px"/>

                                                 <asp:TemplateField Visible="false">
                                                            <HeaderTemplate>Reel_No.</HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="sg1_t1" runat="server" Text='<%#Eval("sg1_t1") %>' Width="100%"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                <asp:TemplateField>
                                                            <HeaderTemplate>Reel_Dia</HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="sg1_t2" runat="server" onkeypress="return isDecimalKey(event)" Text='<%#Eval("sg1_t2") %>' Width="100%"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                               <asp:TemplateField>
                                                            <HeaderTemplate>Reel_Wt</HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="sg1_t3" runat="server" onkeypress="return isDecimalKey(event)" Text='<%#Eval("sg1_t3") %>' Width="100%"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                <asp:TemplateField>
                                                            <HeaderTemplate>Act.Size</HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="sg1_t4" runat="server" onkeypress="return isDecimalKey(event)" Text='<%#Eval("sg1_t4") %>' Width="100%"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                <asp:TemplateField>
                                                            <HeaderTemplate>Act.GSM1</HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="sg1_t5" runat="server" onkeypress="return isDecimalKey(event)" Text='<%#Eval("sg1_t5") %>'  Width="100%" onChange="calculateSum()"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                <asp:TemplateField>
                                                            <HeaderTemplate>Act.GSM2</HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="sg1_t6" runat="server" onkeypress="return isDecimalKey(event)" Text='<%#Eval("sg1_t6") %>' Width="100%" onChange="calculateSum()"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                <asp:TemplateField>
                                                            <HeaderTemplate>Avg.GSM</HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="sg1_t7" runat="server" onkeypress="return isDecimalKey(event)" Text='<%#Eval("sg1_t7") %>' onChange="calculateSum()" Width="100%"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                <asp:TemplateField>
                                                            <HeaderTemplate>B.S1</HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="sg1_t8" runat="server" onkeypress="return isDecimalKey(event)" Text='<%#Eval("sg1_t8") %>' Width="100%" onChange="calculateSum()"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                <asp:TemplateField>
                                                            <HeaderTemplate>B.S2</HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="sg1_t9" runat="server" onkeypress="return isDecimalKey(event)" Text='<%#Eval("sg1_t9") %>' Width="100%"  onChange="calculateSum()"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                <asp:TemplateField>
                                                            <HeaderTemplate>B.S3</HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="sg1_t10" runat="server" onkeypress="return isDecimalKey(event)" Text='<%#Eval("sg1_t10") %>' Width="100%" onChange="calculateSum()"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                <asp:TemplateField>
                                                            <HeaderTemplate>B.S4</HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="sg1_t11" runat="server" onkeypress="return isDecimalKey(event)" Text='<%#Eval("sg1_t11") %>' Width="100%" onChange="calculateSum()"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                <asp:TemplateField>
                                                            <HeaderTemplate>Avg_B.S</HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="sg1_t12" runat="server" onkeypress="return isDecimalKey(event)" Text='<%#Eval("sg1_t12") %>' ReadOnly="true" Width="100%" onChange="calculateSum()"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                <asp:TemplateField>
                                                            <HeaderTemplate>B_Factor</HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="sg1_t13" runat="server" onkeypress="return isDecimalKey(event)" Text='<%#Eval("sg1_t13") %>' ReadOnly="true" Width="100%" onChange="calculateSum()"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                <asp:TemplateField>
                                                            <HeaderTemplate>Moist_1</HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="sg1_t14" runat="server" onkeypress="return isDecimalKey(event)" Text='<%#Eval("sg1_t14") %>' Width="100%"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                <asp:TemplateField>
                                                            <HeaderTemplate>Moist_2</HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="sg1_t15" runat="server" onkeypress="return isDecimalKey(event)" Text='<%#Eval("sg1_t15") %>' Width="100%"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                <asp:TemplateField>
                                                            <HeaderTemplate>Cobb_1</HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="sg1_t16" runat="server" onkeypress="return isDecimalKey(event)" Text='<%#Eval("sg1_t16") %>' Width="100%"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                <asp:TemplateField>
                                                            <HeaderTemplate>Cobb_2</HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="sg1_t17" runat="server" onkeypress="return isDecimalKey(event)" Text='<%#Eval("sg1_t17") %>' Width="100%"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                <asp:TemplateField>
                                                            <HeaderTemplate>Ring_1</HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="sg1_t18" runat="server" onkeypress="return isDecimalKey(event)" Text='<%#Eval("sg1_t18") %>' Width="100%"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                <asp:TemplateField>
                                                            <HeaderTemplate>Ring_2</HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="sg1_t19" runat="server" onkeypress="return isDecimalKey(event)" Text='<%#Eval("sg1_t19") %>' Width="100%"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                <asp:TemplateField>
                                                            <HeaderTemplate>Shade</HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="sg1_t20" runat="server" Text='<%#Eval("sg1_t20") %>' Width="100%"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField><asp:TemplateField>
                                                            <HeaderTemplate>Apperance</HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="sg1_t21" runat="server" Text='<%#Eval("sg1_t21") %>' Width="100%"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>                                               

                                              
                                                <asp:BoundField DataField="sg1_f5" HeaderText="Error_count"  />
                                               <%-- <asp:BoundField DataField="sg1_f18" HeaderText="Act.Size"/>
                                                <asp:BoundField DataField="sg1_f6" HeaderText="Act.GSM1"   />
                                                <asp:BoundField DataField="sg1_f7" HeaderText="Act.GSM2"  />
                                                <asp:BoundField DataField="sg1_f8" HeaderText="Avg.GSM"  />

                                                <asp:BoundField DataField="sg1_f9" HeaderText="B.S1" />
                                                <asp:BoundField DataField="sg1_f10" HeaderText="B.S2"/>
                                                <asp:BoundField DataField="sg1_f11" HeaderText="B.S3"/>
                                                <asp:BoundField DataField="sg1_f12" HeaderText="B.S4"/>
                                                <asp:BoundField DataField="sg1_f13" HeaderText="Avg_B.S"/>
                                                <asp:BoundField DataField="sg1_f14" HeaderText="B_Factor"/>
                                                <asp:BoundField DataField="sg1_f15" HeaderText="Moist_1"/>
                                                <asp:BoundField DataField="sg1_f16" HeaderText="Moist_2"/>
                                                <asp:BoundField DataField="sg1_f17" HeaderText="Cobb_1"/>
                                                <asp:BoundField DataField="sg1_f18" HeaderText="Cobb_2"/>
                                                <asp:BoundField DataField="sg1_f19" HeaderText="Ring_1"  />
                                                <asp:BoundField DataField="sg1_f20" HeaderText="Ring_2"  />
                                                <asp:BoundField DataField="sg1_f21" HeaderText="Shade"  />
                                                <asp:BoundField DataField="sg1_f22" HeaderText="Apperance"  /> --%>                                            

                                                  <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>Add</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btnadd1" runat="server" CommandName="SG1_ROW_ADD1" ImageAlign="Middle" ImageUrl="../tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                
                                               
                                                <asp:TemplateField ItemStyle-Width="90px" Visible="false">
                                                    <HeaderTemplate>Nature Of J/w</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t4i" runat="server" Text='<%#Eval("sg1_t4") %>'  Width="100%" onChange="calrejection()"  maxlength="12" ></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="90px" Visible="false">
                                                    <HeaderTemplate>Waste_Qty</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t5i" runat="server" Text='<%#Eval("sg1_t5") %>'  Width="100%" onChange="caltotalvalue()" onkeypress="return isDecimalKey(event)" maxlength="7" ></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                

                                               <%-- <asp:TemplateField Visible="false">
                                                            <HeaderTemplate>StartTime</HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="sg1_t1i" runat="server" Text='<%#Eval("sg1_t2") Text='<%#Eval("sg1_t1") %>' Width="100%" TextMode="Time"></asp:TextBox>                                                           
                                                            </ItemTemplate>
                                                        </asp:TemplateField>--%>

                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>EndTime</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t2i" runat="server" Text='<%#Eval("sg1_t2") %>' Width="100%" TextMode="Time"></asp:TextBox>
                                                   
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                               
                                                <asp:TemplateField ItemStyle-Width="110px" Visible="false">
                                                    <HeaderTemplate>QtyRcvd</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t3i" runat="server" Text='<%#Eval("sg1_t3") %>'  Width="100%" onkeypress="return isDecimalKey(event)" maxlength="7"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--<asp:TemplateField ItemStyle-Width="80px" Visible="false">
                                                    <HeaderTemplate>No.Of Cuts</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t4I" runat="server" Text='<%#Eval("sg1_t4I") %>'  Width="100%" onChange="calrejection()" onkeypress="return isDecimalKey(event)" maxlength="7" ></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>CutSheet</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t5I" runat="server" Text='<%#Eval("sg1_t5I") %>'  Width="100%" onChange="caltotalvalue()" onkeypress="return isDecimalKey(event)" maxlength="7" ></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>RejSheet</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t6i" runat="server" Text='<%#Eval("sg1_t6") %>' Width="100%" onChange="caloksheet()" onkeypress="return isDecimalKey(event)" maxlength="7" ></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>Remarks</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t7i" runat="server" Text='<%#Eval("sg1_t7") %>' Width="100%" maxlength="100"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>OKSheet</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t8i" runat="server" Text='<%#Eval("sg1_t8") %>' Width="100%" onChange="caltotalvalue1()" onkeypress="return isDecimalKey(event)" maxlength="7" ></asp:TextBox>
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

                                <div role="tabpanel" class="tab-pane active" id="DescTab2">

                                    <div class="lbBody" style="height: 200px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl10" runat="server" Text="lbl10" CssClass="col-sm-2 control-label"></asp:Label></td>

                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl10" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl11" runat="server" Text="lbl11" CssClass="col-sm-2 control-label"></asp:Label></td>

                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl11" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl12" runat="server" Text="lbl12" CssClass="col-sm-2 control-label"></asp:Label></td>

                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl12" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl13" runat="server" Text="lbl13" CssClass="col-sm-2 control-label"></asp:Label></td>

                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl13" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl14" runat="server" Text="lbl14" CssClass="col-sm-2 control-label"></asp:Label></td>

                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl14" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl15" runat="server" Text="lbl15" CssClass="col-sm-2 control-label"></asp:Label></td>

                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl15" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl16" runat="server" Text="lbl16" CssClass="col-sm-2 control-label"></asp:Label></td>

                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl16" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl17" runat="server" Text="lbl17" CssClass="col-sm-2 control-label"></asp:Label></td>

                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl17" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl18" runat="server" Text="lbl18" CssClass="col-sm-2 control-label"></asp:Label></td>

                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl18" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl19" runat="server" Text="lbl19" CssClass="col-sm-2 control-label"></asp:Label></td>

                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl19" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>


                                <div role="tabpanel" class="tab-pane active" id="DescTab3">
                                    <div class="lbBody" style="height: 200px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                        <asp:GridView ID="sg2" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="1200px" Font-Size="Smaller"
                                            AutoGenerateColumns="False" OnRowDataBound="sg1_RowDataBound"
                                            OnRowCommand="sg2_RowCommand">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>

                                                <%-- <asp:BoundField DataField="sg2_h1" HeaderText="sg2_h1" />
                                                <asp:BoundField DataField="sg2_h2" HeaderText="sg2_h2" />
                                                <asp:BoundField DataField="sg2_h3" HeaderText="sg2_h3" />
                                                <asp:BoundField DataField="sg2_h4" HeaderText="sg2_h4" />
                                                <asp:BoundField DataField="sg2_h5" HeaderText="sg2_h5" />
                                                <asp:BoundField DataField="sg2_h6" HeaderText="sg2_h6" />
                                                <asp:BoundField DataField="sg2_h7" HeaderText="sg2_h7" />
                                                <asp:BoundField DataField="sg2_h8" HeaderText="sg2_h8" />
                                                <asp:BoundField DataField="sg2_h9" HeaderText="sg2_h9" />
                                                <asp:BoundField DataField="sg2_h10" HeaderText="sg2_h10" />--%>

                                               
                                                <asp:BoundField DataField="sg2_srno" HeaderText="Srno" ItemStyle-Width="30px"/>
                                                <asp:BoundField DataField="sg2_f1" HeaderText="Reels" ItemStyle-Width="40px" />
                                                <asp:BoundField DataField="sg2_f2" HeaderText="Size" ItemStyle-Width="40px"/>
                                                <asp:BoundField DataField="sg2_f3" HeaderText="GSM" ItemStyle-Width="40px"/>
                                                <asp:BoundField DataField="sg2_f4" HeaderText="Weight" ItemStyle-Width="40px"/>
                                                <asp:BoundField DataField="sg2_f5" HeaderText="Item" ItemStyle-Width="150px"/>
                                                <asp:BoundField DataField="sg2_f6" HeaderText="Icode" ItemStyle-Width="150px"/>

                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>Add</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg2_btnadd" runat="server" CommandName="SG2_ROW_ADD" ImageAlign="Middle" ImageUrl="../tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>Del</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg2_btnrmv" runat="server" CommandName="SG2_RMV" ImageUrl="../tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>Terms</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg2_t1" runat="server" Text='<%#Eval("sg2_t1") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>Conditions</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg2_t2" runat="server" Text='<%#Eval("sg2_t2") %>' Width="100%"></asp:TextBox>
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
                                <div role="tabpanel" class="tab-pane active" id="DescTab4">
                                    <div class="lbBody" style="height: 200px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                        <asp:GridView ID="sg3" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="1200px" Font-Size="Smaller"
                                            AutoGenerateColumns="False"
                                            OnRowCommand="sg3_RowCommand">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Add</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg3_btnadd" runat="server" CommandName="SG3_ROW_ADD" ImageAlign="Middle" ImageUrl="../tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Del</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg3_btnrmv" runat="server" CommandName="SG3_RMV" ImageUrl="../tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="sg3_Srno" HeaderText="Sr.No" />
                                                <asp:BoundField DataField="sg3_f1" HeaderText="ERP_Code" />
                                                <asp:BoundField DataField="sg3_f2" HeaderText="Item_Name" />

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Dlv_Date</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t1" runat="server" Text='<%#Eval("sg3_t1") %>' MaxLength="10" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Sch.Qty</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t2" runat="server" Text='<%#Eval("sg3_t2") %>' MaxLength="10" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Prod.Qty</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t3" runat="server" Text='<%#Eval("sg3_t3") %>' MaxLength="10" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Job_Card</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t4" runat="server" Text='<%#Eval("sg3_t4") %>' MaxLength="1" Width="100%"></asp:TextBox>
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
                                <div role="tabpanel" class="tab-pane active" id="DescTab5">
                                    <div class="lbBody" style="height: 200px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl40" runat="server" Text="lbl40" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl40" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl41" runat="server" Text="lbl41" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl41" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl42" runat="server" Text="lbl42" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl42" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl43" runat="server" Text="lbl43" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl43" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl44" runat="server" Text="lbl44" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl44" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl45" runat="server" Text="lbl45" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl45" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>

                                                    </table>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl46" runat="server" Text="lbl46" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl46" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl47" runat="server" Text="lbl47" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl47" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl48" runat="server" Text="lbl48" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl48" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl49" runat="server" Text="lbl49" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl49" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl50" runat="server" Text="lbl50" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl50" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl51" runat="server" Text="lbl51" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl51" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>

                                                    </table>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>

                
                <div class="col-md-12">
                    <div>
                        <div class="box-body">
                            <label id="Label10" runat="server" class="col-sm-1 control-label">Stiffness</label>
                            <div class="col-sm-3">
                                <asp:TextBox ID="txtlbl22" runat="server" CssClass="form-control" Width="180px" MaxLength="20" ReadOnly="false"></asp:TextBox>
                            </div>
                            <label id="Label11" runat="server" class="col-sm-1 control-label">P.H Value</label>
                            <div class="col-sm-3">
                                <asp:TextBox ID="txtlbl21" runat="server" CssClass="form-control" Width="180px" MaxLength="20" ReadOnly="false"></asp:TextBox>
                            </div>
                            <label id="Label12" runat="server" class="col-sm-1 control-label">Result</label>
                            <div class="col-sm-3">
                                <asp:TextBox ID="txtlbl20" runat="server" CssClass="form-control" Width="180px" MaxLength="25" ReadOnly="false"></asp:TextBox>
                            </div>                            
                        </div>
                    </div>
                </div>

                <div class="col-md-12">
                    <div>
                        <div class="box-body"> 
                            <label id="Label9" runat="server" class="col-sm-12 control-label">Remarks</label>  
                            <div class="col-sm-11">
                                <asp:TextBox ID="txtrmk" runat="server"  MaxLength="150" Width="99%" CssClass="form-control" TextMode="MultiLine" placeholder="Remarks"></asp:TextBox>
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
     <script>
         function calculateSum() {
             var gsm1 = 0; var gsm2 = 0; var bf1 = 0; var bf2 = 0; var bf3 = 0;
             var bf4 = 0; var avgbf = 0; var avggsm = 0; var bfactor = 0;
             var grid = document.getElementById("<%= sg1.ClientID%>");
             for (var i = 0; i < grid.rows.length - 1; i++) {
                 gsm1 = fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg1_sg1_t5_' + i).value));
                 gsm2 = fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg1_sg1_t6_' + i).value));
                 bf1 = fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg1_sg1_t8_' + i).value));
                 bf2 = fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg1_sg1_t9_' + i).value));
                 bf3 = fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg1_sg1_t10_' + i).value));
                 bf4 = fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg1_sg1_t11_' + i).value));
                 //alert(gsm1);
                 //bf1 = $("input[id*=sg1_t8]");
                 //bf2 = $("input[id*=sg1_t9]");
                 //bf3 = $("input[id*=sg1_t10]");
                 //bf4 = $("input[id*=sg1_t11]");
                 avggsm = (gsm1 + gsm2) / 2;
                 avgbf = (bf1 + bf2 + bf3 + bf4) / 4;
                 bfactor = (avgbf / avggsm) * 1000;

                 document.getElementById('ContentPlaceHolder1_sg1_sg1_t7_' + i).value = fill_zero(avggsm).toFixed(2);
                 document.getElementById('ContentPlaceHolder1_sg1_sg1_t12_' + i).value = fill_zero(avgbf).toFixed(2);
                 document.getElementById('ContentPlaceHolder1_sg1_sg1_t13_' + i).value = fill_zero(bfactor).toFixed(3);
             }
         }
         function fill_zero(val) { if (isNaN(val)) return 0; if (isFinite(val)) return val; }
    </script>
  
    <asp:HiddenField ID="TabName" runat="server" />
</asp:Content>
