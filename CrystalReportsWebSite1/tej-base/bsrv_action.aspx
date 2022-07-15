<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="bsrv_action" CodeFile="bsrv_action.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .hidden {
            display: none;
        }
    </style>

     <script type="text/javascript">
         function openfileDialog() {
             $("#Attch").click();
         }
         function submitFile() {
             $("#<%= btnAtt.ClientID%>").click();
        };
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
   <%-- <div class="bSubBlock brandSecondaryBrd secondaryPalette" style="background-image: url('css/images/bgTop.gif');">--%>
     <div class="content-wrapper">
     <%--   <table width="100%">
            <tr>
                <td>
                    <div align="left" style="color: #1797c0; background-image: url(images/bgTop.gif);
                        font-size: medium; font-weight: bold; width: 180px;">
                        &nbsp;<asp:Image ID="Image1" runat="server" Height="24px" ImageUrl="~/css/images/transfer.jpg"
                            Width="28px" />
                        &nbsp;
                        <asp:Label ID="lblheader" runat="server"></asp:Label>
                    </div>
                </td>
                <td>
                    <button id="btnnew" runat="server" accesskey="N" onserverclick="btnnew_ServerClick" class="myButton"><u>N</u>ew</button>
                    <button id="btnedit" runat="server" accesskey="i" class="myButton" onserverclick="btnedit_ServerClick">Ed<u>i</u>t</button>
                    <button id="btnsave" runat="server" accesskey="S" class="myButton" onserverclick="btnsave_ServerClick"><u>S</u>ave</button>
                    <button id="btndel" runat="server" accesskey="l" class="myButton" onserverclick="btndel_ServerClick">De<u>l</u>ete</button>
                    <button id="btnlist" runat="server" accesskey="t" class="myButton" onserverclick="btnlist_ServerClick"> Lis<u>t</u></button>
                    <button id="btnprint" runat="server" accesskey="P" class="myButton" onserverclick="btnprint_ServerClick"><u>P</u>rint</button>
                    <button id="btncan" runat="server" accesskey="c" class="myButton" onserverclick="btncan_ServerClick"><u>C</u>ancel</button>
                    <button id="btnext" runat="server" accesskey="x" class="myButton" onserverclick="btnext_ServerClick">E<u>x</u>it</button>
                </td>
            </tr>
        </table>--%>
         <section class="content-header">
            <table style="width: 100%">
                <tr>
                        <td style="text-align: left">
                        <button type="submit" id="btnnew" class="btn btn-info" style="width: 100px;" runat="server" accesskey="N" onserverclick="btnnew_ServerClick"><u>N</u>ew</button>
                        <button type="submit" id="btnedit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="i" onserverclick="btnedit_ServerClick">Ed<u>i</u>t</button>
                        <button type="submit" id="btnsave" class="btn btn-info" style="width: 100px;" runat="server" accesskey="s" onserverclick="btnsave_ServerClick"><u>S</u>ave</button>
                        <button type="submit" id="btnprint" class="btn btn-info" style="width: 100px;" runat="server" accesskey="P" onserverclick="btnprint_ServerClick"><u>P</u>rint</button>
                        <button type="submit" id="btndel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="l" onserverclick="btndel_ServerClick">De<u>l</u>ete</button>
                        <button type="submit" id="btnlist" class="btn btn-info" style="width: 100px;" runat="server" accesskey="t" onserverclick="btnlist_ServerClick">Lis<u>t</u></button>
                        <button type="submit" id="btncancel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="c" onserverclick="btncancel_ServerClick"><u>C</u>ancel</button>
                        <button type="submit" id="btnexit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="X" onserverclick="btnexit_ServerClick">E<u>x</u>it</button>                        
                    </td>
                    <td>
                        <asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label></td>   
                </tr>
            </table>
        </section>

            <section class="content">
            <div class="row">

                 <div class="col-md-12" id ="tab_bsrvreq" runat="server">
                    <div>                
                        <div class="box-body">
                              <div class="form-group">
                                <asp:Label ID="lbl1" runat="server" Text="lbl1" CssClass="col-sm-1 control-label" Font-Size="14px" Font-Bold="True"> Srv Req No.</asp:Label>
                                <div class="col-sm-1">
                                    <asp:Label ID="lbl1a" runat="server"  Style="width: 22px; float: right;" CssClass="col-sm-1 control-label"></asp:Label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtvchnum" runat="server" Width="100%" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label18" runat="server" Text="lbl1"  CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Date</asp:Label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtvchdate" placeholder="Date"  runat="server" Width="100%" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender7" runat="server" Enabled="True" TargetControlID="txtvchdate" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="MaskedEditExtender10" runat="server" Mask="99/99/9999" MaskType="Date" TargetControlID="txtvchdate" />
                                </div>
                            </div>

                              <div class="form-group">
                                <asp:Label ID="Label19" runat="server" Text="lbl3" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Customer_Name</asp:Label>
                                 <div class="col-sm-1" style="display:none;">
                               <asp:ImageButton ID="btnparty" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnparty_Click" />
                                </div>
                                <div class="col-sm-1">
                                     <asp:TextBox ID="txtacode"  ReadOnly="true" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                                    </div>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtaname" ReadOnly="true" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                                </div>
                            
                                <div class="col-sm-2">
                                     <asp:DropDownList ID="dd_list1" runat="server" CssClass="form-control">
                                                <asp:ListItem Value="0" Text="Prv.Maint"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="Service"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="Service + Complaint"></asp:ListItem>
                                                <asp:ListItem Value="3" Text="Others"></asp:ListItem>
                                            </asp:DropDownList>
                                </div>   
                                    <asp:Label ID="Label6" runat="server" Text="lbl3" CssClass="col-sm-1 control-label" Font-Size="14px" Font-Bold="True">Occr.Time</asp:Label>
                                         <div class="col-sm-1">
                                              <asp:TextBox ID="txtocrhr" runat="server" placeholder="HH:MM" TextMode="Time" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                                             </div>
                                   <asp:Label ID="Label1" runat="server" Text="lbl3" CssClass="col-sm-1 control-label" Font-Size="14px" Font-Bold="True">Reson_for_Failur</asp:Label>
                                   <div class="col-sm-2">
                                        <asp:DropDownList ID="ddresonforfail" runat="server" CssClass="form-control" Height="35px">
                                        <asp:ListItem Value="0" Text="Electrical"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Mechanical"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Other"></asp:ListItem>
                                        </asp:DropDownList>
                                             </div>        
                            </div>

                             <div class="form-group">
                                <asp:Label ID="Label58" runat="server" Text="lbl3" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Product_Name</asp:Label>
                                  <div class="col-sm-1">
                                    <asp:TextBox ID="txticode" runat="server" Width="100%" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                </div>
                                    <div class="col-sm-5">
                                    <asp:TextBox ID="txtiname" runat="server" Width="100%" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                </div>
                                  <asp:Label ID="Label13" runat="server" Text="lbl3" CssClass="col-sm-1 control-label" Font-Size="14px" Font-Bold="True">Email ID</asp:Label>
                               <div class="col-sm-3">
                                    <asp:TextBox ID="txtemailid" runat="server" placeholder="Email ID" MaxLength="30" Width="100%"  CssClass="form-control"></asp:TextBox>
                                </div>
                                    </div>

                                <div class="form-group">
                                <asp:Label ID="Label2" runat="server" Text="lbl3" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Address1</asp:Label>
                                    <div class="col-sm-10">
                                    <asp:TextBox ID="txtaddr1" runat="server" Width="100%" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                </div>
                                    </div>

                            <div class="form-group">
                                <asp:Label ID="Label3" runat="server" Text="lbl3" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Address2</asp:Label>
                                    <div class="col-sm-10">
                                    <asp:TextBox ID="txtaddr2" runat="server" Width="100%" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                </div>
                                    </div>

                              <div class="form-group">
                                <asp:Label ID="Label4" runat="server" Text="lbl3" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Address 3</asp:Label>
                                    <div class="col-sm-10">
                                    <asp:TextBox ID="txtaddr3" ReadOnly="true" runat="server" Width="100%"  CssClass="form-control"></asp:TextBox>
                                </div>
                                    </div>

                             <div class="form-group">
                                <asp:Label ID="Label5" runat="server" Text="lbl3" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">M/C Sr No</asp:Label>
                                    <div class="col-sm-2">
                                    <asp:TextBox ID="txtDGsrno" runat="server" placeholder="M/C SrNo" Width="100%"  CssClass="form-control" OnTextChanged="txtDGsrno_TextChanged" AutoPostBack="true"></asp:TextBox>
                                </div>
                                  <asp:Label ID="Label7" runat="server" Text="lbl3" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Guaranty/Warranty_Term</asp:Label>
                                 <div class="col-sm-1">
                                    <asp:TextBox ID="txtengno" runat="server" placeholder="G/W Year" Width="100%" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                </div>
                                 <asp:Label ID="Label8" runat="server" Text="lbl3" CssClass="col-sm-1 control-label" Font-Size="14px" Font-Bold="True">Invoice No.</asp:Label>
                                  <div class="col-sm-1">
                                    <asp:TextBox ID="txtinvno" runat="server" Width="100%" placeholder="Invoice No." ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                </div>
                                  <asp:Label ID="Label9" runat="server" Text="lbl3" CssClass="col-sm-1 control-label" Font-Size="14px" Font-Bold="True">Invoice Dt</asp:Label>
                                     <div class="col-sm-2">
                                    <asp:TextBox ID="txtinvdate" placeholder="Date"  runat="server" Width="100%" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                  <%--  <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtinvdate" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="MaskedEditExtender1" runat="server" Mask="99/99/9999" MaskType="Date" TargetControlID="txtinvdate" />--%>
                                </div>
                                </div>

                             <div class="form-group">
                                <asp:Label ID="Label10" runat="server" Text="lbl3" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Contact Per</asp:Label>
                                    <div class="col-sm-2">
                                    <asp:TextBox ID="txtcontactper" runat="server" placeholder="Contact Person" Width="100%"  CssClass="form-control"></asp:TextBox>
                                </div>
                                 <asp:Label ID="Label11" runat="server" Text="lbl3" CssClass="col-sm-1 control-label" Font-Size="14px" Font-Bold="True">Tel. No</asp:Label>
                                     <div class="col-sm-1">
                                    <asp:TextBox ID="txttel" runat="server" placeholder="Telephone" Width="100%" MaxLength="10"  CssClass="form-control"></asp:TextBox>
                                </div>
                                  <asp:Label ID="Label12" runat="server" Text="lbl3" CssClass="col-sm-1 control-label" Font-Size="14px" Font-Bold="True">Designation</asp:Label>                              
                             <div class="col-sm-1">
                                    <asp:TextBox ID="txtdesignation" runat="server" placeholder="Designation" Width="100%" MaxLength="30"  CssClass="form-control"></asp:TextBox>
                                </div>
                            <%-- <asp:Label ID="Label13" runat="server" Text="lbl3" CssClass="col-sm-1 control-label" Font-Size="14px" Font-Bold="True">Email ID</asp:Label>
                               <div class="col-sm-1">
                                    <asp:TextBox ID="txtemailid" runat="server" placeholder="Email ID" MaxLength="30" Width="130px"  CssClass="form-control"></asp:TextBox>
                                </div>--%>
                                   <div class="col-sm-4">
                                    <asp:TextBox ID="txtguarnty_status"  placeholder="Guarantee/Warranty Status" ReadOnly="true" runat="server"  MaxLength="30" Width="100%"  CssClass="form-control"></asp:TextBox>
                                </div>
                                 </div>
                            </div> </div> </div>

                 <div class="col-md-12">
                    <div>                
                        <div class="box-body">
                            <div class="form-group">
                                <asp:Label ID="Label14" runat="server" Text="lbl3" CssClass="col-sm-12 control-label" Font-Size="14px" Font-Bold="True"><u><b>Site Detail</b></u></asp:Label>                                    
                                </div>

                            <div class="form-group">
                                <asp:Label ID="Label15" runat="server" Text="lbl3" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Site ID</asp:Label>
                                    <div class="col-sm-6">
                                    <asp:TextBox ID="txtsiteid" runat="server" placeholder="Site Id" Width="100%" MaxLength="25"  CssClass="form-control"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label16" runat="server" Text="lbl3" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Site Name</asp:Label> 
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtsitename" runat="server" placeholder="Site Name" Width="100%" MaxLength="90" CssClass="form-control"></asp:TextBox>
                                </div>
                                </div>

                              <div class="form-group">
                                <asp:Label ID="Label17" runat="server" Text="lbl3" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Address 1</asp:Label>
                                    <div class="col-sm-10">
                                    <asp:TextBox ID="txttaddr1" runat="server" placeholder="Address 1" Width="100%"  MaxLength="60" CssClass="form-control"></asp:TextBox>
                                </div>
                                  </div>

                              <div class="form-group">
                                <asp:Label ID="Label20" runat="server" Text="lbl3" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Address 2</asp:Label>
                                    <div class="col-sm-10">
                                    <asp:TextBox ID="txttaddr2" runat="server" placeholder="Address 2" Width="100%"  MaxLength="60" CssClass="form-control"></asp:TextBox>
                                </div>
                                  </div>

                             <div class="form-group">
                                <asp:Label ID="Label21" runat="server" Text="lbl3" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Address 3</asp:Label>
                                    <div class="col-sm-10">
                                    <asp:TextBox ID="txttaddr3" runat="server" placeholder="Address 3" Width="100%"  MaxLength="60"  CssClass="form-control"></asp:TextBox>
                                </div>
                                  </div>                    
             </div> </div> </div>

                 <div class="col-md-12">
                    <div>                
                        <div class="box-body">
                              <div class="form-group">
                                <asp:Label ID="Label22" runat="server" Text="lbl3" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Call Detail/Attended By</asp:Label>
                                    <div class="col-sm-2">
                                    <asp:TextBox ID="txtcallatt" runat="server" placeholder="Call Attended By"  MaxLength="25" Width="100%"  CssClass="form-control"></asp:TextBox>
                                </div>
                                  <asp:Label ID="Label23" runat="server" Text="lbl3" CssClass="col-sm-1 control-label" Font-Size="14px" Font-Bold="True">Cust_PO_No.</asp:Label>
                                    <div class="col-sm-2">
                                    <asp:TextBox ID="txtcustpo" runat="server" placeholder="Cust PO. No." ReadOnly="true" Width="100%"  MaxLength="25" CssClass="form-control"></asp:TextBox>
                                </div>
                                  <asp:Label ID="Label24" runat="server" Text="lbl3" CssClass="col-sm-1 control-label" Font-Size="14px" Font-Bold="True"> Cust_PO_Dt.</asp:Label>
                                    <div class="col-sm-1">
                                     <asp:TextBox ID="txtcustpodt" placeholder="Date"   runat="server" Width="100%" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                   <%-- <asp:CalendarExtender ID="CalendarExtender5" runat="server" Enabled="True" TargetControlID="txtcustpodt" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="MaskedEditExtender5" runat="server" Mask="99/99/9999" MaskType="Date" TargetControlID="txtcustpodt" /> --%>                           
                                </div>
                                   <asp:Label ID="Label25" runat="server" Text="lbl3" CssClass="col-sm-1 control-label" Font-Size="14px" Font-Bold="True">Equipment</asp:Label> 
                                  <div class="col-sm-1">
                                    <asp:TextBox ID="txtequipment" runat="server" placeholder="Equipment" MaxLength="90" Width="100%"  CssClass="form-control"></asp:TextBox>
                                </div>
                                  </div>      

                               <div class="form-group">
                                <asp:Label ID="Label26" runat="server" Text="lbl3" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Problem Observed</asp:Label>
                                    <div class="col-sm-10">
                                    <asp:TextBox ID="txtprob" runat="server" TextMode="MultiLine"  MaxLength="300"  placeholder="Problem Observed" Width="100%"  CssClass="form-control"></asp:TextBox>
                                </div></div>

                                  <div class="form-group">
                                <asp:Label ID="Label27" runat="server" Text="lbl3" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Remarks <span style="color:red;font-size:x-small;">(Max 300 Char allowed)</span></asp:Label>
                                    <div class="col-sm-10">
                                    <asp:TextBox ID="txtrmk" runat="server" TextMode="MultiLine"   MaxLength="300"  placeholder="Remarks (if any)" Width="100%"  CssClass="form-control"></asp:TextBox>
                                </div></div>

                               </div> </div> </div>

                 <div class="col-md-12" id="tab_actionbyho" runat="server">
                    <div>                
                        <div class="box-body">
                              <div class="form-group" style="text-align:center">
                                <asp:Label ID="Label28" runat="server" Text="lbl3" CssClass="col-sm-12 control-label" Font-Size="18px" Font-Bold="True"> <b><u>Action Taken by H.O.</u></b></asp:Label>
                                 </div>


                              <div class="form-group">
                             <asp:Label ID="Label29" runat="server" Text="lbl3" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True"> <b><u>Doc No.</u></b></asp:Label>  
                                    <div class="col-sm-2">
                                    <asp:TextBox ID="txtdocno" runat="server" placeholder="Doc No."   MaxLength="10" Width="100%"  CssClass="form-control"></asp:TextBox>
                                </div>
                                   <div class="col-sm-5">
                                    <asp:TextBox ID="txtdocdt" runat="server" placeholder="Date" Width="100%"  CssClass="form-control"></asp:TextBox>
                                       <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" TargetControlID="txtdocdt" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="MaskedEditExtender2" runat="server" Mask="99/99/9999" MaskType="Date" TargetControlID="txtdocdt" />                            
                                </div>
                                       <asp:Label ID="Label33" runat="server" Text="lbl3" CssClass="col-sm-1 control-label" Font-Size="14px" Font-Bold="True">Srv_Type</asp:Label>  
                                    <div class="col-sm-1">
                               <asp:ImageButton ID="btnsrvtype" ToolTip="Select Serv. Type" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnsrvtype_Click" />
                                </div>
                                  <div class="col-sm-1">
                                   <asp:TextBox ID="txtsrvtype" runat="server" placeholder="Srv Type"   MaxLength="10"  Width="100%"  CssClass="form-control"></asp:TextBox>
                                </div>

                               </div>



                              <div class="form-group">
                             <asp:Label ID="Label30" runat="server" Text="lbl3" CssClass="col-sm-1 control-label" Font-Size="14px" Font-Bold="True">Eng.Deputed</asp:Label>  
                               <div class="col-sm-1">
                               <asp:ImageButton ID="btnengdeputed" ToolTip="Select Engineer Deputed" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnengdeputed_Click" />
                                </div>
                                 <div class="col-sm-3">
                                    <asp:TextBox ID="txtengdupted" runat="server" ReadOnly="true" placeholder="Eng. Deputed" Width="100%"  CssClass="form-control"></asp:TextBox>
                                </div>
                                   <div class="col-sm-1" style="display:none;">
                                    <asp:TextBox ID="txtcontact" runat="server" ReadOnly="true" placeholder="contact" Width="100%"  CssClass="form-control"></asp:TextBox>
                                </div>
                                   <asp:Label ID="Label31" runat="server" Text="lbl3" CssClass="col-sm-1 control-label" Font-Size="14px" MaxLength="30" Font-Bold="True">Contact_Mode</asp:Label>  
                                   <div class="col-sm-1">
                               <asp:ImageButton ID="btncontactmode" ToolTip="Select Contact Mode" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btncontactmode_Click" />
                                </div>
                                    <div class="col-sm-2">
                                    <asp:TextBox ID="txtcontactmode" runat="server" placeholder="Contact Mode" ReadOnly="true" Width="100%" MaxLength="30"  CssClass="form-control"></asp:TextBox>
                                </div>
                                   <asp:Label ID="Label32" runat="server" Text="lbl3" CssClass="col-sm-1 control-label" Font-Size="14px" Font-Bold="True">Deputation_Date</asp:Label>  
                                  <div class="col-sm-2">
                                   <asp:TextBox ID="txtdeputdt" runat="server" placeholder="Deputation Date"  CssClass="form-control"></asp:TextBox>
                                     <asp:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="True" TargetControlID="txtdeputdt" Format="dd/MM/yyyy"> </asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="MaskedEditExtender3" runat="server" Mask="99/99/9999" MaskType="Date" TargetControlID="txtdeputdt" />   
                                </div>
                            <%--   <asp:Label ID="Label33" runat="server" Text="lbl3" CssClass="col-sm-1 control-label" Font-Size="14px" Font-Bold="True">Srv_Type</asp:Label>  
                                    <div class="col-sm-1">
                               <asp:ImageButton ID="btnsrvtype" ToolTip="Select Serv. Type" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnsrvtype_Click" />
                                </div>
                                  <div class="col-sm-1">
                                   <asp:TextBox ID="txtsrvtype" runat="server" placeholder="Srv Type"   MaxLength="10"  Width="100%"  CssClass="form-control"></asp:TextBox>
                                </div>--%>

                                  </div>

                              <div class="form-group">
                             <asp:Label ID="Label34" runat="server" Text="lbl3" CssClass="col-sm-1 control-label" Font-Size="14px" Font-Bold="True">Dealer_Name</asp:Label>  
                               <div class="col-sm-1">
                               <asp:ImageButton ID="btndealername" ToolTip="Select Dealer Name" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btndealername_Click" />
                                </div>
                                  <div class="col-sm-2">
                                    <asp:TextBox ID="txtdealername" runat="server" placeholder="Dealer Name"   MaxLength="25" Width="100%"  CssClass="form-control"></asp:TextBox>
                                </div>
                                  <asp:Label ID="Label35" runat="server" Text="lbl3" CssClass="col-sm-1 control-label" Font-Size="14px" Font-Bold="True">First_Person</asp:Label>  
                                     <div class="col-sm-1">
                               <asp:ImageButton ID="btnfirstprsn" ToolTip="Select Dealer Name" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnfirstprsn_Click" />
                                </div>
                                    <div class="col-sm-3">
                                    <asp:TextBox ID="txtperson" runat="server" placeholder="Fisrt Person/Responsible"  MaxLength="25"  Width="100%" CssClass="form-control"></asp:TextBox>
                                </div>
                                  <div class="col-sm-1" style="display:none;">
                                    <asp:TextBox ID="txtpersoncontct" runat="server" placeholder="Fisrt Person/Responsible"  MaxLength="25"  Width="100%" CssClass="form-control"></asp:TextBox>
                                </div>
                                 <asp:Label ID="Label36" runat="server" Text="lbl3" CssClass="col-sm-1 control-label" Font-Size="14px" Font-Bold="True">Category</asp:Label>  

                                   <div class="col-sm-1" style="display:none;">
                                        <asp:ImageButton ID="btncatg"   ToolTip="Select Category" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btncatg_Click" />
                                       </div>
                                  <div class="col-sm-2">
                                       <asp:TextBox ID="txtcatg" runat="server" placeholder="Category"  MaxLength="25"  Width="100%" CssClass="form-control"></asp:TextBox>
                                  <%-- <asp:RadioButtonList ID="rdcategory" runat="server" RepeatDirection="Horizontal" Height="35px" BackColor="#FFC107">
                                                <asp:ListItem Text="&nbsp;&nbsp;&nbsp;Major &nbsp;&nbsp;&nbsp;" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="&nbsp;&nbsp;&nbsp;Minor &nbsp;&nbsp;&nbsp;" Value="1"></asp:ListItem>
                                            </asp:RadioButtonList>--%>
                                      </div>
                                 </div>
                            
                             <div class="form-group">
                             <asp:Label ID="Label37" runat="server" Text="lbl3" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Instruction_To_Eng         <span style="color:red;font-size:x-small;">(Max 300 Char allowed)</span></asp:Label>  
                                    <div class="col-sm-10">
                                    <asp:TextBox ID="txtinstruction" runat="server"   MaxLength="300"  TextMode="MultiLine" placeholder="Instruction to Eng. / Remakrs if any" Width="100%"  CssClass="form-control"></asp:TextBox>
                                </div>
                                  </div>

                                  </div></div></div>

                   <div class="col-md-12" id="tab_actionbyjaycee" runat="server">
                   <div>                
                        <div class="box-body">
                                <div class="form-group" style="text-align:center">
                                <asp:Label ID="Label38" runat="server" Text="lbl3" CssClass="col-sm-12 control-label" Font-Size="18px" Font-Bold="True"><b><u>Action by Service Engineer</u></b></asp:Label>
                                 </div>

                               <div class="form-group">
                             <asp:Label ID="Label39" runat="server" Text="lbl3" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Doc No.</asp:Label>  
                                    <div class="col-sm-3">
                                    <asp:TextBox ID="txtactionvchnum" runat="server" placeholder="Doc No" ReadOnly="true" Width="100%"  CssClass="form-control"></asp:TextBox>
                                </div>
                                   <div class="col-sm-5">
                                     <asp:TextBox ID="txtactionvchdt" runat="server" placeholder="Deputation Date"  CssClass="form-control"></asp:TextBox>
                                     <asp:CalendarExtender ID="CalendarExtender4" runat="server" Enabled="True" TargetControlID="txtactionvchdt" Format="dd/MM/yyyy"> </asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="MaskedEditExtender4" runat="server" Mask="99/99/9999" MaskType="Date" TargetControlID="txtactionvchdt" />   
                                    </div>
                                   
                                     <asp:Label ID="Label40" runat="server" Text="lbl3" CssClass="col-sm-1 control-label" Font-Size="14px" Font-Bold="True">Closed</asp:Label>  
                                   <div class="col-sm-1">
                                       <asp:CheckBox ID="chkclose" runat="server" CssClass="form-control"  Height="35px" />
                                       </div>

                                  </div>

                              <div class="form-group">
                             <asp:Label ID="Label41" runat="server" Text="lbl3" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Time_In.</asp:Label>  
                                    <div class="col-sm-3">
                                    <asp:TextBox ID="txttimein" runat="server" placeholder="Time IN" TextMode="Time"  Width="100%"  CssClass="form-control"></asp:TextBox>
                                    <%--  <asp:MaskedEditExtender ID="Maskededitextender8" runat="server" Mask="99:99" MaskType="Time" TargetControlID="txttimein" />--%>
                                </div>
                                    <asp:Label ID="Label42" runat="server" Text="lbl3" CssClass="col-sm-1 control-label" Font-Size="14px" Font-Bold="True">Time_Out.</asp:Label>  
                                 <div class="col-sm-1">
                                      <asp:TextBox ID="txttimeout" runat="server" placeholder="Time Out"  TextMode="Time" Width="100%"  CssClass="form-control"></asp:TextBox>
                                     <%--  <asp:MaskedEditExtender ID="Maskededitextender6" runat="server" Mask="99:99" MaskType="Time" TargetControlID="txttimeout" />--%>
                                     </div>
                                     <asp:Label ID="Label43" runat="server" Text="lbl3" CssClass="col-sm-1 control-label" Font-Size="14px" Font-Bold="True">Next_Target</asp:Label>  
                                      <div class="col-sm-1">
                                           <asp:TextBox ID="txtnexttgt" runat="server" placeholder="Next Target" ReadOnly="true"  CssClass="form-control"></asp:TextBox>
                                    <%-- <asp:CalendarExtender ID="CalendarExtender6" runat="server" Enabled="True" TargetControlID="txtnexttgt" Format="dd/MM/yyyy"> </asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="MaskedEditExtender7" runat="server" Mask="99/99/9999" MaskType="Date" TargetControlID="txtnexttgt" />                                             --%>
                                              </div>
                                   <asp:Label ID="Label44" runat="server" Text="lbl3" CssClass="col-sm-1 control-label" Height="35px" Font-Bold="True">Work_done</asp:Label>  
                                   <div class="col-sm-2">
                                  <asp:RadioButtonList ID="rdworkdone" runat="server" RepeatDirection="Horizontal"  Height="35px"  BackColor="#FFC107" OnSelectedIndexChanged="rdworkdone_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem Text="&nbsp;&nbsp;&nbsp;Yes &nbsp;&nbsp;&nbsp;" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="&nbsp;&nbsp;&nbsp;No &nbsp;&nbsp;&nbsp;" Value="1"></asp:ListItem>
                                            </asp:RadioButtonList>
                                  </div>



                       </div> 
                                                    
                           <div class="form-group">
                             <asp:Label ID="Label45" runat="server" Text="lbl3" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Engineer_Remarks</asp:Label>  
                                    <div class="col-sm-10">
                                    <asp:TextBox ID="txtengrmk" runat="server" MaxLength="300" placeholder="Engineer Remarks"  Width="100%"  CssClass="form-control"></asp:TextBox>
                                </div>
                         </div>

                            <div class="form-group">
                             <asp:Label ID="Label46" runat="server" Text="lbl3" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Corrective Action</asp:Label>  
                                    <div class="col-sm-10">
                                    <asp:TextBox ID="txtcorraction" runat="server" placeholder="Corrective Action" MaxLength="300"  Width="100%"  CssClass="form-control"></asp:TextBox>
                                </div>
                         </div>
                        
                             <div class="form-group">
                             <asp:Label ID="Label47" runat="server" Text="lbl3" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Preventive Action</asp:Label>  
                                    <div class="col-sm-10">
                                    <asp:TextBox ID="txtprevntiveaction" runat="server" placeholder="Preventive Action" MaxLength="300"  Width="100%"  CssClass="form-control"></asp:TextBox>
                                </div>
                         </div>
                        
                             <div class="form-group">
                             <asp:Label ID="Label48" runat="server" Text="lbl3" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Reason for Pendency</asp:Label>  
                                    <div class="col-sm-10">
                                    <asp:TextBox ID="txtreasonforpend" runat="server" placeholder="Reason for Pendency" MaxLength="300"  Width="100%"  CssClass="form-control"></asp:TextBox>
                                </div>
                         </div>

                             <div class="form-group">
                             <asp:Label ID="Label49" runat="server" Text="lbl3" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Spares Reqd if Any</asp:Label>  
                                    <div class="col-sm-10">
                                    <asp:TextBox ID="txtspares" runat="server" placeholder="Spares reqd if Any" MaxLength="70"  Width="100%"  CssClass="form-control"></asp:TextBox>
                                </div>
                         </div>

                             <div class="form-group">
                             <asp:Label ID="Label54" runat="server" Text="lbl3" CssClass="col-sm-2 control-label"  MaxLength="10"  Font-Size="14px" Font-Bold="True">Met_Whom</asp:Label>  
                                    <div class="col-sm-6">
                                    <asp:TextBox ID="txtmetwhom" runat="server" placeholder="Met Whom"  Width="100%" MaxLength="30"  autocomplete="off"  CssClass="form-control"></asp:TextBox>
                                </div>
                                   <asp:Label ID="Label56" runat="server" Text="lbl3" CssClass="col-sm-2 control-label"  MaxLength="10"  Font-Size="14px" Font-Bold="True">Close_Date</asp:Label>  
                                    <div class="col-sm-2">
                                    <asp:TextBox ID="txtclosdt" runat="server" MaxLength="10" placeholder="Close Date"  ReadOnly="true"  Width="100%"  CssClass="form-control"></asp:TextBox>
                                    <%-- <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtclosdt" Format="dd/MM/yyyy"> </asp:CalendarExtender>                                
                                    <asp:MaskedEditExtender ID="MaskedEditExtender1" runat="server" Mask="99/99/9999" MaskType="Date" TargetControlID="txtclosdt" />--%>
                                </div>
                                </div>

                              <div class="form-group">
                             <asp:Label ID="Label50" runat="server" Text="lbl3" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Service_Cost</asp:Label>  
                                    <div class="col-sm-2">
                                    <asp:TextBox ID="txtsrvcost" runat="server" placeholder="Service Cost"  MaxLength="5"  Width="100%" onkeyup="cal()" onkeypress="return isDecimalKey(event)"  autocomplete="off" CssClass="form-control"></asp:TextBox>
                                </div>
                                   <asp:Label ID="Label51" runat="server" Text="lbl3" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Spares_Cost</asp:Label>  
                                    <div class="col-sm-2">
                                    <asp:TextBox ID="txtsparecost" runat="server" placeholder="Spares Cost"  Width="100%" onkeyup="cal()" MaxLength="5" onkeypress="return isDecimalKey(event)"  autocomplete="off" CssClass="form-control"></asp:TextBox>
                                </div>
                                     <asp:Label ID="Label52" runat="server" Text="lbl3" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Misc_Cost</asp:Label>  
                                    <div class="col-sm-2">
                                    <asp:TextBox ID="txtmisccost" runat="server" placeholder="Misc. Cost" Width="100%" onkeyup="cal()" MaxLength="5" onkeypress="return isDecimalKey(event)"  autocomplete="off"  CssClass="form-control"></asp:TextBox>
                                </div> </div>

                                     <div class="form-group">
                                  <asp:Label ID="Label53" runat="server" Text="lbl3" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Travalling_Conv.</asp:Label>  
                                    <div class="col-sm-2">
                                    <asp:TextBox ID="txttravconv" runat="server" placeholder="Travalling Conv."  onkeyup="cal()" MaxLength="5"  Width="100%"  onkeypress="return isDecimalKey(event)"  autocomplete="off" CssClass="form-control"></asp:TextBox>
                                </div>
                                         <asp:Label ID="Label57" runat="server" Text="lbl3" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Total Cost</asp:Label>  
                                    <div class="col-sm-6">
                                    <asp:TextBox ID="txttotcost" runat="server" placeholder="Total Cost"  onkeyup="cal()" ReadOnly="true"  Width="190px"  onkeypress="return isDecimalKey(event)"  autocomplete="off" CssClass="form-control"></asp:TextBox>
                                </div>
                         </div>

        

                                <div class="form-group">
                             <asp:Label ID="Label55" runat="server" Text="lbl3" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Remarks <span style="color:red;font-size:x-small;">(Max 300 Char allowed)</span></asp:Label>  
                                    <div class="col-sm-10">
                                    <asp:TextBox ID="txtrmkactionbyeng" TextMode="MultiLine" runat="server" placeholder="Remarks"  MaxLength="300"   Width="100%"  CssClass="form-control"></asp:TextBox>
                                </div>
                                   </div>

                        </div> </div></div>

                  <div class="col-md-12" id="img_div" runat="server">
                    <div>
                        <div class="box-body">
                            <table>
                                <tr>
                                    <td>
                                        <asp:FileUpload ID="Attch" runat="server" Visible="true" onchange="submitFile()" /></td>
                                    <td>
                                        <asp:TextBox ID="txtAttch" runat="server" MaxLength="100" placeholder="Path Upto 100 Char" ></asp:TextBox></td>
                                </tr>
                            </table>
                            <asp:Button ID="btnAtt" runat="server" Text="Attachment" OnClick="btnAtt_Click" Width="134px" Style="display: none" />

                            <asp:Label ID="lblShow" runat="server"></asp:Label>
                            <asp:Label ID="lblUpload" runat="server" Style="display: none"></asp:Label>

                            <asp:ImageButton ID="btnView1" runat="server" ImageUrl="~/tej-base/images/preview-file.png" OnClick="btnView1_Click" Visible="false" />
                            <asp:ImageButton ID="btnDwnld1" runat="server" ImageUrl="~/tej-base/images/Save.png" OnClick="btnDwnld1_Click" Visible="false" />
                        </div>
                    </div>
                </div>
                  </div>
        </section>
    </div>



