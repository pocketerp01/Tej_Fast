<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="om_EINV_MENU" Title="Tejaxo" CodeFile="om_EINV_MENU.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

     <div class="content-wrapper">
        <section class="content-header">
            <table style="width: 100%">
                <tr>                    
                    <td>
                          <asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large">E-Invoice Management</asp:Label>                      
                    </td>
                </tr>
            </table>
        </section>

            <section class="content">
            <div class="row">
                <div class="col-md-12">
                    <div>                      
                        <div class="box-body">
                            <div class="form-group">
                                    <div class="col-sm-6">
                                  <button type="submit" id="fetch_irn_dtl" class="btn btn-info" style="width:100%;" runat="server" onserverclick="fetch_irn_dtl_ServerClick">Fetch IRN Details</button>
                                        </div>
                                 <div class="col-sm-6">
                                 <button type="submit" id="print_IRN" class="btn btn-info" style="width:100%;" runat="server" onserverclick="print_IRN_ServerClick">Print IRN(Govt. E-Invoice)</button>
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
                                  <button type="submit" id="Cancel_irn_24hr" class="btn btn-info" style="width:100%;" runat="server" onserverclick="Cancel_irn_24hr_ServerClick">Cancel IRN from IRN portal (24 hrs)</button>
                                        </div>
                                 <div class="col-sm-6">
                                  <button type="submit" id="Cancel_irn_finsyslist" class="btn btn-info" style="width:100%" runat="server" onserverclick="Cancel_irn_finsyslist_ServerClick">Cancelled IRN from portal thru Finsys List</button>
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
                                 <button type="submit" id="cancel_drcr_fins_list" class="btn btn-info" style="width:100%;" runat="server" onserverclick="cancel_drcr_fins_list_ServerClick">Cancelled Dr/Cr Note in Finsys List</button>
                                        </div>
                                 <div class="col-sm-6">
                                 <button type="submit" id="Cancel_Inv_fins_list" class="btn btn-info" style="width:100%;" runat="server" onserverclick="Cancel_Inv_fins_list_ServerClick">Cancelled Invoices in Finsys List</button>
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
                                  <button type="submit" id="incorect_hsn_rate" class="btn btn-info" style="width:100%;" runat="server" onserverclick="incorect_hsn_rate_ServerClick">Incorrect HS Rate Master</button>
                                        </div>
                                 <div class="col-sm-6">
                                  <button type="submit" id="unit_master_dtl" class="btn btn-info" style="width:100%;" runat="server" onserverclick="unit_master_dtl_ServerClick">Unit Master Details </button>
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

                             <div class="form-group">
                                    <div class="col-sm-6">
                                  <button type="submit" id="item_master_dtl" class="btn btn-info" style="width:100%;" runat="server" onserverclick="item_master_dtl_ServerClick">Item Master Details</button>
                                        </div>
                                 <div class="col-sm-6">
                                  <button type="submit" id="party_master_dtl" class="btn btn-info" style="width:100%;" runat="server" onserverclick="party_master_dtl_ServerClick">Party Master Details </button>
                                      </div>
                                </div>

                              <div class="form-group">
                                    <div class="col-sm-6">
                                   <asp:Label ID="Label1" runat="server" Text="" Height="15px" Style="text-align: center; font-weight: 700"></asp:Label>
                                        </div>
                                 <div class="col-sm-6">
                                  <asp:Label ID="Label2" runat="server" Text="" Height="15px" Style="text-align: center; font-weight: 700"></asp:Label>
                                      </div>
                                </div>

                             <div class="form-group">
                                    <div class="col-sm-6">
                                  <button type="submit" id="port_master_dtl" class="btn btn-info" style="width:100%;" runat="server" onserverclick="port_master_dtl_ServerClick">Port Master Details</button>
                                        </div>
                                 <div class="col-sm-6">
                                  <button type="submit" id="Consignee_master_dtl" class="btn btn-info" style="width:100%;" runat="server" onserverclick="Consignee_master_dtl_ServerClick">Consignee Master Details </button>
                                      </div>
                                </div>

                              <div class="form-group">
                                    <div class="col-sm-6">
                                   <asp:Label ID="Label3" runat="server" Text="" Height="15px" Style="text-align: center; font-weight: 700"></asp:Label>
                                        </div>
                                 <div class="col-sm-6">
                                  <asp:Label ID="Label4" runat="server" Text="" Height="15px" Style="text-align: center; font-weight: 700"></asp:Label>
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
