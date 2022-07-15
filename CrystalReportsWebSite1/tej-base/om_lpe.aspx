<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="om_lpe" CodeFile="om_lpe.aspx.cs" %>
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

                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <asp:Label ID="lbl1" runat="server" Text="lbl1" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Report No./Date</asp:Label>
                                  <div class="col-sm-1">
                                    <asp:Label ID="lbl1a" runat="server" Text="TC" Style="width: 22px; float: right;" CssClass="col-sm-1 control-label"></asp:Label>
                                </div>
                                <div class="col-sm-3">
                                            <asp:TextBox ID="txtvchnum" runat="server" Width="100%" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                        </div>
                                <div class="col-sm-5">
                                            <asp:TextBox ID="txtvchdate" placeholder="Date" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:CalendarExtender ID="txtvchdate_CalendarExtender" runat="server"
                                                Enabled="True" TargetControlID="txtvchdate"
                                                Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="Maskedit1" runat="server" Mask="99/99/9999"
                                                MaskType="Date" TargetControlID="txtvchdate" />
                                          </div>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="lbl4" runat="server" Text="WO_No" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True"></asp:Label>
                                 <div class="col-sm-1">
                                    <asp:ImageButton ID="btnlbl4" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl4_Click" />
                                 </div>
                                  <div class="col-sm-3">
                                        <asp:TextBox ID="txtlbl4" runat="server" Width="100%" ReadOnly="true" CssClass="form-control" MaxLength="20"></asp:TextBox>
                                      <asp:TextBox ID="txtOrder" runat="server" Width="100%" ReadOnly="true" CssClass="form-control" Visible="false"></asp:TextBox>
                                      <asp:TextBox ID="txtIcode" runat="server" Width="100%" ReadOnly="true" CssClass="form-control" Visible="false"></asp:TextBox>
                                        </div>
                                <asp:Label ID="Label17" runat="server" Text="PO_Line_Item_No" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True"></asp:Label>
                                <div class="col-sm-2">
                                           <asp:TextBox ID="txtWoLineNo" runat="server" CssClass="form-control" MaxLength="5" Visible="false"></asp:TextBox>
                                    <asp:TextBox ID="txtPOLine" runat="server" CssClass="form-control" MaxLength="30"></asp:TextBox>
                                        </div>
                                 <div class="col-sm-2" style="display:none">
                                           <asp:TextBox ID="txtlbl4a" runat="server" CssClass="form-control" MaxLength="20" ReadOnly="True"></asp:TextBox>
                                        </div>
                            </div>
                             <div class="form-group">
                              <asp:Label ID="lbl7" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Customer</asp:Label>
                                <div class="col-sm-1">
                               <asp:ImageButton ID="btnlbl7" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;display:none" OnClick="btnlbl7_Click" />
                                </div>
                                <div class="col-sm-3">
                                 <asp:TextBox ID="txtlbl7" runat="server" Width="100%" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                        </div>
                                <div class="col-sm-5">
                                 <asp:TextBox ID="txtlbl7a" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                             </div> 
                             </div>
                        </div></div></div>
               
                 <div class="col-md-6">
                    <div>
                        <div class="box-body">
                             <div class="form-group">
                                 <asp:Label ID="lbl2" runat="server" Text="lbl2" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">PO_No</asp:Label>
                                 <div class="col-sm-3">
                                           <asp:TextBox ID="txtlbl2" runat="server" CssClass="form-control" Width="100%" ReadOnly="True" MaxLength="100"></asp:TextBox>
                                        </div>
                                 <asp:Label ID="Label1" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Project</asp:Label>
                                        <div class="col-sm-3">
                              <asp:TextBox ID="txtProject" runat="server" CssClass="form-control" Width="100%" MaxLength="50"  ></asp:TextBox>
                                        </div>
                                                               
                             </div>
                            
                            <div class="form-group">
                               <asp:Label ID="lbl3" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Test_Stage</asp:Label>
                                        <div class="col-sm-3">
                              <asp:DropDownList ID="dd_Test_Stage" runat="server" CssClass="form-control" Width="100%" ></asp:DropDownList>
                                        </div>

                                 <asp:Label ID="Label8" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Area_Tested</asp:Label>
                                <div class="col-sm-3">
                              <asp:DropDownList ID="dd_Area_Tested" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                               </div>
                                </div>  

                                       <div class="form-group">
                                <asp:Label ID="Label6" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Product</asp:Label>
                                <div class="col-sm-9">
                                <asp:TextBox ID="txtItem" runat="server" Width="100%" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                               </div>
                            </div>                        
                        </div></div></div>

                 <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <asp:Label ID="lbl102" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Material_Spec</asp:Label>

                                <div class="col-sm-3">
                                        <asp:TextBox ID="txtMaterial" runat="server"  CssClass="form-control" MaxLength="100"></asp:TextBox>
                                        </div>
                                <asp:Label ID="lbl103" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Test_Temprature</asp:Label>
                                <div class="col-sm-3">
                                        <asp:TextBox ID="txtTest_Temp" runat="server"  CssClass="form-control" MaxLength="30"></asp:TextBox>
                                        </div>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="lbl104" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Penetrant_Brand</asp:Label>
                                <div class="col-sm-3">
                               <asp:TextBox ID="txtPenetrant_Brand" runat="server"  CssClass="form-control" MaxLength="100"></asp:TextBox>
                                </div>
                                 <asp:Label ID="Label2" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Penetrant_Batch</asp:Label>
                                <div class="col-sm-3">
                             <asp:TextBox ID="txtPenetrant_Batch" runat="server"  CssClass="form-control" MaxLength="100"></asp:TextBox>
                             </div>
                            </div>
                                 <div class="form-group">
                                <asp:Label ID="Label3" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Lighting_Equip</asp:Label>
                                <div class="col-sm-3">
                                <asp:TextBox ID="txtLighting_Equip" runat="server"  CssClass="form-control" MaxLength="32"></asp:TextBox>
                               </div>
                                <asp:Label ID="Label4" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Component_Thickness</asp:Label>
                                <div class="col-sm-3">
                                <asp:TextBox ID="txtComp_Thick" runat="server"  CssClass="form-control" MaxLength="30"></asp:TextBox>
                               </div>
                            </div>
                              <div class="form-group">
                                <asp:Label ID="Label5" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Weld_Details</asp:Label>
                                <div class="col-sm-3">
                                <asp:TextBox ID="txtWeld_Details" runat="server"  CssClass="form-control" MaxLength="30"></asp:TextBox>
                               </div>
                                  <asp:Label ID="Label13" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Type_Of_Penetrant</asp:Label>
                                <div class="col-sm-3">
                                <asp:TextBox ID="txtPenetrant_Type" runat="server"  CssClass="form-control" MaxLength="30"></asp:TextBox>
                               </div>
                            </div>                             

                              <div class="form-group">
                                <asp:Label ID="Label12" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Technique</asp:Label>
                                <div class="col-sm-9">
                                <asp:TextBox ID="txtTechnique" runat="server"  CssClass="form-control" MaxLength="40"></asp:TextBox>
                               </div>
                            </div>
                     
                             <div class="form-group">
                                <asp:Label ID="Label23" runat="server" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Valve Size & Rating</asp:Label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtValve_Size_Rating" runat="server" MaxLength="60" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div></div></div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">                                                                              
                            <div class="form-group">
                                <asp:Label ID="lbl9" runat="server" Text="lbl9" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Solvent_Brand</asp:Label>                                   
                                <div class="col-sm-3">
                               <asp:TextBox ID="txtSol_Brand" runat="server"  EnableViewState="true" Width="100%" CssClass="form-control" MaxLength="30" ></asp:TextBox>
                                        </div>
                                 <asp:Label ID="lbl10" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Solvent_Batch_No</asp:Label>
                                <div class="col-sm-3">
                                        <asp:TextBox ID="txtSol_Batch_No" runat="server" Width="100%" CssClass="form-control" MaxLength="30"></asp:TextBox>
                                        </div>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="lbl11" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Developer_Brand</asp:Label>
                                <div class="col-sm-3">
                                        <asp:TextBox ID="txtDev_Brand" runat="server" Width="100%" CssClass="form-control" MaxLength="100"></asp:TextBox>
                                        </div>
                                <asp:Label ID="Label7" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Developer_Batch</asp:Label>
                                <div class="col-sm-3">
                                <asp:TextBox ID="txtDev_Batch" runat="server" Width="100%" CssClass="form-control" MaxLength="100"></asp:TextBox>
                               </div>
                            </div>
  
                             <div class="form-group">
                                <asp:Label ID="Label9" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Procedure_Ref</asp:Label>
                                <div class="col-sm-3">
                                <asp:TextBox ID="txtProcedure_Ref" runat="server" Width="100%" CssClass="form-control" MaxLength="30"></asp:TextBox>
                               </div>
                                  <asp:Label ID="Label10" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Acceptance_Std</asp:Label>
                                <div class="col-sm-3">
                                <asp:TextBox ID="txtAcceptance_Std" runat="server" Width="100%" CssClass="form-control" MaxLength="60"></asp:TextBox>
                               </div>
                            </div>

                             <div class="form-group">
                                <asp:Label ID="Label14" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Type_Of_Developer</asp:Label>
                                <div class="col-sm-3">
                                <asp:TextBox ID="txtDev_Type" runat="server" Width="100%" CssClass="form-control" MaxLength="30"></asp:TextBox>
                               </div>
                                     <asp:Label ID="Label11" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Testing_Date</asp:Label>
                                <div class="col-sm-3">
                                <asp:TextBox ID="txtTest_Dt" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server"
                                                Enabled="True" TargetControlID="txtTest_Dt"
                                                Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="MaskedEditExtender1" runat="server" Mask="99/99/9999"
                                                MaskType="Date" TargetControlID="txtTest_Dt" />
                               </div>
                            </div>

                                <div class="form-group">
                                <asp:Label ID="Label15" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Penetration_Time(Mins)</asp:Label>
                                <div class="col-sm-3">
                                <asp:TextBox ID="txtPenetrate_Time" runat="server" Width="100%" CssClass="form-control"  MaxLength="10"></asp:TextBox>
                               </div>
                                    <div class="col-sm-3 control-label">
                                    <asp:Label ID="Label16" runat="server" Text="lbl7" Font-Size="14px" Font-Bold="True">Development_Time(Mins)</asp:Label>
                                        </div>
                                <div class="col-sm-3">
                                <asp:TextBox ID="txtDev_Time" runat="server" Width="100%" CssClass="form-control" MaxLength="10"></asp:TextBox>
                               </div>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="Label24" runat="server" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Component</asp:Label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtComponent" runat="server" MaxLength="30" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div></div></div>
        
                <section class="col-lg-12 connectedSortable">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">
                                <li><a href="#DescTab" id="tab1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab">Valve Details</a></li>
                                 <li><a href="#DescTab2" id="tab2" runat="server" aria-controls="DescTab2" role="tab" data-toggle="tab">Other Details</a></li>
                            </ul>

                            <div class="tab-content" >
                                <div role="tabpanel" class="tab-pane active" id="DescTab">
                                    <div class="lbBody" id="gridDiv" style="color: White; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                        <fin:CoolGridView ID="sg1" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="100%" Height="280px" Font-Size="13px"
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
                                                <asp:BoundField DataField="sg1_f1" HeaderText="JobDT" Visible="false"/>
                                                <asp:BoundField DataField="sg1_f2" HeaderText="Icode"  />
                                                <asp:BoundField DataField="sg1_f3" HeaderText="ERPcode" Visible="false"/>
                                                <asp:BoundField DataField="sg1_f4" HeaderText="Name" Visible="false" />
                                                <asp:BoundField DataField="sg1_f5" HeaderText="Unit" Visible="false"/>
                                                <asp:BoundField DataField="sg1_f6" HeaderText="TagIcode" />
                                     
                                                <asp:TemplateField >
                                                  <HeaderTemplate>PO_SL_No</HeaderTemplate>
                                                 <ItemTemplate>
                                                  <asp:TextBox ID="sg1_t1" runat="server"  Text='<%#Eval("sg1_t1") %>' Width="100%" MaxLength="30"></asp:TextBox>
                                                </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Identification_Tag_No</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t2" runat="server" Text='<%#Eval("sg1_t2") %>' Width="100%" ReadOnly="true" MaxLength="30"></asp:TextBox>                                                   
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                               
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Heat_No</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t3" runat="server" Text='<%#Eval("sg1_t3") %>' Width="100%"  MaxLength="30"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>Job_Description</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t4" runat="server" Text='<%#Eval("sg1_t4") %>' Width="100%" MaxLength="100" ></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Size_Of_Indication</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t5" runat="server" Text='<%#Eval("sg1_t5") %>'  Width="100%" MaxLength="50"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Interpretation</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t6" runat="server" Text='<%#Eval("sg1_t6") %>' Width="100%" MaxLength="30" ></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Remarks</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t7" runat="server" Text='<%#Eval("sg1_t7") %>'  Width="100%" MaxLength="30"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate></HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t8" runat="server" Text='<%#Eval("sg1_t8") %>' Width="100%" ></asp:TextBox>
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
                                
                                <div role="tabpanel" class="tab-pane active" id="DescTab2">
                                    <div class="lbBody" style="height: 200px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                        <div class="col-md-12">
                                            <div>
                                                <div class="box-body">
                                                    <div class="form-group">
                                                    <asp:Label ID="Label18" runat="server" Text="1. Surface Cleaned With Cleaner And Wiped Off With Lint Free Cloth" CssClass="col-sm-8 control-label" Font-Size="14px" Font-Bold="True"></asp:Label>
                                                                <div class="col-sm-4">
                                                                    <asp:DropDownList ID="dd_Surface" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                                                                </div></div>
                                                    <div class="form-group">
                                                                <asp:Label ID="Label19" runat="server" Text="2. Surface Dried & Cleaner Evaporated" CssClass="col-sm-8 control-label" Font-Size="14px" Font-Bold="True"></asp:Label>                                                            
                                                                <div class="col-sm-4">
                                                                    <asp:DropDownList ID="dd_Dried" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                                                                </div></div>
                                                            <div class="form-group">
                                                                <asp:Label ID="lbl12" runat="server" Text="3. Penetrant Application " CssClass="col-sm-8 control-label" Font-Size="14px" Font-Bold="True"></asp:Label>                                                            
                                                                <div class="col-sm-4">
                                                                    <asp:DropDownList ID="dd_Pene_App" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                                                                </div></div>
                                                      <div class="form-group">
                                                                <asp:Label ID="lbl13" runat="server" Text="4. Excess Penetrant Removal By Wiping The Surface First With Dry, Clean, White & Lint Free Cloth Then With Moistened Cloth" CssClass="col-sm-8 control-label" Font-Size="14px" Font-Bold="True"></asp:Label>
                                                           
                                                                <div class="col-sm-4">
                                                                   <asp:DropDownList ID="dd_Excess" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                                                                </div></div>

                                                            <div class="form-group">
                                                                <asp:Label ID="lbl14" runat="server" Text="5. Drying Time Interval Between Penetrant Removal And Developer Application Within 7 Minutes" CssClass="col-sm-8 control-label" Font-Size="14px" Font-Bold="True"></asp:Label>
                                                                <div class="col-sm-4">
                                                                    <asp:DropDownList ID="dd_Drying" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                                                                </div></div>
                                                           <div class="form-group">
                                                                <asp:Label ID="Label20" runat="server" Text="6. Developer Application" CssClass="col-sm-8 control-label" Font-Size="14px" Font-Bold="True"></asp:Label>
                                                           
                                                                <div class="col-sm-4">
                                                                    <asp:DropDownList ID="dd_Dev_App" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                                                                </div></div>
                                                           <div class="form-group">
                                                                <asp:Label ID="Label21" runat="server" Text="7. Evaluation Time (7 Minutes To 60 Minutes)" CssClass="col-sm-8 control-label" Font-Size="14px" Font-Bold="True"></asp:Label>
                                                                <div class="col-sm-4">
                                                                <asp:TextBox ID="txtEvaluation" runat="server" Width="100%" CssClass="form-control" MaxLength="30"></asp:TextBox>
                                                                </div></div>
                                                            <div class="form-group">
                                                                <asp:Label ID="Label22" runat="server" Text="8. Post Cleaning " CssClass="col-sm-8 control-label" Font-Size="14px" Font-Bold="True"></asp:Label>
                                                            
                                                                <div class="col-sm-4">
                                                                    <asp:DropDownList ID="dd_Post" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                                                                </div></div>
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
                            <asp:TextBox ID="txtrmk" runat="server"  MaxLength="150" Width="99%" CssClass="form-control" TextMode="MultiLine" placeholder="Remarks"></asp:TextBox>
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