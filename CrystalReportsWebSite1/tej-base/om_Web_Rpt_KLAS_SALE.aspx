<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="om_Web_Rpt_KLAS_SALE" Title="Tejaxo" CodeFile="om_Web_Rpt_KLAS_SALE.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

     <div class="content-wrapper">
        <section class="content-header">
            <table style="width: 100%">
                <tr>                    
                    <td>
                          <asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large">Web Reports(Sales and Marketing)</asp:Label>                      
                    </td>
                </tr>
            </table>
        </section>

            <section class="content">
            <div class="row">
                <div class="col-md-6" style="display:none;">
                    <div>                      
                        <div class="box-body">
                            <div class="form-group">
                                    <div class="col-sm-6">
                                  <button type="submit" id="rep1" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep1_ServerClick">General item List</button>
                                        </div>
                                 <div class="col-sm-6">
                                 <button type="submit" id="rep2" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep2_ServerClick">FG Item List</button>
                                      </div>
                                </div>
                             <div class="form-group">
                                    <div class="col-sm-6">
                                   <asp:Label ID="lbl1" runat="server" Text="" Height="15px"  Style="text-align: center; font-weight: 700"></asp:Label>
                                        </div>
                                 <div class="col-sm-6">
                                  <asp:Label ID="lb2" runat="server" Text="" Height="15px"  Style="text-align: center; font-weight: 700"></asp:Label>
                                      </div>
                                </div>

                            <div class="form-group">
                                    <div class="col-sm-6">
                                  <button type="submit" id="rep3" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep3_ServerClick">Process Parameter line wise</button>
                                        </div>
                                 <div class="col-sm-6">
                                  <button type="submit" id="rep4" class="btn btn-info" style="width:100%" runat="server" onserverclick="rep4_ServerClick">Process Parameter jumbo roll wise</button>
                                      </div>
                                </div>

                             <div class="form-group">
                                    <div class="col-sm-6">
                                   <asp:Label ID="lbl3" runat="server" Text=""  Height="15px" Style="text-align: center; font-weight: 700"></asp:Label>
                                        </div>
                                 <div class="col-sm-6">
                                  <asp:Label ID="lbl4" runat="server" Text="" Height="15px" Style="text-align: center; font-weight: 700"></asp:Label>
                                      </div>
                                </div>
                            
                            <div class="form-group">
                                    <div class="col-sm-6">
                                 <button type="submit" id="rep5" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep5_ServerClick">RM Physical verification report</button>
                                        </div>
                                 <div class="col-sm-6">
                                 <button type="submit" id="rep6" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep6_ServerClick">FG Physical verification report</button>
                                      </div>
                                </div>

                              <div class="form-group">
                                    <div class="col-sm-6">
                                   <asp:Label ID="lbl5" runat="server" Text=""  Height="15px" Style="text-align: center; font-weight: 700"></asp:Label>
                                        </div>
                                 <div class="col-sm-6">
                                  <asp:Label ID="lbl6" runat="server" Text="" Height="15px" Style="text-align: center; font-weight: 700"></asp:Label>
                                      </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-6">
                                  <button type="submit" id="rep7" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep7_ServerClick">Physical Verification Records</button>
                                        </div>
                                 <div class="col-sm-6">
                                  <button type="submit" id="rep8" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep8_ServerClick">Order Size Report Group,Customer Wise</button>
                                      </div>
                                </div>

                              <div class="form-group">
                                    <div class="col-sm-6">
                                   <asp:Label ID="lbl7" runat="server" Text="" Height="15px" Style="text-align: center; font-weight: 700"></asp:Label>
                                        </div>
                                 <div class="col-sm-6">
                                  <asp:Label ID="lbl8" runat="server" Text="" Height="15px" Style="text-align: center; font-weight: 700"></asp:Label>
                                      </div>
                                </div>

                              </div>
                          </div>
                      </div>


                  <div class="col-md-6" style="display:none;">
                    <div>                      
                        <div class="box-body">
                            <div class="form-group">
                                     <div class="form-group">
                                    <div class="col-sm-6">
                                  <button type="submit" id="rep9" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep9_ServerClick">Time Statement</button>
                                        </div>
                                 <div class="col-sm-6">
                                 <button type="submit" id="rep10" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep10_ServerClick">Time Statement Summary</button>
                                      </div>
                                </div>

                                
                              <div class="form-group">
                                    <div class="col-sm-6">
                                   <asp:Label ID="lbl9" runat="server" Text="" Height="15px" Style="text-align: center; font-weight: 700"></asp:Label>
                                        </div>
                                 <div class="col-sm-6">
                                  <asp:Label ID="lbl10" runat="server" Text="" Height="15px" Style="text-align: center; font-weight: 700"></asp:Label>
                                      </div>
                                </div>

                            <div class="form-group">
                                    <div class="col-sm-6">
                                  <button type="submit" id="rep11" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep11_ServerClick">Time Statement Others</button>
                                        </div>
                                 <div class="col-sm-6">
                                  <button type="submit" id="rep12" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep12_ServerClick">Time Statement 2</button>
                                      </div>
                                </div>

                                 <div class="form-group">
                                    <div class="col-sm-6">
                                   <asp:Label ID="lbl11" runat="server" Text=""  Height="15px" Style="text-align: center; font-weight: 700"></asp:Label>
                                        </div>
                                 <div class="col-sm-6">
                                  <asp:Label ID="lbl12" runat="server" Text="" Height="15px"  Style="text-align: center; font-weight: 700"></asp:Label>
                                      </div>
                                </div>
                            
                            <div class="form-group">
                                    <div class="col-sm-6">
                                 <button type="submit" id="rep13" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep13_ServerClick">Time Statement Summary 2</button>
                                        </div>
                                 <div class="col-sm-6">
                                 <button type="submit" id="rep14" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep14_ServerClick">Time Statement Others 2</button>
                                      </div>
                                </div>

                                 <div class="form-group">
                                    <div class="col-sm-6">
                                   <asp:Label ID="lbl13" runat="server" Text="" Height="15px"  Style="text-align: center; font-weight: 700"></asp:Label>
                                        </div>
                                 <div class="col-sm-6">
                                  <asp:Label ID="lbl14" runat="server" Text="" Height="15px" Style="text-align: center; font-weight: 700"></asp:Label>
                                      </div>
                                </div>

                                 <div class="form-group">
                                    <div class="col-sm-6">
                                 <button type="submit" id="rep15" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep15_ServerClick">jubmo roll details</button>
                                        </div>
                                 <div class="col-sm-6">
                                 <button type="submit" id="rep16" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep16_ServerClick">ITEM WISE REEL LOCATION</button>
                                      </div>
                                </div>

                                 <div class="form-group">
                                    <div class="col-sm-6">
                                   <asp:Label ID="lbl15" runat="server" Text=""  Height="15px" Style="text-align: center; font-weight: 700"></asp:Label>
                                        </div>
                                 <div class="col-sm-6">
                                  <asp:Label ID="lbl16" runat="server" Text="" Height="15px"  Style="text-align: center; font-weight: 700"></asp:Label>
                                      </div>
                                </div>

                                </div>
                              </div>
                          </div>
                      </div>


                 <div class="col-md-6"  style="display:none;">
                    <div>                      
                        <div class="box-body">
                            <div class="form-group"  style="display:none;">
                                     <div class="form-group">
                                    <div class="col-sm-6">
                                  <button type="submit" id="rep17" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep17_ServerClick">REEL WISE REEL LOCATION</button>
                                        </div>
                                 <div class="col-sm-6">
                                 <button type="submit" id="rep18" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep18_ServerClick">FABRIC INSPECTION REPORT</button>
                                      </div>
                                </div>

                                
                              <div class="form-group"  style="display:none;">
                                    <div class="col-sm-6">
                                   <asp:Label ID="lbl17" runat="server" Text=""    Height="15px" Style="text-align: center; font-weight: 700"></asp:Label>
                                        </div>
                                 <div class="col-sm-6">
                                  <asp:Label ID="lbl18" runat="server" Text=""   Height="15px"  Style="text-align: center; font-weight: 700"></asp:Label>
                                      </div>
                                </div>

                            <div class="form-group"  style="display:none;">
                                    <div class="col-sm-6">
                                  <button type="submit" id="rep19" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep19_ServerClick">Fabric Planning Sheet</button>
                                        </div>
                                 <div class="col-sm-6">
                                  <button type="submit" id="rep20" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep20_ServerClick">Yield Grading Report</button>
                                      </div>
                                </div>

                                 <div class="form-group"  style="display:none;">
                                    <div class="col-sm-6">
                                   <asp:Label ID="lbl19" runat="server" Text=""   Height="15px"  Style="text-align: center; font-weight: 700"></asp:Label>
                                        </div>
                                 <div class="col-sm-6">
                                  <asp:Label ID="lbl20" runat="server" Text=""   Height="15px"  Style="text-align: center; font-weight: 700"></asp:Label>
                                      </div>
                                </div>
                            
                            <div class="form-group"  style="display:none;">
                                    <div class="col-sm-6">
                                 <button type="submit" id="rep21" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep21_ServerClick">Yield Grading report JR Wise</button>
                                        </div>
                                 <div class="col-sm-6">
                                 <button type="submit" id="rep22" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep22_ServerClick">DayWise Yield Grading Report</button>
                                      </div>
                                </div>

                                 <div class="form-group"  style="display:none;">
                                    <div class="col-sm-6">
                                   <asp:Label ID="lbl21" runat="server" Text=""   Height="15px"  Style="text-align: center; font-weight: 700"></asp:Label>
                                        </div>
                                 <div class="col-sm-6">
                                  <asp:Label ID="lbl22" runat="server" Text=""  Height="15px"   Style="text-align: center; font-weight: 700"></asp:Label>
                                      </div>
                                </div>

                                 <div class="form-group"  style="display:none;">
                                    <div class="col-sm-6">
                                 <button type="submit" id="rep23" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep23_ServerClick">JR wise  Yield Grading Report</button>
                                        </div>
                              <%--   <div class="col-sm-6">
                                 <button type="submit" id="rep24" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep24_ServerClick">Pending SO</button>
                                      </div>--%>
                                </div>

                                 <div class="form-group"  style="display:none;">
                                    <div class="col-sm-6">
                                   <asp:Label ID="lbl23" runat="server" Text=""   Height="15px"  Style="text-align: center; font-weight: 700"></asp:Label>
                                        </div>
                                 <div class="col-sm-6">
                                  <asp:Label ID="lbl24" runat="server" Text=""   Height="15px"  Style="text-align: center; font-weight: 700"></asp:Label>
                                      </div>
                                </div>

                                </div>
                              </div>
                          </div>
                      </div>

                    <div class="col-md-6">
                    <div>                      
                        <div class="box-body">
                            <div class="form-group">
                                     <div class="form-group">
                                         <div class="col-sm-6">
                                 <button type="submit" id="rep24" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep24_ServerClick">Pending SO</button>
                                      </div>

                                    <div class="col-sm-6">
                                  <button type="submit" id="rep25" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep25_ServerClick">Confirmed order Report</button>
                                        </div>
                             
                                </div>

                                
                              <div class="form-group">
                                   
                                    <div class="col-sm-6">
                                   <asp:Label ID="lbl25" runat="server" Text=""   Height="15px"  Style="text-align: center; font-weight: 700"></asp:Label>
                                        </div>
                                  <div class="col-sm-6">
                                  <asp:Label ID="lbl26" runat="server" Text=""   Height="15px"  Style="text-align: center; font-weight: 700"></asp:Label>
                                      </div>
                              
                                </div>

                            <div class="form-group">
                                      <div class="col-sm-6">
                                 <button type="submit" id="rep26" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep26_ServerClick">Pending Bills</button>
                                      </div>
                                    <div class="col-sm-6">
                                  <button type="submit" id="rep27" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep27_ServerClick">Finsished stock Grade Wise</button>
                                        </div>
                                
                                </div>

                                 <div class="form-group">
                                   
                                    <div class="col-sm-6">
                                   <asp:Label ID="lbl27" runat="server" Text=""   Height="15px"  Style="text-align: center; font-weight: 700"></asp:Label>
                                        </div>
                              <div class="col-sm-6">
                                  <asp:Label ID="lbl28" runat="server" Text=""   Height="15px"  Style="text-align: center; font-weight: 700"></asp:Label>
                                      </div>
                                </div>
                            
                            <div class="form-group">
                                      <div class="col-sm-6">
                                  <button type="submit" id="rep28" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep28_ServerClick">Sales Order Report Status Wise</button>
                                      </div>
                                    <div class="col-sm-6">
                                 <button type="submit" id="rep29" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep29_ServerClick">Sales Order Report Order Wise</button>
                                        </div>
                               
                                </div>

                                 <div class="form-group">
                                       
                                    <div class="col-sm-6">
                                   <asp:Label ID="lbl29" runat="server" Text=""   Height="15px"  Style="text-align: center; font-weight: 700"></asp:Label>
                                        </div>
                              <div class="col-sm-6">
                                  <asp:Label ID="lbl30" runat="server" Text=""   Height="15px"  Style="text-align: center; font-weight: 700"></asp:Label>
                                      </div>
                                </div>

                                 <div class="form-group">
                                        <div class="col-sm-6">
                                 <button type="submit" id="rep30" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep30_ServerClick">Order Size Report Summary</button>
                                      </div>
                                    <div class="col-sm-6">
                                 <button type="submit" id="rep31" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep31_ServerClick">Order Size Report Customer Wise</button>
                                        </div>
                              
                                </div>

                                 <div class="form-group">

                                    <div class="col-sm-6">
                                   <asp:Label ID="lbl31" runat="server" Text=""   Height="15px"  Style="text-align: center; font-weight: 700"></asp:Label>
                                        </div>
                                <div class="col-sm-6">
                                  <asp:Label ID="lbl32" runat="server" Text=""   Height="15px"  Style="text-align: center; font-weight: 700"></asp:Label>
                                      </div>
                                </div>

                                </div>
                              </div>
                          </div>
                      </div>


                 <div class="col-md-6">
                    <div>                      
                        <div class="box-body">
                            <div class="form-group">
                                     <div class="form-group">
                                     <div class="col-sm-6">
                                 <button type="submit" id="rep32" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep32_ServerClick">Order Size Report Product Wise</button>
                                      </div>
                                    <div class="col-sm-6">
                                  <button type="submit" id="rep33" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep33_ServerClick">order size report part no wise</button>
                                        </div>
                            
                                </div>

                                
                              <div class="form-group">                                       
                                    <div class="col-sm-6">
                                   <asp:Label ID="lbl33" runat="server" Text=""   Height="15px"  Style="text-align: center; font-weight: 700"></asp:Label>
                                        </div>
                                 <div class="col-sm-6">
                                  <asp:Label ID="lbl34" runat="server" Text=""   Height="15px"  Style="text-align: center; font-weight: 700"></asp:Label>
                                      </div>
                                </div>

                            <div class="form-group">
                                <div class="col-sm-6">
                                 <button type="submit" id="rep34" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep34_ServerClick">order size report R/Paper Wise</button>
                                      </div>
                                    <div class="col-sm-6">
                                  <button type="submit" id="rep35" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep35_ServerClick">order size report customer group wise</button>
                                        </div>
                                
                                </div>

                                 <div class="form-group">
                                    <div class="col-sm-6">
                                   <asp:Label ID="lbl35" runat="server" Text=""   Height="15px"  Style="text-align: center; font-weight: 700"></asp:Label>
                                        </div>
                                 <div class="col-sm-6">
                                  <asp:Label ID="lbl36" runat="server" Text=""   Height="15px"  Style="text-align: center; font-weight: 700"></asp:Label>
                                      </div>
                                </div>
                            
                                       <div class="form-group">
                                       <div class="col-sm-6">
                                  <button type="submit" id="rep36" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep36_ServerClick">Order Size Report MTR Wise</button>
                                      </div>
                                    <div class="col-sm-6">
                                 <button type="submit" id="rep37" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep37_ServerClick">order size report MTR wise summary</button>
                                        </div>
                                <%-- <div class="col-sm-6">
                                 <button type="submit" id="rep38" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep38_ServerClick">report</button>
                                      </div>--%>
                                      
                                </div>

                                 <div class="form-group">
                                    <div class="col-sm-6">
                                   <asp:Label ID="lbl37" runat="server" Text=""   Height="15px"  Style="text-align: center; font-weight: 700"></asp:Label>
                                        </div>
                                 <div class="col-sm-6">
                                  <asp:Label ID="lbl38" runat="server" Text=""   Height="15px"  Style="text-align: center; font-weight: 700"></asp:Label>
                                      </div>
                                </div>

                                       <div class="form-group">
                                        <div class="col-sm-6">
                                 <button type="submit" id="rep39" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep39_ServerClick">deactivated item list</button>
                                        </div>
                                     <%-- <div class="col-sm-6">
                                 <button type="submit" id="rep40" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep40_ServerClick">Report</button>
                                      </div>--%>
                                </div>


                                 <div class="form-group">
                                    <div class="col-sm-6">
                                   <asp:Label ID="Label1" runat="server" Text=""   Height="15px"  Style="text-align: center; font-weight: 700"></asp:Label>
                                        </div>
                                 <div class="col-sm-6">
                                  <asp:Label ID="Label2" runat="server" Text=""   Height="15px"  Style="text-align: center; font-weight: 700"></asp:Label>
                                      </div>
                                </div>


                                </div>
                              </div>
                          </div>
                      </div>

                  <div class="col-md-6" style="display:none;">
                    <div>                      
                        <div class="box-body">
                            <div class="form-group">

                      

                                 <div class="form-group" >
                                   <%-- <div class="col-sm-6">
                                 <button type="submit" id="rep39" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep39_ServerClick">deactivated item list</button>
                                        </div>--%>
                                  <%--   <div class="col-sm-6">
                                 <button type="submit" id="rep39" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep39_ServerClick">deactivated item list</button>
                                        </div>--%>
                                      <div class="col-sm-6">
                                 <button type="submit" id="rep38" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep38_ServerClick">report</button>
                                      </div>
                           
                                </div>

                                 <div class="form-group"  style="display:none;">
                                    <div class="col-sm-6">
                                   <asp:Label ID="lbl39" runat="server" Text=""   Height="15px"  Style="text-align: center; font-weight: 700"></asp:Label>
                                        </div>
                                 <div class="col-sm-6">
                                  <asp:Label ID="lbl40" runat="server" Text=""   Height="15px"  Style="text-align: center; font-weight: 700"></asp:Label>
                                      </div>
                                </div>
                                
                          
                                 </div>
                               </div>
                           </div>
                         </div>
                </div>
                </section>
         </div>
    
    <asp:HiddenField ID="hfhcid" runat="server" />
    <asp:HiddenField ID="hfval" runat="server" />
    <asp:HiddenField ID="hfcode" runat="server" />
    <asp:HiddenField ID="hfbr" runat="server" />
    <asp:HiddenField ID="hf1" runat="server" />
     <asp:HiddenField ID="hf2" runat="server" />
    <asp:HiddenField ID="hfaskBranch" runat="server" />
     <asp:HiddenField ID="hfid" runat="server" />
    <asp:HiddenField ID="hfaskPrdRange" runat="server" />
    <asp:Button ID="btnhideF" runat="server" OnClick="btnhideF_Click" Style="display: none" />
    <asp:Button ID="btnhideF_s" runat="server" OnClick="btnhideF_s_Click" Style="display: none" />
</asp:Content>