<%--        <div class="toolsContentLeft">
            <div class="bSubBlock brandSecondaryBrd secondaryPalette">
                <div class="lbBody">
                    <table style="width: 100%">
                        <tr>
                            <td colspan="8">
                                <table width="100%" id="tab_bsrvreq" runat="server">
                                   <%--              <tr>
                                        <td>
                                            Srv Req No./Dt
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtvchnum" runat="server" ReadOnly="true" Width="70px" placeholder="Srv Req No"></asp:TextBox>
                                            <asp:TextBox ID="txtvchdate" runat="server" Width="70px" placeholder="Date"></asp:TextBox>
                                            <cc1:CalendarExtender ID="txtvchdate_CalendarExtender" runat="server" Enabled="True"
                                                TargetControlID="txtvchdate" Format="dd/MM/yyyy">
                                            </cc1:CalendarExtender>
                                            <cc1:MaskedEditExtender ID="Maskedit1" runat="server" Mask="99/99/9999" MaskType="Date"
                                                TargetControlID="txtvchdate" />
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>--%>
                                   <%-- <tr>
                                        <td>
                                            Customer Name
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtacode" runat="server" Width="70px" ReadOnly="true" placeholder="Code"></asp:TextBox>
                                            <asp:TextBox ID="txtaname" runat="server" Width="250px" ReadOnly="true" placeholder="Customer Name"></asp:TextBox>
                                        </td>
                                        <td >
                                            <asp:DropDownList ID="dd_list1" runat="server" CssClass="rounded_corner">
                                                <asp:ListItem Value="0" Text="Prv.Maint"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="Service"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="Service + Complaint"></asp:ListItem>
                                                <asp:ListItem Value="3" Text="Others"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            Occr. Time
                                        </td>
                                        <td>
                                            <%--Need to Save in Database--%>
                                   <%--   <asp:TextBox ID="txtocrhr" runat="server" placeholder="HH:MM"></asp:TextBox>
                                            <cc1:maskededitextender ID="Maskededitextender7" runat="server" Mask="99:99" 
                            MaskType="Time" TargetControlID="txtocrhr" />
                                        </td>
                                        <td >
                                        Reson for Failur
                                        </td>
                                        <td>
                                        <asp:DropDownList ID="ddresonforfail" runat="server" CssClass="rounded_corner">
                                        <asp:ListItem Value="0" Text="Electrical"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Mechanical"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Other"></asp:ListItem>
                                        </asp:DropDownList>
                                        </td>
                                    </tr>--%>
                                   <%--<tr>
                                        <td>
                                            Address 1
                                        </td>
                                        <td colspan="7">
                                            <asp:TextBox ID="txtaddr1" runat="server" Width="99%" ReadOnly="true" placeholder="Address 1"></asp:TextBox>
                                        </td>
                                    </tr>--%>
                                   <%--   <tr>
                                        <td>
                                            Address 2
                                        </td>
                                        <td colspan="7">
                                            <asp:TextBox ID="txraddr2" runat="server" Width="99%" ReadOnly="true" placeholder="Address 2"> </asp:TextBox>
                                        </td>
                                    </tr>--%>
                                   <%--<tr>
                                        <td>
                                            Address 3
                                        </td>
                                        <td colspan="7">
                                            <asp:TextBox ID="txtaddr3" runat="server" Width="99%" ReadOnly="true" placeholder="Address 3"></asp:TextBox>
                                        </td>
                                    </tr>--%>
                                   <%--  <tr>
                                        <td>
                                            DG Sr No
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDGsrno" runat="server" placeholder="DG SrNo" ReadOnly="true"></asp:TextBox>
                                        </td>
                                        <td>
                                            Engine No.
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtengno" runat="server" placeholder="Engine No" ReadOnly="true"></asp:TextBox>
                                        </td>
                                        <td>
                                            Invoice No.
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtinvno" runat="server" placeholder="Invoice No." ReadOnly="true"></asp:TextBox>
                                        </td>
                                        <td>
                                            Invoice Dt
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtinvdate" runat="server" placeholder="Invoice Dt" ReadOnly="true"></asp:TextBox>
                                            <cc1:CalendarExtender ID="Calendarextender1" runat="server" Enabled="True" TargetControlID="txtinvdate" Format="dd/MM/yyyy">
                                            </cc1:CalendarExtender>
                                            <cc1:MaskedEditExtender ID="Maskededitextender1" runat="server" Mask="99/99/9999" MaskType="Date" TargetControlID="txtinvdate" />
                                        </td>
                                    </tr>--%>
                                   <%-- <tr>
                                        <td>
                                            Contact Per
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtcontactper" runat="server" placeholder="Contact Person"></asp:TextBox>
                                        </td>
                                        <td>
                                            Tel. No
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txttel" runat="server" placeholder="Telephone"></asp:TextBox>
                                        </td>
                                        <td>
                                            Designation
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtdesignation" runat="server" placeholder="Designation"></asp:TextBox>
                                        </td>
                                        <td>
                                            Email ID
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtemailid" runat="server" placeholder="Email ID"></asp:TextBox>
                                        </td>
                                    </tr>--%>
                                   <%-- <tr>
                                        <td colspan="8">
                                            <b><u>Site Detail</u></b>
                                            <hr />
                                        </td>
                                    </tr>--%>
                                   <%-- <tr>
                                        <td>
                                            Site ID
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtsiteid" runat="server" Width="96%" ReadOnly="true" placeholder="Site ID"></asp:TextBox>
                                        </td>
                                        <td>
                                            Site Name
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtsitename" runat="server" Width="96%" ReadOnly="true" placeholder="Site Name"></asp:TextBox>
                                        </td>
                                    </tr>--%>
                                    <%--<tr>
                                        <td>
                                            Address 1
                                        </td>
                                        <td colspan="7">
                                            <asp:TextBox ID="txttaddr1" runat="server" Width="99%" placeholder="Address 1"></asp:TextBox>
                                        </td>
                                    </tr>--%>
                                   <%-- <tr>
                                        <td>
                                            Address 2
                                        </td>
                                        <td colspan="7">
                                            <asp:TextBox ID="txttaddr2" runat="server" Width="99%" placeholder="Address 2"></asp:TextBox>
                                        </td>
                                    </tr>--%>
                                  <%--  <tr>
                                        <td>
                                            Address 3
                                        </td>
                                        <td colspan="7">
                                            <asp:TextBox ID="txttaddr3" runat="server" Width="99%" placeholder="Address 3"></asp:TextBox>
                                        </td>
                                    </tr>--%>
                                   <%--  <tr>
                                        <td colspan="8">
                                            <hr />
                                        </td>
                                    </tr>
                             <%--       <tr>
                                        <td>
                                            Call Detail/Attended By
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtcallatt" runat="server" placeholder="Call Attended By"></asp:TextBox>
                                        </td>
                                        <td>
                                            Cust PO. No.
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtcustpo" runat="server" placeholder="Cust PO. No."></asp:TextBox>
                                        </td>
                                        <td>
                                            Cust PO. Dt.
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtcustpodt" runat="server" placeholder="Cust PO. Dt."></asp:TextBox>
                                            <cc1:CalendarExtender ID="CalendarExtender5" runat="server" Enabled="True" TargetControlID="txtcustpodt"
                                                Format="dd/MM/yyyy">
                                            </cc1:CalendarExtender>
                                            <cc1:MaskedEditExtender ID="MaskedEditExtender5" runat="server" Mask="99/99/9999"
                                                MaskType="Date" TargetControlID="txtcustpodt" />
                                        </td>
                                        <td>
                                            Equipment
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtequipment" runat="server" placeholder="Equipment"></asp:TextBox>
                                        </td>
                                    </tr>--%>
                              <%--      <tr>
                                        <td>
                                            Problem Observed
                                        </td>
                                        <td colspan="7">
                                            <asp:TextBox ID="txtprob" runat="server" TextMode="MultiLine" Width="99%" placeholder="Problem Observed"></asp:TextBox>
                                        </td>
                                    </tr>--%>
                                   <%-- <tr>
                                        <td>
                                            Remarks
                                        </td>
                                        <td colspan="7">
                                            <asp:TextBox ID="txtrmk" runat="server" TextMode="MultiLine" Width="99%" placeholder="Remarks (if any)"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8">
                                <table width="100%" id="tab_actionbyho" runat="server">
                                    <tr>
                                        <td colspan="8">
                                            <hr />
                                        </td>
                                    </tr>--%>
                                 <%--   <tr style="background-color: #CDE8F0; font-size: large; height: 30px;">
                                        <td colspan="8">
                                            <div align="center">
                                                <b><u>Action Taken by H.O.</u></b>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="8">
                                            <hr />
                                        </td>
                                    </tr>--%>
                                <%--    <tr>
                                        <td>
                                            Doc No.
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtdocno" runat="server" Width="70px" placeholder="Doc No" ReadOnly="true"></asp:TextBox>
                                            <asp:TextBox ID="txtdocdt" runat="server" Width="70px" placeholder="Date"></asp:TextBox>
                                            <cc1:CalendarExtender ID="Calendarextender2" runat="server" Enabled="True" TargetControlID="txtdocdt"  Format="dd/MM/yyyy">                                               
                                            </cc1:CalendarExtender>
                                            <cc1:MaskedEditExtender ID="Maskededitextender2" runat="server" Mask="99/99/9999"  MaskType="Date" TargetControlID="txtdocdt" />                                               
                                        </td>
                                        <td>
                                            
                                        </td>
                                        <td>
                                            
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>

                                 <%--   <tr>
                                        <td>
                                            Eng. Deputed
                                            <asp:ImageButton ID="btnengdeputed" runat="server" ToolTip="Select Engineer Deputed"   ImageUrl="~/css/images/bdsearch5.png" Style="width: 22px; float: right" OnClick="btnengdeputed_Click" />
                                              
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtengdupted" runat="server" placeholder="Eng. Deputed"></asp:TextBox>
                                        </td>
                                        <td>
                                            Contact Mode
                                            <asp:ImageButton ID="btncontactmode" runat="server" ToolTip="Select Contact Mode"  ImageUrl="~/css/images/bdsearch5.png" Style="width: 22px; float: right" OnClick="btncontactmode_Click" />
                                               
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtcontactmode" runat="server" placeholder="Contact Mode"></asp:TextBox>
                                        </td>
                                        <td>
                                            Deputation Date
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtdeputdt" runat="server" placeholder="Deputation Date"></asp:TextBox>
                                            <cc1:CalendarExtender ID="Calendarextender3" runat="server" Enabled="True" TargetControlID="txtdeputdt"  Format="dd/MM/yyyy">                                            
                                            </cc1:CalendarExtender>
                                            <cc1:MaskedEditExtender ID="Maskededitextender3" runat="server" Mask="99/99/9999"  MaskType="Date" TargetControlID="txtdeputdt" />
                                              
                                        </td>
                                        <td>
                                            Srv Type
                                            <asp:ImageButton ID="btnsrvtype" runat="server" ToolTip="Select Service Type" ImageUrl="~/css/images/bdsearch5.png"  Style="width: 22px; float: right; height: 20px;" OnClick="btnsrvtype_Click" />                                              
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtsrvtype" runat="server" placeholder="Srv Type"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <%--<tr>
                                        <td>
                                            Dealer Name
                                            <asp:ImageButton ID="btndealername" runat="server" ToolTip="Select Dealer Name" ImageUrl="~/css/images/bdsearch5.png"
                                                Style="width: 22px; float: right" OnClick="btndealername_Click" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtdealername" runat="server" placeholder="Dealer Name"></asp:TextBox>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            First Person
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtperson" runat="server" placeholder="Fisrt Person/Responsible"></asp:TextBox>
                                        </td>
                                        <td>
                                            Category
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="rdcategory" runat="server" RepeatDirection="Horizontal"
                                                Font-Size="Large" BackColor="#FFC107">
                                                <asp:ListItem Text="&nbsp;&nbsp;&nbsp;Major &nbsp;&nbsp;&nbsp;" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="&nbsp;&nbsp;&nbsp;Minor &nbsp;&nbsp;&nbsp;" Value="1"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                             <%--       <tr>
                                        <td>
                                            Instruction To Eng.<br />
                                            (Remarks if any)
                                        </td>
                                        <td colspan="7">
                                            <asp:TextBox ID="txtinstruction" runat="server" TextMode="MultiLine" Width="99%"
                                                placeholder="Instruction to Eng. / Remakrs if any"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8">
                                <table width="100%" id="tab_actionbyjaycee" runat="server">
                                    <tr>
                                        <td colspan="8">
                                            <hr />
                                        </td>
                                    </tr>
                                 <%--   <tr style="background-color: #CDE8F0; font-size: large; height: 30px;">
                                        <td colspan="8">
                                            <div align="center">
                                                <b><u>Action by JAYCEE SM</u></b>
                                            </div>
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td colspan="8">
                                            <hr />
                                        </td>
                                    </tr>
                        <%--            <tr>
                                        <td>
                                            Doc No.
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtactionvchnum" runat="server" Width="70px" placeholder="Doc No" ReadOnly="true"></asp:TextBox>
                                            <asp:TextBox ID="txtactionvchdt" runat="server" Width="70px" placeholder="Date"></asp:TextBox>
                                            <cc1:CalendarExtender ID="Calendarextender4" runat="server" Enabled="True" TargetControlID="txtactionvchdt"
                                                Format="dd/MM/yyyy">
                                            </cc1:CalendarExtender>
                                            <cc1:MaskedEditExtender ID="Maskededitextender4" runat="server" Mask="99/99/9999"
                                                MaskType="Date" TargetControlID="txtactionvchdt" />
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        Closed
                                        </td>
                                        <td >
                                        <asp:CheckBox ID="chkclose" runat="server" Width="20px" Height="20px" />
                                        </td>
                                    </tr>--%>
                                  <%--  <tr>
                                        <td>
                                            Time In.
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txttimein" runat="server" placeholder="Time IN"></asp:TextBox>
                                                            <cc1:maskededitextender ID="Maskededitextender8" runat="server" Mask="99:99" 
                            MaskType="Time" TargetControlID="txttimein" />
                                        </td>
                                        <td>
                                            Time Out.
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txttimeout" runat="server" placeholder="Time Out"></asp:TextBox>
                                                                            <cc1:maskededitextender ID="Maskededitextender9" runat="server" Mask="99:99" 
                            MaskType="Time" TargetControlID="txttimeout" />
                                        </td>
                                        <td>
                                            Next Target
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtnexttgt" runat="server" placeholder="Next Target"></asp:TextBox>
                                            <cc1:CalendarExtender ID="CalendarExtender6" runat="server" Enabled="True" TargetControlID="txtnexttgt"
                                                Format="dd/MM/yyyy">
                                            </cc1:CalendarExtender>
                                            <cc1:MaskedEditExtender ID="MaskedEditExtender6" runat="server" Mask="99/99/9999"
                                                MaskType="Date" TargetControlID="txtnexttgt" />
                                        </td>
                                        <td>
                                            Work done
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="rdworkdone" runat="server" RepeatDirection="Horizontal"
                                                Font-Size="Medium" BackColor="#FFC107">
                                                <asp:ListItem Text="&nbsp;&nbsp;&nbsp;Yes &nbsp;&nbsp;&nbsp;" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="&nbsp;&nbsp;&nbsp;No &nbsp;&nbsp;&nbsp;" Value="1"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>--%>
                                   <%-- <tr>
                                        <td>
                                            Engineer Remarks
                                        </td>
                                        <td colspan="7">
                                            <asp:TextBox ID="txtengrmk" runat="server" Width="99%" placeholder="Engineer Remarks"></asp:TextBox>
                                        </td>
                                    </tr>--%>
                                  <%--  <tr>
                                        <td>
                                            Corrective Action
                                        </td>
                                        <td colspan="7">
                                            <asp:TextBox ID="txtcorraction" runat="server" Width="99%" placeholder="Corrective Action"></asp:TextBox>
                                        </td>
                                    </tr>--%>
                                   <%-- <tr>
                                        <td>
                                            Preventive Action
                                        </td>
                                        <td colspan="7">
                                            <asp:TextBox ID="txtprevntiveaction" runat="server" Width="99%" placeholder="Preventive Action"></asp:TextBox>
                                        </td>
                                    </tr>--%>
                                   <%-- <tr>
                                        <td>
                                            Reason for Pendency
                                        </td>
                                        <td colspan="7">
                                            <asp:TextBox ID="txtreasonforpend" runat="server" Width="99%" placeholder="Reason for Pendency"></asp:TextBox>
                                        </td>
                                    </tr>--%>
                           <%--         <tr>
                                        <td>
                                            Spares Reqd if Any
                                        </td>
                                        <td colspan="7">
                                            <asp:TextBox ID="txtspares" runat="server" Width="99%" placeholder="Spares reqd if Any"></asp:TextBox>
                                        </td>
                                    </tr>--%>
                           <%--         <tr>
                                        <td>
                                            Service Cost
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtsrvcost" runat="server" placeholder="Service Cost"></asp:TextBox>
                                        </td>
                                        <td>
                                            Spares Cost
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtsparecost" runat="server" placeholder="Spares Cost"></asp:TextBox>
                                        </td>
                                        <td>
                                            Misc. Cost
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtmisccost" runat="server" placeholder="Misc. Cost"></asp:TextBox>
                                        </td>
                                        <td>
                                            Travalling Conv.
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txttravconv" runat="server" placeholder="Travalling Conv."></asp:TextBox>
                                        </td>
                                    </tr>--%>
                                 <%--   <tr>
                                        <td>
                                            HMR
                                        </td>
                                        <td colspan="7">
                                            <asp:TextBox ID="txthmrcost" runat="server" placeholder="HMR"></asp:TextBox>
                                        </td>
                                    </tr>--%>
                                   <%-- <tr>
                                        <td>
                                            Remarks
                                        </td>
                                        <td colspan="7">
                                            <asp:TextBox ID="txtrmkactionbyeng" runat="server" Width="99%" placeholder="Remarks"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>--%>

    
    <asp:Button ID="btnhideF" runat="server" OnClick="btnhideF_Click" Style="display: none" />
    <asp:Button ID="btnhideF_s" runat="server" OnClick="btnhideF_s_Click" Style="display: none" />
    <asp:HiddenField ID="hffield" runat="server" />
    <asp:HiddenField ID="edmode" runat="server" />    
    <asp:HiddenField ID="doc_nf" runat="server" />
    <asp:HiddenField ID="doc_df" runat="server" />
    <asp:HiddenField ID="doc_vty" runat="server" />
    <asp:HiddenField ID="doc_addl" runat="server" /> 
    <asp:HiddenField ID="hf1" runat="server" />
    <asp:HiddenField ID="hf2" runat="server" />   
    <asp:HiddenField ID="hf4" runat="server" />

    <script type="text/javascript">
        $(function () {
            var tabName = $("[id*=TabName]").val() != "" ? $("[id*=TabName]").val() : "DescTab";
            $('#Tabs a[href="#' + tabName + '"]').tab('show');
            $("#Tabs a").click(function () {
                $("[id*=TabName]").val($(this).attr("href").replace("#", ""));
            });
        });

        function cal() {
            var t1 = 0; var t2 = 0; var t3 = 0; var t4 = 0; var t5 = 0;
            t1 = fill_zero(document.getElementById("ContentPlaceHolder1_txtsrvcost").value);
            t2 = fill_zero(document.getElementById("ContentPlaceHolder1_txtsparecost").value);
            t3 = fill_zero(document.getElementById("ContentPlaceHolder1_txtmisccost").value);
            t4 = fill_zero(document.getElementById("ContentPlaceHolder1_txttravconv").value);
            t5 = (t1 * 1) + (t2 * 1) + (t3 * 1) + (t4 * 1);
            document.getElementById('ContentPlaceHolder1_txttotcost').value = (t5 * 1).toFixed(3);
        }
        function fill_zero(val) { if (isNaN(val)) return 0; if (isFinite(val)) return val; }


    </script>
</asp:Content>
