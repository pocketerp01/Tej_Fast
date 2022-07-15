<%--<%@ Page Language="C#" MasterPageFile="~/FinCRM.master" AutoEventWireup="true" CodeFile="gstList.aspx.cs"
    Inherits="gstList" %>--%>
<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="gstList" Title="Tejaxo" CodeFile="gstList.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" src="Scripts/gridviewScroll.min.js"></script>

    <link rel="Stylesheet" href="css/table.css" />

 <%--   <script type="text/javascript" language="javascript">
        $(document).ready(function() {
            gridviewScroll();
        });

        function gridviewScroll() {
            $('#<%=GridView1.ClientID%>').gridviewScroll({
                width: 1250,
                height: 600
            });
        }
    </script>--%>

    <style type="text/css">
        .style7
        {
            height: 24px;
        }
        .vandana
        {
            width: 50px;
        }
        .vandana input
        {
            width: 50px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
       <div class="content-wrapper">
      <section class="content-header">
   <%-- <div class="bSubBlock brandSecondaryBrd secondaryPalette" align="center" style="background-image: url('css/images/bgTop.gif');">
    <table width="100%">    --%>
           <table style="width: 100%">
        <tr>
        <td>
       <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label></td>
            <td style="text-align:center">
            <button type="submit" id="btntrans" accesskey="d"  class="btn btn-info"  runat="server" onserverclick="btntrans_Click" style="height:30px"> Detailed Report    </button>
            <button type="submit" id="btnRep2" accesskey="a"  class="btn btn-info"  runat="server" onserverclick="btnRep2_Click" style="height:30px">Summ. Report (Free) </button>
            <button type="submit" id="btnRep4" accesskey="b"  class="btn btn-info"  runat="server" onserverclick="btnRep4_Click"  style="height:30px">Summ. Report (Paid)</button>
            <button type="submit" id="btnRep3"  class="btn btn-info"  runat="server" onserverclick="btnRep3_Click" visible="false"> 27th June Report</button>
            <button type="submit" id="btnSendEmail"  class="btn btn-info"  runat="server" onserverclick="btnSendEmail_Click"  style="height:30px">Send Email</button>
            <button type="submit" id="btnexit"  class="btn btn-info"  runat="server" style="display: none" onserverclick="btnexit_Click"> Exit</button>
           </td>
                </tr>
            </table>
        </section>    

     <section class="content">
   

       <%-- <div class="toolsContentLeft">
            <div class="bSubBlock brandSecondaryBrd secondaryPalette" style="background-image: url('css/images/bgTop.gif');">
                <%--<div align="left" style="color: #1797c0; background-image: url(images/bgTop.gif);
                    font-size: medium; font-weight: bold;">
                    &nbsp;<asp:Image ID="Image1" runat="server" Height="24px" Width="28px" ImageUrl="~/css/images/app.jpg" />
                    &nbsp;<asp:Label ID="lblhead" runat="server"></asp:Label>
                </div>--%>


                <table style="width: 100%;">
                    <tr>
                        <td colspan="4">
                            <div id="alermsg" style="color: #f00; font-weight: bold; font-size: medium; text-align: center;
                                display: none;" runat="server" />
                        </td>
                    </tr>
                </table>
               
                <div class="col-md-12">
                    <div>
                        <div class="box-body">                                    
                                 <div class="form-group">                              
                                <div class="col-sm-4">
                                   <asp:TextBox ID="txtfrom" runat="server" CssClass="textboxStyle" onblur="Change(this, event)"
                                            onfocus="Change(this, event)" placeholder="User Name" ReadOnly="True" TabIndex="-1"
                                            Style="display: none"></asp:TextBox>
                                </div>

                                      <div class="col-sm-8">                                       
                                        <asp:ImageButton ID="btnfrom" runat="server" Height="22px" ImageAlign="Middle" ImageUrl="~/css/images/bdsearch5.png"
                                            ToolTip="To" Width="24px" OnClick="btnfrom_Click" Style="display: none" />
                                     </div></div>

                                  <div class="form-group">                              
                                <div class="col-sm-2" style="display:none">
                                        <asp:TextBox ID="txtto" runat="server" CssClass="textboxStyle" onblur="Change(this, event)"
                                            onfocus="Change(this, event)" placeholder="Contact Person" ReadOnly="True" TabIndex="-1"
                                            Style="display: none"></asp:TextBox>
                                    </div>
                                        <div class="col-sm-7" style="display:none">
                                        <asp:ImageButton ID="btnto" runat="server" Height="22px" ImageAlign="Middle" ImageUrl="~/css/images/bdsearch5.png"
                                            ToolTip="To" Width="24px" OnClick="btnto_Click" Style="display: none" />
                                            </div>
                                       <div class="col-sm-3">
                                        <input id="txtrows" style="width: 120px;" type="text" placeholder="No. of rows" runat="server" />
                                          <%-- </div>
                                        <div class="col-sm-3">--%>
                                        <asp:Button ID="btnshow" runat="server" CssClass="searchbtn" Text="Show" OnClick="btnshow_Click" />
                                        <asp:Label ID="lblshow"  runat="server" Text="0"></asp:Label>
                                     </div>
                                      </div>
                               
                             <div class="form-group" style="display:none">                                                                         
                                        <asp:TextBox ID="txtbcode" runat="server" Style="display: none;"></asp:TextBox>
                                        <asp:TextBox ID="txtbname" runat="server" CssClass="textboxStyle" onblur="Change(this, event)"
                                            onfocus="Change(this, event)" placeholder="Branch Name" ReadOnly="True" TabIndex="-1"
                                            Width="773px" Style="display: none"></asp:TextBox>
                                        <asp:ImageButton ID="btnmbr" runat="server" Height="22px" ImageAlign="Middle" ImageUrl="~/css/images/bdsearch5.png"
                                            ToolTip="Branch Name" Width="24px" OnClick="btnmbr_Click" />
                                   </div>     

                              </div>
                </div>
            </div>

                            <div class="col-md-12">
                    <div>
                        <div class="box-body">      

                                 <div class="form-group">                     
                                   <div class="col-sm-12">
                                        <asp:TextBox ID="txtsearch" runat="server" CssClass="textboxStyle" onblur="Change(this, event)"
                                            onfocus="Change(this, event)" placeholder="Search here..." TabIndex="-1" Width="700px"></asp:TextBox>
                                        <asp:Button ID="btnsearch" runat="server" CssClass="searchbtn" Text="Search" OnClick="btnsearch_Click"
                                            ToolTip="click here to search" />                                    
                                        <asp:Button ID="btnexp" runat="server" CssClass="searchbtn" OnClick="btnexp_Click"
                                            Text="Export" ToolTip="click here to export data" />
                                        <asp:Label ID="txtTotDel" runat="server" Style="float: right"></asp:Label><br /><asp:Label ID="txtFreeDel" runat="server" Style="float: right"></asp:Label>                                     
                                    </div>
                              
                              </div>
                </div>
            </div> </div>                                
          <div class="col-md-12" style="display:none">                                                            
                                    <div class="lbBody" id="gridDiv1" style="color: White; height: 350px; overflow: hidden; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">

                                                <asp:GridView ID="GridView2" Width="100%" runat="server" CellPadding="2" ForeColor="#333333"
                                                    GridLines="Both" Style="border-color: #E2DED6; background-color: #FFFFFF; color: White;"
                                                    AutoGenerateColumns="true" OnRowDataBound="GridView1_RowDataBound">
                                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                    <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                    <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader" />
                                                    <EditRowStyle BackColor="#999999" />
                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                           <%--  <asp:GridView ID="GridView1" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="100%" Font-Size ="13px" 
                                            AutoGenerateColumns="False" OnRowDataBound="GridView1_RowDataBound">
                                           
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />--%>

                                                    <Columns>
                                                        <asp:TemplateField HeaderText="View">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="LnkBtnv" runat="server" ToolTip="View , see the RFQ in Print format"
                                                                    OnClick="LnkBtnv_Click" ForeColor="#1797c0">View</asp:LinkButton>
                                                                &nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:LinkButton ID="LnkBtnd" runat="server" ToolTip="Download the link" OnClick="LnkBtnd_Click"
                                                                    ForeColor="#1797c0">Download</asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="PStatus" ItemStyle-Width="50px">
                                                            <ItemStyle BackColor="YellowGreen" />
                                                            <ItemTemplate>
                                                                <div class="vandana">
                                                                    <asp:CheckBox ID="chkok" runat="server" ToolTip="Select RFQ for approval" style="display:none" />
                                                                    <asp:RadioButton ID="radOk" runat="server" ToolTip="Payment Rcvd" AutoPostBack="true" OnCheckedChanged="radOk_Click" />
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Mail" ItemStyle-Width="50px">
                                                            <ItemStyle BackColor="Red" />
                                                            <ItemTemplate>
                                                                <div class="vandana">
                                                                    <asp:CheckBox ID="chkno" runat="server" ToolTip="Select RFQ for refusal" />
                                                                                             </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Date">
                                                            <ItemTemplate>
                                                                <asp:TextBox runat="server" ID="txtdate" placeholder="dd/mm/yyyy" Width="80px"></asp:TextBox>
                                                                <cc1:MaskedEditExtender ID="MEE1" runat="server" Mask="99/99/9999" MaskType="Date"
                                                                    TargetControlID="txtdate" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Time Out">
                                                            <ItemTemplate>
                                                                <asp:TextBox runat="server" ID="txttout" placeholder="Time Out" Width="80px" ReadOnly="true"></asp:TextBox>
                                                                <cc1:MaskedEditExtender ID="me1" runat="server" Mask="99:99" MaskType="Time" UserTimeFormat="TwentyFourHour"
                                                                    TargetControlID="txttout">
                                                                </cc1:MaskedEditExtender>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Reason">
                                                            <ItemTemplate>
                                                                <asp:TextBox runat="server" ID="txtrsn" placeholder="Reason" Width="200px" TextMode="MultiLine"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>

                                              <%-- <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />--%>
                                        </asp:GridView>
                                         </div>
                                </div>     
         
           <div class="col-md-12">             
                                           
                                        <div class="lbBody" id="gridDiv" style="color: White; box-shadow: 0 2px 4px rgba(127,127,127,.3);">                          
                                     <fin:CoolGridView ID="GridView1" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="100%" Height="400px" Font-Size="13px"
                                            AutoGenerateColumns="False" OnRowDataBound="GridView1_RowDataBound"
                                           >
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />

                                      <Columns>
                                                        <asp:TemplateField HeaderText="View">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="LnkBtnv" runat="server" ToolTip="View , see the RFQ in Print format"
                                                                    OnClick="LnkBtnv_Click" ForeColor="#1797c0">View</asp:LinkButton>
                                                                &nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:LinkButton ID="LnkBtnd" runat="server" ToolTip="Download the link" OnClick="LnkBtnd_Click"
                                                                    ForeColor="#1797c0">Download</asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="PStatus" ItemStyle-Width="10px">
                                                            <ItemStyle BackColor="YellowGreen" />
                                                            <ItemTemplate>
                                                                <div class="vandana">
                                                                    <asp:CheckBox ID="chkok" runat="server" ToolTip="Select RFQ for approval" style="display:none" />
                                                                    <asp:RadioButton ID="radOk" runat="server" ToolTip="Payment Rcvd" AutoPostBack="true" OnCheckedChanged="radOk_Click" />
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Mail" ItemStyle-Width="10px">
                                                            <ItemStyle BackColor="Red" />
                                                            <ItemTemplate>
                                                                <div class="vandana">
                                                                    <asp:CheckBox ID="chkno" runat="server" ToolTip="Select RFQ for refusal" />
                                                                                             </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Date">
                                                            <ItemTemplate>
                                                                <asp:TextBox runat="server" ID="txtdate" placeholder="dd/mm/yyyy" Width="80px"></asp:TextBox>
                                                                <cc1:MaskedEditExtender ID="MEE1" runat="server" Mask="99/99/9999" MaskType="Date"
                                                                    TargetControlID="txtdate" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Time Out">
                                                            <ItemTemplate>
                                                                <asp:TextBox runat="server" ID="txttout" placeholder="Time Out" Width="80px" ReadOnly="true"></asp:TextBox>
                                                                <cc1:MaskedEditExtender ID="me1" runat="server" Mask="99:99" MaskType="Time" UserTimeFormat="TwentyFourHour"
                                                                    TargetControlID="txttout">
                                                                </cc1:MaskedEditExtender>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Reason">
                                                            <ItemTemplate>
                                                                <asp:TextBox runat="server" ID="txtrsn" placeholder="Reason" Width="200px" TextMode="MultiLine"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                <asp:BoundField DataField="sg1_f5" HeaderText="FSTR"  />
                                                <asp:BoundField DataField="sg1_f6" HeaderText="Type"  />
                                                <asp:BoundField DataField="sg1_f7" HeaderText="Comp_Code" />
                                                <asp:BoundField DataField="sg1_f8" HeaderText="Comp_Name"  />
                                                <asp:BoundField DataField="sg1_f9" HeaderText="No_of_Del"  />
                                                <asp:BoundField DataField="sg1_f10" HeaderText="Amt"  />
                                                <asp:BoundField DataField="sg1_f11" HeaderText="Contact_Per"  />
                                                <asp:BoundField DataField="sg1_f12" HeaderText="Email_Id"   />
                                                <asp:BoundField DataField="sg1_f13" HeaderText="Mobile" />
                                                <asp:BoundField DataField="sg1_f14" HeaderText="Delegate_Name" />
                                                <asp:BoundField DataField="sg1_f15" HeaderText="Designation"  />
                                                <asp:BoundField DataField="sg1_f16" HeaderText="Delegate_Mobile" />
                                                <asp:BoundField DataField="sg1_f17" HeaderText="Srno"  />
                                                <asp:BoundField DataField="sg1_f18" HeaderText="Email_Sent" />
                                                <asp:BoundField DataField="sg1_f19" HeaderText="Pstatus"  />
                                          <asp:BoundField DataField="sg1_f20" HeaderText="Attend_By" />
                                                <asp:BoundField DataField="sg1_f21" HeaderText="Attend_Dt"  />
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
           <div class="col-md-12" style="display:none;"> 
              <div id="datadiv" style="overflow: scroll; width: auto; height: 420px;" runat="server" class="handsontable" data-originalstyle="width: auto; height: 420px; overflow: scroll"> </div>                                                                                                                  
        </div>  
                </section>     
    </div>
    <asp:HiddenField ID="hfbtnmode" runat="server" />    
        <asp:HiddenField ID="hf1" runat="server" />
     <asp:HiddenField ID="hfqry" runat="server" />
    <CR:CrystalReportViewer ID="CRV1" runat="server" AutoDataBind="true" Visible="false" />               <%--  --%>
    <input type="button" id="btnhideF" runat="server" onserverclick="btnhideF_Click"
        style="display: none" />
</asp:Content>
