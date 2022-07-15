<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="om_loan_req" Title="Tejaxo" CodeFile="om_loan_req.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style1 {
            font-family: Arial, Helvetica, sans-serif;
            font-weight: 700;
            color: #474646;
            font-size: 12px;
            width: 68px;
        }plete

        .auto-style2 {
            font-family: Arial, Helvetica, sans-serif;
            font-weight: 700;
            color: #474646;
            font-size: 12px;
            width: 485px;
        }

        .auto-style3 {
            width: 148px;
        }

        .auto-style4 {
            font-family: Arial, Helvetica, sans-serif;
            font-weight: 700;
            color: #474646;
            font-size: 12px;
            width: 148px;
        }
        .auto-style5 {
            width: 485px;
        }
            </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper">
        <section class="content-header">
            <table style="width: 100%">
                <tr>
                     <td>
                        <asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label>
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
                        <%-- <div class="form-group">                              
                                    <label id="lbl1" runat="server" class="col-sm-12 control-label" >Employee_Details</label>
                             </div>--%>

                                        
                          <div class="form-group"> 
                                <label id="lblno" runat="server" class="col-sm-2 control-label" >Entry_No</label>
                             <div class="col-sm-1">
                            <asp:Label ID="lbl1a" runat="server" Text="TC" Style="width: 22px; float: right;" CssClass="col-sm-1 control-label"></asp:Label>
                                    </div>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtvchnew" CssClass="form-control" runat="server" ReadOnly="true"  onfocus="Change(this, event)" Width="100%"></asp:TextBox>
                               </div>
                               
                                    <label id="lbldt" runat="server" class="col-sm-2 control-label" >Entry_Date</label>                             
                                <div class="col-sm-3">
                            <asp:TextBox ID="txtdate" runat="server" Width="100%" CssClass="form-control" onblur="Change(this, event)"
                                onfocus="Change(this, event)"></asp:TextBox>
                               </div>
                                </div>
                                                                                                                                                            
                                      <div class="form-group"> 
                                <label id="Label1" runat="server" class="col-sm-2 control-label" >Employee_Code</label>                             
                                           <div class="col-sm-1" id="divCocd" runat="server">
                                    <asp:ImageButton ID="btnCocd"  runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" Height="20px" OnClick="btnCocd_Click" />
                                </div>
                                <div class="col-sm-4">                                
                                    <asp:TextBox ID="txtcode" CssClass="form-control" runat="server" ReadOnly="true"  onfocus="Change(this, event)" Width="100%"></asp:TextBox>
                                 </div>
                            <label id="Label14" runat="server" class="col-sm-2 control-label" >Grade</label>                             
                          <div class="col-sm-3">                                
                                    <asp:TextBox ID="txtgrade" runat="server" ReadOnly="true" CssClass="form-control"
                                        onblur="Change(this, event)" onfocus="Change(this, event)" Width="100%"></asp:TextBox>
                                 </div>
                                      </div>
                          
                                            <div class="form-group"> 
                                <label id="Label3" runat="server" class="col-sm-3 control-label" >Name</label>                             
                                <div class="col-sm-4">                            
                            <asp:TextBox ID="txtname" runat="server" ReadOnly="true" Width="100%" CssClass="form-control"></asp:TextBox>
                                </div>
                                    <label id="lblsal" runat="server" class="col-sm-2 control-label" >Current_Salary</label>                             
                                <div class="col-sm-3">                                         
                                    <asp:TextBox ID="txtsalry"  CssClass="form-control" ReadOnly="true" runat="server" Width="100%" Style="text-align: right"></asp:TextBox>
                                 </div>   
                           </div>

                           <div class="form-group"> 
                                <label id="Label2" runat="server" class="col-sm-3 control-label" >Department</label>                             
                                <div class="col-sm-4">                                         
                                    <asp:TextBox ID="txtreason" CssClass="form-control" ReadOnly="true" runat="server" Width="100%"></asp:TextBox>
                                 </div>
                           
                                 <label id="Label4" runat="server" class="col-sm-2 control-label" >Date_of_Join</label>                             
                                <div class="col-sm-3">                            
                            <asp:TextBox ID="txtjoindt" ReadOnly="true" runat="server" Width="100%" CssClass="form-control"
                                onblur="Change(this, event)" onfocus="Change(this, event)"></asp:TextBox>
                                 </div>
                              </div>                              
                    </div>
                </div>
                </div>

                 <div class="col-md-6">
                    <div>
                        <div class="box-body">
                <%-- <div class="form-group">                              
                                    <label id="Label7" runat="server" class="col-sm-12 control-label" >Loan_Details</label>
                             </div>--%>
                                                        <div class="form-group" id="DivAdvance" runat="server"> 
                                <label id="Label7" runat="server" class="col-sm-2 control-label" >Surety By</label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnSurety" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" Height="20px" OnClick="btnSurety_Click" />
                                </div>                           
                                <div class="col-sm-2">                                         
                                    <asp:TextBox ID="txtSurety_Code"  CssClass="form-control" ReadOnly="true" runat="server" Width="105px"></asp:TextBox>
                                 </div>
                                 <div class="col-sm-7">                                         
                                    <asp:TextBox ID="txtSurety"  CssClass="form-control" ReadOnly="true" runat="server" Width="100%"></asp:TextBox>
                                 </div>                                                                         
                                     </div>

                         <div class="form-group"> 
                                <label id="lblamt" runat="server" class="col-sm-3 control-label" >Amount</label>                             
                                <div class="col-sm-3">                                         
                                    <asp:TextBox ID="txtamt" runat="server" CssClass="form-control" onkeyup="cal()" MaxLength="8" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                 </div>

                              <label id="Label8" runat="server" class="col-sm-3 control-label" >No_of_Install./Monthly</label>                             
                                <div class="col-sm-3">                                         
                                    <asp:TextBox ID="txtinst" runat="server" CssClass="form-control" onkeyup="cal()" MaxLength="3" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                 </div>
                              </div>

                            <div class="form-group"> 
                                <label id="lblamt1" runat="server" class="col-sm-3 control-label" >Amt_Per_Install./Monthly</label>                             
                                <div class="col-sm-3">                                         
                                    <asp:TextBox ID="txtamt1" MaxLength="8" CssClass="form-control" runat="server" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                 </div>

                                <label id="Label10" runat="server" class="col-sm-3 control-label"  >Installment_start_dt
                                </label>                             
                                <div class="col-sm-3">                                         
                                    <asp:TextBox ID="txtstartdt" runat="server" CssClass="form-control" Width="100%" Font-Size="Small"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtstartdt_CalendarExtender" runat="server"
                                                Enabled="True" TargetControlID="txtstartdt"
                                                Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="Maskedit1" runat="server" Mask="99/99/9999"
                                                MaskType="Date" TargetControlID="txtstartdt" />
                                 </div>
                                </div>

                                <div class="form-group" id="DivLoan" runat="server"> 
                                <label id="Label12" runat="server" class="col-sm-3 control-label" >Any_Running_Loan</label>                             
                                <div class="col-sm-9">                                         
                                    <asp:TextBox ID="txtrunningloan"  CssClass="form-control" Placeholder="YES/NO" OnTextChanged="txtrunningloan_TextChanged" AutoPostBack="true"  MaxLength="3" runat="server" Width="130px"></asp:TextBox>
                                 </div>                                                                         
                                     </div>

                        <div class="form-group" id="DivLoan1" runat="server"> 
                                     <label id="Label13" runat="server"  class="col-sm-3 control-label" >OutStand_Amt_as_on_dt</label>                             
                          
                                <div class="col-sm-3">                                         
                                    <asp:TextBox ID="txtosamt" CssClass="form-control" MaxLength="8" Placeholder="Oustanding Amount" runat="server" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                 </div>
                              <div class="col-sm-6">                          
                            <span style="font-size: 10px">(If yes only then enter outstanding amt)</span>   
                                      </div>
                                </div>                     
                    </div></div>               

                     </div>
             <div class="col-md-12">
                    <div>  
                           <div class="box-body">
                               <div class="form-group"> 
                                     <label id="Label11" runat="server" class="col-sm-12 control-label" >Reason</label>                             
                                <div class="col-sm-11">                                         
                                    <asp:TextBox ID="txtrmk" CssClass="form-control" MaxLength="50" runat="server" Width="100%"></asp:TextBox>
                                 </div>
                               </div>

                                 </div></div></div>


             <section class="col-lg-12 connectedSortable" id="AllTabs" runat="server">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">
                                <li><a href="#DescTab" id="tab1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab">List</a></li>
                            </ul>

                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane active" id="DescTab">
                                    <div class="lbBody" style="height: 250px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                        <asp:GridView ID="sg1" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="100%" Font-Size="13px">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>

                 
                <%--next div--%>
                  <div class="col-md-12">
                    <div>  
                           <div class="box-body" style="visibility:hidden;">   
                        
                         <%--<div class="form-group"> 
                                    <label id="Label5" runat="server" class="col-sm-3 control-label" >Entry Date</label>                             
                                <div class="col-sm-9">
                            <asp:TextBox ID="TextBox1" runat="server" Width="120px" CssClass="textboxStyle" onblur="Change(this, event)"
                                onfocus="Change(this, event)"></asp:TextBox>
                               </div>
                                </div>--%>  
                                                
                       <div class="form-group"> 
                           <label id="lbltskno" runat="server"  class="col-sm-2 control-label" >Task No</label>                                                        

                                <div class="col-sm-4">                              
                                    <asp:TextBox ID="txtvchnum" runat="server" ReadOnly="true"  CssClass="textboxStyle"
                                        onblur="Change(this, event)" onfocus="Change(this, event)" Width="50%"></asp:TextBox>
                                 </div>
                           <label id="Label5" runat="server" class="col-sm-2 control-label" >Task Date</label>
                                  <div class="col-sm-4"> 
                            <asp:TextBox ID="txtvchdate" runat="server" Width="120px" CssClass="textboxStyle"
                                ReadOnly="true"></asp:TextBox>
                               </div>
                           </div>

                               <div class="form-group"> 
                           <label id="lbluserid" runat="server" class="col-sm-2 control-label" >User ID:</label>                             
                                <div class="col-sm-10">    
                                      <asp:TextBox ID="txtuserid" runat="server" Width="100%" CssClass="textboxStyle" ReadOnly="true"
                                        onblur="Change(this, event)" onfocus="Change(this, event)"></asp:TextBox>
                                     </div>
                                    </div>
                                                          
                            <%--<asp:ImageButton ID="imguserid" runat="server"
        ImageUrl="~/images/Btn_addn.png" style="float:right" Width="18px" Height="18px" ToolTip="Created User's"
        onclick="imguserid_Click" />

        <asp:ImageButton ID="imguserid1" runat="server"
        ImageUrl="~/images/Btn_addn.png" style="float:right" Width="18px"
        Height="18px" ToolTip="ERP User's" onclick="imguserid1_Click"
        />--%>
                             
                               <div class="form-group"> 
                           <label id="lblsub" runat="server" class="col-sm-2 control-label" >Subject:</label>                             
                                <div class="col-sm-10">                           
                                    <asp:TextBox ID="txtsubject" runat="server" ReadOnly="true" Width="100%" CssClass="textboxStyle"
                                        onblur="Change(this, event)" onfocus="Change(this, event)"></asp:TextBox>
                                </div>
                             </div>

                                 <div class="form-group"> 
                           <label id="lblcc" runat="server" class="col-sm-2 control-label" >CC:</label>                             
                                <div class="col-sm-10">     
                                    <asp:TextBox ID="txtemailcc" runat="server" ReadOnly="true" Width="100%" CssClass="textboxStyle"
                                        onblur="Change(this, event)" onfocus="Change(this, event)"></asp:TextBox>
                                 </div>
                             </div>


                               <div class="form-group">
                            <label id="lblmesg" runat="server" class="col-sm-4 control-label" >Message:</label>      
                               <label id="Label9" runat="server" class="col-sm-1 control-label" >Task Date:</label> 
                                 <div class="col-sm-1"> 
                            <asp:TextBox ID="txttskdate" runat="server" Width="100px" ReadOnly="true"></asp:TextBox>
                                </div>                                
                                    <label id="Label6" runat="server" class="col-sm-2 control-label" >Priority</label> 
                                 <div class="col-sm-4"> 
                            <asp:TextBox ID="txtdrop" ReadOnly="true" runat="server" Width="120px"></asp:TextBox>
                                        </div>     
                                      </div>     
                                    <%--<asp:DropDownList ID="ddl1" runat="server" >
