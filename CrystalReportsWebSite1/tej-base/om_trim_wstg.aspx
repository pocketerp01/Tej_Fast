<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="om_trim_wstg" CodeFile="om_trim_wstg.aspx.cs" %>

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
                                       
                        <td style="text-align: left">
                        <button type="submit" id="btnnew" class="btn btn-info" style="width: 100px;" runat="server" accesskey="N" onserverclick="btnnew_ServerClick"><u>N</u>ew</button>
                        <button type="submit" id="btnedit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="i" onserverclick="btnedit_ServerClick">Ed<u>i</u>t</button>
                        <button type="submit" id="btnsave" class="btn btn-info" style="width: 100px;" runat="server" accesskey="s" onserverclick="btnsave_ServerClick"><u>S</u>ave</button>
                        <button type="submit" id="btnprint" class="btn btn-info" style="width: 100px;" runat="server" accesskey="P" onserverclick="btnprint_ServerClick"><u>P</u>rint</button>
                        <button type="submit" id="btndel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="l" onserverclick="btndel_ServerClick">De<u>l</u>ete</button>
                        <button type="submit" id="btnlist" class="btn btn-info" style="width: 100px;" runat="server" accesskey="t" onserverclick="btnlist_ServerClick">Lis<u>t</u></button>
                        <button type="submit" id="btncancel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="c" onserverclick="btncancel_ServerClick"><u>C</u>ancel</button>
                        <button type="submit" id="btnexit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="X" onserverclick="btnexit_ServerClick">E<u>x</u>it</button>
                        <button type="submit" id="btnrefresh" class="btn btn-info" style="width: 100px;" runat="server" accesskey="N" onserverclick="btnrefresh_ServerClick">R<u>e</u>fresh</button>
                    </td>
                       <td>
                        <asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label></td>  
                </tr>
            </table>
        </section>

        <section class="content">
            <div class="row">

                <div class="col-md-6">
                    <div>                
                        <div class="box-body">
                            <div class="form-group">
                                <asp:Label ID="lbl1" runat="server" Text="lbl1" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Entry No.</asp:Label>
                                <div class="col-sm-1">
                                    <asp:Label ID="lbl1a" runat="server" Text="TM"  Style="width: 22px; float: right;" CssClass="col-sm-1 control-label"></asp:Label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtvchnum" runat="server" Width="100%" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label18" runat="server" Text="lbl1"  CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Date</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtvchdate" placeholder="Date"  runat="server" Width="100%" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtvchdate_CalendarExtender" runat="server" Enabled="True" TargetControlID="txtvchdate" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="Maskedit1" runat="server" Mask="99/99/9999" MaskType="Date" TargetControlID="txtvchdate" />
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="Label19" runat="server" Text="lbl3" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Customer_Name</asp:Label>
                                 <div class="col-sm-1">
                               <asp:ImageButton ID="btnparty" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnparty_Click" />
                                </div>
                                <div class="col-sm-1" style="display:none">
                                     <asp:TextBox ID="txtacode" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                                    </div>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtaname" ReadOnly="true" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                                </div>

                              <%--  <asp:Label ID="Label1" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Process Wastage</asp:Label>--%>
                                <div class="col-sm-3" style="display:none;">
                                    <asp:TextBox ID="txtprocess_wstg" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                                </div>                               
                            </div>

                            <div class="form-group">
                                <asp:Label ID="Label1" runat="server" Text="lbl3" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Job_Name</asp:Label>
                                 <div class="col-sm-1">
                               <asp:ImageButton ID="btnicode" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnicode_Click" />
                                </div>
                                   <div class="col-sm-1" style="display:none">
                                     <asp:TextBox ID="txticode" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                                    </div>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtiname" ReadOnly="true" runat="server" CssClass="form-control" Width="100%" MaxLength="5"></asp:TextBox>
                                </div>                                                                             
                            </div>
                         
                             <div class="form-group">
                                <asp:Label ID="Label4" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Width(in MM)</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtwidthmm" runat="server" BackColor="LightBlue"  autocomplete="off" onkeyup="cal()" onkeypress="return isDecimalKey(event)" CssClass="form-control" Width="100%" MaxLength="5"></asp:TextBox>
                                </div>
                                 <asp:Label ID="Label5" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Height(in MM)</asp:Label>                              
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtvarnish" runat="server"  BackColor="LightBlue" autocomplete="off" MaxLength="5"  onkeyup="cal()" CssClass="form-control" Width="100%" ></asp:TextBox>
                                </div>
                               </div>                            

                        </div>
                    </div>
                </div>
      
                <div class="col-md-6">
                    <div>                   
                        <div class="box-body">     
                                                   

                            <div class="form-group">
                                <asp:Label ID="Label27" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Ent_by</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtentby" runat="server" ReadOnly="true" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label28" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Ent_Date</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtendtdt" runat="server" ReadOnly="true" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="Label20" runat="server" Text="lbl3" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Cylinder</asp:Label>
                                <div class="col-sm-1">
                               <asp:ImageButton ID="btncylinder" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btncylinder_Click" />
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtfoil" ReadOnly="true"  runat="server" CssClass="form-control" Width="100%" ></asp:TextBox>
                                </div>
                                 <asp:Label ID="Label9" runat="server" Text="lbl3" autocomplete="off" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Gap</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtfoil1"   ReadOnly="true" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>                             
                            </div>

                             <div class="form-group">
                                  <asp:Label ID="Label3" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Around</asp:Label>                                                              
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtinkgsm"  runat="server" ReadOnly="true" autocomplete="off"   CssClass="form-control" Width="100%" MaxLength="5"></asp:TextBox>
                                </div> 
                                <asp:Label ID="Label8" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Across</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtink" runat="server" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" onkeyup="cal()" CssClass="form-control" Width="100%" ></asp:TextBox>
                                </div>                                                           
                            </div>


                            <div class="form-group">
                                <asp:Label ID="Label12" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Paper_Size </asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtoh" ReadOnly="true" runat="server" CssClass="form-control" Width="100%" ></asp:TextBox>
                                </div>
                                <%-- <asp:Label ID="Label28" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Varnish+Papr+Ink+Foil</asp:Label>--%>
                                <div class="col-sm-3" style="display:none;">
                                    <asp:TextBox ID="txttot" runat="server"  ReadOnly="true" CssClass="form-control" Width="100%" ></asp:TextBox>
                                </div>
                            </div>

                         <%--    <div class="form-group" style="display:none;">
                                 <asp:Label ID="Label10" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Varnish</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtvarnish1" runat="server" BackColor="LightBlue" ReadOnly="true" CssClass="form-control" Width="100%" ></asp:TextBox>
                                </div>
                                
                                  <asp:Label ID="Label13" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Total Cost</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txttotcost" ReadOnly="true" BackColor="LightBlue"  runat="server" CssClass="form-control" Width="100%" ></asp:TextBox>
                                </div>
                                 </div>--%>

