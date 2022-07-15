<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="drawdest" Title="Destroy Entry" CodeFile="drawdest.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	
	

	<script type="text/javascript">
        //$(function() {
        //    $('input:text:first').focus();
        //    var $inp = $('input:text');
	    //    $inp.live('keysdown', function(e) {
        //        var key = (e.keyCode ? e.keyCode : e.charCode);
        //        if (key == 13) {
        //            e.preventDefault();
        //            var nxtIdx = $inp.index(this) + 1;
        //            $(":input:text:eq(" + nxtIdx + ")").focus();
        //        }
        //    });
	    //});

	    function openfileDialog() {
	        $("#Attch").click();
	    }
	    function submitFile() {
	        $("#<%= btnAtt.ClientID%>").click();
        };
    </script>
	
   
    
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">  
          
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
<%--                        <button type="submit" id="btnprint" class="btn btn-info" style="width: 100px;" runat="server" accesskey="P" onserverclick="btnprint_ServerClick"><u>P</u>rint</button>--%>
                        <button type="submit" id="btndel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="l" onserverclick="btndel_ServerClick">De<u>l</u>ete</button>
                        <button type="submit" id="btnlist" class="btn btn-info" style="width: 100px;" runat="server" accesskey="t" onserverclick="btnlist_ServerClick">Lis<u>t</u></button>
                        <button type="submit" id="btncancel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="c" onserverclick="btncancel_ServerClick"><u>C</u>ancel</button>
                        <button type="submit" id="btnexit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="X" onserverclick="btnexit_ServerClick">E<u>x</u>it</button>
                    </td>
                </tr>
            </table>
        </section>

  <%--  <div class="bSubBlock brandSecondaryBrd secondaryPalette" align="center" style="background-image: url('css/images/bgTop.gif'); ">
    <h3 class="lbHeader" align="center" >

     <button id="btnnew"  accesskey="N" class="frmbtn"  runat="server" onserverclick="btnnew_Click"  ><u>N</u>ew</button>
     <button id="btnedit"  accesskey="i" class="frmbtn"  runat="server"  onserverclick="btnedit_Click"  >Ed<u>i</u>t</button>
     <button id="btnsave"  accesskey="S" class="frmbtn"  runat="server" onserverclick="btnsave_Click"  ><u>S</u>ave</button>
     
     <button id="btndelete"  accesskey="l" class="frmbtn"  runat="server" onserverclick="btndelete_Click" >De<u>l</u>ete</button>
     <button id="btnlist"  accesskey="t" class="frmbtn"  runat="server"  onserverclick="btnlist_Click"  >Lis<u>t</u></button>
     <button id="btnexit"  class="frmbtn"  runat="server" onserverclick="btnexit_Click"  >Exit</button></h3>
        <br />--%>
        


        <%--<div class="toolsContentLeft">
            <div class="bSubBlock brandSecondaryBrd secondaryPalette">
                <div class="lbBody">
                <div style="color:#1797c0; background-image:url(images/bgTop.gif); font-size: medium; font-weight: bold;" 
                        align="left">&nbsp;<asp:Image ID="Image1" runat="server" Height="24px" 
                        ImageUrl="~/css/images/draw.png" Width="22px" />
                    &nbsp;Drawing Destroy Entry
<a  style="float:right;" class="tooltip" >
   <img src="css/images/info.png" style="width:30px; height:30px" />
   <span style="align:center;">   
       <img src="css/images/bdsearch5.png"/> Search 
    <br />
     <img src="css/images/icon.jpg"/> Create  
       <br /> 
       <img src="css/images/mandatory.gif"/>&nbsp;&nbsp;&nbsp; Mandatory Fields</span>    
