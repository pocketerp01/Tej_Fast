<%@ Page Language="C#"MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="om_cmplnt" Title="Tejaxo" CodeFile="om_cmplnt.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 500px;
        }
        .style2
        {
            width: 100px;
        }        
    </style>
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

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

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
                                 <div class="col-sm-2">
                             <asp:Label ID="tdinvoice" runat="server" Text="lbl1" Font-Size="14px" Font-Bold="True">Invoice_No.</asp:Label>
                                <%-- <span id="tdinvoice" runat="server">Invoice_No.</span>--%>
                                     </div>                                           
                                  <div class="col-sm-1">
                                    <asp:Label ID="lbl1a" runat="server" Text="TC" Style="width: 100%; float: right;" CssClass="col-sm-1 control-label"></asp:Label>
                                </div>
                                 <div class="col-sm-1">
                                    <asp:ImageButton ID="btninvno" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btninvno_Click" />
                                 </div>
                              <div class="col-sm-2">
                                    <asp:TextBox ID="txtinvno" runat="server" CssClass="form-control"  Width="100%"></asp:TextBox>
                                  </div>                             
                                 <asp:Label ID="Label1" runat="server" Text="lbl1" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Date</asp:Label>
                                    <div class="col-sm-4">
                                            <asp:TextBox ID="txtinvdate" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                          <span id="spnjobno" runat="server">Job No.</span>
                                    <asp:TextBox ID="txtjobno" runat="server" ReadOnly="true" Width="70px" Placeholder="Job No."></asp:TextBox>
                                        </div>
                                  </div>
                            <div class="form-group" id="DivParty" runat="server">
                              <div class="form-group">                                             
                                 <asp:Label ID="Label2" runat="server" Text="lbl1" CssClass="col-sm-4 control-label" Font-Size="14px" Font-Bold="True">Party</asp:Label>
                                  <div class="col-sm-2">
                                       <asp:TextBox ID="txtacode"  CssClass="form-control" runat="server"  ReadOnly="True" Width="100%"></asp:TextBox>
                                      </div>
                                       <div class="col-sm-6">
                                       <asp:TextBox ID="txtaname" CssClass="form-control" runat="server"  ReadOnly="True" Width="100%"></asp:TextBox>
                                           </div>                                  
                              </div>
                            </div>

                               <div class="form-group">   
                                     <asp:Label ID="Label3" runat="server" Text="lbl1" CssClass="col-sm-4 control-label" Font-Size="14px" Font-Bold="True">Item Name</asp:Label>
                                   <div class="col-sm-2">
                                         <asp:TextBox ID="txticode" CssClass="form-control" runat="server"  ReadOnly="True" Width="100%"></asp:TextBox>
                                         </div>
                                       <div class="col-sm-6">
                                    <asp:TextBox ID="txtiname" runat="server" CssClass="form-control"  ReadOnly="True" Width="100%"></asp:TextBox>
                                           </div>
                                   </div>
                             <div class="form-group" id="DivAddress" runat="server">
                                <label class="col-sm-4 control-label">Address</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtPaddr" runat="server" placeholder="Address"
                                        ReadOnly="True" CssClass="form-control" Height="32px"></asp:TextBox>
                                </div>
                            </div>
                              <div class="form-group">  
                                   <asp:Label ID="tdbatch1" runat="server" Text="lbl1" CssClass="col-sm-4 control-label" Font-Size="14px" Font-Bold="True">Batch No</asp:Label>
                                   <div class="col-sm-2">
                                        <asp:TextBox ID="txtinvbtch" CssClass="form-control" runat="server" ReadOnly="true" Width="100%"></asp:TextBox>
                                       </div>
                                   <asp:Label ID="Label5" runat="server" Text="lbl1" CssClass="col-sm-1 control-label" Font-Size="14px" Font-Bold="True">Qty</asp:Label>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtinvqty" runat="server" CssClass="form-control" ReadOnly="true" Width="100%"></asp:TextBox>
                                        </div>
                                    <asp:Label ID="Label6" runat="server" Text="lbl1" CssClass="col-sm-1 control-label" Font-Size="14px" Font-Bold="True">PMFG</asp:Label>
                                  <div class="col-sm-2">
                                       <asp:TextBox ID="txtpmrg" runat="server" CssClass="form-control" onkeypress="return isDecimalKey(event)" MaxLength="40" Width="100%"></asp:TextBox>
                                  </div>
                                  </div>

                                <div class="form-group">  
                                  <asp:Label ID="tdtechnicalper" runat="server" Text="lbl1" CssClass="col-sm-4 control-label" Font-Size="14px" Font-Bold="True">Tech. Person</asp:Label>
                                    <div class="col1-sm-8">
                                         <asp:TextBox ID="txttechper" runat="server"  CssClass="form-control" Width="200px" MaxLength="50"></asp:TextBox>
                                    </div>
                                    </div>
                            
                            <div class="form-group" runat="server" id="SEL1">
                                <label class="col-sm-3 control-label" id="lblGur" runat="server">Guaranty/WarrantyStatus</label>
                                <asp:Label ID="lblGRStatus" runat="server" CssClass="col-sm-9 control-label" Font-Bold="true" ForeColor="Red"></asp:Label>
                            </div>
                          </div>
                       </div>
                     </div>

                       <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">                                             
                                 <asp:Label ID="tdcomplaint" runat="server" Text="lbl1" CssClass="col-sm-4 control-label" Font-Size="14px" Font-Bold="True">Complaint No.</asp:Label>                                  
                               
                              <div class="col-sm-3">
                                   <asp:TextBox ID="txtvchnum" runat="server" CssClass="form-control"  MaxLength="6" ReadOnly="True" Width="100%"></asp:TextBox>  
                                  </div>                             
                                 <asp:Label ID="Label4" runat="server" Text="lbl2" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Date</asp:Label>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtvchdate" runat="server" Width="100%"  CssClass="form-control"></asp:TextBox>
                <asp:CalendarExtender ID="txtvchdate_CalendarExtender" runat="server" Enabled="True" TargetControlID="txtvchdate"  Format="dd/MM/yyyy">
            </asp:CalendarExtender>
            <asp:MaskedEditExtender ID="Maskedit1" runat="server" Mask="99/99/9999" MaskType="Date" TargetControlID="txtvchdate" />                                        
                                        </div>
                                 </div>                     

                               <div class="form-group">           
                                <asp:Label ID="tdtypcomplaint" runat="server" Text="lbl3" CssClass="col-sm-4 control-label" Font-Size="14px" Font-Bold="True">Type of Complaint</asp:Label>                                  
                                     <div class="col-sm-8">
                                          <asp:DropDownList ID="ddntrofcmlnt" CssClass="form-control" runat="server" Width="100%"></asp:DropDownList>
                                         </div>
                                   </div>
                            
                               <div class="form-group">           
                                <asp:Label ID="tdnaturcomplaint" runat="server" Text="lbl3" CssClass="col-sm-4 control-label" Font-Size="14px" Font-Bold="True">Nature of Complaint</asp:Label>                                  
                                     <div class="col-sm-8">
                                         <asp:TextBox ID="txtntrcmpln" runat="server" Width="100%" MaxLength="50" CssClass="form-control"></asp:TextBox>
                                         </div>
                                   </div>
      
                             <div class="form-group">  
                                     <asp:Label ID="tddivision" runat="server" Text="lbl3" CssClass="col-sm-4 control-label" Font-Size="14px" Font-Bold="True">Division of Complaint</asp:Label>                                  
                                  <div class="col-sm-8">
                                      <asp:DropDownList ID="dddivisioncmltn" CssClass="form-control" runat="server" Width="100%"></asp:DropDownList>
                                      </div>
                                 </div>

                            <div class="form-group" runat="server" id="SEL2">
                                <label class="col-sm-4 control-label">Guaranty/Warranty Terms</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtGur" runat="server" ReadOnly="True" CssClass="form-control" Height="32px"></asp:TextBox>
                                </div>
                                <label class="col-sm-2 control-label">Date</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtGurDate" runat="server" CssClass="form-control" Height="32px"></asp:TextBox>
                                </div>
                            </div>
                              </div>
                          </div>
                       </div>        


  <section class="col-lg-12 connectedSortable">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">
                                <li><a href="#DescTab" id="tab1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab">Complaint Details</a></li>
                                 <li><a href="#DescTab2" id="tab2" runat="server" aria-controls="DescTab2" role="tab" data-toggle="tab">Other Details</a></li>
                            </ul>
                            <div class="tab-content" >
                                <div role="tabpanel" class="tab-pane active" id="DescTab">
                  <div class="lbBody" style="color:White; max-height:300px; overflow:auto;  box-shadow:0 2px 4px rgba(127,127,127,.3);box-shadow:inset 0 0 3px #387bbe,0 0 9px #387bbe;">
        <fin:CoolGridView ID="sg1" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="100%" Height="280px" Font-Size="13px"
                                            AutoGenerateColumns="False" OnRowDataBound="sg1_RowDataBound" OnRowCommand="sg1_RowCommand">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
             <asp:BoundField DataField="sg1_h1" HeaderText="sg1_h1"  />
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
                                                <asp:BoundField DataField="sg1_f2" HeaderText="Icode" Visible="false"  />
                                                <asp:BoundField DataField="sg1_f3" HeaderText="ERPcode" Visible="false"/>
                                                <asp:BoundField DataField="sg1_f4" HeaderText="Name" Visible="false"/>
                                                <asp:BoundField DataField="sg1_f5" HeaderText="Unit" Visible="false"/>
                                                <asp:BoundField DataField="sg1_f6" HeaderText="QtyIssue" Visible="false" />        
                                                    <asp:TemplateField HeaderStyle-Width="850px" >
                                                  <HeaderTemplate>Remarks</HeaderTemplate>
                                                 <ItemTemplate>
                                                  <asp:TextBox ID="sg1_t1" runat="server"  Text='<%#Eval("sg1_t1") %>' Width="100%" MaxLength="175"></asp:TextBox>
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
                                                        <asp:TextBox ID="sg1_t3" Visible="false" runat="server" Text='<%#Eval("sg1_t3") %>' Width="100%"  MaxLength="30"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Job_Description</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t4" Visible="false" runat="server" Text='<%#Eval("sg1_t4") %>' Width="100%" MaxLength="100" ></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Size_Of_Indication</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t5" Visible="false" runat="server" Text='<%#Eval("sg1_t5") %>'  Width="100%" MaxLength="50"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Interpretation</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t6" Visible="false" runat="server" Text='<%#Eval("sg1_t6") %>' Width="100%" MaxLength="30" ></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Remarks</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t7" Visible="false" runat="server" Text='<%#Eval("sg1_t7") %>'  Width="100%" MaxLength="30"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate></HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t8"  Visible="false" runat="server" Text='<%#Eval("sg1_t8") %>' Width="100%" ></asp:TextBox>
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

           <div class="col-md-12"  id="trextraval" runat="server" >
                    <div>
                        <div class="box-body"> 
                            <div class="form-group">     
                                 <asp:Label ID="lbltpt" runat="server" Text="lbl1" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Tpt_Amt</asp:Label>
                                  <div class="col-sm-2">
                                       <asp:TextBox ID="txttpt" runat="server" CssClass="form-control"  ReadOnly="True" Width="70px"></asp:TextBox>
                                      </div>
                                    <asp:Label ID="lblloding" runat="server" Text="lbl4" CssClass="col-sm-1 control-label" Font-Size="14px" Font-Bold="True">Lodging_Amt</asp:Label>
                                <div class="col-sm-1">
                                    <asp:TextBox ID="txtlodging" runat="server" CssClass="form-control" Width="80px" style="text-align:right;"  onkeyup="calculateSum();" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false"></asp:TextBox>
                                </div>
                                <asp:Label ID="lblfooding" runat="server" Text="lbl5" CssClass="col-sm-1 control-label" Font-Size="14px" Font-Bold="True">Fooding_Amt</asp:Label>
                                <div class="col-sm-1">
                                    <asp:TextBox ID="txtfooding" runat="server" CssClass="form-control" Width="80px" style="text-align:right;" onkeyup="calculateSum();"></asp:TextBox>
                                </div>
                                <asp:Label ID="lblmisc" runat="server" Text="lbl6" CssClass="col-sm-1 control-label" Font-Size="14px" Font-Bold="True">Misc_Amt</asp:Label>
                                <div class="col-sm-1">
                                     <asp:TextBox ID="txtmisc" runat="server" CssClass="form-control" Width="80px"  style="text-align:right;"  onkeyup="calculateSum();" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false"></asp:TextBox>
                                </div>
                                 <asp:Label ID="lbltot" runat="server" Text="lbl6" CssClass="col-sm-1 control-label" Font-Size="14px" Font-Bold="True">Total_Amt</asp:Label>
                                <div class="col-sm-1">
                                     <asp:TextBox ID="txttot" runat="server" CssClass="form-control" Width="80px"  style="text-align:right;"  ReadOnly="true" ></asp:TextBox>
                                </div>
                                     </div>
                                </div>
                         </div>
                </div>

           <div class="col-md-12">
                    <div>
                        <div class="box-body">
                            <div class="form-group">    
                              <asp:Label ID="Label11" runat="server" Text="lbl6" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Remarks</asp:Label>
                                 <div class="col-sm-10">
                                     <asp:TextBox ID="txtrmk" runat="server" MaxLength="125" TextMode="MultiLine" Width="100%"></asp:TextBox>
                                </div>
                                </div>

                                     </div>
                                </div>
                         </div> 
                </div>      
                
                </section></div>                                                          
            
<asp:Button ID="btnhideF" runat="server" onclick="btnhideF_Click" style="display:none" />
<asp:Button ID="btnhideF_s" runat="server" OnClick="btnhideF_s_Click" style="display:none" />
<asp:HiddenField ID="hffield" runat="server" />
        <asp:HiddenField ID="doc_nf" runat="server" />
    <asp:HiddenField ID="doc_df" runat="server" />
     <asp:HiddenField ID="hf1" runat="server" />
    <asp:HiddenField ID="doc_vty" runat="server" />
    <asp:HiddenField ID="doc_addl" runat="server" />
        <asp:HiddenField ID="hfGridView1SV" runat="server" />
    <asp:HiddenField ID="hfGridView1SH" runat="server" />
<asp:HiddenField ID="edmode" runat="server" />
      </div>
      </div>
</asp:Content>