<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="om_Web_Rpt_DLJM" Title="Tejaxo" CodeFile="om_Web_Rpt_DLJM.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

     <div class="content-wrapper">
        <section class="content-header">
            <table style="width: 100%">
                <tr>                    
                    <td>
                          <asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large">Sales Reports(Tronica)</asp:Label>                      
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
                                   <div class="col-sm-12" >
                                        <asp:Label ID="Label4" runat="server" Text="" Height="30px"  Style="text-align: center; font-weight: 200"></asp:Label>
                                   </div>                                                                  
                                   <div class="col-sm-12">
                                        <button type="submit" id="rep1" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep1_ServerClick">1.All Party Wise Month (Sale Analysis)</button>
                                    </div>
                               <%--     <div class="col-sm-12" >
                                        <asp:Label ID="Label3" runat="server" Text="" Height="2px"  Style="text-align: center; font-weight: 200"></asp:Label>
                                    </div>                  --%>            
                                    <div class="col-sm-12">
                                        <button type="submit" id="rep4" class="btn btn-info" style="width:100%" runat="server" onserverclick="rep4_ServerClick">2.Item Group Wise (Qty)</button>
                                    </div>
   <%--                                 <div class="col-sm-12">
                                        <asp:Label ID="lbl1" runat="server" Text="" Height="2px"  Style="text-align: center; font-weight: 200"></asp:Label>
                                    </div>--%>
                                    <div class="col-sm-12">
                                        <button type="submit" id="rep11" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep11_ServerClick">3.One Item All Party(Sales Analysis)</button>
                                     </div>                                            
  <%--                                  <div class="col-sm-12">
                                        <asp:Label ID="lb2" runat="server" Text="" Height="2px"  Style="text-align: center; font-weight: 200"></asp:Label>
                                    </div>--%>
                                    <div class="col-sm-12">
                                        <button type="submit" id="rep13" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep13_ServerClick">4. One Party All Items (Sales Analysis)</button>
                                    </div>
  <%--                                   <div class="col-sm-12">
                                        <asp:Label ID="lbl3" runat="server" Text=""  Height="2px" Style="text-align: center; font-weight: 200"></asp:Label>
                                     </div>      --%>                         
                                     <div class="col-sm-12">
                                         <button type="submit" id="rep15" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep15_ServerClick">5.One Item Group of Party(Sales Analysis)</button>
                                      </div>
<%--                                    <div class="col-sm-12">
                                        <asp:Label ID="Label6" runat="server" Text=""  Height="5px" Style="text-align: center; font-weight: 200"></asp:Label>
                                     </div>           --%>
     <%--                                <div class="col-sm-12">
                                        <asp:Label ID="lbl4" runat="server" Text="" Height="5px" Style="text-align: center; font-weight: 200"></asp:Label>--%>
                                    <div class="col-sm-12">
                                        <button type="submit" id="rep12" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep12_ServerClick">6.Market Group wise Item wise</button>
                                     </div>
 <%--                                   <div class="col-sm-12">
                                        <asp:Label ID="lbl8" runat="server" Text="" Height="5px" Style="text-align: center; font-weight: 200"></asp:Label>
                                    </div>--%>
                                    <div class="col-sm-12">
                                        <button type="submit" id="rep14" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep14_ServerClick">7.One Party All Items Grp(Sales Analysis)</button>
                                    </div>
 <%--                                   <div class="col-sm-12">
                                        <asp:Label ID="Label1" runat="server" Text="" Height="5px" Style="text-align: center; font-weight: 200"></asp:Label>
                                     </div>--%>
                                    <div class="col-sm-12">
                                        <button type="submit" id="rep19" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep19_ServerClick">8.Party Group wise Std. Report</button>
                                     </div>     
 <%--                                    <div class="col-sm-12">
                                        <asp:Label ID="Label2" runat="server" Text="" Height="5px" Style="text-align: center; font-weight: 200"></asp:Label>
                                    </div>--%>
                                    <div class="col-sm-12">
                                         <button type="submit" id="rep21" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep21_ServerClick">9.Distributors Wise Sales </button>
                                     </div>