<asp:ListItem Enabled="true" Text="Medium" Value="0"></asp:ListItem>
<asp:ListItem Text="High" Value="1"></asp:ListItem>
</asp:DropDownList>--%>
                                <div class="form-group">
                               <div class="col-sm-12"> 
                                    <asp:TextBox ID="txtmsg" runat="server" ReadOnly="true" TextMode="MultiLine" Height="200px"
                                        Width="99%" CssClass="textboxStyle" onblur="Change(this, event)" onfocus="Change(this, event)"></asp:TextBox>
                                </div>
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
    <asp:HiddenField ID="edmode" runat="server" />
    <asp:HiddenField ID="doc_nf" runat="server" />
    <asp:HiddenField ID="doc_df" runat="server" />
    <asp:HiddenField ID="doc_vty" runat="server" />
    <asp:HiddenField ID="doc_addl" runat="server" />
    <asp:HiddenField ID="hffromdt" runat="server" />
    <asp:HiddenField ID="hftodt" runat="server" />
    <asp:HiddenField ID="hf1" runat="server" />
    <asp:HiddenField ID="hffirst" runat="server" />
       <asp:HiddenField ID="deptt" runat="server" />

        <script type="text/javascript">
        $(function () {
            var tabName = $("[id*=TabName]").val() != "" ? $("[id*=TabName]").val() : "DescTab";
            $('#Tabs a[href="#' + tabName + '"]').tab('show');
            $("#Tabs a").click(function () {
                $("[id*=TabName]").val($(this).attr("href").replace("#", ""));
            });
        });

        function cal() {
            var inst = 0;        
            var loan = 0;
            var amt = 0;
            var amt1 = 0;         
            loan = fill_zero(document.getElementById('ContentPlaceHolder1_txtamt').value) * 1;
            inst = fill_zero(document.getElementById('ContentPlaceHolder1_txtinst').value) * 1;
            if(inst>0)
            {             
                amt = loan / inst;
                amt1 = amt.toFixed(2);
            }
                document.getElementById('ContentPlaceHolder1_txtamt1').value = fill_zero(amt1);           
        }
        function fill_zero(val) { if (isNaN(val)) return 0; if (isFinite(val)) return val; }

    </script>
</asp:Content>
