<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="om_mpe" CodeFile="om_mpe.aspx.cs" %>

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
                headerrowcount: headerFreeze,T
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

        <section class="content">
            <div class="row">
                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <asp:Label ID="lbl1" runat="server" Text="lbl1" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Report No.</asp:Label>
                                <div class="col-sm-1">
                                    <asp:Label ID="lbl1a" runat="server" Text="TC" Style="width: 22px; float: right;" CssClass="col-sm-1 control-label"></asp:Label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtvchnum" runat="server" Width="100%" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                    <asp:TextBox ID="txtgrade" runat="server" Width="100%" ReadOnly="true" CssClass="form-control" Visible="false"></asp:TextBox>
                                    <asp:TextBox ID="txtIcode" runat="server" Width="100%" ReadOnly="true" CssClass="form-control" Visible="false"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label1" runat="server" Text="lbl1" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Date</asp:Label>
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
                                <asp:Label ID="lbl4" runat="server" Text="lbl4" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">WO_No</asp:Label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnlbl4" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl4_Click" />
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtlbl4" runat="server" Width="100%" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                </div>
                                <asp:Label ID="lbl2" runat="server" Text="lbl2" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">PO_No</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtpono" runat="server" ReadOnly="true" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="Label4" runat="server" CssClass="col-sm-4 control-label" Font-Size="14px" Font-Bold="True">Work_Order_Line_Item_No</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtwolno" runat="server" CssClass="form-control" ReadOnly="false" MaxLength="5"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label14" runat="server" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">PO_Line_Item</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtwoline" runat="server" CssClass="form-control" ReadOnly="true" Visible="false"></asp:TextBox>
                                    <asp:TextBox ID="txtPoLine" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="lbl7" runat="server" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Customer</asp:Label>
                                <div class="col-sm-1" style="visibility: hidden;">
                                    <asp:ImageButton ID="btnlbl7" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl7_Click" />
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtacode" runat="server" Width="100%" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-sm-5">
                                    <asp:TextBox ID="txtaname" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                 <asp:Label ID="Label3" runat="server" Text="lbl9" CssClass="col-sm-4 control-label" Font-Size="14px" Font-Bold="True">Project</asp:Label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtproject" runat="server" MaxLength="50" Width="100%" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                             <div class="form-group">
                                  <asp:Label ID="Label5" runat="server" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Product</asp:Label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtItem" runat="server" MaxLength="30" ReadOnly="true" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>                               
                            </div>

                            <div class="form-group">                               
                                 <asp:Label ID="lbl102" runat="server" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Material_Grade</asp:Label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtmatlspec" runat="server" MaxLength="100" ReadOnly="false" CssClass="form-control"></asp:TextBox>
                                </div>                                
                                <asp:Label ID="Label2" runat="server" CssClass="col-sm-2 control-label"  Font-Size="14px" Font-Bold="True" >Testing_Date</asp:Label>
                                <div  class="col-sm-3">
                                    <asp:TextBox ID="txttstDate" runat="server" ReadOnly="false" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="txttstDate_CalendarExtender" runat="server"
                                        Enabled="True" TargetControlID="txttstDate"
                                        Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="MaskedEditExtender1" runat="server" Mask="99/99/9999"
                                        MaskType="Date" TargetControlID="txttstDate" />
                                </>                               
                            </div></div>

                            <div class="form-group">
                                  <asp:Label ID="Label6" runat="server" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Job_Thickness(MM)</asp:Label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtjbthick" runat="server" MaxLength="60" ReadOnly="false" CssClass="form-control"></asp:TextBox>
                                </div>                 
                            </div>
                           
                            <div class="form-group">
                                 <asp:Label ID="lbl3" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Stage_of_Testing</asp:Label>
                                <div class="col-sm-9">
                                    <asp:DropDownList ID="ddstgtest" runat="server" TabIndex="11" Width="100%" Height="35px"></asp:DropDownList>
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="Label8" runat="server" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Acceptance_Standard</asp:Label>
                                <div class="col-sm-9">
                                    <asp:DropDownList ID="ddacptstand" runat="server" TabIndex="11" Width="100%" Height="35px"></asp:DropDownList>
                                </div>                               
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                 <asp:Label ID="Label12" runat="server" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Surface_Preparation</asp:Label>
                                <div class="col-sm-3">
                                    <asp:DropDownList ID="ddsurfprep2" runat="server" TabIndex="11" Width="100%" Height="35px"></asp:DropDownList>
                                </div>
                              <asp:Label ID="lbl5" runat="server" Text="lbl5" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Surface_Prep</asp:Label>
                                <div class="col-sm-3">
                                    <asp:DropDownList ID="ddsurfprep" runat="server" TabIndex="11" Width="100%" Height="35px">
                                    </asp:DropDownList>
                            </div>
                            </div>

                            <div class="form-group">
                                  <asp:Label ID="lbl6" runat="server" Text="lbl6" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Surf_Temperature</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtsurf_temp" runat="server" MaxLength="30" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div> 
                                
                                 <asp:Label ID="lbl103" runat="server" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Mag_Technique</asp:Label>
                                <div class="col-sm-3">
                                    <%--<asp:TextBox ID="txtlbl103" runat="server"  ReadOnly="false" CssClass="form-control"></asp:TextBox>--%>
                                    <asp:DropDownList ID="ddmagntech" runat="server" TabIndex="11" Width="100%" Height="35px"></asp:DropDownList>
                                </div>                                                                                                                        
                            </div>

                            <div class="form-group">
                                <asp:Label ID="lbl104" runat="server" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Method_Of_App_Powder</asp:Label>
                                <div class="col-sm-9">
                                    <asp:DropDownList ID="ddmthodpoder" runat="server" TabIndex="11" Width="100%" Height="35px"></asp:DropDownList>
                                </div>                                                                                                    
                                </div>
                               
                            <div class="form-group">
                                <asp:Label ID="lbl9" runat="server" Text="lbl9" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Type_of_Mag_Particles</asp:Label>
                                <div class="col-sm-9">
                                    <asp:DropDownList ID="ddmagparti" runat="server" TabIndex="11" Width="100%" Height="35px"></asp:DropDownList>                                    
                                </div>
                            </div>
                            
                            <div class="form-group">
                                <asp:Label ID="lbl10" runat="server" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Magn_Current_type</asp:Label>
                                <div class="col-sm-3">
                                    <asp:DropDownList ID="ddmagntype" runat="server" TabIndex="11" Width="100%" Height="35px"></asp:DropDownList>
                                </div>
                                <asp:Label ID="lbl11" runat="server" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Lifting_Power_Of_Yoke</asp:Label>
                                <div class="col-sm-3">
                                    <asp:DropDownList ID="ddlift_pwer" runat="server" TabIndex="11" Width="100%" Height="35px"></asp:DropDownList>
                                </div>
                            </div>                            
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                 <asp:Label ID="lbl12" runat="server" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Min_Lighting_Intensity</asp:Label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtlightequip" runat="server" MaxLength="100" ReadOnly="false" CssClass="form-control"></asp:TextBox>
                                </div>                                
                            </div>
                            <div class="form-group">
                                <asp:Label ID="Label7" runat="server" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Procedure_Ref.</asp:Label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtprocref" runat="server" MaxLength="60" ReadOnly="false" CssClass="form-control"></asp:TextBox>
                                </div>                                                                                        
                            </div>

                             <div class="form-group">
                                <asp:Label ID="Label16" runat="server" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Component</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtComponent" runat="server" MaxLength="30" CssClass="form-control"></asp:TextBox>
                                </div>                           
                               <asp:Label ID="Label15" runat="server" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Valve Size & Rating</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtValve_Size_Rating" runat="server" MaxLength="60" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="Label9" runat="server" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Demagnetization</asp:Label>
                                <div class="col-sm-3">
                                    <asp:DropDownList ID="dddemag" runat="server" TabIndex="11" Width="100%" Height="35px"></asp:DropDownList>
                                </div>
                       
                                <asp:Label ID="Label10" runat="server" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Post_Cleaning</asp:Label>
                                <div class="col-sm-3">
                                    <asp:DropDownList ID="ddpostclean" runat="server" TabIndex="11" Width="100%" Height="35px"></asp:DropDownList>
                                </div>
                            </div>

                            <div class="form-group">
                                 <asp:Label ID="Label13" runat="server" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Mag_Adequacy_Check</asp:Label>
                                <div class="col-sm-9">
                                    <asp:DropDownList ID="ddmagcheck" runat="server" TabIndex="11" Width="100%" Height="35px"></asp:DropDownList>
                                </div>   
                            </div>
                        </div>
                    </div>
                </div>

                <section class="col-lg-12 connectedSortable">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">
                                <li><a href="#DescTab" id="tab1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab">Valve Details</a></li>
                                <li><a href="#DescTab2" id="tab2" runat="server" aria-controls="DescTab2" role="tab" data-toggle="tab">Comm.Terms</a></li>
                                <li><a href="#DescTab3" id="tab3" runat="server" aria-controls="DescTab3" role="tab" data-toggle="tab">Reel/Lot Dtl</a></li>
                                <li><a href="#DescTab4" id="tab4" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Lot.Dtl</a></li>
                                <li><a href="#DescTab5" id="tab5" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Inv.Dtl</a></li>
                                <li><a href="#DescTab6" id="tab6" runat="server" aria-controls="DescTab6" role="tab" data-toggle="tab">UDF Data</a></li>
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

                                                <asp:TemplateField ItemStyle-Width="5px">
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

                                                <asp:BoundField DataField="sg1_srno" HeaderText="SrNo" ItemStyle-Width="80px" />
                                                <asp:BoundField DataField="sg1_f1" HeaderText="JobDT" ItemStyle-Width="80px" visible="false" />
                                                <asp:BoundField DataField="sg1_f2" HeaderText="Icode" ItemStyle-Width="80px" />
                                                <asp:BoundField DataField="sg1_f3" HeaderText="ERPcode" ItemStyle-Width="80px" Visible="false" />
                                                <asp:BoundField DataField="sg1_f4" HeaderText="Name" ItemStyle-Width="80px" Visible="false" />
                                                <asp:BoundField DataField="sg1_f5" HeaderText="Unit" ItemStyle-Width="80px" Visible="false" />
                                                <asp:BoundField DataField="sg1_f6" HeaderText="TagIcode" />

                                                <asp:TemplateField>
                                                    <HeaderTemplate>PO.SL.NO.</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t1" runat="server" Text='<%#Eval("sg1_t1") %>' Width="100%" MaxLength="30"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Identification_Tag_No/Component</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t2" runat="server" Text='<%#Eval("sg1_t2") %>' Width="100%" MaxLength="30"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Identification_Heat_No</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t3" runat="server" Text='<%#Eval("sg1_t3") %>' Width="100%" MaxLength="30"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>Job_Description</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t4" runat="server" Text='<%#Eval("sg1_t4") %>' ReadOnly="true" Width="100%" MaxLength="100"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Record_Of_Indication</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t5" runat="server" Text='<%#Eval("sg1_t5") %>' Width="100%" MaxLength="50"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Interpretation</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t6" runat="server" Text='<%#Eval("sg1_t6") %>' Width="100%" MaxLength="30"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Remarks</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t7" runat="server" Text='<%#Eval("sg1_t7") %>' Width="100%" MaxLength="30"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>abcd</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t8" runat="server" Text='<%#Eval("sg1_t8") %>' Width="100%" onkeypress="return isDecimalKey(event)" MaxLength="7"></asp:TextBox>
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

                <div class="col-md-12">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <asp:Label ID="Label11" runat="server" CssClass="col-sm-1 control-label" Font-Size="14px" Font-Bold="True">Footer_Notes</asp:Label>
                                <div class="col-sm-11">
                                    <asp:TextBox ID="txtfootnote" MaxLength="200" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
                <div class="col-md-12">
                    <div>
                        <div class="box-body">
                            <asp:TextBox ID="txtrmk" runat="server" MaxLength="150" Width="99%" CssClass="form-control" TextMode="MultiLine" placeholder="Remarks"></asp:TextBox>
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
        function calrejection() {
            var rowTot = 0;
            var colTot = 0;
            var mul = 0;
            var total = 0;
            var grid = document.getElementById("<%= sg1.ClientID%>");
            for (var i = 0; i < grid.rows.length - 1; i++) {
                colTot = fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg1_sg1_t3_' + i).value));
                mul = fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg1_sg1_t4_' + i).value));

                //row total is total of total_qty field row wise
                rowTot = colTot * mul;
                document.getElementById('ContentPlaceHolder1_sg1_sg1_t5_' + i).value = fill_zero(rowTot);
                total += rowTot;
                document.getElementById('ContentPlaceHolder1_txtlbl8').value = fill_zero(total);
            }
        }
        function fill_zero(val) { if (isNaN(val)) return 0; if (isFinite(val)) return val; }

        function caloksheet() {
            var rowTot = 0;
            var colTot = 0;
            var min = 0;
            var total = 0;
            var grid = document.getElementById("<%= sg1.ClientID%>");
            for (var i = 0; i < grid.rows.length - 1; i++) {
                colTot = fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg1_sg1_t5_' + i).value));
                min = fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg1_sg1_t6_' + i).value));
                // row total is total of total_qty field row wise
                rowTot = colTot - min;
                document.getElementById('ContentPlaceHolder1_sg1_sg1_t8_' + i).value = fill_zero(rowTot);
                total += rowTot;
                document.getElementById('ContentPlaceHolder1_txtlbl9').value = fill_zero(total);
            }

        }
        function fill_zero(val) { if (isNaN(val)) return 0; if (isFinite(val)) return val; }

        function caltotalvalue() {
            var rowTot = 0;
            var colTot = 0;
            var min = 0;
            var grid = document.getElementById("<%= sg1.ClientID%>");
            for (var i = 0; i < grid.rows.length - 1; i++) {
                colTot += fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg1_sg1_t5_' + i).value));
                //row total is total of total_qty field row wise
                document.getElementById('ContentPlaceHolder1_txtlbl8').value = fill_zero(colTot);
            }
        }
        function fill_zero(val) { if (isNaN(val)) return 0; if (isFinite(val)) return val; }
        function caltotalvalue1() {
            var rowTot = 0;
            var colTot = 0;
            var min = 0;
            var grid = document.getElementById("<%= sg1.ClientID%>");

            for (var i = 0; i < grid.rows.length - 1; i++) {

                colTot += fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg1_sg1_t8_' + i).value));
                document.getElementById('ContentPlaceHolder1_txtlbl9').value = fill_zero(colTot);

            }
        }

    </script>
    <asp:HiddenField ID="TabName" runat="server" />
</asp:Content>