<%--                                    <div class="col-sm-12">
                                        <asp:Label ID="lbl13" runat="server" Text="" Height="5px"  Style="text-align: center; font-weight: 200"></asp:Label>
                                    </div>--%>
                                    <div class="col-sm-12">
                                         <button type="submit" id="rep23" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep23_ServerClick">10.Party Wise Area Manager Wise(Sales Analysis)</button>
                                     </div>
                                    <div class="col-sm-12">
                                        <asp:Label ID="Label5" runat="server" Text="" Height="40px" Style="text-align: center; font-weight: 200"></asp:Label>
                                    </div> 
                                 </div>
                             </div>
                        </div>
                     </div>
  
                <div class="col-md-6">
                    <div>                      
                        <div class="box-body">

                                           <table style="width: 100%;">
                                                <tr>
                                                     <td rowspan="5">
                                                        <img src="../tej-base/images/jaypee.jpg" height="430" style="padding-left:200px" />
                                                    </td>
                                                </tr>
                                            </table>

                             </div>
                         </div>
                    </div>
                </div>
       </section> 
   


                <div class="col-md-12">
                    <div>                      
                        <div class="box-body">
                            <div class="form-group" style="text-align:center">
                                   <div class="col-sm-12" >
                                        <button type="submit" id="btnexit" class="btn btn-info" style="width: 200px" runat="server" accesskey="X" onserverclick="btnexit_ServerClick">E<u>x</u>it</button>
                                     </div> 
                                </div>
                             </div>
                          </div>
                      </div>
  
 </div>
   <%--                         <div class="form-group">
                                     <div class="form-group" style="display:none;" >
                                    <div class="col-sm-6" style="display:none;">
                                  <button type="submit" id="rep9" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep9_ServerClick">Item Std Qtr Report</button>
                                        </div>
                                
                                </div>--%>

                                
  
                               
 <%--                           <div class="form-group">                                
                                   <div class="col-sm-6">
                                  <button type="submit" id="rep19" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep19_ServerClick">13.Party Grp Std (Sales Analysis)</button>
                                        </div>

                                </div>

                                 <div class="form-group">
 
                                 <div class="col-sm-6">
                                  <asp:Label ID="lbl14" runat="server" Text="" Height="15px" Style="text-align: center; font-weight: 700"></asp:Label>
                                      </div>
                                </div>
                                
                                 <div class="form-group" style="display:none;">
                               
                                 <div class="col-sm-6">
                                 <button type="submit" id="rep16" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep16_ServerClick">Party Group Qtr(Sales Analysis)</button>
                                      </div>
                                    
                                       <div class="col-sm-6">
                                 <button type="submit" id="rep18" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep18_ServerClick">12.Party Group Wise Std Report</button>
                                      </div>
                                </div>

                                 <div class="form-group" style="display:none;">
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
                            <div class="form-group">
                                     <div class="form-group" style="display:none;">                                  
 
                                </div>
                                
                              <div class="form-group" style="display:none;">
                                    <div class="col-sm-6">
                                   <asp:Label ID="lbl17" runat="server" Text=""    Height="15px" Style="text-align: center; font-weight: 700"></asp:Label>
                                        </div>
                                 <div class="col-sm-6">
                                  <asp:Label ID="lbl18" runat="server" Text=""   Height="15px"  Style="text-align: center; font-weight: 700"></asp:Label>
                                      </div>
                                </div>

                            <div class="form-group" style="display:none;">                                  
                                 <div class="col-sm-6">
                                  <button type="submit" id="rep20" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep20_ServerClick">Party Wise Qtr(Sales Analysis)</button>
                                      </div>
                                </div>

                                 <div class="form-group" style="display:none;">
                                    <div class="col-sm-6">
                                   <asp:Label ID="lbl19" runat="server" Text=""   Height="15px"  Style="text-align: center; font-weight: 700"></asp:Label>
                                        </div>
                                 <div class="col-sm-6">
                                  <asp:Label ID="lbl20" runat="server" Text=""   Height="15px"  Style="text-align: center; font-weight: 700"></asp:Label>
                                      </div>
                                </div>
                            
                            <div class="form-group" style="display:none;">
                                   
                                 <div class="col-sm-6" style="display:none;">
                                 <button type="submit" id="rep22" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep22_ServerClick">16.Party Grp std qtry (Sales Analysis) </button>
                                      </div>
                                </div>

                                 <div class="form-group" style="display:none;">
                                    <div class="col-sm-6">
                                   <asp:Label ID="lbl21" runat="server" Text=""   Height="15px"  Style="text-align: center; font-weight: 700"></asp:Label>
                                        </div>
                                 <div class="col-sm-6">
                                  <asp:Label ID="lbl22" runat="server" Text=""  Height="15px"   Style="text-align: center; font-weight: 700"></asp:Label>
                                      </div>
                                </div>

                      

                                                 <div class="form-group" style="display:none;">
                                    <div class="col-sm-6">
                                  <button type="submit" id="rep25" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep25_ServerClick">14.Items Sales Analysis</button>
                                        </div>
                                 <div class="col-sm-6">
                                 <button type="submit" id="rep26" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep26_ServerClick">16.Sales 2 (Sales Analyis)</button>
                                      </div>
                                </div>

                                
                              <div class="form-group" style="display:none;">
                                    <div class="col-sm-6">
                                   <asp:Label ID="lbl25" runat="server" Text=""   Height="15px"  Style="text-align: center; font-weight: 700"></asp:Label>
                                        </div>
                                 <div class="col-sm-6">
                                  <asp:Label ID="lbl26" runat="server" Text=""   Height="15px"  Style="text-align: center; font-weight: 700"></asp:Label>
                                      </div>
                                </div>

                                </div>
                              </div>
                          </div>
                      </div>

                    <div class="col-md-6"  style="display:none;">
                    <div>                      
                        <div class="box-body">
                            <div class="form-group">
                    

                            <div class="form-group" style="display:none;">
                                    <div class="col-sm-6">
                                  <button type="submit" id="rep27" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep27_ServerClick">17.Item Grp Wise (Sales Analysis)</button>
                                        </div>
                                 <div class="col-sm-6">
                                  <button type="submit" id="rep28" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep28_ServerClick">19.Party Wise Sales Analysis Report</button>
                                      </div>
                                </div>

                                 <div class="form-group" style="display:none;">
                                    <div class="col-sm-6">
                                   <asp:Label ID="lbl27" runat="server" Text=""   Height="15px"  Style="text-align: center; font-weight: 700"></asp:Label>
                                        </div>
                                 <div class="col-sm-6">
                                  <asp:Label ID="lbl28" runat="server" Text=""   Height="15px"  Style="text-align: center; font-weight: 700"></asp:Label>
                                      </div>
                                </div>

                                     <div class="form-group" style="display:none;">
                                 <%--   <div class="col-sm-6">
                                 <button type="submit" id="rep23" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep23_ServerClick">18.Party Wise Area Magr Wise(Sales Analysis)</button>
                                        </div>--%>
