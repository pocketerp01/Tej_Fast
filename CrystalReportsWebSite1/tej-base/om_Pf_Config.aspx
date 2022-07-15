<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="om_Pf_Config" CodeFile="om_Pf_Config.aspx.cs" %>
<%--  --%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../tej-base/Scripts/gridviewScroll.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function () {
           <%-- gridviewScroll('#<%=sg1.ClientID%>', gridDiv, 1, 5);--%>

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
                    <td style="text-align:right">
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
                 <div class="col-md-6" style="display:none;" >
                    <div>
                        <div class="box-body">                                                                            
                        </div></div></div>

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
                                <asp:Label ID="Label1" runat="server" Text="lbl1" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Entry_Date</asp:Label>
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
                                <asp:Label ID="lbl4" runat="server" Text="lbl4" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Grade</asp:Label>
                                 <div class="col-sm-1">
                                    <asp:ImageButton ID="btnlbl4" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl4_Click" />
                                 </div>
                                  <div class="col-sm-3">
                                        <asp:TextBox ID="txtlbl4" runat="server" Width="100%" ReadOnly="true" CssClass="form-control"></asp:TextBox>                                         
                                        </div>                                
                                <div class="col-sm-6">
                                      <asp:TextBox ID="txtgraden" runat="server" ReadOnly="true" CssClass="form-control" Width="100%" ></asp:TextBox> 
                                        </div>                                 
                            </div>
                            </div></div>
                       </div>
                 <div class="col-md-6">
                    <div>
                        <div class="box-body">   
                   
                            <div class="form-group">
                                <asp:Label ID="Label25" runat="server" Text="Eff_From" Font-Bold="true" CssClass="col-sm-3 control-label"></asp:Label>
                                
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtefffrom" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server"
                                                Enabled="True" TargetControlID="txtefffrom"
                                                Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="MaskedEditExtender2" runat="server" Mask="99/99/9999"
                                                MaskType="Date" TargetControlID="txtefffrom" />
                                </div>
                                <asp:Label ID="Label26" runat="server" Text="Eff_upto" Font-Bold="true" CssClass="col-sm-3 control-label"></asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtlbl4a" runat="server" CssClass="form-control" Width="100%"  MaxLength="10"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender2" runat="server"
                                                Enabled="True" TargetControlID="txtlbl4a"
                                                Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="MaskedEditExtender3" runat="server" Mask="99/99/9999"
                                                MaskType="Date" TargetControlID="txtlbl4a" />
                                </div>
                            </div>

                                <div class="form-group" style="display:none;">
                                <asp:Label ID="Label19" runat="server" Text="Ent_By" Font-Bold="true" CssClass="col-sm-3 control-label"></asp:Label>                          
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtentby" runat="server" Width="100%" CssClass="form-control" MaxLength="30" ReadOnly="true"></asp:TextBox>
                                 </div>  
                                     <asp:Label ID="Label22" runat="server" Text="Ent_Dt" Font-Bold="true" CssClass="col-sm-3 control-label"></asp:Label>                      
                                <div class="col-sm-3">    
                                    <asp:TextBox ID="txtentdt" runat="server" Width="100%" CssClass="form-control" MaxLength="10" ReadOnly="true"></asp:TextBox>
                                </div>                               
                            </div>
                                <div class="form-group">
                                           <asp:Label ID="Label20" runat="server" Text="Deactivated Y/N" Font-Bold="true" CssClass="col-sm-3 control-label"></asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtstatus" runat="server" CssClass="form-control" MaxLength="1" Text="N" ForeColor="Red"></asp:TextBox>
                                </div>
                                <asp:Label ID="lbldeac" runat="server" Text="(Y:Close,N:Open)" Font-Size="Small" Font-Bold="true" CssClass="col-sm-6 control-label" ForeColor="Red"></asp:Label>
                            </div>
                                 <div class="form-group" style="display:none;">
                                <asp:Label ID="Label23" runat="server" Text="Edit_By" Font-Bold="true" CssClass="col-sm-3 control-label"></asp:Label>                          
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtedtby" runat="server" Width="100%" CssClass="form-control" MaxLength="30" ReadOnly="true"></asp:TextBox>
                                 </div>  
                                     <asp:Label ID="Label24" runat="server" Text="Edit_Dt" Font-Bold="true" CssClass="col-sm-3 control-label"></asp:Label>                      
                                <div class="col-sm-3">    
                                    <asp:TextBox ID="txtedtdt" runat="server" Width="100%" CssClass="form-control" MaxLength="10" ReadOnly="true"></asp:TextBox>
                                </div>                               
                            </div>

                            </div></div></div>
                <section class="col-lg-12 connectedSortable">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">
                                <li><a href="#DescTab" id="tab1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab">Provident Fund</a></li>
                                <li><a href="#DescTab2" id="tab2" runat="server" aria-controls="DescTab2" role="tab" data-toggle="tab">EDLI Charges</a></li>
                                <li><a href="#DescTab3" id="tab3" runat="server" aria-controls="DescTab3" role="tab" data-toggle="tab">E.S.I Details</a></li>
                                <li><a href="#DescTab4" id="tab4" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Lot.Dtl</a></li>
                                <li><a href="#DescTab5" id="tab5" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Inv.Dtl</a></li>
                                <li><a href="#DescTab6" id="tab6" runat="server" aria-controls="DescTab6" role="tab" data-toggle="tab">UDF Data</a></li>
                            </ul>
                               <div class="tab-content" >
                                <div role="tabpanel" class="tab-pane active" id="DescTab"  >

                                      <div class="col-md-6">
                    <div>
                        <div class="box-body">                  
                                     <div class="form-group" style="display:none;">
                                <asp:Label ID="Label3" runat="server" Text="Region Code" Font-Bold="true" CssClass="col-sm-3 control-label"></asp:Label>                            
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtregion" runat="server" CssClass="form-control" MaxLength="10" ></asp:TextBox>
                                </div>
                                  <asp:Label ID="Label4" runat="server" Text="Office Code" Font-Bold="true" CssClass="col-sm-3 control-label"></asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtofcecd" runat="server" CssClass="form-control" Width="100%" MaxLength="10"  onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                </div>
                            </div>

                              <div class="form-group" style="display:none;">>
                                <asp:Label ID="Label5" runat="server" Text="Estas Code" Font-Bold="true" CssClass="col-sm-3 control-label"></asp:Label>                          
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtestncd" runat="server" CssClass="form-control" MaxLength="10" ></asp:TextBox>
                                </div>
                                  <asp:Label ID="Label6" runat="server" Text="Ecn. if any" Font-Bold="true" CssClass="col-sm-3 control-label"></asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtecn" runat="server" CssClass="form-control" Width="100%" MaxLength="10" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                </div>
                            </div>

                                    <div class="form-group">
                                <asp:Label ID="Label7" runat="server" Text="Employer Pension Fund %age(EPS)" Font-Bold="true" CssClass="col-sm-6 control-label"></asp:Label>                          
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtpfpercent" runat="server" CssClass="form-control" MaxLength="4" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                </div>                               
                            </div>

                            <div class="form-group">
                                <asp:Label ID="Label8" runat="server" Text="Employer Provident Fund Limit(EPF)" Font-Bold="true" CssClass="col-sm-6 control-label"></asp:Label>                          
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtpflimit" runat="server" CssClass="form-control" MaxLength="4" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                </div>                               
                            </div>
                              <div class="form-group">
                                <asp:Label ID="Label9" runat="server" Text="Employer Pension Fund Limit" Font-Bold="true" CssClass="col-sm-6 control-label"></asp:Label>                          
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtpensionlimit" runat="server" CssClass="form-control" MaxLength="8" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                </div>                               
                            </div>
                                 </div></div></div>
                               <div class="col-md-6">
                    <div>
                        <div class="box-body">   
                              

                                     <div class="form-group">
                                <asp:Label ID="Label10" runat="server" Text="Admin Inspection Charges" Font-Bold="true" CssClass="col-sm-6 control-label"></asp:Label>                          
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtinsp" runat="server" CssClass="form-control" MaxLength="8" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                </div>                               
                            </div>

                                           <div class="form-group">
                                <asp:Label ID="Label12" runat="server" Text="Employer VPF Salary Limit" Font-Bold="true" CssClass="col-sm-6 control-label"></asp:Label>                          
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtvpf" runat="server" CssClass="form-control" MaxLength="8" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                </div>                               
                            </div>

                             <div class="form-group">
                                <asp:Label ID="Label13" runat="server" Text="Employer Pension Fund Limit" Font-Bold="true" CssClass="col-sm-6 control-label"></asp:Label>                          
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtpensionfundlimit" runat="server" CssClass="form-control" MaxLength="8" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                </div>                               
                            </div>

                            <div class="form-group"  style="display:none;">
                                <asp:Label ID="Label14" runat="server" Text="State" Font-Bold="true" CssClass="col-sm-2 control-label"></asp:Label>
                                <div class="col-sm-1" >
                                    <asp:ImageButton ID="btnlbl7" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl7_Click" />
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtlbl7" runat="server" CssClass="form-control" MaxLength="80" ></asp:TextBox>
                                </div>
                                
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtlbl7a" runat="server" CssClass="form-control" Width="100%" MaxLength="25"  onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                </div>
                            </div>

                                                         
                        </div></div></div>
                                    </div>

                                      <div role="tabpanel" class="tab-pane active" id="DescTab2"  >
                                          <div class="col-md-6">
                    <div>
                        <div class="box-body">   
                                <div class="form-group">
                                <asp:Label ID="Label15" runat="server" Text="Minimum Wages" Font-Bold="true" CssClass="col-sm-3 control-label"></asp:Label>                          
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtwages" runat="server" CssClass="form-control" MaxLength="8" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                </div>                               
                           <%-- </div>

                             <div class="form-group">--%>
                                <asp:Label ID="Label16" runat="server" Text="A/c 21 EDLI %age" Font-Bold="true" CssClass="col-sm-3 control-label"></asp:Label>                          
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtedlipercent" runat="server" CssClass="form-control" MaxLength="8" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                </div>                               
                            </div> 

                              </div></div></div>

                    <div class="col-md-6">
                    <div>
                        <div class="box-body">   
                                <div class="form-group">
                                    <asp:Label ID="Label17" runat="server" Text="EDLI Admin Charges(A/c 22)" Font-Bold="true" CssClass="col-sm-3 control-label"></asp:Label>                          
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtadmin" runat="server" CssClass="form-control" MaxLength="8" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                </div>   
                                         <asp:Label ID="Label18" runat="server" Text="Salary Limit" Font-Bold="true" CssClass="col-sm-3 control-label"></asp:Label>                          
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtsalry" runat="server" CssClass="form-control" MaxLength="8" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                </div>   
                                      </div></div></div></div>
                                    </div>   
                                 
                                <div role="tabpanel" class="tab-pane active" id="DescTab4" style="display:none;"  >
                                    <%--<div class="lbBody" id="gridDiv" style="color: White; height: 300px; overflow: hidden; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <fin:CoolGridView ID="sg1" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="1500px" Font-Size ="13px" 
                                            AutoGenerateColumns="False" OnRowDataBound="sg1_RowDataBound"
                                            OnRowCommand="sg1_RowCommand">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />--%>
                                    <div class="lbBody" id="gridDiv" style="color: White; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                        <fin:CoolGridView ID="sg1" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="100%" Height="270px" Font-Size="13px"
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

                                                <asp:TemplateField ItemStyle-Width="5px">
                                                    <HeaderTemplate>Add</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btnadd" runat="server" CommandName="SG1_ROW_ADD" ImageAlign="Middle" ImageUrl="../tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Row" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Del</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btnrmv" runat="server" CommandName="SG1_RMV" ImageUrl="../tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Row" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="sg1_srno" HeaderText="SrNo" ItemStyle-Width="80px" />
                                                <asp:BoundField DataField="sg1_f1" HeaderText="DED"  ItemStyle-Width="80px" Visible="false"/>
                                                <asp:BoundField DataField="sg1_f2" HeaderText="DED_TYPE" ItemStyle-Width="80px" Visible="false"/>
                                                <asp:BoundField DataField="sg1_f3" HeaderText="Invoice_No." ItemStyle-Width="80px" Visible="false"/>
                                                <asp:BoundField DataField="sg1_f4" HeaderText="Invoice_Date"  ItemStyle-Width="80px" Visible="false"/>
                                                <asp:BoundField DataField="sg1_f5" HeaderText="Icode" ItemStyle-Width="80px" Visible="false" />
                                                <asp:BoundField DataField="sg1_f6" HeaderText="Rm_Name" Visible="false"/>
                                                 
                                                <asp:TemplateField ItemStyle-Width="15px">
                                                            <HeaderTemplate>DED_TYPE</HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="sg1_t1" runat="server" Text='<%#Eval("sg1_t1") %>' Width="100%" MaxLength="15"></asp:TextBox>
                                                                  </ItemTemplate>
                                                        </asp:TemplateField>

                                                <asp:TemplateField >
                                                    <HeaderTemplate>Salary_From</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t2" runat="server" Text='<%#Eval("sg1_t2") %>'  Width="100%" MaxLength="6" onkeypress="return isDecimalKey(event)"></asp:TextBox>                                                   
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                               
                                                <asp:TemplateField ItemStyle-Width="110px">
                                                    <HeaderTemplate>Salary_Upto</HeaderTemplate>
                                                    <ItemTemplate>                                          
                                                        <asp:TextBox ID="sg1_t3" runat="server" Text='<%#Eval("sg1_t3") %>' onChange="Cal()" Width="100%" MaxLength="6" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="80px" >
                                                    <HeaderTemplate>April</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t4" runat="server" Text='<%#Eval("sg1_t4") %>' onChange="Cal()" onkeypress="return isDecimalKey(event)" Width="100%" maxlength="6" ></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField >
                                                    <HeaderTemplate>May</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t5" runat="server" Text='<%#Eval("sg1_t5") %>' onChange="Cal()" Width="100%" onkeypress="return isDecimalKey(event)" maxlength="6" ></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField >
                                                    <HeaderTemplate>June</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t6" runat="server" Text='<%#Eval("sg1_t6") %>' onChange="Cal()" Width="100%" onkeypress="return isDecimalKey(event)" maxlength="6" ></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField >
                                                    <HeaderTemplate>July</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t7" runat="server" Text='<%#Eval("sg1_t7") %>' onChange="Cal()" Width="100%" onkeypress="return isDecimalKey(event)" maxlength="6"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField >
                                                    <HeaderTemplate>August</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t8" runat="server" Text='<%#Eval("sg1_t8") %>' onChange="Cal()" Width="100%" onkeypress="return isDecimalKey(event)" maxlength="6" ></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField >
                                                    <HeaderTemplate>September</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t9" runat="server" Text='<%#Eval("sg1_t9") %>' onChange="Cal()" Width="100%" onkeypress="return isDecimalKey(event)"  MaxLength="6"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField >
                                                    <HeaderTemplate>October</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t10" runat="server" Text='<%#Eval("sg1_t10") %>' onChange="Cal()" Width="100%" onkeypress="return isDecimalKey(event)" MaxLength="6"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField >
                                                    <HeaderTemplate>November</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t11" runat="server" Text='<%#Eval("sg1_t11") %>' onChange="Cal()" Width="100%" onkeypress="return isDecimalKey(event)" MaxLength="6"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField >
                                                    <HeaderTemplate>December</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t12" runat="server" Text='<%#Eval("sg1_t12") %>' onChange="Cal()" Width="100%" onkeypress="return isDecimalKey(event)" MaxLength="6"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField >
                                                    <HeaderTemplate>January</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t13" runat="server" Text='<%#Eval("sg1_t13") %>' onChange="Cal()" Width="100%" onkeypress="return isDecimalKey(event)" MaxLength="6"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField >
                                                    <HeaderTemplate>Febuary</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t14" runat="server" Text='<%#Eval("sg1_t14") %>' onChange="Cal()" Width="100%" onkeypress="return isDecimalKey(event)" MaxLength="6"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField >
                                                    <HeaderTemplate>March</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t15" runat="server" Text='<%#Eval("sg1_t15") %>' Width="100%" onChange="Cal()" onkeypress="return isDecimalKey(event)" MaxLength="6" ></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                
                                                <asp:TemplateField >
                                                    <HeaderTemplate>Total</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t16" runat="server" Text='<%#Eval("sg1_t16") %>' Width="100%" onkeypress="return isDecimalKey(event)" MaxLength="6" ReadOnly="true"></asp:TextBox>
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
               
                  <div class="col-md-12" style="display:none;">
                    <div>
                        <div class="box-body">
                           <div class="form-group">
                               <asp:Label ID="Label11" runat="server" Text="lbl7" CssClass="col-sm-1 control-label" Font-Size="14px" Font-Bold="True">Footer_Notes</asp:Label>
                               <div class="col-sm-11">
                                   <asp:TextBox ID="txtfootnote" MaxLength="200" runat="server" CssClass="form-control" Width="100%" ></asp:TextBox>
                               </div>
                           </div>

                        </div></div></div>

                 <div class="col-md-12">
                    <div>
                        <div class="box-body">
                            <asp:TextBox ID="txtrmk" runat="server"  MaxLength="80" Width="99%" CssClass="form-control" TextMode="MultiLine" placeholder="Remarks"></asp:TextBox>
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
    <asp:HiddenField ID="hf2" runat="server" />
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
        function Cal() {
            var m1 = 0; var m2 = 0; var m3 = 0; var m4 = 0; var m5 = 0; var m6 = 0; var m7 = 0; var m8 = 0; var m9 = 0; var m10 = 0; var m11 = 0; var m12 = 0;var m13=0;
            var grid = document.getElementById("<%= sg1.ClientID%>");
            for (var i = 0; i < grid.rows.length - 1; i++) {
                m1 = fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg1_sg1_t4_' + i).value));
                m2 = fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg1_sg1_t5_' + i).value));
                m3 = fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg1_sg1_t6_' + i).value));
                m4 = fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg1_sg1_t7_' + i).value));
                m5 = fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg1_sg1_t8_' + i).value));
                m6 = fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg1_sg1_t9_' + i).value));
                m7 = fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg1_sg1_t10_' + i).value));
                m8 = fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg1_sg1_t11_' + i).value));
                m9 = fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg1_sg1_t12_' + i).value));
                m10 = fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg1_sg1_t13_' + i).value));
                m11 = fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg1_sg1_t14_' + i).value));
                m12 = fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg1_sg1_t15_' + i).value));                
                m13 += (m1 * 1) + (m2 * 1) + (m3 * 1) + (m4 * 1) + (m5 * 1) + (m6 * 1) + (m7 * 1) + (m8 * 1) + (m9 * 1) + (m10 * 1) + (m11 * 1) + (m12 * 1);                
                document.getElementById('ContentPlaceHolder1_sg1_sg1_t16_' + i).value = fill_zero(m13);
            }
        }
        
        function fill_zero(val) { if (isNaN(val)) return 0; if (isFinite(val)) return val; }       
    </script>
  
    <asp:HiddenField ID="TabName" runat="server" />

</asp:Content>