<%--                            <div class="form-group" style="display:none;">
                                <asp:Label ID="Label14" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Profit1</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtprofit1" runat="server" autocomplete="off" BackColor="LightBlue" ReadOnly="true" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label15" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">SP</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtsp" runat="server" BackColor="LightBlue" ReadOnly="true" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>
                            </div>--%>

                            <table style="width: 100%">
                                <tr style="display: none;">
                                    <td>
                                        <asp:Label ID="lbl2" runat="server" Text="lbl2" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl2" runat="server"></asp:TextBox>
                                        </div>
                                    </td>

                                    <td>
                                        <asp:Label ID="lbl3" runat="server" Text="lbl3" CssClass="col-sm-2 control-label"></asp:Label>

                                    </td>
                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl3" runat="server"></asp:TextBox>
                                        </div>
                                    </td>

                                  
                                </tr>
                                <tr style="display: none;">
                                    <td>
                                        <asp:Label ID="lbl5" runat="server" Text="lbl5" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl5" runat="server"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl6" runat="server" Text="lbl6" CssClass="col-sm-2 control-label"></asp:Label>

                                    </td>


                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl6" runat="server"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr style="display: none;">
                                    <td>
                                        <asp:Label ID="lbl8" runat="server" Text="lbl8" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl8" runat="server"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl9" runat="server" Text="lbl9" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl9" runat="server"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>

                    </div>
                </div>

                   <div class="col-md-12">
                    <div>                   
                        <div class="box-body">     

                                 <div class="form-group">
                                <asp:Label ID="Label2" runat="server" Text="lbl3" CssClass="col-sm-1 control-label" Font-Size="14px" Font-Bold="True">Trim_Wastage</asp:Label>
                                <div class="col-sm-1">
                                    <asp:TextBox ID="txttrmwastg" runat="server" ReadOnly="true" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label6" runat="server" Text="lbl3" CssClass="col-sm-1 control-label" Font-Size="14px" Font-Bold="True">Step1</asp:Label>
                                <div class="col-sm-1">
                                    <asp:TextBox ID="txtstep1" runat="server" ReadOnly="true" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>
                                       <asp:Label ID="Label7" runat="server" Text="lbl3" CssClass="col-sm-1 control-label" Font-Size="14px" Font-Bold="True">Step2</asp:Label>
                                <div class="col-sm-1">
                                    <asp:TextBox ID="txtstep2" runat="server" ReadOnly="true" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>
                                     <asp:Label ID="Label21" runat="server" Text="lbl3" CssClass="col-sm-1 control-label" Font-Size="14px" Font-Bold="True">Step3</asp:Label>
                                <div class="col-sm-1">
                                    <asp:TextBox ID="txtstep3" runat="server" ReadOnly="true" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>

                                     <asp:Label ID="Label29" runat="server" Text="lbl3" CssClass="col-sm-1 control-label" Font-Size="14px" Font-Bold="True">Step4</asp:Label>
                                <div class="col-sm-1">
                                    <asp:TextBox ID="txtstep4" runat="server" ReadOnly="true" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>

                                     <asp:Label ID="Label31" runat="server" Text="lbl3" CssClass="col-sm-1 control-label" Font-Size="14px" Font-Bold="True">Step5</asp:Label>
                                <div class="col-sm-1">
                                    <asp:TextBox ID="txtstep5" runat="server" ReadOnly="true" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>
                            </div>
                            </div></div></div>



                <div class="col-md-6"  style="display:none;">
                    <div>                   
                        <div class="box-body">
                              <div class="form-group">
                                   <asp:Label ID="Label22" runat="server" Text="lbl3" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Width</asp:Label>
                                   <div class="col-sm-1">
                               <asp:ImageButton ID="btnwidth" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnwidth_Click" />
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtwidth" runat="server" ReadOnly="true" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>
                                  <div class="form-group">
                                <asp:Label ID="Label23" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Height</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtheight" runat="server" ReadOnly="true" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>                               
                            </div>
                           
                                   <asp:Label ID="Label17" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">SQ Inch</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtsqinch" runat="server" BackColor="LightBlue"  ReadOnly="true" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>

                                <asp:Label ID="Label24" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Sq. Inch of Label</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtsq_inch_lbl" runat="server" BackColor="LightBlue" ReadOnly="true" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>
                            </div>

                            </div>
                    </div>
                </div>


                 <div class="col-md-6"  style="display:none;">
                    <div>                   
                        <div class="box-body">

                              <div class="form-group">
                                <asp:Label ID="Label25" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Price_per_thousand</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtprice" runat="server" ReadOnly="true" BackColor="LightBlue" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>    
                                  <asp:Label ID="Label11" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Wastage</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtwastge" runat="server" BackColor="LightBlue" ReadOnly="true"  CssClass="form-control" Width="100%" MaxLength="5"></asp:TextBox>
                                </div>                          
                            </div>

                            <div class="form-group">
                                <asp:Label ID="Label30" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Price_per_PC</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtperpc" runat="server" ReadOnly="true" BackColor="LightBlue" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>    
                                </div>

                               <div class="form-group" style="display:none;">
                                <asp:Label ID="Label26" runat="server" Text="lbl3" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Ent_by/Date</asp:Label>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="TextBox14" runat="server" ReadOnly="true" CssClass="form-control" Width="50px"></asp:TextBox>
                               </div>                             
                                   <div class="col-sm-2">
                                    <asp:TextBox ID="TextBox15" runat="server" ReadOnly="true" CssClass="form-control" Width="60px"></asp:TextBox>
                                </div>
                            </div>
                              
                    </div>
                </div>
            </div>

                <div class="col-md-12"  style="display:none;">
                    <div>                       
                        <div class="box-body">
                            <table style="display:none;">
                                <tr>
                                    <td>
                                        <asp:Label ID="Label16" runat="server" Text="lbl3" CssClass="col-sm-1 control-label">Params</asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtp1" runat="server" CssClass="form-control" Width="50px" onkeyup="cal()" MaxLength="2"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtp2" runat="server" CssClass="form-control" Width="50px" onkeyup="cal()" MaxLength="2"></asp:TextBox>
                                    </td>
                                    <td></td>
                                    <td>
                                        <asp:TextBox ID="txtp3" runat="server" CssClass="form-control" Width="50px" onkeyup="cal()" MaxLength="2"></asp:TextBox>
                                    </td>
                                    <td>
                                     <asp:TextBox ID="txtp4" runat="server" CssClass="form-control" Width="50px" onkeyup="cal()" MaxLength="2"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtp5" runat="server" CssClass="form-control" Width="50px" onkeyup="cal()" MaxLength="2"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtp6" runat="server" CssClass="form-control" Width="50px" onkeyup="cal()" MaxLength="2"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtp7" runat="server" CssClass="form-control" Width="50px" onkeyup="cal()" MaxLength="2"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtp8" runat="server" CssClass="form-control" Width="50px" onkeyup="cal()" MaxLength="2"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtp9" runat="server" CssClass="form-control" Width="50px" onkeyup="cal()" MaxLength="2"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtp10" runat="server" CssClass="form-control" Width="50px" onkeyup="cal()" MaxLength="2"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtp11" runat="server" CssClass="form-control" Width="50px" onkeyup="cal()" MaxLength="2"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtp12" runat="server" CssClass="form-control" Width="50px" onkeyup="cal()" MaxLength="2"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtp13" runat="server" CssClass="form-control" Width="50px" onkeyup="cal()" MaxLength="2"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtp14" runat="server" CssClass="form-control" Width="50px" onkeyup="cal()" MaxLength="2"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtp15" runat="server" CssClass="form-control" Width="50px" onkeyup="cal()" MaxLength="2"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>


                <section class="col-lg-12 connectedSortable" style="display:none;">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">
                                <li><a href="#DescTab" id="tab1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab">Export Details</a></li>
                                <li><a href="#DescTab2" id="tab2" runat="server" aria-controls="DescTab2" role="tab" data-toggle="tab">Comm.Terms</a></li>
                                <li><a href="#DescTab3" id="tab3" runat="server" aria-controls="DescTab3" role="tab" data-toggle="tab">Addl.Terms</a></li>
                                <li><a href="#DescTab4" id="tab4" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Delv.Sch</a></li>
                                <li><a href="#DescTab5" id="tab5" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Inv.Dtl</a></li>
                            </ul>
                            <div class="tab-content" >
                                <div role="tabpanel" class="tab-pane active" id="DescTab">
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
                                                <asp:BoundField DataField="sg1_f1" HeaderText="Height" Visible="false" />
                                                <asp:BoundField DataField="sg1_f2" HeaderText="Width" Visible="false" />
                                                <asp:BoundField DataField="sg1_f16" HeaderText="sg1_f16" Visible="false" />
                                                <asp:BoundField DataField="sg1_f17" HeaderText="sg1_f17" Visible="false" />
                                                <asp:BoundField DataField="sg1_f3" HeaderText="sg1_f3" Visible="false" />
                                                <asp:BoundField DataField="sg1_f4" HeaderText="sg1_f4" Visible="false" />
                                                <asp:BoundField DataField="sg1_f19" HeaderText="sg1_f19" Visible="false" />
                                                <asp:BoundField DataField="sg1_f20" HeaderText="sg1_f20" Visible="false" />
                                                <asp:BoundField DataField="sg1_f5" HeaderText="sg1_f5" Visible="false" />
                                                <asp:BoundField DataField="sg1_f6" HeaderText="sg1_f6" Visible="false" />
                                                <asp:BoundField DataField="sg1_f7" HeaderText="sg1_f7" Visible="false" />
                                                <%--  <asp:BoundField DataField="sg1_f8" HeaderText="sg1_f8"  />
                                                <asp:BoundField DataField="sg1_f9" HeaderText="sg1_f9"/>--%>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Height</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_f8" runat="server" Text='<%#Eval("sg1_f8") %>' Width="100%" ReadOnly="true" onkeyup="cal()"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Width</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_f9" runat="server" Text='<%#Eval("sg1_f9") %>' Width="100%" ReadOnly="true" onkeyup="cal()"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="sg1_f18" HeaderText="sg1_f18" Visible="false" />
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>sg1_t30</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t30" runat="server" Text='<%#Eval("sg1_t30") %>' onkeypress="return isDecimalKey(event)" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="sg1_f10" HeaderText="sg1_f10" Visible="false" />
                                                <asp:BoundField DataField="sg1_f11" HeaderText="sg1_f11" Visible="false" />
                                                <asp:BoundField DataField="sg1_f12" HeaderText="sg1_f12" Visible="false" />
                                                <%--  <asp:BoundField DataField="sg1_f13" HeaderText="sg1_f13" />--%>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>sg1_f13</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_f13" runat="server" Text='<%#Eval("sg1_f13") %>' Width="100%" MaxLength="40"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="sg1_f14" HeaderText="sg1_f14" Visible="false" />
                                                <asp:BoundField DataField="sg1_f15" HeaderText="sg1_f15" Visible="false" />
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>sg1_t28</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t28" runat="server" Text='<%#Eval("sg1_t28") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>sg1_t29</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t29" runat="server" Text='<%#Eval("sg1_t29") %>' onkeypress="return isDecimalKey(event)" Width="100%"></asp:TextBox>
                                                        <asp:CalendarExtender ID="sg1_t29_CalendarExtender" runat="server"
                                                            Enabled="True" TargetControlID="sg1_t29"
                                                            Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                        <asp:MaskedEditExtender ID="Maskedit9" runat="server" Mask="99/99/9999"
                                                            MaskType="Date" TargetControlID="sg1_t29" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
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
                                                <asp:TemplateField Visible="false">
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
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>sg1_t16</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t16" runat="server" Text='<%#Eval("sg1_t16") %>' onkeypress="return isDecimalKey(event)" Width="100%" ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
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
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>sg1_t18</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t18" runat="server" Text='<%#Eval("sg1_t18") %>' onkeypress="return isDecimalKey(event)" onkeyup="cal()" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>sg1_t19</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t19" runat="server" Text='<%#Eval("sg1_t19") %>' onkeypress="return isDecimalKey(event)" onkeyup="cal()" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>sg1_t20</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t20" runat="server" Text='<%#Eval("sg1_t20") %>' onkeypress="return isDecimalKey(event)" onkeyup="cal()" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>sg1_t21</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t21" runat="server" Text='<%#Eval("sg1_t21") %>' onkeypress="return isDecimalKey(event)" onkeyup="cal()" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>sg1_t22</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t22" runat="server" Text='<%#Eval("sg1_t22") %>' onkeypress="return isDecimalKey(event)" onkeyup="cal()" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>sg1_t23</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t23" runat="server" Text='<%#Eval("sg1_t23") %>' onkeypress="return isDecimalKey(event)" Width="100%" ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>sg1_t24</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t24" runat="server" Text='<%#Eval("sg1_t24") %>' onkeypress="return isDecimalKey(event)" Width="100%" ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>sg1_t25</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t25" runat="server" Text='<%#Eval("sg1_t25") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
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
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>sg1_t31</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t31" runat="server" Text='<%#Eval("sg1_t31") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>sg1_t32</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t32" runat="server" Text='<%#Eval("sg1_t32") %>' onkeypress="return isDecimalKey(event)" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>sg1_t33</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t33" runat="server" Text='<%#Eval("sg1_t33") %>' onkeypress="return isDecimalKey(event)" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>sg1_t34</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t34" runat="server" Text='<%#Eval("sg1_t34") %>' onkeypress="return isDecimalKey(event)" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
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
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl10" runat="server" Text="lbl10" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl10" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl10_Click" /></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl10" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl11" runat="server" Text="lbl11" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl11" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl11_Click" /></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl11" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl12" runat="server" Text="lbl12" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl12" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl12_Click" /></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl12" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl13" runat="server" Text="lbl13" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl13" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl13_Click" /></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl13" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl14" runat="server" Text="lbl14" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl14" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl14_Click" /></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl14" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl15" runat="server" Text="lbl15" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl15" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl15_Click" /></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl15" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl16" runat="server" Text="lbl16" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl16" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl16_Click" /></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl16" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl17" runat="server" Text="lbl17" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl17" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl17_Click" /></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl17" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl18" runat="server" Text="lbl18" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl18" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl18_Click" /></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl18" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl19" runat="server" Text="lbl19" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl19" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl19_Click" /></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl19" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
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
                                <div role="tabpanel" class="tab-pane active" id="DescTab5">
                                    <div class="lbBody" style="height: 200px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl40" runat="server" Text="lbl40" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl40" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl41" runat="server" Text="lbl41" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl41" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl42" runat="server" Text="lbl42" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl42" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl43" runat="server" Text="lbl43" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl43" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl44" runat="server" Text="lbl44" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl44" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl45" runat="server" Text="lbl45" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl45" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>

                                                    </table>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl46" runat="server" Text="lbl46" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl46" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl47" runat="server" Text="lbl47" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl47" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl48" runat="server" Text="lbl48" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl48" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl49" runat="server" Text="lbl49" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl49" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl50" runat="server" Text="lbl50" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl50" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl51" runat="server" Text="lbl51" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl51" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>

                                                    </table>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>
            
                  <div class="col-md-12">
                    <div>                       
                        <div class="box-body">
                            <div class="form-group">
                              <asp:Label ID="Label10" runat="server" Text="lbl3" CssClass="col-sm-12 control-label" Font-Size="14px" Font-Bold="True">NOTE : Blue Textbox for Data Entry Fields , Grey TextBox for Automatic Calculation and Selected value through Popup button</asp:Label>
                                </div>

                            </div></div></div>
            <%--   <div class="col-md-12">
                    <div>                       
                        <div class="box-body">
                            <div class="form-group">
                                <asp:Label ID="Label29" runat="server" Text="lbl3" CssClass="col-sm-12 control-label" Font-Size="14px" Font-Bold="True">NOTE : White Textbox for Data Entry Fields , Light Blue TextBox for Automatic Calculation.Grey Textbox for Selected value through Popup button</asp:Label>
                                </div></div></div></div>--%>
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
            var t1 = 0; var t2 = 0; var t3 = 0; var t4 = 0; var t5 = 0; var t6 = 0; var papaersize = 0;
            var paper = 0; var width = 0; var height = 0; var acros = 0; var wstg = 0;var gap=0;
            paper = fill_zero(document.getElementById("ContentPlaceHolder1_txtoh").value); //paper size     
            width = fill_zero(document.getElementById("ContentPlaceHolder1_txtwidthmm").value);
            height = fill_zero(document.getElementById("ContentPlaceHolder1_txtvarnish").value);
            acros = fill_zero(document.getElementById("ContentPlaceHolder1_txtink").value);
            gap= fill_zero(document.getElementById("ContentPlaceHolder1_txtfoil1").value);
            
            papaersize = ((height * acros) + (acros - 1 * 3) + 15) / 10;
            t1 = 1000000 / (paper * 10); //step 1
            t2 = t1 / (width + gap);
            t3 = 1550 / ((width * height) / 645);
            t4 = t3 / acros;
            t5 = t4 - t2;
            t6 = t5 / t4 * 100;

            document.getElementById('ContentPlaceHolder1_txtstep1').value = t1.toFixed(2);
            document.getElementById('ContentPlaceHolder1_txtstep2').value = t2.toFixed(2);
            document.getElementById('ContentPlaceHolder1_txtstep3').value = t3.toFixed(2);
            document.getElementById('ContentPlaceHolder1_txtstep4').value = t4.toFixed(2);
            document.getElementById('ContentPlaceHolder1_txtstep5').value = t5.toFixed(2);
            document.getElementById('ContentPlaceHolder1_txttrmwastg').value = t6.toFixed(2);
            document.getElementById('ContentPlaceHolder1_txtoh').value = papaersize.toFixed(2);
        }
        function fill_zero(val) { if (isNaN(val)) return 0; if (isFinite(val)) return val; }

      
    </script>

    <asp:HiddenField ID="TabName" runat="server" />
</asp:Content>