<%--                                 <div class="col-sm-6">
                                 <button type="submit" id="rep24" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep24_ServerClick">20.Party Wise Columan(Sales Analysis)</button>
                                      </div>
                                </div>

                                 <div class="form-group" style="display:none;">
                                    <div class="col-sm-6">
                                   <asp:Label ID="lbl23" runat="server" Text=""   Height="15px"  Style="text-align: center; font-weight: 700"></asp:Label>
                                        </div>
                                 <div class="col-sm-6">
                                  <asp:Label ID="lbl24" runat="server" Text=""   Height="15px"  Style="text-align: center; font-weight: 700"></asp:Label>
                                      </div>
                                </div>
                            
                            <div class="form-group" style="display:none;">
                                    <div class="col-sm-6">
                                 <button type="submit" id="rep29" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep29_ServerClick"> Report</button>
                                        </div>
                                 <div class="col-sm-6">
                                 <button type="submit" id="rep30" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep30_ServerClick"> Report</button>
                                      </div>
                                </div>

                               <div class="form-group" style="display:none;">
                                    <div class="col-sm-6">
                                   <asp:Label ID="lbl29" runat="server" Text=""   Height="15px"  Style="text-align: center; font-weight: 700"></asp:Label>
                                        </div>
                                 <div class="col-sm-6">
                                  <asp:Label ID="lbl30" runat="server" Text=""   Height="15px"  Style="text-align: center; font-weight: 700"></asp:Label>
                                      </div>
                                </div>

                                <div class="form-group" style="display:none;">
                                    <div class="col-sm-6">
                                 <button type="submit" id="rep31" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep31_ServerClick"> Report</button>
                                        </div>
                                 <div class="col-sm-6">
                                 <button type="submit" id="rep32" class="btn btn-info" style="width:100%;" runat="server" onserverclick="rep32_ServerClick"> Report</button>
                                      </div>
                                </div>

                                 <div class="form-group" style="display:none;">
                                    <div class="col-sm-6">
                                   <asp:Label ID="lbl31" runat="server" Text=""   Height="15px"  Style="text-align: center; font-weight: 700"></asp:Label>
                                        </div>
                                 <div class="col-sm-6">
                                  <asp:Label ID="lbl32" runat="server" Text=""   Height="15px"  Style="text-align: center; font-weight: 700"></asp:Label>
                                      </div>--%>
     <%--                           </div>--%>

    
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
