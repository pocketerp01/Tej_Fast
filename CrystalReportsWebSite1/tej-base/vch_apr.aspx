<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="vch_apr" Title="Tejaxo" CodeFile="vch_apr.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style2
        {
            width: 69px;
        }
        .style3
        {
            width: 70px;
        }
        .style4
        {
            width: 90px;
        }
        .style5
        {
            width: 42px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <div class="content-wrapper">
        <section class="content-header">
             <table style="width: 100%">
             <tr>
                 <td>
            <button id="btnview" runat="server" accesskey="v" class="btn btn-info" style="width: 100px" onserverclick="btnview_ServerClick"><u>V</u>iew</button>
            <button id="btnlist" runat="server" accesskey="t" class="btn btn-info" style="width: 100px" onserverclick="btnlist_ServerClick">Lis<u>t</u></button>
            <button id="btnapr" runat="server" accesskey="a" class="btn btn-info" style="width: 100px" onserverclick="btnapr_ServerClick"><u>A</u>pprove</button>
            <button id="btncan" runat="server" accesskey="c" class="btn btn-info" style="width: 100px" onserverclick="btncan_ServerClick"><u>C</u>ancel</button>
            <button id="btnext" runat="server" accesskey="x" class="btn btn-info" style="width: 100px" onserverclick="btnext_ServerClick">E<u>x</u>it</button>
                 </td>
                 <td>
                      <asp:Label ID="lbHeader" runat="server" Text="Voucher Approval" Font-Bold="True" Font-Size="X-Large"></asp:Label>
                 </td>
             </tr>    
             </table>
        </section>
        
          <section class="content">
               <div class="row">
                   <div class="col-md-6">
                       <div>
                           <div class="box-body" style="background:url('../tej-base/images/fm-bg1.jpg') 200% 80%;">
                               <div class="form-group">
                                    <label id="voucher" runat="server" class="col-sm-4 control-label">Voucher No/Date</label>
                                     <div class="col-sm-3">
                                          <asp:TextBox ID="txtvchnum0" runat="server" Width="100%" Height="32px" placeholder="Voucher No"
                        ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                     </div>
                                     <div class="col-sm-5">
                                          <asp:TextBox ID="txtvchdt0" runat="server" Width="100%" Height="32px" placeholder="Voucher Date"
                        ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                     </div>
                                </div>
                               <div class="form-group">
                                   <label id="MrrNoDate" runat="server" class="col-sm-4 control-label">MRR No/Date</label>
                                   <div class="col-sm-3">
                                       <asp:TextBox ID="txtvchnum" runat="server" Width="100%" Height="32px" placeholder="MRR No" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                   </div>
                                    <div class="col-sm-5">
                                         <asp:TextBox ID="txtvchdt" runat="server" Width="100%" Height="32px" placeholder="MRR Date" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                    </div>
                               </div>
                               <div class="form-group">
                                <label id="PartyCodeName" runat="server" class="col-sm-4 control-label">Party Code/Name</label>
                              <div class="col-sm-3">
                                   <asp:TextBox ID="txtacode" runat="server" Width="100%" Height="32px" placeholder="Party Code" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                              </div>
                                   <div class="col-sm-5">
                                       <asp:TextBox ID="txtaname" runat="server" Width="100%" Height="32px" placeholder="Party Name" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                   </div>
                                    </div>


                           </div>
                       </div>
                   </div>

                    <div class="col-md-6">
                              <div>
                                  <div class="box-body">
                                     
                                       <div class="form-group">
                                          <label id="vcher" runat="server" class="col-sm-3 control-label">Vch Type/Name</label>
                                          <div class="col-sm-3">
                                          <asp:TextBox ID="txttype0" runat="server" Width="100%" Height="32px" placeholder="Voucher Type"
                                                ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                          </div>
                                          <div class="col-sm-6">
                                            <asp:TextBox ID="txttypename0" runat="server" Width="100%" Height="32px" placeholder="Voucher Name"
                                                 ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                          </div>
                                          </div>

                                      <div class="form-group">
                                        <label id="Label1" runat="server" class="col-sm-3 control-label">Mrr Type/Name</label>
                                        <div class="col-sm-3">
                                        <asp:TextBox ID="txttype" runat="server" Width="100%" Height="32px" placeholder="MRR Type"
                                                 ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-6">
                                        <asp:TextBox ID="txttypename" runat="server" Width="100%" Height="32px" placeholder="MRR Type"
                                                  ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                        </div>
                                      </div>
                                             
                                       <div class="form-group">
                                                <label id="Label2" runat="server" class="col-sm-3 control-label">Bill NO./Date</label>
                                                <div class="col-sm-3">
                                                <asp:TextBox ID="txtbillno" runat="server" Width="100%" Height="32px" placeholder="Bill No"
                                                 ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                                </div>
                                                <div class="col-sm-2">
                                                <asp:TextBox ID="txtbilldt" runat="server" Width="100%" Height="32px" placeholder="Bill Date"
                                                 ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                                </div>
                                                <label id="Label3" runat="server" class="col-sm-2 control-label">Amount</label>
                                                <div class="col-sm-2">
                                                <asp:TextBox ID="txtamt" runat="server" Width="100%" Height="32px" placeholder="Amount"
                                                 ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                                </div>
                                             </div>

                                      </div>
                                  </div>
                        </div> 

                    <div class="col-md-12">
                        <div>
                           <div class="box-body" style="background:url('../tej-base/images/fm-bg1.jpg') 200% 80%;">
                               <div class="form-group">
                                     <label id="Label4" runat="server" class="col-sm-3 control-label">View MRR Detail</label>
                                    <div class="col-sm-1">
                                        <asp:ImageButton ID="btnvchnum" runat="server" ToolTip="Preview MRR"
                ImageUrl="~/tej-base/css/images/info.png" ReadOnly="true"
                        style="width:25px; height:25px;" onclick="btnvchnum_Click" />
                                    </div>
                                    <label id="Label5" runat="server" class="col-sm-3 control-label">View PO Detail</label>
                                   <div class="col-sm-1">
                                       <asp:ImageButton ID="btnpodetails" runat="server" ToolTip="Preview PO Details"
                ImageUrl="~/tej-base/css/images/info.png" ReadOnly="true"
                        style="width:25px; height:25px;" onclick="btnpodetails_Click"/>
                                   </div>
                                   <label id="Label6" runat="server" class="col-sm-3 control-label">View Voucher Detail</label>
                                  <div class="col-sm-1">
                                      <asp:ImageButton ID="btnvchdetails" runat="server" ToolTip="Preview Voucher Details"
                ImageUrl="~/tej-base/css/images/info.png" ReadOnly="true"
                        style="width:25px; height:25px; " onclick="btnvchdetails_Click" />
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
                            </ul>
                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane active" id="DescTab">
                                    <div class="lbBody" style="color: White; max-height: 300px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <asp:GridView ID="sg1" Width="100%" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" AutoGenerateColumns="false"  onrowdatabound="sg1_RowDataBound" onrowcommand="sg1_RowCommand">
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#1797C0" Font-Bold="True" ForeColor="White"
                                                CssClass="GridviewScrollHeader" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                            <Columns>
                                                <asp:TemplateField>
                                                <HeaderTemplate>R</HeaderTemplate>
                                                <ItemTemplate>
                                                 <asp:ImageButton ID="btnrmv" runat="server" CommandName="Rmv" ImageUrl="~/tej-base\images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove It" />                                               
                                                     </ItemTemplate>
                                                    <ItemStyle Width="30px" />            
                                                 </asp:TemplateField>

                                              <asp:BoundField DataField="srno" HeaderText="Srno" ReadOnly="True" >
                                                <ItemStyle Width="10px" />
                                              </asp:BoundField>
                                                 
                                                <asp:TemplateField>
                                                <HeaderTemplate>D</HeaderTemplate>
                                                 <ItemTemplate>
                                                 <asp:ImageButton ID="btndnlwd" runat="server" CommandName="Dwl" CommandArgument='<%# Eval("FILNO") %>' ImageUrl="~/tej-base\images/save.png" Width="22px" ImageAlign="Middle" ToolTip="Download file" />
                                                 </ItemTemplate>
                                                    <ItemStyle Width="30px" />
                                                  </asp:TemplateField>

                                                <asp:TemplateField>
                                                <HeaderTemplate>V</HeaderTemplate>
                                                <ItemTemplate>
                                                <asp:ImageButton ID="btnview" runat="server" CommandName="View" CommandArgument='<%# Eval("FILNO") %>' ImageUrl="~/tej-base\images/preview-file.png" Width="20px" ImageAlign="Middle" ToolTip="View file" />
                                                </ItemTemplate>
                                                <ItemStyle Width="30px" />
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="filno" HeaderText="File Name" ReadOnly="True" >
                                                </asp:BoundField>  
                                                                                                                                             
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>


                   </div>
         </section>
      </div>
  

  <%--  <div class="bSubBlock brandSecondaryBrd secondaryPalette" align="center" style="background-image: url('css/images/bgTop.gif'); ">
<%--<h3 class="lbHeader" align="center" >
            <button id="btnview1" runat="server" accesskey="v" class="myButton" onserverclick="btnview_ServerClick"><u>V</u>iew</button>
            <button id="btnlist1" runat="server" accesskey="t" class="myButton" onserverclick="btnlist_ServerClick">Lis<u>t</u></button>
            <button id="btnapr1" runat="server" accesskey="a" class="myButton" onserverclick="btnapr_ServerClick"><u>A</u>pprove</button>
            <button id="btncan1" runat="server" accesskey="c" class="myButton" onserverclick="btncan_ServerClick"><u>C</u>ancel</button>
            <button id="btnext1" runat="server" accesskey="x" class="myButton" onserverclick="btnext_ServerClick">E<u>x</u>it</button>

</h3>
         <div class="toolsContentLeft">
                   <div class="bSubBlock brandSecondaryBrd secondaryPalette" style="background-image: url('css/images/bgTop.gif');">
                <%--<div align="left" style="color: #1797c0; background-image: url(images/bgTop.gif); font-size: medium; font-weight: bold;">
                                                    &nbsp;<asp:Image ID="Image1" runat="server" Height="24px" ImageUrl="~/css/images/transfer.jpg" Width="28px" />
                                                    &nbsp;Voucher Approval</div>
<div class="toolsContentLeft">
            <div class="bSubBlock brandSecondaryBrd secondaryPalette">
                <div class="lbBody">
                <table width="100%">
                <tr >
                <td>Vch No. / Date</td>
                <td class="style2">
                <asp:TextBox ID="txtvchnum01" runat="server" Width="70px" placeholder="Voucher No"
                        ReadOnly="true"></asp:TextBox>
                                                                     </td>
                <td colspan="4"><asp:TextBox ID="txtvchdt01" runat="server" Width="80px"
                        placeholder="Voucher Date" ReadOnly="true"></asp:TextBox></td>
                <td>Vch Type / Name</td>
                <td class="style3">
                <asp:TextBox ID="txttype01" runat="server" Width="70px" ReadOnly="true"
                        placeholder="Type" ></asp:TextBox>
                                                                     </td>
                <td colspan="3"><asp:TextBox ID="txttypename01" runat="server" Width="90%"
                        ReadOnly="true" placeholder="Type Name"></asp:TextBox></td>
                </tr>
                <tr style="background-color: #CDE8F0">
                <td>MRR No.
                    / Date</td>
                <td class="style2">
                <asp:TextBox ID="txtvchnum1" runat="server" Width="70px" placeholder="MRR No" ReadOnly="true"></asp:TextBox>
                </td>
                <td colspan="4"><asp:TextBox ID="txtvchdt1" runat="server" Width="80px" placeholder="MRR Date" ReadOnly="true"></asp:TextBox></td>
                <td>MRR Type
                    / Name</td>
                <td class="style3">
                <asp:TextBox ID="txttype1" runat="server" Width="70px" ReadOnly="true" placeholder="Type" ></asp:TextBox>
                </td>
                <td colspan="3"><asp:TextBox ID="txttypename1" runat="server" Width="90%" ReadOnly="true" placeholder="Type Name"></asp:TextBox></td>
                </tr>
                <tr>
                <td>Party Code / Name</td>
                <td class="style2">
                    <asp:TextBox ID="txtacode1" runat="server" Width="70px" ReadOnly="true" placeholder="Code" ></asp:TextBox>
                    </td>
                <td colspan="4" >
                <asp:TextBox ID="txtaname1" runat="server" Width="300px" ReadOnly="true" placeholder="Name" ></asp:TextBox>
                </td>
                <td>Bill No. / Date</td>
                <td class="style3">
                <asp:TextBox ID="txtbillno1" runat="server" Width="70px" placeholder="Bill No" ReadOnly="true"></asp:TextBox>
                    </td>
                <td><asp:TextBox ID="txtbilldt1" runat="server" Width="80px" ReadOnly="true"
                        placeholder="Bill Date"></asp:TextBox></td>
                <td>Amount</td>
                <td><asp:TextBox ID="txtamt1" runat="server" Width="80px" style="text-align:right" ReadOnly="true"
                        placeholder="Amount"></asp:TextBox></td>
                </tr>

                <tr style="background-color: #CDE8F0">
                <td>View MRR Details</td>
                <td class="style2">
                    <asp:ImageButton ID="btnvchnum1" runat="server" ToolTip="Preview MRR"
                ImageUrl="~/css/images/info.png" ReadOnly="true"
                        style="width:25px; height:25px;" onclick="btnvchnum_Click" />
                    </td>
                <td class="style4" >
                    View PO Detail</td>
                <td >
                    <asp:ImageButton ID="btnpodetails1" runat="server" ToolTip="Preview PO Details"
                ImageUrl="~/css/images/info.png" ReadOnly="true"
                        style="width:25px; height:25px;" onclick="btnpodetails_Click"/>
                </td>
                <td >
                    View Voucher Details</td>
                <td class="style5" >
                    <asp:ImageButton ID="btnvchdetails1" runat="server" ToolTip="Preview Voucher Details"
                ImageUrl="~/css/images/info.png" ReadOnly="true"
                        style="width:25px; height:25px; " onclick="btnvchdetails_Click" />
                </td>
                <td>&nbsp;</td>
                <td class="style3">
                    &nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                </tr>

                <tr>
                <td class="style1" colspan="11">
                <div class="lbBody" style="color:White; height:180px; max-height:180px; overflow:auto; box-shadow:0 2px 4px rgba(127,127,127,.3);box-shadow:inset 0 0 3px #387bbe,0 0 9px #387bbe;">
        <asp:GridView ID="sg1" runat="server" Width="100%" AutoGenerateColumns="false"
CellPadding="2" ForeColor="#333333"
        GridLines="Both" style="background-color: #FFFFFF; color:White; font-size: small;"
                        onrowdatabound="sg1_RowDataBound" onrowcommand="sg1_RowCommand"  >
<RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem"  />
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager"/>
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader" />
        <EditRowStyle BackColor="#999999"  />
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
<HeaderStyle BackColor="#1797C0" ForeColor="White" Height="20px"/>
<RowStyle CssClass="grdrow" />
<Columns>
<asp:TemplateField>
<HeaderTemplate>R</HeaderTemplate>
<ItemTemplate>
<asp:ImageButton ID="btnrmv" runat="server" CommandName="Rmv" ImageUrl="~/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove It" />
    </ItemTemplate>
<ItemStyle Width="11px" />
</asp:TemplateField>
<asp:BoundField DataField="srno" HeaderText="Srno" ReadOnly="True" >
    <ItemStyle Width="10px" />
    </asp:BoundField>
    <asp:TemplateField>
<HeaderTemplate>D</HeaderTemplate>
<ItemTemplate>
<asp:ImageButton ID="btndnlwd" runat="server" CommandName="Dwl" CommandArgument='<%# Eval("FILNO") %>' ImageUrl="~/images/save.png" Width="22px" ImageAlign="Middle" ToolTip="Download file" />
    </ItemTemplate>
<ItemStyle Width="11px" />
</asp:TemplateField>
<asp:TemplateField>
<HeaderTemplate>V</HeaderTemplate>
<ItemTemplate>
<asp:ImageButton ID="btnview" runat="server" CommandName="View" CommandArgument='<%# Eval("FILNO") %>' ImageUrl="~/images/preview-file.png" Width="20px" ImageAlign="Middle" ToolTip="View file" />
    </ItemTemplate>
<ItemStyle Width="11px" />
</asp:TemplateField>

    <asp:BoundField DataField="filno" HeaderText="File Name" ReadOnly="True" >
    </asp:BoundField>
</Columns>
            </asp:GridView>
         </div>
                </td>
                </tr>
                </table>
                </div>
                </div>
                </div>
                </div>
                </div>
                </div>--%>
<asp:Button ID="btnhideF" runat="server" onclick="btnhideF_Click" style="display:none" />
<asp:Button ID="btnhideF_s" runat="server" OnClick="btnhideF_s_Click" style="display:none" />
<asp:HiddenField ID="hffield" runat="server" />
<asp:HiddenField ID="edmode" runat="server" />
    <asp:HiddenField ID="hf_form_mode" runat="server" />
</asp:Content>