</a>                    </div>--%>

          <section class="content">
            <div class="row">
                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                              <div class="form-group">
                                <asp:Label ID="lbl1" runat="server" Text="lbl1" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Entry_No.</asp:Label>
                                <div class="col-sm-1">
                                    <asp:Label ID="lbl1a" runat="server" Text="DD" Style="width: 22px; float: right;" CssClass="col-sm-1 control-label"></asp:Label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtdocno" runat="server" Width="100%" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label18" runat="server" Text="lbl1" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Date</asp:Label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtdate" placeholder="Date" runat="server" Width="100%" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtdate_CalendarExtender" runat="server" Enabled="True" TargetControlID="txtdate" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="Maskedit1" runat="server" Mask="99/99/9999" MaskType="Date" TargetControlID="txtdate" />
                                </div>
                            </div>

                             <div class="form-group">
                                <asp:Label ID="Label19" runat="server" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">EDN</asp:Label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnedn" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnedn_Click" />
                                </div>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtedn" ReadOnly="true" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>
                                     <div class="col-sm-3"style="display:none;">
                                  <asp:Label ID="Label12" runat="server" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">EDN_Date</asp:Label>
                                    <asp:TextBox ID="txtedate" ReadOnly="true" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="Label1" runat="server" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Drawing_No.</asp:Label>                               
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtdno" ReadOnly="true" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>                                     
                            </div>

                             <div class="form-group">
                                <asp:Label ID="Label2" runat="server" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Drawing_Type</asp:Label>                               
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtdtype" ReadOnly="true" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>                                     
                            </div>
                             </div>
                         </div>
                     </div> 
       
                   <div class="col-md-6">
                    <div>
                        <div class="box-body">
                             <div class="form-group">
                                <asp:Label ID="Label3" runat="server" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Revision_No.</asp:Label>                               
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtrno" ReadOnly="true" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>                                     
                            </div>

                              <div class="form-group">
                                <asp:Label ID="Label4" runat="server" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Destroy_By</asp:Label>
                                <div class="col-sm-1" style="display:none;">
                                    <asp:ImageButton ID="btndby" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btndby_Click" />
                                </div>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtdby" ReadOnly="true" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>                                     
                            </div>

                          <%--       <div class="form-group">
                                <asp:Label ID="Label5" runat="server" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Destroy_By</asp:Label>
                                <div class="col-sm-1" style="display:none;">
                                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnedn_Click" />
                                </div>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="TextBox1" ReadOnly="true" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>                                     
                            </div>--%>

                              <div class="form-group">
                                <asp:Label ID="Label10" runat="server" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Created_By</asp:Label>                               
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtpre" ReadOnly="true" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>  
                                </div>  

                                    <div class="form-group">
                                      <asp:Label ID="Label11" runat="server" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Modified_By</asp:Label>                               
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtedit" ReadOnly="true" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>                                  
                            </div>

                             </div>
                         </div>
                     </div> 

                  <div class="col-md-12">
                <div>
                    <div class="box-body">
                        <div class="form-group">
                            <table>
                                <tr id="attch1" runat="server">
                                    <td>
                                        <asp:FileUpload ID="Attch" runat="server" Visible="true" onchange="submitFile()" /></td>
                                    <td>
                                        <asp:TextBox ID="txtAttch" runat="server" MaxLength="100" placeholder="Path Upto 100 Char"></asp:TextBox></td>
                                </tr>
                            </table>
                            <asp:Button ID="btnAtt" runat="server" Text="File" OnClick="btnAtt_Click" Width="50px" />
                            <%--Style="display: none"--%>

                            <asp:Label ID="lblShow" runat="server"></asp:Label>
                            <asp:Label ID="lblUpload" runat="server"></asp:Label>

                            <asp:ImageButton ID="btnView1" runat="server" ImageUrl="~/tej-base/images/preview-file.png" OnClick="btnView1_Click" Visible="false" />
                            <asp:ImageButton ID="btnDown" runat="server" ImageUrl="~/tej-base/images/Save.png" OnClick="btnDown_Click" Visible="false" />
                        </div>
                    </div>
                </div>
            </div>

                   <div style="display: none;">
                    <div class="lbBody" id="gridDiv" style="color: White; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                        <fin:CoolGridView ID="sg1" runat="server" ForeColor="#333333"
                            Style="background-color: #FFFFFF; color: White;" Width="100%" Height="375px" Font-Size="13px"
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
                                    <HeaderStyle Width="30px" />
                                    <ItemTemplate>
                                        <asp:ImageButton ID="sg1_btnadd" runat="server" CommandName="SG1_ROW_ADD" ImageAlign="Middle" ImageUrl="../tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Export Invoice" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>Del</HeaderTemplate>
                                    <HeaderStyle Width="30px" />
                                    <ItemTemplate>
                                        <asp:ImageButton ID="sg1_btnrmv" runat="server" CommandName="SG1_RMV" ImageUrl="../tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Export Invoice" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="sg1_srno" HeaderText="SrNo" HeaderStyle-Width="40px" />
                                <asp:BoundField DataField="sg1_f1" HeaderText="Height" />
                                <asp:BoundField DataField="sg1_f2" HeaderText="Width" />
                                <asp:BoundField DataField="sg1_f3" HeaderText="sg1_f3" />
                                <asp:BoundField DataField="sg1_f4" HeaderText="sg1_f4" />
                                <asp:BoundField DataField="sg1_f5" HeaderText="sg1_f5" />
                                <asp:BoundField DataField="sg1_f6" HeaderText="sg1_f6" />
                                <asp:BoundField DataField="sg1_f7" HeaderText="sg1_f7" />
                                <asp:BoundField DataField="sg1_f8" HeaderText="sg1_f8" />
                                <asp:BoundField DataField="sg1_f9" HeaderText="sg1_f9" />
                                <asp:BoundField DataField="sg1_f10" HeaderText="sg1_f10" />

                                <asp:TemplateField>
                                    <HeaderTemplate>P1</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t1" runat="server" Text='<%#Eval("sg1_t1") %>' onkeypress="return isDecimalKey(event)" onkeyup="cal()" Width="100%" ReadOnly="true"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>P2</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t2" runat="server" Text='<%#Eval("sg1_t2") %>' onkeypress="return isDecimalKey(event)" onkeyup="cal()" Width="100%" ReadOnly="true"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderTemplate>P3</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t3" runat="server" Text='<%#Eval("sg1_t3") %>' Width="100%" onkeypress="return isDecimalKey(event)" ReadOnly="true"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>P4</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t4" runat="server" Text='<%#Eval("sg1_t4") %>' Width="100%" onkeypress="return isDecimalKey(event)" ReadOnly="true"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>P5</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t5" runat="server" Text='<%#Eval("sg1_t5") %>' Width="100%" onkeypress="return isDecimalKey(event)" ReadOnly="true"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>P6</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t6" runat="server" Text='<%#Eval("sg1_t6") %>' onkeypress="return isDecimalKey(event)" onkeyup="cal()" Width="100%" ReadOnly="true"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>P7</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t7" runat="server" Text='<%#Eval("sg1_t7") %>' onkeypress="return isDecimalKey(event)" Width="100%" ReadOnly="true"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>P27</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t27" runat="server" Text='<%#Eval("sg1_t27") %>' onkeypress="return isDecimalKey(event)" Width="100%" ReadOnly="true"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>P8</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t8" runat="server" Text='<%#Eval("sg1_t8") %>' onkeypress="return isDecimalKey(event)" Width="100%" ReadOnly="true"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>P9</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t9" runat="server" Text='<%#Eval("sg1_t9") %>' onkeypress="return isDecimalKey(event)" Width="100%" ReadOnly="true"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>P10</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t10" runat="server" Text='<%#Eval("sg1_t10") %>' onkeypress="return isDecimalKey(event)" Width="100%" ReadOnly="true"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>P11</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t11" runat="server" Text='<%#Eval("sg1_t11") %>' onkeypress="return isDecimalKey(event)" Width="100%" ReadOnly="true"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>P12</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t12" runat="server" Text='<%#Eval("sg1_t12") %>' onkeypress="return isDecimalKey(event)" Width="100%" ReadOnly="true"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderTemplate>P13</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t13" runat="server" Text='<%#Eval("sg1_t13") %>' onkeypress="return isDecimalKey(event)" Width="100%" ReadOnly="true"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>P14</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t14" runat="server" Text='<%#Eval("sg1_t14") %>' onkeypress="return isDecimalKey(event)" Width="100%" ReadOnly="true"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>P15</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t15" runat="server" Text='<%#Eval("sg1_t15") %>' onkeypress="return isDecimalKey(event)" Width="100%" ReadOnly="true"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>sg1_t16</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t16" runat="server" Text='<%#Eval("sg1_t16") %>' onkeypress="return isDecimalKey(event)" Width="100%" ReadOnly="true"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>sg1_t17</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t17" runat="server" Text='<%#Eval("sg1_t17") %>' onkeypress="return isDecimalKey(event)" Width="100%"></asp:TextBox>
                                        <asp:CalendarExtender ID="sg1_t17_CalendarExtender" runat="server"
                                            Enabled="True" TargetControlID="sg1_t17"
                                            Format="dd/MM/yyyy">
                                        </asp:CalendarExtender>
                                        <asp:MaskedEditExtender ID="Maskedit7" runat="server" Mask="99/99/9999"
                                            MaskType="Date" TargetControlID="sg1_t17" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>sg1_t18</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t18" runat="server" Text='<%#Eval("sg1_t18") %>' onkeypress="return isDecimalKey(event)" onkeyup="cal()" Width="100%"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>sg1_t19</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t19" runat="server" Text='<%#Eval("sg1_t19") %>' onkeypress="return isDecimalKey(event)" onkeyup="cal()" Width="100%"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>sg1_t20</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t20" runat="server" Text='<%#Eval("sg1_t20") %>' onkeypress="return isDecimalKey(event)" onkeyup="cal()" Width="100%"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>sg1_t21</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t21" runat="server" Text='<%#Eval("sg1_t21") %>' onkeypress="return isDecimalKey(event)" onkeyup="cal()" Width="100%"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>sg1_t22</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t22" runat="server" Text='<%#Eval("sg1_t22") %>' onkeypress="return isDecimalKey(event)" onkeyup="cal()" Width="100%"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderTemplate>sg1_t23</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t23" runat="server" Text='<%#Eval("sg1_t23") %>' onkeypress="return isDecimalKey(event)" Width="100%" ReadOnly="true"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>sg1_t24</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t24" runat="server" Text='<%#Eval("sg1_t24") %>' onkeypress="return isDecimalKey(event)" Width="100%" ReadOnly="true"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>sg1_t25</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t25" runat="server" Text='<%#Eval("sg1_t25") %>' Width="100%"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>sg1_t26</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t26" runat="server" Text='<%#Eval("sg1_t26") %>' Width="100%"></asp:TextBox>
                                        <asp:CalendarExtender ID="sg1_t26_CalendarExtender" runat="server"
                                            Enabled="True" TargetControlID="sg1_t26"
                                            Format="dd/MM/yyyy">
                                        </asp:CalendarExtender>
                                        <asp:MaskedEditExtender ID="Maskedit8" runat="server" Mask="99/99/9999"
                                            MaskType="Date" TargetControlID="sg1_t26" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>sg1_t31</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t31" runat="server" Text='<%#Eval("sg1_t31") %>' Width="100%"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>sg1_t32</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t32" runat="server" Text='<%#Eval("sg1_t32") %>' onkeypress="return isDecimalKey(event)" Width="100%"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>sg1_t33</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t33" runat="server" Text='<%#Eval("sg1_t33") %>' onkeypress="return isDecimalKey(event)" Width="100%"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderTemplate>sg1_t34</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t34" runat="server" Text='<%#Eval("sg1_t34") %>' onkeypress="return isDecimalKey(event)" Width="100%"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>sg1_t35</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t35" runat="server" Text='<%#Eval("sg1_t35") %>' onkeypress="return isDecimalKey(event)" Width="100%"></asp:TextBox>
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
       </section>
    </div>


                     <%--  <table style="width:100%;">
                     <tr >
                                                       <td  colspan="4">
                                                       <div id="alermsg" style="color:#f00; font-weight:bold; font-size:medium; text-align:center; display:none;" runat="server"  />
                                                           </td>
                        </tr>
                        
                        <tr style="background-color: #CDE8F0">
                                                       <td >
                                                           Entry No.</td>
                            <td >
                                <asp:TextBox ID="txtdocno" runat="server" placeholder="Entry No." 
                                    MaxLength="6" ReadOnly="True" tabindex="-1"></asp:TextBox>
                                                       </td>
                            <td >
                                Date</td>
                            <td  >
                            <input name="txtdate" type="text" id="txtdate"  placeholder="dd/mm/yyyy" runat="server" readonly="readonly" tabindex="-1"/>
							</td>
                        </tr>--%>
                        
						<%--<tr>
                            <td >
                                EDN<span style="color:red">*</span></td>
                            <td colspan="3">							
                                <asp:TextBox ID="txtedn" runat="server"  
                                    placeholder="EDN" onfocus="Change(this, event)" 
                                    onblur ="Change(this, event)" ReadOnly="true"  ></asp:TextBox>									
									<input name="txtedate" type="text" id="txtedate"  placeholder="dd/mm/yyyy" runat="server" readonly="readonly" tabindex="-1" style="display:none;"/>									
									<asp:ImageButton ID="btnedn" runat="server" Height="22px" ImageUrl="~/css/images/bdsearch5.png" ToolTip="EDN" Width="24px" onclick="btnctye_Click" ImageAlign="Middle"/>                                    
                            </td>
                        </tr>--%>
						<%--<tr style="background-color: #CDE8F0">
                            <td >
                                Drawing No.<span style="color:red">*</span></td>
                            <td colspan="3">
                                <asp:TextBox ID="txtdno" runat="server"  
                                    placeholder="Drawing No." onfocus="Change(this, event)" 
                                    onblur ="Change(this, event)"  ReadOnly="true"  ></asp:TextBox>
									
									
                                    
                            </td>
                        </tr>--%>
                      <%--  <tr>
                            <td >
                                Drawing Type<span style="color:red">*</span></td>
                            <td colspan="3">
							
                                <asp:TextBox ID="txtdtype" runat="server"  
                                    placeholder="Drawing Type" onfocus="Change(this, event)" 
                                    onblur ="Change(this, event)" ReadOnly="true"  ></asp:TextBox>						 
                            </td>
                        </tr>--%>
						
						<%--<tr style="background-color: #CDE8F0">
                            <td >
                                Revision No.<span style="color:red">*</span></td>
                            <td colspan="3">
                                <asp:TextBox ID="txtrno" runat="server"  
                                    placeholder="Revision No." onfocus="Change(this, event)" 
                                    onblur ="Change(this, event)"  ReadOnly="true"  ></asp:TextBox>
								
                            </td>
                        </tr>--%>
						<%--<tr >
                            <td >
                                Destroy By<span style="color:red">*</span></td>
                            <td colspan="3">
                                <asp:TextBox ID="txtdby" runat="server"  
                                    placeholder="Destroy By" onfocus="Change(this, event)" 
                                    onblur ="Change(this, event)" ReadOnly="true"   ></asp:TextBox>
									
									<asp:ImageButton ID="btndby" runat="server" Height="22px" 
                                    ImageUrl="~/css/images/bdsearch5.png" ToolTip="Destroy By" Width="24px" 
                                    onclick="btnctye_Click" ImageAlign="Middle" style="display:none;" />
                                    
                            </td>
                        </tr>--%>
						<%--<tr>
                            <td  colspan="4"  >
								<asp:GridView ID="grd" runat="server" AutoGenerateColumns="false" 
                                    CellPadding="4" ForeColor="#333333" GridLines="nONE" 
                                    onrowdatabound="grd_RowDataBound" 
                                    style="background-color: #FFFFFF; color:White; display:none;" Width="100%">
                                    <RowStyle BackColor="#F7F6F3" CssClass="GridviewScrollItem" 
                                        ForeColor="#333333" />
                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#284775" CssClass="GridviewScrollPager" 
                                        ForeColor="White" />
                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle BackColor="#1797c0" CssClass="GridviewScrollHeader" 
                                        Font-Bold="True" ForeColor="White" />
                                    <EditRowStyle BackColor="#999999" />
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    <Columns>
                                        <asp:BoundField DataField="SRNO" HeaderText="Srno" ReadOnly="True" />
                                        <asp:TemplateField HeaderText="Preview">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="LnkGtn" runat="server" ForeColor="#1797c0" 
                                                    onclick="LnkGtn_Click" 
                                                    ToolTip="Click here to Preview(you can see only preview of PDF and IMGAE file)">Preview</asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Download">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="LnkGtn1" runat="server" ForeColor="#1797c0" 
                                                    onclick="LnkGtn1_Click" ToolTip="Click here to Download">Download</asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FILENAME" HeaderText="File Name" ReadOnly="True" />
                                        <asp:BoundField DataField="FILEPATH" HeaderText="File Path" ReadOnly="True" />
                                        <asp:BoundField DataField="FILETYPE" HeaderText="File Type" ReadOnly="True" />
                                    </Columns>
                                </asp:GridView>
                            </td>
							</tr>--%>
				<%--		<tr >
                            <td  >
                                Created By</td>
                            <td >
                                                           <asp:TextBox ID="txtpre" runat="server"  
                                    placeholder="Prepared By" onfocus
 ="Change(this, event)" onblur ="Change(this, event)"
                                    ReadOnly="true" ></asp:TextBox>
                                                                             </td>
                          <td>
                                
								Modified By
								</td>
                            <td>
                                <asp:TextBox ID="txtedit" runat="server"  
                                    placeholder="Modified By" onfocus
 ="Change(this, event)" onblur ="Change(this, event)"
                                    ReadOnly="true" ></asp:TextBox>
                                </td>
                            
                        </tr>
                        </table>--%>
      
    <asp:Button ID="btnhideF" runat="server" OnClick="btnhideF_Click" Style="display: none" />
    <asp:Button ID="btnhideF_s" runat="server" OnClick="btnhideF_s_Click" Style="display: none" />
    <asp:HiddenField ID="doc_nf" runat="server" />
    <asp:HiddenField ID="doc_df" runat="server" />
    <asp:HiddenField ID="doc_vty" runat="server" />
    <asp:HiddenField ID="doc_addl" runat="server" />
    <asp:HiddenField ID="edmode" runat="server" />
    <asp:HiddenField ID="hffield" runat="server" />
    <asp:HiddenField ID="hf1" runat="server" />
    <asp:HiddenField ID="lbledmode" runat="server" />
    <asp:HiddenField ID="lblname" runat="server" />
    <asp:HiddenField ID="HFOLDDT" runat="server" />
    <asp:HiddenField ID="HFOPT" runat="server" />
    <asp:HiddenField ID="hf2" runat="server" />
    <asp:HiddenField ID="hfdept" runat="server" />
    <asp:HiddenField ID="hfbtnmode" runat="server" />
    <asp:HiddenField ID="TabName" runat="server" />
    <asp:Button ID="btnOKTarget" runat="server" Text="!" OnClick="btnOKTarget_Click" Style="display: none;" />
    <asp:Button ID="btnCancelTarget" runat="server" Text="!" OnClick="btnCancelTarget_Click" Style="display: none;" />
				
 <%--               <asp:HiddenField ID="hf1" runat="server" />
                           <asp:HiddenField ID="hfbtnmode" runat="server" />
<asp:HiddenField ID="hfedmode" runat="server" />
<asp:HiddenField ID="hffielddt" runat="server" />                              
<input type="button" ID="btnhideF" runat="server" onserverclick="btnhideF_Click"  style="display:none" />
<input type="button" ID="btnhideF_S" runat="server"  onserverclick="btnhideF_S_Click" style="display:none" />

<asp:Button ID="btnOKTarget" runat="server" Text="!" onclick="btnOKTarget_Click" style="display:none;"  />
<asp:Button ID="btnCancelTarget" runat="server" Text="!" onclick="btnCancelTarget_Click"  style="display:none;"  />--%>



</asp:Content>

