<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="om_Contain_detail" CodeFile="om_Contain_detail.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
 
    <style type="text/css">
        .hidden {
            display: none;
        }
    </style>
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
                        <div class="box-body" >
                            <div class="form-group">
                                <asp:Label ID="lbl1" runat="server" Text="Entry_No" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True"></asp:Label>
                                 <div class="col-sm-1">
                                    <asp:Label ID="lbl1a" runat="server" Text="TC" Style="width: 22px; float: right;" CssClass="col-sm-1 control-label"></asp:Label>
                                </div>
                                 <div class="col-sm-3">
                                     <asp:TextBox ID="txtvchnum" runat="server" Width="100%" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                 </div>
                                <asp:Label ID="Label1" runat="server" Text="Label1" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Date</asp:Label>
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
                                <asp:Label ID="lbl4" runat="server" Text="lbl4" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Forward_Agent</asp:Label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnlbl4" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl4_Click" />
                                 </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtlbl4" runat="server" Width="100%" ReadOnly="true" CssClass="form-control"></asp:TextBox>  
                                    </div>                                
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtlbl4a" runat="server" Width="100%" ReadOnly="true" CssClass="form-control" ></asp:TextBox>
                                    </div>
                            </div>
                            <div class="form-group" style="display:none;">
                                <asp:Label ID="lbl5" runat="server" Text="Nature_Ship" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="true"></asp:Label>
                                <div class="col-sm-1">
                                     <asp:ImageButton ID="btnlbl5" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl5_Click" />
                                     </div>
                                <div class="col-sm-1" style="display:none;">
                                    <asp:TextBox ID="txtnatcode" runat="server" MaxLength="30" CssClass="form-control"></asp:TextBox>
                                    </div>
                                <div class="col-sm-3">
                                            <asp:TextBox ID="txtlbl5" runat="server" ReadOnly="true" MaxLength="30" CssClass="form-control"></asp:TextBox>
                                        </div>
                                <asp:Label ID="Label2" runat="server" Text="Shipping_Line" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="true"></asp:Label>
                                <div class="col-sm-1">
                                     <asp:ImageButton ID="btnshipline" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnshipline_Click" />
                                     </div>
                                <div class="col-sm-1" style="display:none;">
                                    <asp:TextBox ID="txtshipcode" runat="server" MaxLength="30" CssClass="form-control"></asp:TextBox>
                                    </div>
                                <div class="col-sm-3">
                                            <asp:TextBox ID="txtshipline" runat="server" ReadOnly="true" MaxLength="40" CssClass="form-control"></asp:TextBox>
                                        </div>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="Label6" runat="server" Text="Name_Ocean_Vessel" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="true"></asp:Label>
                                <div class="col-sm-3">
                                            <asp:TextBox ID="txtocnvess" runat="server" MaxLength="130" CssClass="form-control"></asp:TextBox>
                                        </div>
                                <asp:Label ID="Label19" runat="server" Text="Special_Instn" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="true"></asp:Label>
                                <div class="col-sm-3">
                                            <asp:TextBox ID="txtspclinstn" runat="server" MaxLength="100" CssClass="form-control"></asp:TextBox>
                                        </div>
                            </div>
                                 <div class="form-group">
                                <asp:Label ID="Label17" runat="server" Text="lbl4" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Port_Of_Discharge</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtportdisc" runat="server" Width="100%" MaxLength="30" CssClass="form-control"></asp:TextBox>  
                                    </div>
                                <asp:Label ID="Label18" runat="server" Text="lbl4" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Place_Of_Del</asp:Label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtpdelv" runat="server" Width="100%" MaxLength="30" CssClass="form-control"></asp:TextBox>  
                                    </div>
                                </div>
                            <div class="form-group" style="display:none;">
                                 <asp:Label ID="lbl7" runat="server" Text="lbl7" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True"></asp:Label>
                                 <div class="col-sm-1">
                                     <asp:ImageButton ID="btnlbl7" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl7_Click" />
                                     </div>
                                 <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl7" runat="server" Width="100%" ReadOnly="true" CssClass="form-control" ></asp:TextBox>
                                        </div>
                                 <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl7a" runat="server" Width="100%" ReadOnly="true" CssClass="form-control" ></asp:TextBox>
                                        </div>
                                 </div>                           
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body" >
                           
                               <%--   <div class="form-group">
                                <asp:Label ID="Label22" runat="server" Text="lbl4" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Forward_Agent</asp:Label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl4_Click" />
                                 </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="TextBox1" runat="server" Width="100%" ReadOnly="true" CssClass="form-control"></asp:TextBox>  
                                    </div>                                
                                <div class="col-sm-6">
                                    <asp:TextBox ID="TextBox2" runat="server" Width="100%" ReadOnly="true" CssClass="form-control" ></asp:TextBox>
                                    </div>
                            </div>--%>

                                <div class="form-group">
                                    <asp:Label ID="lbl25" runat="server" Text="lbl4" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Branch_Add</asp:Label>
                                    <div class="col-sm-1">
                                    <asp:ImageButton ID="btnmbr" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnmbr_Click" />
                                 </div>
                                      <div class="col-sm-3">
                                    <asp:TextBox ID="txtmbrcode" runat="server" Width="100%" ReadOnly="true" CssClass="form-control" ></asp:TextBox>
                                    </div>
                                     <div class="col-sm-6">
                                    <asp:TextBox ID="txtmbr" runat="server" Width="100%" ReadOnly="true" CssClass="form-control" ></asp:TextBox>
                                    </div>
                                    </div>



                                <div class="form-group">
                                <asp:Label ID="lbl2" runat="server" Text="Container_No" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="true"></asp:Label>
                                <div class="col-sm-3">
                                            <asp:TextBox ID="txtlbl2" runat="server" MaxLength="30" CssClass="form-control"></asp:TextBox>
                                        </div>
                                <asp:Label ID="lbl3" runat="server" Text="Container_Size" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="true"></asp:Label>
                                 <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl3" runat="server" MaxLength="30" CssClass="form-control"></asp:TextBox>
                                        </div>
                                </div>
                            <div class="form-group" style="display:none;">
                                <asp:Label ID="lbl6" runat="server" Text="Full_Commodity_Desc" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="true"></asp:Label>
                                <div class="col-sm-3">
                                            <asp:TextBox ID="txtlbl6" runat="server" MaxLength="50" CssClass="form-control"></asp:TextBox>
                                        </div>
                                <asp:Label ID="Label7" runat="server" Text="B/L" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="true"></asp:Label>
                                <div class="col-sm-4">
                                            <asp:TextBox ID="txtbl" runat="server" MaxLength="50" CssClass="form-control"></asp:TextBox>
                                        </div>
                                </div>

                            <%--     <div class="form-group">
                                <asp:Label ID="Label17" runat="server" Text="lbl4" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Port_Of_Discharge</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtportdisc" runat="server" Width="100%" MaxLength="30" CssClass="form-control"></asp:TextBox>  
                                    </div>
                                <asp:Label ID="Label18" runat="server" Text="lbl4" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Place_Of_Del</asp:Label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtpdelv" runat="server" Width="100%" MaxLength="30" CssClass="form-control"></asp:TextBox>  
                                    </div>
                                </div>--%>
                          <%--  <div class="form-group">
                                <asp:Label ID="lbl20" runat="server" Text="Freight_Rmk1" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True" ></asp:Label>
                                <div class="col-sm-3">
                                            <asp:TextBox ID="txtlbl20" runat="server" MaxLength="100" CssClass="form-control"></asp:TextBox>
                                        </div>
                                <asp:Label ID="lbl21" runat="server" Text="Freight_Rmk2" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True"></asp:Label>
                                <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl21" runat="server" MaxLength="100" CssClass="form-control"></asp:TextBox>
                                        </div>
                                </div>  --%>                          
                           <div class="form-group">
                                <asp:Label ID="Label15" runat="server" Text="lbl4" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Place_Of_Reciept</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtrcpt" runat="server" Width="100%" MaxLength="30" CssClass="form-control"></asp:TextBox>  
                                    </div>
                                <asp:Label ID="Label16" runat="server" Text="lbl4" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Place_Of_Load</asp:Label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtload" runat="server" Width="100%" MaxLength="30" CssClass="form-control"></asp:TextBox>  
                                    </div>
                                </div>
                        </div>
                    </div>
                </div>
                 <div class="col-md-6">
                    <div>
                        <div class="box-body" >
                             <div class="form-group">
                                <asp:Label ID="Label13" runat="server" Text="lbl4" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Consignee</asp:Label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnconsign" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnconsign_Click" />
                                 </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtconsigncode" runat="server" Width="100%" ReadOnly="true" CssClass="form-control"></asp:TextBox>  
                                    </div>                                
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtconsign" runat="server" Width="100%" ReadOnly="true" CssClass="form-control" ></asp:TextBox>
                                    </div>
                            </div>
                       <%--     <div class="form-group">
                                <asp:Label ID="Label17" runat="server" Text="lbl4" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Port_Of_Discharge</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtportdisc" runat="server" Width="100%" MaxLength="30" CssClass="form-control"></asp:TextBox>  
                                    </div>
                                <asp:Label ID="Label18" runat="server" Text="lbl4" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Place_Of_Del</asp:Label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtpdelv" runat="server" Width="100%" MaxLength="30" CssClass="form-control"></asp:TextBox>  
                                    </div>
                                </div>--%>

                              <div class="form-group">
                                <asp:Label ID="lbl20" runat="server" Text="Freight_Rmk1" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True" ></asp:Label>
                                <div class="col-sm-3">
                                            <asp:TextBox ID="txtlbl20" runat="server" MaxLength="100" CssClass="form-control"></asp:TextBox>
                                        </div>
                                <asp:Label ID="lbl21" runat="server" Text="Freight_Rmk2" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True"></asp:Label>
                                <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl21" runat="server" MaxLength="100" CssClass="form-control"></asp:TextBox>
                                        </div>
                                </div>  

                               <div class="form-group">
                                <asp:Label ID="Label20" runat="server" Text="Freight_Rmk3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True" ></asp:Label>
                                <div class="col-sm-9">
                                            <asp:TextBox ID="txtlbl22" runat="server" MaxLength="80" CssClass="form-control"></asp:TextBox>
                                        </div>
                            
                                </div>  

                            </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div>
                        <div class="box-body" >
                            
                         <div class="form-group">
                                <asp:Label ID="Label14" runat="server" Text="lbl4" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Notify1</asp:Label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnnotify" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnnotify_Click" />
                                 </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtnotcode" runat="server" Width="100%" ReadOnly="true" CssClass="form-control"></asp:TextBox>  
                                    </div>                                
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtnotifyname" runat="server" Width="100%" ReadOnly="true" CssClass="form-control" ></asp:TextBox>
                                    </div>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="lbl8" runat="server" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="true">Notify2</asp:Label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnNotify2" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnNotify2_Click" />
                                 </div>
                                <div class="col-sm-3">
                                            <asp:TextBox ID="txtlbl8" runat="server" MaxLength="100" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                <div class="col-sm-6">
                                            <asp:TextBox ID="txtlbl9" runat="server" MaxLength="100" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                </div>

                             <div class="form-group">
                                 <asp:Label ID="Label21" runat="server" Text="Freight_Rmk4" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True"></asp:Label>
                                <div class="col-sm-9">
                                            <asp:TextBox ID="txtlbl23" runat="server" MaxLength="80" CssClass="form-control"></asp:TextBox>
                                        </div>
                            </div>
                    </div>
                    </div>
                </div>

                <div class="col-md-6" style="display:none;">
                    <div>
                        <div class="box-body" >
                             <div class="form-group">
                                <asp:Label ID="Label3" runat="server" Text="Dispatch_Date" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="true"></asp:Label>
                                <div class="col-sm-3">
                                            <asp:TextBox ID="txtdispdate" runat="server" MaxLength="10" placeholder="DD/MM/YYYY" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtdispdate_CalendarExtender1" runat="server"
                                                Enabled="True" TargetControlID="txtdispdate"
                                                Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="MaskedEditExtender1" runat="server" Mask="99/99/9999"
                                                MaskType="Date" TargetControlID="txtdispdate" />
                                        </div>
                                <asp:Label ID="Label4" runat="server" Text="Stuffing_Date" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="true"></asp:Label>
                                <div class="col-sm-3">
                                            <asp:TextBox ID="txtsuffdate" runat="server" MaxLength="10" CssClass="form-control" placeholder="DD/MM/YYYY"  ></asp:TextBox>
                                    <asp:CalendarExtender ID="txtsuffdate_CalendarExtender2" runat="server"
                                                Enabled="True" TargetControlID="txtsuffdate"
                                                Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="MaskedEditExtender2" runat="server" Mask="99/99/9999"
                                                MaskType="Date" TargetControlID="txtsuffdate" />
                                        </div>
                                </div>
                            <div class="form-group">
                                <asp:Label ID="Label5" runat="server" Text="RailOut_Date" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="true"></asp:Label>
                                <div class="col-sm-3">
                                            <asp:TextBox ID="txtrailout" runat="server" MaxLength="10" CssClass="form-control" placeholder="DD/MM/YYYY" ></asp:TextBox>
                                    <asp:CalendarExtender ID="txtrailout_CalendarExtender3" runat="server"
                                                Enabled="True" TargetControlID="txtrailout"
                                                Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="MaskedEditExtender3" runat="server" Mask="99/99/9999"
                                                MaskType="Date" TargetControlID="txtrailout" />
                                        </div>
                                <asp:Label ID="Label8" runat="server" Text="SOB_Date" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="true"></asp:Label>
                                <div class="col-sm-3">
                                            <asp:TextBox ID="txtsobdate" runat="server" MaxLength="10" CssClass="form-control" placeholder="DD/MM/YYYY" ></asp:TextBox>
                                    <asp:CalendarExtender ID="txtsobdate_CalendarExtender4" runat="server"
                                                Enabled="True" TargetControlID="txtsobdate"
                                                Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="MaskedEditExtender4" runat="server" Mask="99/99/9999"
                                                MaskType="Date" TargetControlID="txtsobdate" />
                                        </div>                              
                                
                                </div>
                            </div>
                    </div>
                </div>
                <div class="col-md-6" style="display:none;">
                    <div>
                        <div class="box-body" >
                             <div class="form-group">                                
                                <asp:Label ID="Label9" runat="server" Text="ETA" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="true"></asp:Label>
                                <div class="col-sm-3">
                                            <asp:TextBox ID="txteta" runat="server" MaxLength="10" CssClass="form-control" placeholder="DD/MM/YYYY"></asp:TextBox>
                                    <asp:CalendarExtender ID="txteta_CalendarExtender5" runat="server"
                                                Enabled="True" TargetControlID="txteta"
                                                Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="MaskedEditExtender5" runat="server" Mask="99/99/9999"
                                                MaskType="Date" TargetControlID="txteta" />
                                        </div>
                                <asp:Label ID="Label10" runat="server" Text="Docs_Sent_On" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="true" ></asp:Label>
                                <div class="col-sm-3">
                                            <asp:TextBox ID="txtdocsent" runat="server" MaxLength="10" CssClass="form-control" placeholder="DD/MM/YYYY"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtdocsent_CalendarExtender6" runat="server"
                                                Enabled="True" TargetControlID="txtdocsent"
                                                Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="MaskedEditExtender6" runat="server" Mask="99/99/9999"
                                                MaskType="Date" TargetControlID="txtdocsent" />
                                        </div>
                                </div>
                            <div class="form-group">                                
                                <asp:Label ID="Label11" runat="server" Text="D/O Date" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="true"></asp:Label>
                                <div class="col-sm-3">
                                            <asp:TextBox ID="txtdodate" runat="server" MaxLength="10" CssClass="form-control" placeholder="DD/MM/YYYY" ></asp:TextBox>
                                    <asp:CalendarExtender ID="txtdodate_CalendarExtender7" runat="server"
                                                Enabled="True" TargetControlID="txtdodate"
                                                Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="MaskedEditExtender7" runat="server" Mask="99/99/9999"
                                                MaskType="Date" TargetControlID="txtdodate" />
                                        </div>
                                <asp:Label ID="Label12" runat="server" Text="Warehouse_Del_Date" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="true"></asp:Label>
                                <div class="col-sm-3">
                                            <asp:TextBox ID="txtwaredate" runat="server" MaxLength="10" CssClass="form-control" placeholder="DD/MM/YYYY"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtwaredate_CalendarExtender8" runat="server"
                                                Enabled="True" TargetControlID="txtwaredate"
                                                Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="MaskedEditExtender8" runat="server" Mask="99/99/9999"
                                                MaskType="Date" TargetControlID="txtwaredate" />
                                        </div>
                                </div>
                            </div>
                    </div>
                </div>

                <section class="col-lg-12 connectedSortable">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">
                                <li><a href="#DescTab" id="tab1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab">Export Details</a></li>
                                <li><a href="#DescTab2" id="tab2" runat="server" aria-controls="DescTab2" role="tab" data-toggle="tab">Comm.Terms</a></li>
                                <li><a href="#DescTab3" id="tab3" runat="server" aria-controls="DescTab3" role="tab" data-toggle="tab">Addl.Terms</a></li>
                                <li><a href="#DescTab4" id="tab4" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Delv.Sch</a></li>
                                <li><a href="#DescTab5" id="tab5" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Inv.Dtl</a></li>
                            </ul>

                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane active" id="DescTab">
                                    <div class="lbBody" id="gridDiv" style="color: White; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                        <fin:CoolGridView ID="sg1" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="100%" Height="300px" Font-Size="13px"
                                            AutoGenerateColumns="False" OnRowDataBound="sg1_RowDataBound"
                                            OnRowCommand="sg1_RowCommand">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:BoundField DataField="sg1_h1" HeaderText="sg1_h1"   />
                                                <asp:BoundField DataField="sg1_h2" HeaderText="sg1_h2"   />
                                                <asp:BoundField DataField="sg1_h3" HeaderText="sg1_h3"   />
                                                <asp:BoundField DataField="sg1_h4" HeaderText="sg1_h4"   />
                                                <asp:BoundField DataField="sg1_h5" HeaderText="sg1_h5"   />
                                                <asp:BoundField DataField="sg1_h6" HeaderText="sg1_h6"   />
                                                <asp:BoundField DataField="sg1_h7" HeaderText="sg1_h7"   />
                                                <asp:BoundField DataField="sg1_h8" HeaderText="sg1_h8"   />
                                                <asp:BoundField DataField="sg1_h9" HeaderText="sg1_h9"   />
                                                <asp:BoundField DataField="sg1_h10" HeaderText="sg1_h10"   />

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
                                                <asp:BoundField DataField="sg1_f1" HeaderText="Inv No"  />
                                                <asp:BoundField DataField="sg1_f2" HeaderText="Value (FC)" />
                                                <asp:BoundField DataField="sg1_f3" HeaderText="Shipment_Date"  />
                                                <asp:BoundField DataField="sg1_f4" HeaderText="Net_Weight"  />
                                                <asp:BoundField DataField="sg1_f5" HeaderText="Total_Weight" />
                                               <asp:BoundField DataField="sg1_f6" HeaderText="No_OF_Pkgs"  />
                                                <asp:BoundField DataField="sg1_f7" HeaderText="acode" Visible="false" />
                                                <asp:BoundField DataField="sg1_f8" HeaderText="sg1_f8" Visible="false" />
                                                <asp:BoundField DataField="sg1_f9" HeaderText="sg1_f9" Visible="false"/>
                                                <asp:BoundField DataField="sg1_f10" HeaderText="sg1_f10" Visible="false"/>
                                               <asp:BoundField DataField="sg1_f11" HeaderText="sg1_f11" Visible="false"/>
                                                <asp:BoundField DataField="sg1_f12" HeaderText="sg1_f12" Visible="false"/>
                                                <asp:BoundField DataField="sg1_f13" HeaderText="sg1_f13" Visible="false"/>
                                                <asp:BoundField DataField="sg1_f14" HeaderText="sg1_f14" Visible="false" />
                                                <asp:BoundField DataField="sg1_f15" HeaderText="sg1_f15" Visible="false"/>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Pkg_No_From</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t1" runat="server" Text='<%#Eval("sg1_t1") %>' onkeypress="return isDecimalKey(event)"  Width="100%" MaxLength="10"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Pkg_No_To</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t2" runat="server" Text='<%#Eval("sg1_t2") %>' onkeypress="return isDecimalKey(event)"  Width="100%" MaxLength="10"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField >
                                                    <HeaderTemplate>Full_Commodity_Desc</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t3" runat="server" Text='<%#Eval("sg1_t3") %>' Width="100%" MaxLength="50"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField >
                                                    <HeaderTemplate>Package</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t4" runat="server" Text='<%#Eval("sg1_t4") %>'  Width="100%" MaxLength="50"></asp:TextBox>                                                        
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Shipping_Bill_No</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t5" runat="server" Text='<%#Eval("sg1_t5") %>' onkeypress="return isDecimalKey(event)" Width="100%" MaxLength="40"></asp:TextBox>                                                          
                                                    </ItemTemplate>
                                                </asp:TemplateField>                                      

                                                       <asp:TemplateField>
                                                    <HeaderTemplate>Shipping_Bill_Dt</HeaderTemplate>
                                                    <ItemTemplate>
                                                      <asp:TextBox ID="sg1_t6" runat="server" Text='<%#Eval("sg1_t6") %>' Width="100%" MaxLength="10"></asp:TextBox> 
                                                        <%--<asp:TextBox ID="sg1_t6" runat="server" Text='<%#Eval("sg1_t6") %>'  Width="100%"></asp:TextBox>
                                                 <asp:CalendarExtender ID="sg1_t6_CalendarExtender" runat="server"
                                                Enabled="True" TargetControlID="sg1_t6"
                                                Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="Maskedit9" runat="server" Mask="99/99/9999"
                                                MaskType="Date" TargetControlID="sg1_t6" />--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>




                                                <asp:TemplateField  Visible="false">
                                                    <HeaderTemplate>Total Freight</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t7" runat="server" Text='<%#Eval("sg1_t7") %>' onkeypress="return isDecimalKey(event)"  Width="100%" MaxLength="27"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField  Visible="false">
                                                    <HeaderTemplate>Concor_Rail_Charges(FC)</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t27" runat="server" Text='<%#Eval("sg1_t27") %>' onkeypress="return isDecimalKey(event)"  Width="100%" MaxLength="27"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField  Visible="false">
                                                    <HeaderTemplate>Ocean_Freight(INR)</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t8" runat="server" Text='<%#Eval("sg1_t8") %>' onkeypress="return isDecimalKey(event)"  Width="100%" MaxLength="27"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField  Visible="false">
                                                    <HeaderTemplate>Ocean_Freight(FC)</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t9" runat="server" Text='<%#Eval("sg1_t9") %>' onkeypress="return isDecimalKey(event)"  Width="100%" MaxLength="27"></asp:TextBox>                                                        
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField  Visible="false">
                                                    <HeaderTemplate>THC_Charges(INR)</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t10" runat="server" Text='<%#Eval("sg1_t10") %>' onkeypress="return isDecimalKey(event)"  Width="100%" MaxLength="27"></asp:TextBox>                                                        
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField  Visible="false">
                                                    <HeaderTemplate>THC_Charges(FC)</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t11" runat="server" Text='<%#Eval("sg1_t11") %>' onkeypress="return isDecimalKey(event)"  Width="100%" MaxLength="27"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField  Visible="false">
                                                    <HeaderTemplate>Doc_Charges(INR)</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t12" runat="server" Text='<%#Eval("sg1_t12") %>' onkeypress="return isDecimalKey(event)"  Width="100%" MaxLength="27"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField  Visible="false">
                                                    <HeaderTemplate>Doc_Charges(FC)</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t13" runat="server" Text='<%#Eval("sg1_t13") %>' onkeypress="return isDecimalKey(event)" Width="100%" MaxLength="27"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField  Visible="false">
                                                    <HeaderTemplate>Other_Charges(INR)</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t14" runat="server" Text='<%#Eval("sg1_t14") %>' onkeypress="return isDecimalKey(event)" Width="100%" MaxLength="27"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField  Visible="false">
                                                    <HeaderTemplate>Other_Charges(FC)</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t15" runat="server" Text='<%#Eval("sg1_t15") %>' onkeypress="return isDecimalKey(event)"  Width="100%" MaxLength="27"></asp:TextBox>                                                      
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>Ex_Rate</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t16" runat="server" Text='<%#Eval("sg1_t16") %>' onChange="cal()" onkeypress="return isDecimalKey(event)"  Width="100%" MaxLength="10"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>Total_Freight(FC)</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t17" runat="server" Text='<%#Eval("sg1_t17") %>' onChange="cal()" onkeypress="return isDecimalKey(event)"  Width="100%" MaxLength="10"></asp:TextBox>                                            
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate >Total_Freight(INR)</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t18" runat="server" Text='<%#Eval("sg1_t18") %>'  ReadOnly="true" onkeypress="return isDecimalKey(event)"  Width="100%" MaxLength="27"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>sg1_t19</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t19" runat="server" Text='<%#Eval("sg1_t19") %>' onkeypress="return isDecimalKey(event)"  Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>sg1_t20</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t20" runat="server" Text='<%#Eval("sg1_t20") %>' onkeypress="return isDecimalKey(event)"  Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>sg1_t21</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t21" runat="server" Text='<%#Eval("sg1_t21") %>' onkeypress="return isDecimalKey(event)"  Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>sg1_t22</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t22" runat="server" Text='<%#Eval("sg1_t22") %>' onkeypress="return isDecimalKey(event)"  Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>sg1_t23</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t23" runat="server" Text='<%#Eval("sg1_t23") %>' onkeypress="return isDecimalKey(event)"  Width="100%" ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField  Visible="false">
                                                    <HeaderTemplate>sg1_t24</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t24" runat="server" Text='<%#Eval("sg1_t24") %>' onkeypress="return isDecimalKey(event)"  Width="100%" ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField  Visible="false">
                                                    <HeaderTemplate>sg1_t25</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t25" runat="server" Text='<%#Eval("sg1_t25") %>'  Width="100%" ></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField  Visible="false">
                                                    <HeaderTemplate>sg1_t26</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t26" runat="server" Text='<%#Eval("sg1_t26") %>'  Width="100%"></asp:TextBox>
                                                 <asp:CalendarExtender ID="sg1_t26_CalendarExtender" runat="server"
                                                Enabled="True" TargetControlID="sg1_t26"
                                                Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="Maskedit8" runat="server" Mask="99/99/9999"
                                                MaskType="Date" TargetControlID="sg1_t26" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField  Visible="false">
                                                    <HeaderTemplate>sg1_t28</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t28" runat="server" Text='<%#Eval("sg1_t28") %>' onkeypress="return isDecimalKey(event)"  Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField  Visible="false">
                                                    <HeaderTemplate>sg1_t29</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t29" runat="server" Text='<%#Eval("sg1_t29") %>' onkeypress="return isDecimalKey(event)"  Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField  Visible="false">
                                                    <HeaderTemplate>sg1_t30</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t30" runat="server" Text='<%#Eval("sg1_t30") %>' onkeypress="return isDecimalKey(event)"  Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField  Visible="false">
                                                    <HeaderTemplate>sg1_t31</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t31" runat="server" Text='<%#Eval("sg1_t31") %>' onkeypress="return isDecimalKey(event)"  Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField  Visible="false">
                                                    <HeaderTemplate>sg1_t32</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t32" runat="server" Text='<%#Eval("sg1_t32") %>' onkeypress="return isDecimalKey(event)"  Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField  Visible="false">
                                                    <HeaderTemplate>sg1_t33</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t33" runat="server" Text='<%#Eval("sg1_t33") %>' onkeypress="return isDecimalKey(event)"  Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField  Visible="false">
                                                    <HeaderTemplate>sg1_t34</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t34" runat="server" Text='<%#Eval("sg1_t34") %>' onkeypress="return isDecimalKey(event)" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField  Visible="false">
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

                                <div role="tabpanel" class="tab-pane active" id="DescTab2">
                                    <div class="lbBody" style="height: 200px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">
                                                    <table style="width: 100%">                                                       
                                                        
                                                    </table>
                                                </div>
                                            </div>
                                        </div>                                      

                                    </div>
                                </div>
                                <div role="tabpanel" class="tab-pane active" id="DescTab3">
                                    <div class="lbBody" style="height: 200px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                        <asp:GridView ID="sg2" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="1200px" Font-Size="Smaller"
                                            AutoGenerateColumns="False" OnRowDataBound="sg1_RowDataBound"
                                            OnRowCommand="sg2_RowCommand">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Add</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg2_btnadd" runat="server" CommandName="SG2_ROW_ADD" ImageAlign="Middle" ImageUrl="../tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Del</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg2_btnrmv" runat="server" CommandName="SG2_RMV" ImageUrl="../tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="sg2_srno" HeaderText="Sr.No." />

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Terms</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg2_t1" runat="server" Text='<%#Eval("sg2_t1") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Conditions</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg2_t2" runat="server" Text='<%#Eval("sg2_t2") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        </asp:GridView>
                                    </div>
                                </div>
                                <div role="tabpanel" class="tab-pane active" id="DescTab4">
                                    <div class="lbBody" style="height: 200px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                        <asp:GridView ID="sg3" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="1200px" Font-Size="Smaller"
                                            AutoGenerateColumns="False"
                                            OnRowCommand="sg3_RowCommand">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Add</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg3_btnadd" runat="server" CommandName="SG3_ROW_ADD" ImageAlign="Middle" ImageUrl="../tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Del</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg3_btnrmv" runat="server" CommandName="SG3_RMV" ImageUrl="../tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="sg3_Srno" HeaderText="Sr.No" />
                                                <asp:BoundField DataField="sg3_f1" HeaderText="ERP_Code" />
                                                <asp:BoundField DataField="sg3_f2" HeaderText="Item_Name" />

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Dlv_Date</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t1" runat="server" Text='<%#Eval("sg3_t1") %>' MaxLength="10" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Sch.Qty</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t2" runat="server" Text='<%#Eval("sg3_t2") %>' MaxLength="10" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Prod.Qty</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t3" runat="server" Text='<%#Eval("sg3_t3") %>' MaxLength="10" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Job_Card</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t4" runat="server" Text='<%#Eval("sg3_t4") %>' MaxLength="1" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White"
                                                CssClass="GridviewScrollHeader" />
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
                <div class="col-md-12" style="display:none">
                    <div>
                        <div class="box-body">
                            <asp:TextBox ID="txtrmk" runat="server" MaxLength="150" Width="99%" TextMode="MultiLine" placeholder="Remarks"></asp:TextBox>
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
    <asp:HiddenField ID="hf2" runat="server" />
    <script type="text/javascript">
        $(function () {
            var tabName = $("[id*=TabName]").val() != "" ? $("[id*=TabName]").val() : "DescTab";
            $('#Tabs a[href="#' + tabName + '"]').tab('show');
            $("#Tabs a").click(function () {
                $("[id*=TabName]").val($(this).attr("href").replace("#", ""));
            });
        });

        function cal() {
            var fob_inr = 0;
            var fob_brc = 0;
            var fob_sb = 0;
            var fob_foreign = 0;
            var grid = document.getElementById("<%= sg1.ClientID%>");
            for (var i = 0; i < grid.rows.length - 1; i++) {
                fob_inr = fill_zero(document.getElementById('ContentPlaceHolder1_sg1_sg1_t16_' + i).value);                
                fob_brc = fill_zero(document.getElementById('ContentPlaceHolder1_sg1_sg1_t17_' + i).value);
                fob_sb = fob_inr * fob_brc;
                document.getElementById('ContentPlaceHolder1_sg1_sg1_t18_' + i).value = fill_zero(fob_sb).toFixed(2);
            }
        }
        function fill_zero(val) { if (isNaN(val)) return 0; if (isFinite(val)) return val; }
    </script>
    <asp:HiddenField ID="TabName" runat="server" />
</asp:Content>
