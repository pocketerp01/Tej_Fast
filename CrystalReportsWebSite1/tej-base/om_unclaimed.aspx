<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="om_unclaimed" CodeFile="om_unclaimed.aspx.cs" %>
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
                                <asp:Label ID="Label1" runat="server" Text="lbl1" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True" >Date</asp:Label>
                                <div class="col-sm-4">
                                            <asp:TextBox ID="txtvchdate" placeholder="Date" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                            <asp:CalendarExtender ID="txtvchdate_CalendarExtender" runat="server"
                                                Enabled="True" TargetControlID="txtvchdate"
                                                Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="Maskedit1" runat="server" Mask="99/99/9999"
                                                MaskType="Date" TargetControlID="txtvchdate" />
                                          </div>
                                
                                                                    
                            </div>

                            <div class="form-group" style="display:none;">
                                <asp:Label ID="lbl4" runat="server" Text="lbl4" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Licence_No</asp:Label>
                                 <div class="col-sm-1">
                                    <asp:ImageButton ID="btnlbl4" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl4_Click" />
                                 </div>
                                  <div class="col-sm-3">
                                        <asp:TextBox ID="txtlbl4" runat="server" Width="100%" ReadOnly="true" CssClass="form-control"></asp:TextBox>  
                                        </div>
                                <asp:Label ID="lbl2" runat="server" Text="lbl2" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Licence_Date</asp:Label>                                
                                <div class="col-sm-4">
                                           <asp:TextBox ID="txtlicdt" placeholder="Date" runat="server" ReadOnly="true" CssClass="form-control" Width="100%" ></asp:TextBox>
                                        </div>                                 
                            </div>
                                                    
                            <div class="form-group" style="display:none;">
                                <asp:Label ID="Label14" runat="server" Text="DGFT_File No" Font-Bold="true" CssClass="col-sm-3 control-label"></asp:Label>
                                <div class="col-sm-1" style="display:none;">
                                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl7_Click" />
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtlbl7" runat="server" CssClass="form-control" MaxLength="80" ReadOnly="true"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label15" runat="server" Text="Value_Addition" Font-Bold="true" CssClass="col-sm-2 control-label"></asp:Label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtlbl7a" runat="server" CssClass="form-control" Width="100%" MaxLength="25" ReadOnly="true" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                </div>
                            </div>

                             <div class="form-group" style="display:none;">
                               <asp:Label ID="lbl7" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Customer</asp:Label>
                                <div class="col-sm-1" style="visibility:hidden;">
                               <asp:ImageButton ID="btnlbl7" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl7_Click" />
                                </div>
                                <div class="col-sm-3">
                                 <asp:TextBox ID="txtacode" runat="server" Width="100%" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                        </div>                                 
                                        <div class="col-sm-5">
                                        <asp:TextBox ID="txtaname" runat="server"  ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                        </div>
                            </div>
                        </div></div></div>
               
                 <div class="col-md-6" >
                    <div>
                        <div class="box-body">
                           <div class="form-group" style="display:none;">
                                 <asp:Label ID="Label2" runat="server" Text="lbl4" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Licence_Qty</asp:Label>
                                 <div class="col-sm-4">
                                           <asp:TextBox ID="txtcurrqty" runat="server" ReadOnly="true" CssClass="form-control" MaxLength="25"></asp:TextBox>
                                    <%-- <asp:CalendarExtender ID="txttstDate_CalendarExtender" runat="server"
                                                Enabled="True" TargetControlID="txttstDate"
                                                Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="MaskedEditExtender1" runat="server" Mask="99/99/9999"
                                                MaskType="Date" TargetControlID="txttstDate" />--%>
                                        </div>
                               <asp:Label ID="lbl6" runat="server" Text="lbl6" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Licence_Val</asp:Label>
                                       <div class="col-sm-3">
                                            <asp:TextBox ID="txtcurrval" ReadOnly="true" runat="server" MaxLength="28" CssClass="form-control" Width="100%"></asp:TextBox>
                                        </div>
                             </div>

                           <div class="form-group" style="display:none;">
                                 <asp:Label ID="lbl5" runat="server" Text="lbl5" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">HSN_Code</asp:Label>                                    
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl5" ReadOnly="true" runat="server" CssClass="form-control" Width="100%" ></asp:TextBox>
                                            <%--<asp:DropDownList ID="ddsurfprep" runat="server" TabIndex="11" Width="100%" Height="35px" CssClass="rounded_corners">                                                
                                            </asp:DropDownList>--%>                                            
                                        </div>
                                   <div class="col-sm-5">
                                            <asp:TextBox ID="txthsname" ReadOnly="true" runat="server" CssClass="form-control" Width="100%" ></asp:TextBox>                                                                              
                                        </div>
                                        
                            </div>
                             <div class="form-group" style="display:none;">
                                 <asp:Label ID="lbl3" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Selected_Qty</asp:Label>
                                 <div class="col-sm-4">
                              <asp:TextBox ID="txtlbl3" runat="server" ReadOnly="true" CssClass="form-control" Width="100%" ></asp:TextBox>                                            
                                        </div>
                                 <asp:Label ID="Label4" runat="server" Text="lbl3" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Bal_Qty</asp:Label>
                                 <div class="col-sm-3">
                              <asp:TextBox ID="txtbalqty" runat="server" ReadOnly="true" CssClass="form-control" onkeypress="return isDecimalKey(event)" Width="100%" MaxLength="22"></asp:TextBox>                                            
                                        </div>
                             </div>
                            <div class="form-group" style="display:none;">
                                <asp:Label ID="Label18" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Item_Description</asp:Label>
                                 <div class="col-sm-4">
                              <asp:TextBox ID="txtitemdesc" runat="server" ReadOnly="true" CssClass="form-control" Width="100%" MaxLength="135"></asp:TextBox>                                            
                                        </div>
                                </div>

                            <div class="form-group">
                                 <asp:Label ID="Label16" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Ent_by</asp:Label>
                                 <div class="col-sm-4">
                              <asp:TextBox ID="txtent_by" runat="server" ReadOnly="true" CssClass="form-control" Width="100%" ></asp:TextBox>                                            
                                        </div>
                                 <asp:Label ID="Label17" runat="server" Text="lbl3" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Ent_Date</asp:Label>
                                 <div class="col-sm-3">
                              <asp:TextBox ID="txtent_dt" runat="server" ReadOnly="true" CssClass="form-control" Width="100%" ></asp:TextBox>                                            
                                        </div>
                             </div>

                           <div class="form-group" style="display:none;">
                                <asp:Label ID="Label3" runat="server" Text="lbl9" CssClass="col-sm-4 control-label" Font-Size="14px" Font-Bold="True">Project</asp:Label>
                                <div class="col-sm-8">
                                            <asp:TextBox ID="txtproject" runat="server" MaxLength="30" Width="100%" CssClass="form-control" ></asp:TextBox>
                                        </div>
                            </div>
                           <div class="form-group" style="display:none;">
                                <asp:Label ID="lbl9" runat="server" Text="lbl9" CssClass="col-sm-4 control-label" Font-Size="14px" Font-Bold="True">Type_of_Mag_Particles</asp:Label>
                                        <div class="col-sm-8">
                                            <asp:DropDownList ID="ddmagparti" runat="server" TabIndex="11" Width="100%" Height="35px"></asp:DropDownList>
                                            <%--<asp:TextBox ID="txtmagparti" runat="server"  EnableViewState="true" Width="100%" CssClass="form-control" ></asp:TextBox>--%>
                                        </div>
                            </div>
                            
                        </div></div></div>

                <div class="col-md-6" style="display:none;">
                    <div>
                        <div class="box-body"> 
                                
                            <div class="form-group">
                                <asp:Label ID="lbl103" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Mag_Technique</asp:Label>
                                <div class="col-sm-3">
                                        <%--<asp:TextBox ID="txtlbl103" runat="server"  ReadOnly="false" CssClass="form-control"></asp:TextBox>--%>                                    
                                    <asp:DropDownList ID="ddmagntech" runat="server" TabIndex="11" Width="100%" Height="35px"></asp:DropDownList>                                
                                        </div>
                            <%--</div>
                            <div class="form-group">--%>
                                <asp:Label ID="lbl104" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Method_Of_Powder</asp:Label>
                                <div class="col-sm-3">
                                        <%--<asp:TextBox ID="txtlbl104" runat="server"  ReadOnly="false" CssClass="form-control"></asp:TextBox>--%>
                                    <asp:DropDownList ID="ddmthodpoder" runat="server" TabIndex="11" Width="100%" Height="35px"></asp:DropDownList>
                                        </div>
                            </div>
                           
                            <div class="form-group">
                                <asp:Label ID="Label12" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Surface_Preparation</asp:Label>
                                <div class="col-sm-3">
                                        <asp:DropDownList ID="ddsurfprep2" runat="server" TabIndex="11" Width="100%" Height="35px"></asp:DropDownList>
                                        </div>
                            <%--</div>
                            <div class="form-group">--%>
                                <asp:Label ID="Label13" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Mag_Adequacy_Check</asp:Label>
                                <div class="col-sm-3">
                                        <asp:DropDownList ID="ddmagcheck" runat="server" TabIndex="11" Width="100%" Height="35px"></asp:DropDownList>
                                        </div>
                            </div>
                            <div class="form-group">
                                    <asp:Label ID="Label7" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Procedure_Ref.</asp:Label>
                                    <div class="col-sm-3">                                        
                                        <asp:TextBox ID="txtprocref" runat="server" MaxLength="60" ReadOnly="false" CssClass="form-control"></asp:TextBox>
                                    </div>

                               <%-- </div>                                                    
                            <div class="form-group">--%>
                                <asp:Label ID="lbl102" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Material_Spec.</asp:Label>
                                <div class="col-sm-3">
                                        <asp:TextBox ID="txtmatlspec" runat="server" MaxLength="100" ReadOnly="false" CssClass="form-control"></asp:TextBox>
                                        </div>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="lbl10" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Magn_Current_type</asp:Label>
                                <div class="col-sm-9">
                                    <asp:DropDownList ID="ddmagntype" runat="server" TabIndex="11" Width="100%" Height="35px"></asp:DropDownList>
                                        <%--<asp:TextBox ID="txtmagntype" runat="server"  ReadOnly="false" CssClass="form-control"></asp:TextBox>--%>
                                        </div>
                            </div>
                        </div></div></div>

                <div class="col-md-6" style="display:none;">
                    <div>
                        <div class="box-body">
                             <div class="form-group">
                                <asp:Label ID="Label8" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Acceptance_Standard</asp:Label>
                                <div class="col-sm-3">
                                    <asp:DropDownList ID="ddacptstand" runat="server" TabIndex="11" Width="100%" Height="35px"></asp:DropDownList>
                                </div>
                                 <%--</div>
                             <div class="form-group">--%>
                                <asp:Label ID="Label5" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Item</asp:Label>
                                <div class="col-sm-3">
                                    <asp:DropDownList ID="dditem" runat="server" TabIndex="11" Width="100%" Height="35px"></asp:DropDownList>
                                </div>                                
                            </div>
                            <div class="form-group">
                                <asp:Label ID="Label9" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Demagnetization</asp:Label>
                                <div class="col-sm-3">
                                        <asp:DropDownList ID="dddemag" runat="server" TabIndex="11" Width="100%" Height="35px"></asp:DropDownList>
                                        </div>
                            <%--</div>
                            <div class="form-group">--%>
                                <asp:Label ID="Label10" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Post_Cleaning</asp:Label>
                                <div class="col-sm-3">
                                        <asp:DropDownList ID="ddpostclean" runat="server" TabIndex="11" Width="100%" Height="35px"></asp:DropDownList>
                                        </div>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="lbl12" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Lighting_Equipment</asp:Label>
                                <div class="col-sm-3">
                                        <asp:TextBox ID="txtlightequip" runat="server" MaxLength="100" ReadOnly="false" CssClass="form-control"></asp:TextBox>
                                        </div>
                            <%--</div>
                            <div class="form-group">--%>
                                <asp:Label ID="Label6" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Job_Thickness(MM)</asp:Label>
                                <div class="col-sm-3">
                                        <asp:TextBox ID="txtjbthick" runat="server" MaxLength="60" ReadOnly="false" CssClass="form-control"></asp:TextBox>
                                        </div>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="lbl11" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Lifting_Power_Of_Yoke</asp:Label>
                                <div class="col-sm-9">
                                    <asp:DropDownList ID="ddlift_pwer" runat="server" TabIndex="11" Width="100%" Height="35px"></asp:DropDownList>
                                        <%--<asp:TextBox ID="txtlift_pwer" runat="server"  ReadOnly="false" CssClass="form-control"></asp:TextBox>--%>
                                        </div>
                            </div>

                        </div></div></div>

                <section class="col-lg-12 connectedSortable">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">
                                <li><a href="#DescTab" id="tab1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab">Details</a></li>
                                <li><a href="#DescTab2" id="tab2" runat="server" aria-controls="DescTab2" role="tab" data-toggle="tab">Comm.Terms</a></li>
                                <li><a href="#DescTab3" id="tab3" runat="server" aria-controls="DescTab3" role="tab" data-toggle="tab">Reel/Lot Dtl</a></li>
                                <li><a href="#DescTab4" id="tab4" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Lot.Dtl</a></li>
                                <li><a href="#DescTab5" id="tab5" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Inv.Dtl</a></li>
                                <li><a href="#DescTab6" id="tab6" runat="server" aria-controls="DescTab6" role="tab" data-toggle="tab">UDF Data</a></li>
                            </ul>

                            <div class="tab-content" >
                                <div role="tabpanel" class="tab-pane active" id="DescTab">
                                    <%--<div class="lbBody" id="gridDiv" style="color: White; height: 300px; overflow: hidden; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <fin:CoolGridView ID="sg1" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="1500px" Font-Size ="13px" 
                                            AutoGenerateColumns="False" OnRowDataBound="sg1_RowDataBound"
                                            OnRowCommand="sg1_RowCommand">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />--%>
                                    <div class="lbBody" id="gridDiv" style="color: White; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
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

                                                <asp:TemplateField ItemStyle-Width="5px">
                                                    <HeaderTemplate>Add</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btnadd" runat="server" CommandName="SG1_ROW_ADD" ImageAlign="Middle" ImageUrl="../tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert MRR" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Del</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btnrmv" runat="server" CommandName="SG1_RMV" ImageUrl="../tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove MRR" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="sg1_srno" HeaderText="SrNo" ItemStyle-Width="80px" />
                                                <asp:BoundField DataField="sg1_f1" HeaderText="Party_Code"  ItemStyle-Width="80px" />
                                                <asp:BoundField DataField="sg1_f2" HeaderText="Party_Name" ItemStyle-Width="180px" />
                                                <asp:BoundField DataField="sg1_f3" HeaderText="Bill_No" ItemStyle-Width="80px"/>
                                                <asp:BoundField DataField="sg1_f4" HeaderText="Bill_Date"  ItemStyle-Width="80px" />
                                                <asp:BoundField DataField="sg1_f5" HeaderText="Net_amount" ItemStyle-Width="80px" Visible="false"/>
                                                <asp:BoundField DataField="sg1_f6" HeaderText="QtyIssue" Visible="false" />
                                                 
                                                <asp:TemplateField ItemStyle-Width="15px" Visible="false">
                                                            <HeaderTemplate>Qty(Kgs)</HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="sg1_t1" runat="server" Text='<%#Eval("sg1_t1") %>' onChange="caltotalvalue()" Width="100%" MaxLength="22" ReadOnly="false"></asp:TextBox>
                                                                  </ItemTemplate>
                                                        </asp:TemplateField>

                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>CIF_Value</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t2" runat="server" Text='<%#Eval("sg1_t2") %>' Width="100%" MaxLength="22" onChange="caltotalvalue()" ReadOnly="false"></asp:TextBox>                                                   
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                               
                                                <asp:TemplateField ItemStyle-Width="110px" Visible="false">
                                                    <HeaderTemplate>CIF_Value_As_Per_BE</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t3" runat="server" Text='<%#Eval("sg1_t3") %>' onkeypress="return isDecimalKey(event)" Width="100%" MaxLength="22" ReadOnly="false"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>Duty_Save</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t4" runat="server" Text='<%#Eval("sg1_t4") %>' onkeypress="return isDecimalKey(event)"  Width="100%" maxlength="27" ></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>Balance_Qty</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t5" runat="server" Text='<%#Eval("sg1_t5") %>' onkeypress="return isDecimalKey(event)" Width="100%" maxlength="27" ReadOnly="true" ></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false" >
                                                    <HeaderTemplate>Balance_Value</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t6" runat="server" Text='<%#Eval("sg1_t6") %>' onkeypress="return isDecimalKey(event)" Width="100%" maxlength="27" ReadOnly="true" ></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>Remarks</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t7" runat="server" Text='<%#Eval("sg1_t7") %>' Width="100%" maxlength="30"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>abcd</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t8" runat="server" Text='<%#Eval("sg1_t8") %>' Width="100%" onkeypress="return isDecimalKey(event)" maxlength="7" ></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField >
                                                    <HeaderTemplate>Legal</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="sg1_t9" runat="server" Width="100%" >
                                                            <asp:ListItem Text="N" Value="N"></asp:ListItem>
                                                            <asp:ListItem Text="PLEASE SELECT" Value="PLEASE SELECT"></asp:ListItem>
                                                            <asp:ListItem Text="Y" Value="Y"></asp:ListItem>
                                                            <asp:ListItem Text="N" Value="N"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:HiddenField ID="cmd" runat="server" Value='<%#Eval("sg1_t9") %>' />
                                                        <%--<asp:TextBox ID="sg1_t11" runat="server" Text='<%#Eval("sg1_t11") %>' Width="100%" onkeypress="return isDecimalKey(event)" maxlength="20" ></asp:TextBox>--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField >
                                                    <HeaderTemplate>Dossier</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="sg1_t10" runat="server" Width="100%" >
                                                            <asp:ListItem Text="N" Value="N"></asp:ListItem>
                                                            <asp:ListItem Text="PLEASE SELECT" Value="PLEASE SELECT"></asp:ListItem>
                                                            <asp:ListItem Text="Y" Value="Y"></asp:ListItem>
                                                            <asp:ListItem Text="N" Value="N"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:HiddenField ID="cmd1" runat="server" Value='<%#Eval("sg1_t10") %>' />
                                                        <%--<asp:TextBox ID="sg1_t11" runat="server" Text='<%#Eval("sg1_t11") %>' Width="100%" onkeypress="return isDecimalKey(event)" maxlength="20" ></asp:TextBox>--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField >
                                                    <HeaderTemplate>BG</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="sg1_t11" runat="server" Width="100%" >
                                                            <asp:ListItem Text="N" Value="N"></asp:ListItem>
                                                            <asp:ListItem Text="PLEASE SELECT" Value="PLEASE SELECT"></asp:ListItem>
                                                            <asp:ListItem Text="Y" Value="Y"></asp:ListItem>
                                                            <asp:ListItem Text="N" Value="N"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:HiddenField ID="cmd2" runat="server" Value='<%#Eval("sg1_t11") %>' />
                                                        <%--<asp:TextBox ID="sg1_t11" runat="server" Text='<%#Eval("sg1_t11") %>' Width="100%" onkeypress="return isDecimalKey(event)" maxlength="20" ></asp:TextBox>--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>

                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#1797C0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader2" />
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
                 <div class="col-md-12" style="display:none;">
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

    <script>
        function caltotalvalue() {
            var rowTot = 0;
            var colTot = 0;
            var qty1 = 0;
            var qty2 = 0;
            var qty3 = 0;
            var val1 = 0; var val2 = 0; var val3 = 0;

            var grid = document.getElementById("<%= sg1.ClientID%>");
                for (var i = 0; i < grid.rows.length - 1; i++) {
                    colTot += fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg1_sg1_t1_' + i).value));                    
                    //row total is total of total_qty field row wise txtcurrqty txtcurrval
                    document.getElementById('ContentPlaceHolder1_txtlbl3').value = fill_zero(colTot);
                    qty1 = fill_zero(Number(document.getElementById('ContentPlaceHolder1_txtlbl3').value));
                    qty2 = fill_zero(Number(document.getElementById('ContentPlaceHolder1_txtcurrqty').value));
                    qty3 = qty2 - qty1;
                    document.getElementById('ContentPlaceHolder1_sg1_sg1_t5_' + i).value = fill_zero(qty3);
                    document.getElementById('ContentPlaceHolder1_txtbalqty').value = fill_zero(qty3);
                    //alert(val1);
                    val1 += fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg1_sg1_t2_' + i).value));
                    //alert(val1);
                    val2 = fill_zero(Number(document.getElementById('ContentPlaceHolder1_txtcurrval').value));
                    val3 = val2 - val1;
                    //alert(val3);
                    document.getElementById('ContentPlaceHolder1_sg1_sg1_t6_' + i).value = fill_zero(val3);
                }             
            }
           <%-- function fill_zero(val) { if (isNaN(val)) return 0; if (isFinite(val)) return val; }
            function caltotalvalue1() {
                var rowTot = 0;
                var colTot = 0;
                var min = 0;
                var grid = document.getElementById("<%= sg1.ClientID%>");

            for (var i = 0; i < grid.rows.length - 1; i++) {

                colTot += fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg1_sg1_t8_' + i).value));
                document.getElementById('ContentPlaceHolder1_txtlbl9').value = fill_zero(colTot);

            }
        }--%>

    </script>
  
    <asp:HiddenField ID="TabName" runat="server" />

</asp:Content>
