<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="om_pick_pass" CodeFile="om_pick_pass.aspx.cs" %>
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
                                <asp:Label ID="Label17" runat="server" Text="PO_Item_Qty" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True"></asp:Label>
                                 <div class="col-sm-3">
                                           <asp:TextBox ID="txtlbl4a" runat="server" CssClass="form-control" MaxLength="20" ReadOnly="True" Visible="false"></asp:TextBox>
                                     <asp:TextBox ID="txtPo_Qty" runat="server" CssClass="form-control" MaxLength="20" ></asp:TextBox>
                                        </div>
                            </div>
                             <div class="form-group">
                              <asp:Label ID="lbl7" runat="server"  CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Customer</asp:Label>
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

                              <div class="form-group">
                                 <asp:Label ID="lbl2" runat="server" Text="lbl2" CssClass="col-sm-4 control-label" Font-Size="14px" Font-Bold="True">PO_No</asp:Label>
                                 <div class="col-sm-8">
                                           <asp:TextBox ID="txtlbl2" runat="server" CssClass="form-control" Width="100%" ReadOnly="True" MaxLength="100"></asp:TextBox>
                                        </div>                                                               
                             </div>

                            <div class="form-group">
                             <asp:Label ID="Label1" runat="server" Text="lbl3" CssClass="col-sm-4 control-label" Font-Size="14px" Font-Bold="True">Project</asp:Label>
                                        <div class="col-sm-8">
                              <asp:TextBox ID="txtProject" runat="server" CssClass="form-control" Width="100%" MaxLength="50"  ></asp:TextBox>
                                        </div>
                            </div>                          
                        </div></div></div>
               
                 <div class="col-md-6">
                    <div>
                        <div class="box-body">                                                      
                         <div class="form-group">
                                <asp:Label ID="Label8" runat="server"  CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">ITP_No</asp:Label>
                                <div class="col-sm-4">
                               <asp:TextBox ID="txtITP" runat="server" CssClass="form-control" Width="100%" MaxLength="30"></asp:TextBox>
                               </div>
                             <asp:Label ID="Label3" runat="server"  CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Testing_Dt</asp:Label>
                                <div class="col-sm-3">
                               <asp:TextBox ID="txtTest_Dt" runat="server" CssClass="form-control" Width="100%" ></asp:TextBox>
                                     <asp:CalendarExtender ID="CalendarExtender1" runat="server"
                                                Enabled="True" TargetControlID="txtTest_Dt"
                                                Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="MaskedEditExtender1" runat="server" Mask="99/99/9999"
                                                MaskType="Date" TargetControlID="txtTest_Dt" />
                               </div>
                                </div>  

                                       <div class="form-group">
                                <asp:Label ID="Label6" runat="server"  CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Product</asp:Label>
                                <div class="col-sm-9">
                                <asp:TextBox ID="txtItem" runat="server" CssClass="form-control" Width="100%" MaxLength="30"  ReadOnly="true"></asp:TextBox>
                               </div>
                            </div>

                             <div class="form-group">
                                <asp:Label ID="Label23" runat="server"  CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Material</asp:Label>
                                <div class="col-sm-9">
                               <asp:TextBox ID="txtMaterial" runat="server" CssClass="form-control" Width="100%" MaxLength="60"></asp:TextBox>
                               </div>
                            </div>
                            
                             <div class="form-group">
                                <asp:Label ID="Label24" runat="server"  CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Stage_Of_Testing</asp:Label>
                                <div class="col-sm-9">
                               <asp:TextBox ID="txtStage" runat="server" CssClass="form-control" Width="100%" MaxLength="30" ></asp:TextBox>
                               </div>
                            </div>   
                            
                               <div class="form-group">
                                <asp:Label ID="Label4" runat="server"  CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Procedure_Ref</asp:Label>
                                <div class="col-sm-9">
                               <asp:TextBox ID="txtProcedure" runat="server" CssClass="form-control" Width="100%" MaxLength="100" ></asp:TextBox>
                               </div>
                            </div>     
                        </div></div></div>

                 <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <asp:Label ID="lbl102" runat="server"  CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Client_Tag_No</asp:Label>
                                <div class="col-sm-9">
                                        <asp:TextBox ID="txtClientTag" runat="server"  CssClass="form-control" MaxLength="30"></asp:TextBox>
                                        </div>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="lbl104" runat="server" Text="lbl104" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Surface_Preparation</asp:Label>
                                <div class="col-sm-9">
                               <asp:TextBox ID="txtSurfacePrep" runat="server"  CssClass="form-control" MaxLength="100"></asp:TextBox>
                                </div>                               
                            </div>

                                 <div class="form-group">
                                <asp:Label ID="Label2" runat="server" Text="Label2" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Surface_Temprature</asp:Label>
                                <div class="col-sm-9">
                             <asp:TextBox ID="txtSurface_Temp" runat="server"  CssClass="form-control" MaxLength="30"></asp:TextBox>
                             </div>
                            </div>   
                            
                              <div class="form-group">
                                <asp:Label ID="lbl9" runat="server" Text="lbl9" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Passivation_Technique</asp:Label>                                   
                                <div class="col-sm-9">
                             <asp:DropDownList ID="dd_Passivation" runat="server" CssClass="form-control" Width="100%" ></asp:DropDownList>
                                        </div>                               
                            </div>  
                            
                            <div class="form-group">
                                <asp:Label ID="Label10" runat="server" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Valve Size & Rating</asp:Label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtValve_Size_Rating" runat="server" MaxLength="60" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>                
                        </div></div></div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">                                                                              
                            <div class="form-group">
                                 <asp:Label ID="lbl10" runat="server" Text="lbl10" CssClass="col-sm-4 control-label" Font-Size="14px" Font-Bold="True">Meth_Of_App_Nitric_Acid</asp:Label>
                                <div class="col-sm-8">
                                        <asp:TextBox ID="txtNitric" runat="server" Width="100%" CssClass="form-control" MaxLength="100"></asp:TextBox>
                                        </div>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="lbl11" runat="server"  CssClass="col-sm-4 control-label" Font-Size="14px" Font-Bold="True">Chemical_Used</asp:Label>
                                <div class="col-sm-8">
                                        <asp:TextBox ID="txtChemical" runat="server" Width="100%" CssClass="form-control" MaxLength="100"></asp:TextBox>
                                        </div>                                
                            </div>
  
                            <div class="form-group">
                                <asp:Label ID="Label7" runat="server"  CssClass="col-sm-4 control-label" Font-Size="14px" Font-Bold="True">Chemical_Used1</asp:Label>
                                <div class="col-sm-8">
                                <asp:TextBox ID="txtChemical1" runat="server" Width="100%" CssClass="form-control" MaxLength="100"></asp:TextBox>
                               </div>
                            </div>
                             <div class="form-group">
                                <asp:Label ID="Label9" runat="server" Text="Label9" CssClass="col-sm-4 control-label" Font-Size="14px" Font-Bold="True">Dur_For_App_Of_Mixture</asp:Label>
                                <div class="col-sm-8">
                                <asp:TextBox ID="txtAppMix" runat="server" Width="100%" CssClass="form-control" MaxLength="30"></asp:TextBox>
                               </div>
                            </div>
                              <div class="form-group">
                                <asp:Label ID="Label5" runat="server" CssClass="col-sm-4 control-label" Font-Size="14px" Font-Bold="True">Component</asp:Label>
                                <div class="col-sm-8">
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
                                                <asp:BoundField DataField="sg1_f1" HeaderText="-" Visible="false"/>
                                                <asp:BoundField DataField="sg1_f2" HeaderText="Icode"  />
                                                <asp:BoundField DataField="sg1_f3" HeaderText="ERPcode" Visible="false"/>
                                                <asp:BoundField DataField="sg1_f4" HeaderText="Name" Visible="false"/>
                                                <asp:BoundField DataField="sg1_f5" HeaderText="Unit" Visible="false"/>
                                                <asp:BoundField DataField="sg1_f6" HeaderText="TagIcode" />
                                     
                                                <asp:TemplateField >
                                                  <HeaderTemplate>Identification_Tag_No</HeaderTemplate>
                                                 <ItemTemplate>
                                                  <asp:TextBox ID="sg1_t1" runat="server"  Text='<%#Eval("sg1_t1") %>' Width="100%" ReadOnly="true" MaxLength="30"></asp:TextBox>
                                                </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Heat_No</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t2" runat="server" Text='<%#Eval("sg1_t2") %>' Width="100%"  MaxLength="30"></asp:TextBox>                                                   
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                               
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>Job_Description</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t3" runat="server" Text='<%#Eval("sg1_t3") %>' Width="100%"  MaxLength="100"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Size_Of_Indication</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t4" runat="server" Text='<%#Eval("sg1_t4") %>' Width="100%" MaxLength="30" ></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Interpretation</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t5" runat="server" Text='<%#Eval("sg1_t5") %>'  Width="100%" MaxLength="30"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Remarks</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t6" runat="server" Text='<%#Eval("sg1_t6") %>' Width="100%" MaxLength="30" ></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>Remarks1</HeaderTemplate>
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
                                                    <asp:Label ID="Label18" runat="server" Text="1. Surface Preparation" CssClass="col-sm-8 control-label" Font-Size="14px" Font-Bold="True"></asp:Label>
                                                                <div class="col-sm-4">
                                                                   <asp:TextBox ID="txt_GridSurface" runat="server" Width="100%" CssClass="form-control" MaxLength="100"></asp:TextBox>
                                                                </div></div>
                                                    <div class="form-group">
                                                                <asp:Label ID="Label19" runat="server" Text="2. Post Cleaning" CssClass="col-sm-8 control-label" Font-Size="14px" Font-Bold="True"></asp:Label>                                                            
                                                                <div class="col-sm-4">
                                                                <asp:TextBox ID="txtPost" runat="server" Width="100%" CssClass="form-control" MaxLength="60"></asp:TextBox>    
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
               
                 <div class="col-md-12" style="display:none">
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