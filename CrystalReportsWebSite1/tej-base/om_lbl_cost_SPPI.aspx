<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="om_lbl_cost_SPPI" CodeFile="om_lbl_cost_SPPI.aspx.cs" %>

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
                <tr style="display: none;">
                    <td>
                        <asp:Label ID="Label11" runat="server" Font-Bold="True" Font-Size="16px"><span><b>(Trim Wastage Part)</b></span></asp:Label></td>

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
                                    <asp:Label ID="lbl1a" runat="server" Text="LC" Style="width: 22px; float: right;" CssClass="col-sm-1 control-label"></asp:Label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtvchnum" runat="server" Width="100%" ReadOnly="true" CssClass="form-control" Height="28px"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label18" runat="server" Text="lbl1" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Date</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtvchdate" placeholder="Date" runat="server" Width="100%" CssClass="form-control" ReadOnly="true" Height="28px"></asp:TextBox>
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
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtacode" ReadOnly="true" runat="server" CssClass="form-control" Width="100%" MaxLength="50" Height="28px"></asp:TextBox>
                                </div>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtaname" runat="server" CssClass="form-control" Width="100%" MaxLength="150" Height="28px"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="Label1" runat="server" Text="lbl3" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Item_Name</asp:Label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnicode" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnicode_Click" />
                                </div>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txticode" ReadOnly="true" runat="server" CssClass="form-control" Width="100%" MaxLength="50" Height="28px"></asp:TextBox>
                                </div>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtiname" runat="server" CssClass="form-control" Width="100%" MaxLength="150" Height="28px"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="Label5" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Label_Height<span style="font-size:x-small;">(mm)</span></asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtlbl_hyt" runat="server" BackColor="LightBlue" autocomplete="off" onkeypress="return isDecimalKey(event)" MaxLength="5" onkeyup="cal()" CssClass="form-control" Width="100%" TabIndex="1" Height="28px"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label52" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">No of Passes<span style="font-size:x-small;">(Nos)</span></asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtpass" runat="server" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" autocomplete="off" onkeyup="cal()" CssClass="form-control" Width="100%" MaxLength="4" TabIndex="7" Height="28px"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="Label4" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Label_Width<span style="font-size:x-small;">(mm)</span></asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtlbl_width" runat="server" BackColor="LightBlue" autocomplete="off" onkeyup="cal()" onkeypress="return isDecimalKey(event)" CssClass="form-control" Width="100%" MaxLength="5" TabIndex="2" Height="28px"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label53" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Req_Width<span style="font-size:x-small;">(mm)</span></asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtreqwidth" runat="server" style="background-color:#eee" autocomplete="off" CssClass="form-control" Width="100%" TabIndex="8" Height="28px"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group" id="lblcostingdiv" runat="server">
                                <asp:Label ID="Label93" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Min</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtMIN" runat="server" BackColor="LightBlue" autocomplete="off" onkeypress="return isDecimalKey(event)" MaxLength="10" onkeyup="cal()" CssClass="form-control" Width="100%" TabIndex="1" Height="28px"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label94" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Max</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtMAX" runat="server" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" autocomplete="off" onkeyup="cal()" CssClass="form-control" Width="100%" MaxLength="4" TabIndex="7" Height="28px"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="Label24" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Quantity<span style="font-size:x-small;">(Nos)</span> </asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtqty" runat="server" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" autocomplete="off" onkeyup="cal()" CssClass="form-control" Width="130px" MaxLength="6" TabIndex="3" Height="28px"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label14" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">No_of_Colour</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtcolor" runat="server" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" autocomplete="off" onkeyup="cal()" CssClass="form-control" Width="130px" MaxLength="6" TabIndex="9" Height="28px"></asp:TextBox>
                                </div>
                            </div>


                            <div class="form-group">
                                <asp:Label ID="Label8" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Across<span style="font-size:x-small;">(Nos)</span> </asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txt_acros" runat="server" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" autocomplete="off" onkeyup="cal()" CssClass="form-control" Width="100%" MaxLength="6" TabIndex="4" Height="28px"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label3" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Total_Wastage<span style="font-size:x-small;">(mtr)</span> </asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txt_tot_wstg" runat="server" style="background-color:#eee" onkeypress="return isDecimalKey(event)" autocomplete="off" onkeyup="cal()" CssClass="form-control" Width="100%" MaxLength="6" TabIndex="10" Height="28px"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">

                                <asp:Label ID="Label92" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Around<span style="font-size:x-small;">(Nos)</span> </asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtaround" runat="server" BackColor="LightBlue" autocomplete="off" onkeypress="return isDecimalKey(event)" onkeyup="cal()" CssClass="form-control" Width="100%" MaxLength="6" TabIndex="5" Height="28px"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label91" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Gap_Across<span style="font-size:x-small;">(mm)</span> </asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txt_gap_acros" runat="server" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" autocomplete="off" onkeyup="cal()" CssClass="form-control" Width="100%" MaxLength="6" TabIndex="11" Height="28px"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="Label25" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">UPS</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtups" runat="server" style="background-color:#eee" onkeyup="cal()" CssClass="form-control" Width="100%" MaxLength="6" TabIndex="6" Height="28px"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label30" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Gap Around<span style="font-size:x-small;">(mm)</span> </asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtgaparound" runat="server" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" autocomplete="off" onkeyup="cal()" CssClass="form-control" Width="100%" MaxLength="6" TabIndex="12" Height="28px"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group" id="lblcostingdiv2" runat="server">
                                <asp:Label ID="Label107" runat="server" Text="lbl3" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Cylinder<span style="font-size:x-small;">(mm)</span></asp:Label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnCyl" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnCyl_Click" />
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtCylInch" runat="server" BackColor="LightBlue" autocomplete="off" onkeypress="return isDecimalKey(event)" MaxLength="10" onkeyup="cal()" CssClass="form-control" Width="100%" TabIndex="1" Height="28px"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label108" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Teeth </asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtTeeth" runat="server" style="background-color:#eee" onkeypress="return isDecimalKey(event)" autocomplete="off" onkeyup="cal()" CssClass="form-control" Width="100%" MaxLength="6" TabIndex="12" Height="28px"></asp:TextBox>
                                </div>
                            </div>

                            <%--                               <div class="form-group">
                                <asp:Label ID="Label2" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Actual_Width_of_Matl</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txttrmwastg" runat="server"  BackColor="LightBlue" onkeypress="return isDecimalKey(event)" autocomplete="off" onkeyup="cal()" MaxLength="5" CssClass="form-control" Width="100%" Height="28px"></asp:TextBox>
                                </div>                                     
                                     <asp:Label ID="Label6" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Difference</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtdiff" runat="server" style="background-color:#eee" CssClass="form-control" Width="100%" Height="28px"></asp:TextBox>
                                </div>
                                </div>  
                            
                              <div class="form-group">
                                <asp:Label ID="Label12" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Setting_Wstg_Per_Colour</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtsetting_wstg_pclor" runat="server" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" autocomplete="off" onkeyup="cal()" MaxLength="5" CssClass="form-control" Width="100%" Height="28px"></asp:TextBox>
                                </div>                                     
                                     <asp:Label ID="Label17" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Running_Mtr(MM)</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtrungmtr_mm" runat="server" style="background-color:#eee" CssClass="form-control" Width="100%" Height="28px"></asp:TextBox>
                                </div>
                                </div>   
                            
                             <div class="form-group">
                                <asp:Label ID="Label54" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Running_Mtr(Mtr)</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtrungmtr_mtr" runat="server" style="background-color:#eee" CssClass="form-control" Width="100%" Height="28px"></asp:TextBox>
                                </div>                                     
                                     <asp:Label ID="Label55" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Total_RMtr_Used</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txttot_rung_mtr" runat="server" style="background-color:#eee" CssClass="form-control" Width="100%" Height="28px"></asp:TextBox>
                                </div>
                                </div>
                                <div class="form-group">
                                <asp:Label ID="Label60" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Tot_Sq_Mtr</asp:Label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txt_tot_sqm" runat="server" style="background-color:#eee" CssClass="form-control" Width="100%" Height="28px"></asp:TextBox>
                                </div>                    
                                     </div>--%>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <asp:Label ID="Label2" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Act_Width_of_Matl<span style="font-size:x-small;">(mm)</span> </asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txttrmwastg" runat="server" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" autocomplete="off" onkeyup="cal()" MaxLength="5" CssClass="form-control" Width="100%" TabIndex="13" Height="28px"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label6" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Difference<span style="font-size:x-small;">(mm)</span></asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtdiff" runat="server" style="background-color:#eee" CssClass="form-control" Width="100%" TabIndex="22" Height="28px"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="Label12" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Setting_Wstg_Per_Clr<span style="font-size:x-small;">(mtr)</span></asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtsetting_wstg_pclor" runat="server" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" autocomplete="off" onkeyup="cal()" MaxLength="5" CssClass="form-control" Width="100%" TabIndex="14" Height="28px"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label17" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Running_Mtr<span style="font-size:x-small;">(mm)</span></asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtrungmtr_mm" runat="server" style="background-color:#eee" CssClass="form-control" Width="100%" TabIndex="23" Height="28px"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="Label54" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Running_Mtr<span style="font-size:x-small;">(mtr)</span></asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtrungmtr_mtr" runat="server" style="background-color:#eee" CssClass="form-control" Width="100%" TabIndex="15" Height="28px"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label55" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Tot_RMtr_Used<span style="font-size:x-small;">(mtr)</span></asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txttot_rung_mtr" runat="server" style="background-color:#eee" CssClass="form-control" Width="100%" TabIndex="24" Height="28px"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="Label60" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Tot_Sq_Mtr<span style="font-size:x-small;">(sqm)</span></asp:Label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txt_tot_sqm" runat="server" style="background-color:#eee" CssClass="form-control" Width="130px" TabIndex="16" Height="28px"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group" style="display: none;">
                                <asp:Label ID="Label100" runat="server" Text="lbl3" CssClass="col-sm-12 control-label" Font-Size="18px" Style="text-align: center; border-style: groove; border-width: medium" Font-Bold="True">Machine Detail</asp:Label>
                            </div>


                            <div class="form-group">
                                <asp:Label ID="Label105" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="11px" Font-Bold="True">TotRate_Emboss_Varnish<span style="font-size:x-small;">(Amt)</span></asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txttot_rt_For_emb_varnish" runat="server" style="background-color:#eee" CssClass="form-control" Width="100%" MaxLength="10" onkeypress="return isDecimalKey(event)" onkeyup="cal()" TabIndex="17" Height="28px"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label106" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Screen_Printing<span style="font-size:x-small;">(Amt)</span></asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txt_screen_print" runat="server" style="background-color:#eee" onkeypress="return isDecimalKey(event)" CssClass="form-control" Width="100%" MaxLength="10" onkeyup="cal()" TabIndex="25" Height="28px"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="Label59" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Delivery_&_Dispatch<span style="font-size:x-small;">(Amt)</span></asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txt_del_desp" runat="server" BackColor="LightBlue" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" MaxLength="10" onkeyup="cal()" TabIndex="18" Height="28px"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label7" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Production_Cost<span style="font-size:x-small;">(Amt)</span></asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtprod_cost" runat="server" style="background-color:#eee" CssClass="form-control" Width="100%" TabIndex="26" Height="28px"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="Label9" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Margin_Considered<span style="font-size:x-small;">(%)</span></asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txt_margin_considered" runat="server" BackColor="LightBlue" CssClass="form-control" onkeyup="cal()" MaxLength="5" Width="100%" TabIndex="19" Height="28px"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label21" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Margin_Considered<span style="font-size:x-small;">(Amt)</span></asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtmargin_cost_AED" runat="server" style="background-color:#eee" CssClass="form-control" MaxLength="5" Width="100%" onkeyup="cal()" TabIndex="27" Height="28px"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="Label20" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Total</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txttotal" runat="server" style="background-color:#eee" CssClass="form-control" Width="100%" onkeyup="cal()" TabIndex="20" Height="28px"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label29" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Vat<span style="font-size:x-small;">(%)</span></asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtvat_percent" runat="server" BackColor="LightBlue" onkeyup="cal()" CssClass="form-control" MaxLength="3" Width="100%" TabIndex="28" Height="28px"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="Label31" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Vat_Value<span style="font-size:x-small;">(Amt)</span></asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtval_Value" runat="server" style="background-color:#eee" CssClass="form-control" Width="100%" onkeyup="cal()" TabIndex="21" Height="28px"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label56" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Grand_Total<span style="font-size:x-small;">(Amt)</span></asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txt_grand_tot" runat="server" style="background-color:#eee" onkeyup="cal()" CssClass="form-control" MaxLength="3" Width="100%" TabIndex="29" Height="28px"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group" id="lblcostingdiv3" runat="server">
                                <button type="submit" id="btnCylView" class="bg-green btn-foursquare" style="width: 200px; margin-left: 10px; margin-top: 3px;" runat="server" onserverclick="btnCylView_ServerClick">View Cylinder Inventory</button>
                            </div>

                            <div class="form-group" style="display: none">
                                <asp:Label ID="Label27" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Ent_by</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtentby" runat="server" style="background-color:#eee" CssClass="form-control" Width="100%" Height="28px"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label28" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Ent_Date</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtendtdt" runat="server" style="background-color:#eee" CssClass="form-control" Width="100%" TabIndex="30" Height="28px"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-6" style="display: none;">
                    <div>
                        <div class="box-body">

                            <div class="form-group">
                                <asp:Label ID="Label15" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Paper/Film(Rate/sq.mtr)</asp:Label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtpaper_Film" runat="server" style="background-color:#eee" CssClass="form-control" Width="100%" Height="28px"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="Label38" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Ink GSM</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtinkgsm" runat="server" BackColor="LightBlue" onkeyup="cal()" autocomplete="off" onkeypress="return isDecimalKey(event)" CssClass="form-control" Width="100%" MaxLength="5" Height="28px"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label39" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Ink(Rate/sq.mtr)</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtink" runat="server" style="background-color:#eee" CssClass="form-control" Width="100%" Height="28px"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="Label33" runat="server" Text="lbl3" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Varnish_Used</asp:Label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnvarnish" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnvarnish_Click" />
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtvarnish" runat="server" style="background-color:#eee" CssClass="form-control" Width="100%" Height="28px"></asp:TextBox>
                                </div>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtvarnishname" runat="server" style="background-color:#eee" CssClass="form-control" Width="100%" Height="28px"></asp:TextBox>
                                </div>
                            </div>


                            <div class="form-group">
                                <asp:Label ID="Label32" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Varnish_GSM</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtvarnish_gsm" runat="server" BackColor="LightBlue" autocomplete="off" onkeyup="cal()" onkeypress="return isDecimalKey(event)" CssClass="form-control" Width="100%" MaxLength="5" Height="28px"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label51" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Process Wastage%</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtprocess_wstg" runat="server" BackColor="LightBlue" CssClass="form-control" onkeyup="cal()" autocomplete="off" onkeypress="return isDecimalKey(event)" Width="100%" MaxLength="5" Height="28px"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="Label34" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Overheads %</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtoverheads" onkeyup="cal()" BackColor="LightBlue" autocomplete="off" onkeypress="return isDecimalKey(event)" runat="server" MaxLength="5" CssClass="form-control" Width="100%" Height="28px"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label35" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Profit %</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtprofit" runat="server" autocomplete="off" BackColor="LightBlue" onkeyup="cal()" onkeypress="return isDecimalKey(event)" CssClass="form-control" Width="100%" MaxLength="5" Height="28px"></asp:TextBox>
                                </div>
                            </div>


                            <div class="form-group">
                                <asp:Label ID="Label50" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Price_per_Pcs</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtperpc" runat="server" style="background-color:#eee" CssClass="form-control" Width="100%" Height="28px"></asp:TextBox>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>

                <div class="col-md-6" style="display: none;">
                    <div>
                        <div class="box-body">

                            <div class="form-group">
                                <asp:Label ID="Label36" runat="server" Text="lbl3" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Foil</asp:Label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnFoil" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnFoil_Click" />
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtfoil" style="background-color:#eee" runat="server" CssClass="form-control" Width="100%" Height="28px"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label37" runat="server" Text="lbl3" autocomplete="off" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Foil_Value</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtfoil1" style="background-color:#eee" runat="server" CssClass="form-control" Width="100%" Height="28px"></asp:TextBox>
                                </div>
                            </div>


                            <div class="form-group">
                                <asp:Label ID="Label40" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">O/H % </asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtoh" style="background-color:#eee" runat="server" CssClass="form-control" Width="100%" Height="28px"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label41" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Varnish+Papr+Ink+Foil</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txttot" runat="server" style="background-color:#eee" CssClass="form-control" Width="100%" Height="28px"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="Label42" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Varnish(Rate/Sq.mtr)</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtvarnish1" runat="server" style="background-color:#eee" CssClass="form-control" Width="100%" Height="28px"></asp:TextBox>
                                </div>

                                <asp:Label ID="Label43" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Total Cost(Sq.mtr)</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txttotcost" style="background-color:#eee" runat="server" CssClass="form-control" Width="100%" Height="28px"></asp:TextBox>
                                </div>
                            </div>


                            <div class="form-group">
                                <asp:Label ID="Label44" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Profit(tot_cost*profit%)</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtprofit1" runat="server" autocomplete="off" style="background-color:#eee" CssClass="form-control" Width="100%" Height="28px"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label45" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">SP</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtsp" runat="server" style="background-color:#eee" CssClass="form-control" Width="100%" Height="28px"></asp:TextBox>
                                </div>
                            </div>


                            <div class="form-group">
                                <asp:Label ID="Label46" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">SQ Inch</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtsqinch" runat="server" style="background-color:#eee" CssClass="form-control" Width="100%" Height="28px"></asp:TextBox>
                                </div>

                                <asp:Label ID="Label47" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Sq. Inch of Label</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtsq_inch_lbl" runat="server" style="background-color:#eee" CssClass="form-control" Width="100%" Height="28px"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="Label48" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Price_per_thousand</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtprice" runat="server" style="background-color:#eee" CssClass="form-control" Width="100%" Height="28px"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label49" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Wastage</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtwastge" runat="server" style="background-color:#eee" CssClass="form-control" Width="100%" MaxLength="5" Height="28px"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-6" style="display: none;">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <asp:Label ID="Label22" runat="server" Text="lbl3" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Width</asp:Label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnwidth" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnwidth_Click" />
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtwidth" runat="server" style="background-color:#eee" CssClass="form-control" Width="100%" Height="28px"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <asp:Label ID="Label23" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Height</asp:Label>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtheight" runat="server" style="background-color:#eee" CssClass="form-control" Width="100%" Height="28px"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>

                <div class="col-md-6" style="display: none;">
                    <div>
                        <div class="box-body">
                            <div class="form-group" style="display: none;">
                                <asp:Label ID="Label26" runat="server" Text="lbl3" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Ent_by/Date</asp:Label>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="TextBox14" runat="server" style="background-color:#eee" CssClass="form-control" Width="50px" Height="28px"></asp:TextBox>
                                </div>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="TextBox15" runat="server" style="background-color:#eee" CssClass="form-control" Width="60px" Height="28px"></asp:TextBox>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>

                <div class="col-md-12" style="display: none;">
                    <div>
                        <div class="box-body">
                            <table style="display: none;">
                                <tr>
                                    <td>
                                        <asp:Label ID="Label16" runat="server" Text="lbl3" CssClass="col-sm-1 control-label">Params</asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtp1" runat="server" CssClass="form-control" Width="50px" onkeyup="cal()" MaxLength="2" Height="28px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtp2" runat="server" CssClass="form-control" Width="50px" onkeyup="cal()" MaxLength="2" Height="28px"></asp:TextBox>
                                    </td>
                                    <td></td>
                                    <td>
                                        <asp:TextBox ID="txtp3" runat="server" CssClass="form-control" Width="50px" onkeyup="cal()" MaxLength="2" Height="28px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtp4" runat="server" CssClass="form-control" Width="50px" onkeyup="cal()" MaxLength="2" Height="28px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtp5" runat="server" CssClass="form-control" Width="50px" onkeyup="cal()" MaxLength="2" Height="28px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtp6" runat="server" CssClass="form-control" Width="50px" onkeyup="cal()" MaxLength="2" Height="28px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtp7" runat="server" CssClass="form-control" Width="50px" onkeyup="cal()" MaxLength="2" Height="28px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtp8" runat="server" CssClass="form-control" Width="50px" onkeyup="cal()" MaxLength="2" Height="28px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtp9" runat="server" CssClass="form-control" Width="50px" onkeyup="cal()" MaxLength="2" Height="28px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtp10" runat="server" CssClass="form-control" Width="50px" onkeyup="cal()" MaxLength="2" Height="28px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtp11" runat="server" CssClass="form-control" Width="50px" onkeyup="cal()" MaxLength="2" Height="28px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtp12" runat="server" CssClass="form-control" Width="50px" onkeyup="cal()" MaxLength="2" Height="28px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtp13" runat="server" CssClass="form-control" Width="50px" onkeyup="cal()" MaxLength="2" Height="28px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtp14" runat="server" CssClass="form-control" Width="50px" onkeyup="cal()" MaxLength="2" Height="28px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtp15" runat="server" CssClass="form-control" Width="50px" onkeyup="cal()" MaxLength="2" Height="28px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
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
                                        <asp:TextBox ID="sg1_f8" runat="server" Text='<%#Eval("sg1_f8") %>' Width="100%" ReadOnly="true" onkeyup="cal()" Height="28px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>Width</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_f9" runat="server" Text='<%#Eval("sg1_f9") %>' Width="100%" ReadOnly="true" onkeyup="cal()" Height="28px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="sg1_f18" HeaderText="sg1_f18" Visible="false" />
                                <asp:TemplateField Visible="false">
                                    <HeaderTemplate>sg1_t30</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t30" runat="server" Text='<%#Eval("sg1_t30") %>' onkeypress="return isDecimalKey(event)" Width="100%" Height="28px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="sg1_f10" HeaderText="sg1_f10" Visible="false" />
                                <asp:BoundField DataField="sg1_f11" HeaderText="sg1_f11" Visible="false" />
                                <asp:BoundField DataField="sg1_f12" HeaderText="sg1_f12" Visible="false" />
                                <%--  <asp:BoundField DataField="sg1_f13" HeaderText="sg1_f13" />--%>
                                <asp:TemplateField Visible="false">
                                    <HeaderTemplate>sg1_f13</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_f13" runat="server" Text='<%#Eval("sg1_f13") %>' Width="100%" MaxLength="40" Height="28px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="sg1_f14" HeaderText="sg1_f14" Visible="false" />
                                <asp:BoundField DataField="sg1_f15" HeaderText="sg1_f15" Visible="false" />
                                <asp:TemplateField Visible="false">
                                    <HeaderTemplate>sg1_t28</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t28" runat="server" Text='<%#Eval("sg1_t28") %>' Width="100%" Height="28px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <HeaderTemplate>sg1_t29</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t29" runat="server" Text='<%#Eval("sg1_t29") %>' onkeypress="return isDecimalKey(event)" Width="100%" Height="28px"></asp:TextBox>
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
                                        <asp:TextBox ID="sg1_t1" runat="server" Text='<%#Eval("sg1_t1") %>' onkeypress="return isDecimalKey(event)" onkeyup="cal()" Width="100%" ReadOnly="true" Height="28px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>P2</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t2" runat="server" Text='<%#Eval("sg1_t2") %>' onkeypress="return isDecimalKey(event)" onkeyup="cal()" Width="100%" ReadOnly="true" Height="28px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderTemplate>P3</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t3" runat="server" Text='<%#Eval("sg1_t3") %>' Width="100%" onkeypress="return isDecimalKey(event)" ReadOnly="true" Height="28px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>P4</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t4" runat="server" Text='<%#Eval("sg1_t4") %>' Width="100%" onkeypress="return isDecimalKey(event)" ReadOnly="true" Height="28px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>P5</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t5" runat="server" Text='<%#Eval("sg1_t5") %>' Width="100%" onkeypress="return isDecimalKey(event)" ReadOnly="true" Height="28px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>P6</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t6" runat="server" Text='<%#Eval("sg1_t6") %>' onkeypress="return isDecimalKey(event)" onkeyup="cal()" Width="100%" ReadOnly="true" Height="28px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>P7</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t7" runat="server" Text='<%#Eval("sg1_t7") %>' onkeypress="return isDecimalKey(event)" Width="100%" ReadOnly="true" Height="28px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <HeaderTemplate>P27</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t27" runat="server" Text='<%#Eval("sg1_t27") %>' onkeypress="return isDecimalKey(event)" Width="100%" ReadOnly="true" Height="28px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>P8</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t8" runat="server" Text='<%#Eval("sg1_t8") %>' onkeypress="return isDecimalKey(event)" Width="100%" ReadOnly="true" Height="28px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>P9</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t9" runat="server" Text='<%#Eval("sg1_t9") %>' onkeypress="return isDecimalKey(event)" Width="100%" ReadOnly="true" Height="28px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>P10</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t10" runat="server" Text='<%#Eval("sg1_t10") %>' onkeypress="return isDecimalKey(event)" Width="100%" ReadOnly="true" Height="28px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>P11</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t11" runat="server" Text='<%#Eval("sg1_t11") %>' onkeypress="return isDecimalKey(event)" Width="100%" ReadOnly="true" Height="28px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>P12</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t12" runat="server" Text='<%#Eval("sg1_t12") %>' onkeypress="return isDecimalKey(event)" Width="100%" ReadOnly="true" Height="28px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderTemplate>P13</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t13" runat="server" Text='<%#Eval("sg1_t13") %>' onkeypress="return isDecimalKey(event)" Width="100%" ReadOnly="true" Height="28px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>P14</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t14" runat="server" Text='<%#Eval("sg1_t14") %>' onkeypress="return isDecimalKey(event)" Width="100%" ReadOnly="true" Height="28px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>P15</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t15" runat="server" Text='<%#Eval("sg1_t15") %>' onkeypress="return isDecimalKey(event)" Width="100%" ReadOnly="true" Height="28px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <HeaderTemplate>sg1_t16</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t16" runat="server" Text='<%#Eval("sg1_t16") %>' onkeypress="return isDecimalKey(event)" Width="100%" ReadOnly="true" Height="28px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <HeaderTemplate>sg1_t17</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t17" runat="server" Text='<%#Eval("sg1_t17") %>' onkeypress="return isDecimalKey(event)" Width="100%" Height="28px"></asp:TextBox>
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
                                        <asp:TextBox ID="sg1_t18" runat="server" Text='<%#Eval("sg1_t18") %>' onkeypress="return isDecimalKey(event)" onkeyup="cal()" Width="100%" Height="28px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <HeaderTemplate>sg1_t19</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t19" runat="server" Text='<%#Eval("sg1_t19") %>' onkeypress="return isDecimalKey(event)" onkeyup="cal()" Width="100%" Height="28px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <HeaderTemplate>sg1_t20</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t20" runat="server" Text='<%#Eval("sg1_t20") %>' onkeypress="return isDecimalKey(event)" onkeyup="cal()" Width="100%" Height="28px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <HeaderTemplate>sg1_t21</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t21" runat="server" Text='<%#Eval("sg1_t21") %>' onkeypress="return isDecimalKey(event)" onkeyup="cal()" Width="100%" Height="28px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <HeaderTemplate>sg1_t22</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t22" runat="server" Text='<%#Eval("sg1_t22") %>' onkeypress="return isDecimalKey(event)" onkeyup="cal()" Width="100%" Height="28px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField Visible="false">
                                    <HeaderTemplate>sg1_t23</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t23" runat="server" Text='<%#Eval("sg1_t23") %>' onkeypress="return isDecimalKey(event)" Width="100%" ReadOnly="true" Height="28px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <HeaderTemplate>sg1_t24</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t24" runat="server" Text='<%#Eval("sg1_t24") %>' onkeypress="return isDecimalKey(event)" Width="100%" ReadOnly="true" Height="28px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <HeaderTemplate>sg1_t25</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t25" runat="server" Text='<%#Eval("sg1_t25") %>' Width="100%" Height="28px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <HeaderTemplate>sg1_t26</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t26" runat="server" Text='<%#Eval("sg1_t26") %>' Width="100%" Height="28px"></asp:TextBox>
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
                                        <asp:TextBox ID="sg1_t31" runat="server" Text='<%#Eval("sg1_t31") %>' Width="100%" Height="28px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <HeaderTemplate>sg1_t32</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t32" runat="server" Text='<%#Eval("sg1_t32") %>' onkeypress="return isDecimalKey(event)" Width="100%" Height="28px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <HeaderTemplate>sg1_t33</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t33" runat="server" Text='<%#Eval("sg1_t33") %>' onkeypress="return isDecimalKey(event)" Width="100%" Height="28px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField Visible="false">
                                    <HeaderTemplate>sg1_t34</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t34" runat="server" Text='<%#Eval("sg1_t34") %>' onkeypress="return isDecimalKey(event)" Width="100%" Height="28px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <HeaderTemplate>sg1_t35</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="sg1_t35" runat="server" Text='<%#Eval("sg1_t35") %>' onkeypress="return isDecimalKey(event)" Width="100%" Height="28px"></asp:TextBox>
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

                <section class="col-lg-12 connectedSortable" id="AllTabs" runat="server">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">
                                <li><a href="#DescTab" id="tab1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab">Material Details</a></li>
                                <li><a href="#DescTab1" id="tab2" runat="server" aria-controls="DescTab1" role="tab" data-toggle="tab">Ink Details</a></li>
                                <li><a href="#DescTab2" id="tab3" runat="server" aria-controls="DescTab2" role="tab" data-toggle="tab">Plates Details</a></li>
                                <li><a href="#DescTab3" id="tab4" runat="server" aria-controls="DescTab3" role="tab" data-toggle="tab">Varnish Details</a></li>
                                <li><a href="#DescTab4" id="tab5" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Die Details</a></li>
                                <li><a href="#DescTab5" id="tab6" runat="server" aria-controls="DescTab5" role="tab" data-toggle="tab">Embossing varnish Details</a></li>
                                <li><a href="#DescTab6" id="tab7" runat="server" aria-controls="DescTab6" role="tab" data-toggle="tab">Embossing White / Screen Printing</a></li>
                                <li><a href="#DescTab7" id="tab8" runat="server" aria-controls="DescTab7" role="tab" data-toggle="tab">Machine Details</a></li>
                            </ul>

                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane active" id="DescTab">
                                    <div class="lbBody" style="height: 170px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <div class="col-md-12">
                                            <div>
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <label id="Label61" runat="server" class="col-sm-1 control-label">Material_Paper</label>
                                                        <div class="col-sm-1">
                                                            <asp:ImageButton ID="btnmatl1" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnmatl1_Click" />
                                                        </div>
                                                        <div class="col-sm-3" style="display: none;">
                                                            <asp:TextBox ID="txtmatl1_code" runat="server" ReadOnly="true" CssClass="form-control" Width="100%" MaxLength="50" Height="28px"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtmatl1" runat="server" ReadOnly="true" CssClass="form-control" Width="100%" MaxLength="50" Height="28px"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtmatl1_val" runat="server" ReadOnly="true" CssClass="form-control" Width="100%" MaxLength="50" Height="28px"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label127" runat="server" class="col-sm-1 control-label">Lamination</label>
                                                        <div class="col-sm-1">
                                                            <asp:ImageButton ID="btnmatl2" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnmatl2_Click" />
                                                        </div>
                                                        <div class="col-sm-3" style="display: none;">
                                                            <asp:TextBox ID="txtmatl2_code" runat="server" ReadOnly="true" CssClass="form-control" Width="100%" MaxLength="50" Height="28px"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtmatl2" runat="server" ReadOnly="true" CssClass="form-control" Width="100%" MaxLength="50" Height="28px"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtmatl2_val" runat="server" ReadOnly="true" CssClass="form-control" Width="100%" MaxLength="50" Height="28px"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label74" runat="server" class="col-sm-1 control-label">Foil_Material</label>
                                                        <div class="col-sm-1">
                                                            <asp:ImageButton ID="btnmatl3" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnmatl3_Click" />
                                                        </div>
                                                        <div class="col-sm-3" style="display: none;">
                                                            <asp:TextBox ID="txtmatl3_code" runat="server" ReadOnly="true" CssClass="form-control" Width="100%" MaxLength="50" Height="28px"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtmatl3" runat="server" ReadOnly="true" CssClass="form-control" Width="100%" MaxLength="50" Height="28px"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtmatl3_val" runat="server" ReadOnly="true" CssClass="form-control" Width="100%" MaxLength="50" Height="28px"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label75" runat="server" class="col-sm-1 control-label">Other_Material</label>
                                                        <div class="col-sm-1">
                                                            <asp:ImageButton ID="btnmatl4" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnmatl4_Click" />
                                                        </div>
                                                        <div class="col-sm-3" style="display: none;">
                                                            <asp:TextBox ID="txtmatl4_code" runat="server" ReadOnly="true" CssClass="form-control" Width="100%" MaxLength="50" Height="28px"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtmatl4" runat="server" ReadOnly="true" CssClass="form-control" Width="100%" MaxLength="50" Height="28px"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtmatl4_val" runat="server" ReadOnly="true" CssClass="form-control" Width="100%" MaxLength="50" Height="28px"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label62" runat="server" class="col-sm-2 control-label">Unit price of the material<span style="font-size: x-small;">(Amt/sqm)</span></label>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txt_unit_price_matl" runat="server" ReadOnly="true" CssClass="form-control" Width="130px" MaxLength="50" Height="28px"></asp:TextBox>
                                                        </div>
                                                        <label id="Label63" runat="server" class="col-sm-2 control-label">Total Cost for the material<span style="font-size: x-small;">(Amt)</span></label>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtcost_matl" runat="server" ReadOnly="true" CssClass="form-control" Width="130px" MaxLength="50" Height="28px"></asp:TextBox>
                                                        </div>
                                                    </div>


                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane active" id="DescTab1">
                                    <div class="lbBody" style="height: 170px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <div class="col-md-12">
                                            <div>
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <label id="Label13" runat="server" class="col-sm-1 control-label">Ink</label>
                                                        <div class="col-sm-1">
                                                            <asp:ImageButton ID="btnink" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnink_Click" />
                                                        </div>
                                                        <div class="col-sm-3" style="display: none;">
                                                            <asp:TextBox ID="txtinkcode" runat="server" ReadOnly="true" CssClass="form-control" Width="100%" Height="28px"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtinkname" runat="server" ReadOnly="true" CssClass="form-control" Width="100%" Height="28px"></asp:TextBox>
                                                        </div>
                                                        <%--<span style="font-size:x-small;">(sqm)</span>--%>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtinkval" runat="server" ReadOnly="true" CssClass="form-control" Width="100%" Height="28px"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label64" runat="server" class="col-sm-2 control-label">Uasage_of_Ink/sqm/Colour<span style="font-size: x-small;">(sqm)</span></label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtusage_ink" BackColor="LightBlue" runat="server" onkeyup="cal()" onkeypress="return isDecimalKey(event)" CssClass="form-control" Width="100%" MaxLength="10" Height="28px"></asp:TextBox>
                                                        </div>
                                                        <label id="Label69" runat="server" class="col-sm-2 control-label">Total_ink_usage<span style="font-size: x-small;">(sqm)</span></label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txt_Tot_ink_usage" runat="server" BackColor="LightBlue" onkeyup="cal()" CssClass="form-control" onkeypress="return isDecimalKey(event)" Width="100%" MaxLength="10" Height="28px"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label109" runat="server" class="col-sm-2 control-label">Total Ink Consumption<span style="font-size: x-small;">(kg)</span></label>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtTotInkCons" runat="server" ReadOnly="true" CssClass="form-control" Width="130px" MaxLength="50" Height="28px"></asp:TextBox>
                                                        </div>
                                                        <label id="Label95" runat="server" class="col-sm-2 control-label">Total Cost for the Ink<span style="font-size: x-small;">(Amt)</span></label>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txt_tot_ink_cost" ReadOnly="true" runat="server" onkeyup="cal()" CssClass="form-control" Width="130px" MaxLength="10" Height="28px"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane active" id="DescTab2">
                                    <div class="lbBody" style="height: 170px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <div class="col-md-12">
                                            <div>
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <label id="lblplate" runat="server" class="col-sm-1 control-label">Name</label>
                                                        <div class="col-sm-1">
                                                            <asp:ImageButton ID="btnplate" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnplate_Click" />
                                                        </div>
                                                        <div class="col-sm-2" style="display: none;">
                                                            <asp:TextBox ID="txtplatecode" runat="server" ReadOnly="true" CssClass="form-control" Width="100%" Height="28px"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtplate_name" runat="server" ReadOnly="true" CssClass="form-control" Width="100%" Height="28px"></asp:TextBox>
                                                        </div>
                                                        <label id="Label98" runat="server" class="col-sm-1 control-label">Unit_Cost<span style="font-size: x-small;">(Amt)</span></label>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtplate_unit_cost" runat="server" ReadOnly="true" CssClass="form-control" Width="100%" Height="28px"></asp:TextBox>
                                                        </div>
                                                        <label id="Label65" runat="server" class="col-sm-2 control-label">Total_Plate_Cost<span style="font-size: x-small;">(Amt)</span></label>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txt_tot_plate_cost" runat="server" ReadOnly="true" CssClass="form-control" Width="100%" Height="28px"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label110" runat="server" class="col-sm-1 control-label">Plate_Area<span style="font-size: x-small;">(cm)</span></label>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtPlateAreaCM" runat="server" ReadOnly="true" CssClass="form-control" Width="100%" Height="28px"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane active" id="DescTab3">
                                    <div class="lbBody" style="height: 170px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <div class="col-md-12">
                                            <div>
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <label id="Label66" runat="server" class="col-sm-1 control-label">Varnish</label>
                                                        <div class="col-sm-1">
                                                            <asp:ImageButton ID="btnvarnish1" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnvarnish1_Click" />
                                                        </div>
                                                        <div class="col-sm-2" style="display: none;">
                                                            <asp:TextBox ID="txtvarnish_code" runat="server" ReadOnly="true" CssClass="form-control" Width="100%" MaxLength="5" Height="28px"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtvarnish_name" runat="server" ReadOnly="true" CssClass="form-control" Width="100%" MaxLength="30" Height="28px"></asp:TextBox>
                                                        </div>
                                                        <label id="Label99" runat="server" class="col-sm-1 control-label">Cost<span style="font-size: x-small;">(Amt)</span></label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txt_varnish_cost" runat="server" ReadOnly="true" CssClass="form-control" Width="100%" MaxLength="10" Height="28px"></asp:TextBox>
                                                        </div>

                                                        <label id="Label68" runat="server" class="col-sm-2 control-label">Usage_of_varnish<span style="font-size: x-small;">(sqm)</span></label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txt_varnish_usage" runat="server" onkeyup="cal()" BackColor="LightBlue" CssClass="form-control" Width="100%" MaxLength="10" onkeypress="return isDecimalKey(event)" Height="28px"></asp:TextBox>
                                                        </div>
                                                        <label id="Label67" runat="server" class="col-sm-2 control-label">Total_Cost_for_Varnish<span style="font-size: x-small;">(Amt)</span></label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txt_tot_varnish_cost" runat="server" ReadOnly="true" CssClass="form-control" Width="100%" MaxLength="10" Height="28px"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane active" id="DescTab4">
                                    <div class="lbBody" style="height: 170px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <label id="Label70" runat="server" class="col-sm-2 control-label">Name_of_Die</label>
                                                        <div class="col-sm-1">
                                                            <asp:ImageButton ID="btndie" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btndie_Click" />
                                                        </div>
                                                        <div class="col-sm-1" style="display: none;">
                                                            <asp:TextBox ID="txtdie_Code" runat="server" BackColor="LightBlue" onkeyup="cal()" CssClass="form-control" Width="100%" MaxLength="5" Height="28px"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtdiename" runat="server" BackColor="LightBlue" onkeyup="cal()" CssClass="form-control" Width="100%" MaxLength="30" Height="28px"></asp:TextBox>
                                                        </div>
                                                        <label id="Label78" runat="server" class="col-sm-3 control-label">Die_Unit_Rt<span style="font-size: x-small;">(Amt/sqcm)</span></label>
                                                        <%-- Unit_Rt_of_Die--%>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtdierate" runat="server" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" onkeyup="cal()" CssClass="form-control" Width="100%" MaxLength="10" Height="28px"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label71" runat="server" class="col-sm-3 control-label">Die_Width<span style="font-size: x-small;">(mm)</span></label>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtdie_width" runat="server" ReadOnly="true" CssClass="form-control" Width="100%" MaxLength="10" Height="28px"></asp:TextBox>
                                                        </div>
                                                        <label id="Label72" runat="server" class="col-sm-3 control-label">Die_Height<span style="font-size: x-small;">(mm)</span></label>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txt_die_hight" runat="server" ReadOnly="true" CssClass="form-control" Width="100%" MaxLength="10" Height="28px"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <label id="Label73" runat="server" class="col-sm-3 control-label">Area_of_Die<span style="font-size: x-small;">(sqcm)</span></label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtdie_area" runat="server" ReadOnly="true" CssClass="form-control" Width="100%" MaxLength="5" Height="28px"></asp:TextBox>
                                                        </div>
                                                        <label id="Label76" runat="server" class="col-sm-3 control-label">No_of_Dies_Req<span style="font-size: x-small;">(Nos)</span></label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtdie_reqd" runat="server" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" onkeyup="cal()" CssClass="form-control" Width="100%" MaxLength="10" Height="28px"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label77" runat="server" class="col-sm-3 control-label">Total_Die_Cost<span style="font-size: x-small;">(Amt)</span></label>
                                                        <div class="col-sm-9">
                                                            <asp:TextBox ID="txtdiecost" runat="server" ReadOnly="true" CssClass="form-control" Width="130px" MaxLength="10" Height="28px"></asp:TextBox>
                                                        </div>
                                                    </div>


                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane active" id="DescTab5">
                                    <div class="lbBody" style="height: 170px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <div class="col-md-12">
                                            <div>
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <label id="Label79" runat="server" class="col-sm-2 control-label">Embossing_Varnish_Name</label>
                                                        <div class="col-sm-1">
                                                            <asp:ImageButton ID="btnemb_varnish" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnemb_varnish_Click" />
                                                        </div>
                                                        <div class="col-sm-1" style="display: none;">
                                                            <asp:TextBox ID="txtemb_varnish_code" runat="server" BackColor="LightBlue" CssClass="form-control" Width="100%" MaxLength="25" Height="28px"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtembvarnish_name" runat="server" ReadOnly="true" CssClass="form-control" Width="100%" MaxLength="5" Height="28px"></asp:TextBox>
                                                        </div>
                                                        <label id="Label80" runat="server" class="col-sm-3 control-label">Unit_Rate<span style="font-size: x-small;">(Amt)</span></label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtembvarnish_val" runat="server" ReadOnly="true" CssClass="form-control" Width="100%" MaxLength="5" Height="28px"></asp:TextBox>
                                                        </div>
                                                        <label id="Label81" runat="server" class="col-sm-2 control-label">%_of_area_for_embosssing<span style="font-size: x-small;">(%)</span></label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtemb_area_varnish" runat="server" BackColor="LightBlue" onkeyup="cal()" onkeypress="return isDecimalKey(event)" CssClass="form-control" Width="100%" MaxLength="10" Height="28px"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label82" runat="server" class="col-sm-3 control-label">Unit_Consumption_for_Varnish<span style="font-size: x-small;">(sqm)</span></label>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txt_consum_varnish" runat="server" BackColor="LightBlue" onkeyup="cal()" onkeypress="return isDecimalKey(event)" CssClass="form-control" Width="100%" MaxLength="10" Height="28px"></asp:TextBox>
                                                        </div>
                                                        <label id="Label85" runat="server" class="col-sm-3 control-label">Tot_Consum_of_embossing_Varnish<span style="font-size: x-small;">(gm)</span></label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txt_tot_embas_varnish_Val" runat="server" ReadOnly="true" CssClass="form-control" Width="100%" MaxLength="5" Height="28px"></asp:TextBox>
                                                        </div>
                                                        <label id="Label83" runat="server" class="col-sm-2 control-label">Screen_Exposing_Charge<span style="font-size: x-small;">(Amt)</span></label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txt_screen_exposing_chg" runat="server" onkeyup="cal()" BackColor="LightBlue" CssClass="form-control" Width="100%" MaxLength="10" onkeypress="return isDecimalKey(event)" Height="28px"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label97" runat="server" class="col-sm-3 control-label">Total_Rate_For_Embossing_Varnish<span style="font-size: x-small;">(Amt)</span></label>
                                                        <div class="col-sm-9">
                                                            <asp:TextBox ID="txt_totrate_emb_varnish" runat="server" ReadOnly="true" onkeyup="cal()" CssClass="form-control" Width="130px" MaxLength="10" Height="28px"></asp:TextBox>
                                                        </div>

                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                    </div>


                                </div>

                                <div role="tabpanel" class="tab-pane active" id="DescTab6">
                                    <div class="lbBody" style="height: 170px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <div class="col-md-12">
                                            <div>
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <label id="Label84" runat="server" class="col-sm-2 control-label">Embossing_White_Screen_Printing</label>
                                                        <div class="col-sm-1">
                                                            <asp:ImageButton ID="btnembossing_Var" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnembossing_Var_Click" />
                                                        </div>
                                                        <div class="col-sm-1" style="display: none;">
                                                            <asp:TextBox ID="txtembossing_var_code" runat="server" ReadOnly="true" CssClass="form-control" Width="100%" Height="28px"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtembossing_var_name" runat="server" ReadOnly="true" CssClass="form-control" Width="100%" Height="28px"></asp:TextBox>
                                                        </div>
                                                        <label id="Label96" runat="server" class="col-sm-3 control-label">Unit_Rate<span style="font-size: x-small;">(Amt/kg)</span></label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtembossing_var_rate" runat="server" ReadOnly="true" CssClass="form-control" Width="100%" Height="28px"></asp:TextBox>
                                                        </div>
                                                        <label id="Label87" runat="server" class="col-sm-2 control-label">%_of_area_for_embosssing<span style="font-size: x-small;">(%)</span></label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtarea_embosing_white" runat="server" BackColor="LightBlue" onkeyup="cal()" onkeypress="return isDecimalKey(event)" CssClass="form-control" Width="100%" MaxLength="10" Height="28px"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label88" runat="server" class="col-sm-3 control-label">Unit_Consumption<span style="font-size: x-small;">(sqm)</span></label>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtunit_conum_white_var" runat="server" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" onkeyup="cal()" CssClass="form-control" Width="100%" MaxLength="10" Height="28px"></asp:TextBox>
                                                        </div>
                                                        <label id="Label89" runat="server" class="col-sm-3 control-label">Tot_Consum_of_embossing_Varnish<span style="font-size: x-small;">(gm)</span></label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtemb_Var_conum_white" runat="server" ReadOnly="true" CssClass="form-control" Width="100%" MaxLength="10" Height="28px"></asp:TextBox>
                                                        </div>
                                                        <label id="Label90" runat="server" class="col-sm-2 control-label">Screen_Exposing_Charge<span style="font-size: x-small;">(Amt)</span></label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtscreen_exposing" runat="server" BackColor="LightBlue" CssClass="form-control" onkeypress="return isDecimalKey(event)" onkeyup="cal()" Width="100%" MaxLength="10" Height="28px"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label86" runat="server" class="col-sm-3 control-label">Total_Rate_For_Embossing_White<span style="font-size: x-small;">(Amt)</span></label>
                                                        <div class="col-sm-9">
                                                            <asp:TextBox ID="txt_totrate_emb_white" runat="server" ReadOnly="true" onkeyup="cal()" CssClass="form-control" Width="130px" MaxLength="5" Height="28px"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane active" id="DescTab7">
                                    <div class="lbBody" style="height: 170px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <div class="col-md-12">
                                            <div>
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <label id="Label57" runat="server" class="col-sm-1 control-label">Machine_A</label>
                                                        <div class="col-sm-1">
                                                            <asp:ImageButton ID="btnmch1" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnmch1_Click" />
                                                        </div>
                                                        <div class="col-sm-1" style="display: none;">
                                                            <asp:TextBox ID="txtmch1_code" runat="server" ReadOnly="true" CssClass="form-control" Width="100%" MaxLength="50" Height="28px"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtmchname1" runat="server" ReadOnly="true" CssClass="form-control" Width="100%" MaxLength="50" Height="28px"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtmch1_cost" runat="server" ReadOnly="true" CssClass="form-control" Width="100%" MaxLength="50" Height="28px"></asp:TextBox>
                                                        </div>

                                                        <label id="Label58" runat="server" class="col-sm-1 control-label">Machine_B</label>
                                                        <div class="col-sm-1">
                                                            <asp:ImageButton ID="btnmch2" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnmch2_Click" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <div class="col-sm-1" style="display: none;">
                                                            <asp:TextBox ID="txtmch2_code" runat="server" ReadOnly="true" CssClass="form-control" Width="100%" MaxLength="50" Height="28px"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtmchname2" runat="server" ReadOnly="true" CssClass="form-control" Width="100%" MaxLength="50" Height="28px"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtmch2_cost" runat="server" ReadOnly="true" CssClass="form-control" Width="100%" MaxLength="50" Height="28px"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label101" runat="server" class="col-sm-2 control-label">Time_for_the_Job</label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtJobTime1" runat="server" ReadOnly="true" CssClass="form-control" Width="100%" MaxLength="50" Height="28px"></asp:TextBox>
                                                        </div>
                                                        <label id="Label102" runat="server" class="col-sm-2 control-label">Tot_Electricity_Charge_for_the_Job</label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtElectricityChg1" runat="server" ReadOnly="true" CssClass="form-control" Width="100%" MaxLength="50" Height="28px"></asp:TextBox>
                                                        </div>

                                                        <label id="Label103" runat="server" class="col-sm-2 control-label">Time_for_the_Job</label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtJobTime2" runat="server" ReadOnly="true" CssClass="form-control" Width="100%" MaxLength="50" Height="28px"></asp:TextBox>
                                                        </div>
                                                        <label id="Label104" runat="server" class="col-sm-2 control-label">Tot_Electicity_Charge_for_the_Job</label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtElectricityChg2" runat="server" ReadOnly="true" CssClass="form-control" Width="100%" MaxLength="50" Height="28px"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <%-- <div class="form-group">                                
                                                            <label id="Label59" runat="server" class="col-sm-3 control-label">Tot_Rate_for_Embossing_Varnish<span style="font-size:x-small;">(Amt)</span></label>
                                                           <div class="col-sm-1">
                                                              <asp:TextBox ID="txttot_rt_For_emb_varnish" runat="server" BackColor="LightBlue" CssClass="form-control" Width="100%" MaxLength="10" onkeypress="return isDecimalKey(event)" Height="28px"></asp:TextBox>
                                                        </div>
                                                          <label id="Label93" runat="server" class="col-sm-2 control-label">Screen_Printing<span style="font-size:x-small;">(Amt)</span></label>
                                                           <div class="col-sm-2">
                                                              <asp:TextBox ID="txt_screen_print" runat="server" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" CssClass="form-control" Width="100%" MaxLength="10" Height="28px"></asp:TextBox>
                                                        </div>
                                                           <label id="Label94" runat="server" class="col-sm-2 control-label">Delivery_&_Dispatch<span style="font-size:x-small;">(Amt)</span></label>
                                                           <div class="col-sm-2">
                                                              <asp:TextBox ID="txt_del_desp" runat="server" BackColor="LightBlue" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" MaxLength="10" Height="28px"></asp:TextBox>
                                                        </div>
                                                             </div>--%>
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

                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>

    <asp:Button ID="btnhideF" runat="server" OnClick="btnhideF_Click" Style="display: none" />
    <asp:Button ID="btnhideF_s" runat="server" OnClick="btnhideF_s_Click" Style="display: none" />
    <asp:HiddenField ID="hffield" runat="server" />
    <asp:HiddenField ID="hfFormID" runat="server" />
    <asp:HiddenField ID="doc_nf" runat="server" />
    <asp:HiddenField ID="doc_df" runat="server" />
    <asp:HiddenField ID="doc_vty" runat="server" />
    <asp:HiddenField ID="doc_addl" runat="server" />
    <asp:HiddenField ID="edmode" runat="server" />
    <asp:HiddenField ID="hf1" runat="server" />
    <asp:HiddenField ID="hf2" runat="server" />
    <asp:HiddenField ID="hf3" runat="server" />
    <asp:HiddenField ID="hf4" runat="server" />
    <asp:HiddenField ID="hf5" runat="server" />


    <script type="text/javascript">
        $(function () {
            var tabName = $("[id*=TabName]").val() != "" ? $("[id*=TabName]").val() : "DescTab";
            $('#Tabs a[href="#' + tabName + '"]').tab('show');
            $("#Tabs a").click(function () {
                $("[id*=TabName]").val($(this).attr("href").replace("#", ""));
            });
        });

        function cal() {
            var t1 = 0; var t2 = 0; var t3 = 0; var t4 = 0; var t5 = 0; var t6 = 0; var t7 = 0; var papaersize = 0; var ups = 0; var lbl_hyt = 0; var req_width = 0;
            var paper = 0; var width = 0; var height = 0; var acros = 0; var wstg = 0; var gap = 0; var around = 0; var gap_acros = 0; var gap_arnd = 0; var setting_wstg_colr = 0;
            var act_width = 0; var diff = 0; var lbl_width = 0; var qty = 0; var rung_mtr_mm = 0; var rung_mtr_mtr = 0; var tot_wstg = 0; var gd_tot = 0; var color = 0;
            var tot_rmtr_used = 0; var tot_sqm = 0; var unit_prc_of_matl = 0; var tot_unit_cost = 0; var usage_ink_sqm_color = 0; var tot_ink_usage = 0; var die_area = 0; var die_unit_rate = 0;
            var plate_unit_cost = 0; var tot_plate_cost = 0; var varnish = 0; var tot_varnish_cost = 0; var tot_varnish_cost1 = 0; var die_width = 0; var die_hyt = 0; var die_cost = 0;
            var no_of_die_reqd = 0; var emb_var = 0; var unit_consum_var = 0; var tot_emb_var = 0; var screen_emb_chg = 0; var tot_rt_emb_var = 0; var emb_var1 = 0; var emb_Var_conum_white = 0;
            var area_embosing_white = 0; var unit_conum_white_var = 0; var prod_cost = 0; var mch1_cost = 0; var tot_rt_For_emb_varnish = 0; var screen_print = 0;
            var margin_cost_AED = 0; var margin_considered = 0; var total = 0; var vat_percent = 0; var val_Value = 0; var v1 = 0; var v2 = 0; var v3 = 0; var del_desp = 0;
            //=============== required width======sppi=======formula is====(lbl height * across)+20+(gap across*(acros-1)
            lbl_hyt = fill_zero(document.getElementById("ContentPlaceHolder1_txtlbl_hyt").value);
            acros = fill_zero(document.getElementById("ContentPlaceHolder1_txt_acros").value);
            gap_acros = fill_zero(document.getElementById("ContentPlaceHolder1_txt_gap_acros").value);
            req_width = ((lbl_hyt * 1) * (acros * 1)) + 20 + ((gap_acros * 1) * ((acros * 1) - 1));
            document.getElementById('ContentPlaceHolder1_txtreqwidth').value = (req_width * 1).toFixed(3);
            ////=======for ups====sppi=====formula is======txtups = txt_acros X txtaround
            around = fill_zero(document.getElementById("ContentPlaceHolder1_txtaround").value);
            ups = (acros * 1) * (around * 1);
            document.getElementById('ContentPlaceHolder1_txtups').value = (ups * 1).toFixed(3);
            ////=====  for Diff for SPPI=======txtdiff = txttrmwastg - txtreqwidth
            wstg = fill_zero(document.getElementById("ContentPlaceHolder1_txttrmwastg").value);
            diff = ((wstg * 1) - (req_width * 1));
            document.getElementById('ContentPlaceHolder1_txtdiff').value = (diff * 1).toFixed(3);
            //===========formula for Running Mtr (MM)=========== txtrungmtr_mm  = (txtlbl_width +3) x (txtqty / txt_acros)
            lbl_width = fill_zero(document.getElementById("ContentPlaceHolder1_txtlbl_width").value);

            //** cylinder
            if (document.getElementById('ContentPlaceHolder1_hfFormID').value == "F10197") {
                //document.getElementById("ContentPlaceHolder1_txtCylinder").value = (document.getElementById("ContentPlaceHolder1_txtTeeth").value * 3.175).toFixed(2);
                //document.getElementById("ContentPlaceHolder1_txtgaparound").value = (document.getElementById("ContentPlaceHolder1_txtCylinder").value / lbl_width).toFixed(2);
            }

            qty = fill_zero(document.getElementById("ContentPlaceHolder1_txtqty").value);
            rung_mtr_mm = ((lbl_width * 1) + 3) * ((qty * 1) / (acros * 1));
            document.getElementById('ContentPlaceHolder1_txtrungmtr_mm').value = (rung_mtr_mm * 1).toFixed(3);
            ///==========Formula for Running Mtr mtr=======txtrungmtr_mtr = txtrungmtr_mm/1000
            rung_mtr_mtr = (rung_mtr_mm * 1) / 1000;
            document.getElementById('ContentPlaceHolder1_txtrungmtr_mtr').value = (rung_mtr_mtr * 1).toFixed(3);
            //============fromula for total wastage===txt_tot_wstg =  txtcolor x txtsetting_wstg_pclor
            color = fill_zero(document.getElementById("ContentPlaceHolder1_txtcolor").value);
            setting_wstg_colr = fill_zero(document.getElementById("ContentPlaceHolder1_txtsetting_wstg_pclor").value);
            tot_wstg = (color * 1) * (setting_wstg_colr * 1);
            document.getElementById('ContentPlaceHolder1_txt_tot_wstg').value = (tot_wstg * 1).toFixed(3);
            ///===========  TOTAL RUNNING METER USED FORMULA===========  txttot_rung_mtr = txtrungmtr_mtr + txt_tot_wstg
            tot_rmtr_used = (rung_mtr_mtr * 1) + (tot_wstg * 1);
            document.getElementById('ContentPlaceHolder1_txttot_rung_mtr').value = (tot_rmtr_used * 1).toFixed(3);
            //=============tot sq meter=======txt_tot_sqm = txttot_rung_mtr x (txttrmwastg/1000)
            tot_sqm = (tot_rmtr_used * 1) * ((wstg * 1) / 1000);
            document.getElementById('ContentPlaceHolder1_txt_tot_sqm').value = (tot_sqm * 1).toFixed(3);
            //=============UNIT PRICE OF MATERIAL========  txt_unit_price_matl  =   txtmatl1_val  + txtmatl2_val  +  txtmatl3_val + txtmatl4_val
            t1 = fill_zero(document.getElementById("ContentPlaceHolder1_txtmatl1_val").value);
            t2 = fill_zero(document.getElementById("ContentPlaceHolder1_txtmatl2_val").value);
            t3 = fill_zero(document.getElementById("ContentPlaceHolder1_txtmatl3_val").value);
            t4 = fill_zero(document.getElementById("ContentPlaceHolder1_txtmatl4_val").value);
            unit_prc_of_matl = (t1 * 1) + (t2 * 1) + (t3 * 1) + (t4 * 1);
            document.getElementById('ContentPlaceHolder1_txt_unit_price_matl').value = (unit_prc_of_matl * 1).toFixed(3);
            ///////=================total cost for the material=======txtcost_matl =  txt_tot_sqm x txt_unit_price_matl  
            tot_unit_cost = (tot_sqm * 1) * (unit_prc_of_matl * 1);
            document.getElementById('ContentPlaceHolder1_txtcost_matl').value = (tot_unit_cost * 1).toFixed(3);
            ///=================for Ink details ====total ink usage sq meter=========txt_Tot_ink_usage= txtusage_ink   x   txtcolor
            usage_ink_sqm_color = fill_zero(document.getElementById("ContentPlaceHolder1_txtusage_ink").value);
            tot_ink_usage = (usage_ink_sqm_color * 1) * (color * 1);
            document.getElementById('ContentPlaceHolder1_txt_Tot_ink_usage').value = (tot_ink_usage * 1).toFixed(3);
            //=============  total ink cost formula=====txt_tot_ink_cost =  txtinkval  x  txt_Tot_ink_usage x txt_tot_sqm x txtpass
            t5 = fill_zero(document.getElementById("ContentPlaceHolder1_txtinkval").value);
            t6 = fill_zero(document.getElementById("ContentPlaceHolder1_txtpass").value);

            // new changes 01/07/2020
            document.getElementById("ContentPlaceHolder1_txtTotInkCons").value = ((fill_zero(document.getElementById("ContentPlaceHolder1_txt_tot_sqm").value) * fill_zero(document.getElementById("ContentPlaceHolder1_txtcolor").value)) / 1000).toFixed(6);// new changes 01/07/2020

            t7 = (t5 * 1) * (tot_ink_usage * 1) * (tot_sqm * 1) * (t6 * 1);
            // this was old formula, changed on 01/07/20 after discuss with arvind sir
            //document.getElementById('ContentPlaceHolder1_txt_tot_ink_cost').value = (t7 * 1).toFixed(3);
            document.getElementById('ContentPlaceHolder1_txt_tot_ink_cost').value = (document.getElementById("ContentPlaceHolder1_txtTotInkCons").value * t5).toFixed(5);
            t7 = document.getElementById('ContentPlaceHolder1_txt_tot_ink_cost').value;
            ////total plate cost formula======txt_tot_plate_cost = txtplate_unit_cost x txtcolor
            plate_unit_cost = fill_zero(document.getElementById("ContentPlaceHolder1_txtplate_unit_cost").value);
            tot_plate_cost = (plate_unit_cost * 1) * (color * 1);

            // changed in 01/07/2020 - arvind sir                         
            if (document.getElementById('ContentPlaceHolder1_hfFormID').value != "F10199") {
                document.getElementById("ContentPlaceHolder1_txtPlateAreaCM").value = ((fill_zero(document.getElementById("ContentPlaceHolder1_txtCylInch").value * 1) * fill_zero(document.getElementById('ContentPlaceHolder1_txtreqwidth').value * 1) / 100)).toFixed(3);
                tot_plate_cost = (document.getElementById("ContentPlaceHolder1_txtPlateAreaCM").value);
                document.getElementById('ContentPlaceHolder1_txt_tot_plate_cost').value = (tot_plate_cost * plate_unit_cost * color).toFixed(3) // changed in 01/07/2020 - arvind sir
            }
            else
                document.getElementById('ContentPlaceHolder1_txt_tot_plate_cost').value = (tot_plate_cost * 1).toFixed(3);


            //varnish formula=====txt_tot_varnish_cost  = txt_varnish_usage x (txt_varnish_cost / 1000 ) x txt_tot_sqm           
            varnish = fill_zero(document.getElementById("ContentPlaceHolder1_txt_varnish_usage").value);
            tot_varnish_cost = fill_zero(document.getElementById("ContentPlaceHolder1_txt_varnish_cost").value);
            tot_varnish_cost1 = (varnish * 1) * ((tot_varnish_cost * 1) / 1000) * (tot_sqm * 1);
            document.getElementById('ContentPlaceHolder1_txt_tot_varnish_cost').value = (tot_varnish_cost1 * 1).toFixed(3);
            ///die details -------------DIE WIDTH FORMULA---------txtdie_width  =  ((txtlbl_width + txtgaparound ) x txtaround) + txtgaparound 
            gap_arnd = fill_zero(document.getElementById("ContentPlaceHolder1_txtgaparound").value);
            die_width = (((lbl_width * 1) + (gap_arnd * 1)) * (around * 1)) + (gap_arnd * 1);
            document.getElementById('ContentPlaceHolder1_txtdie_width').value = (die_width * 1).toFixed(3);
            ///die details -------------DIE hight FORMULA---------txt_die_hight = (( txtlbl_hyt  + txt_gap_acros) * txt_acros) + txt_gap_acros
            die_hyt = (((lbl_hyt * 1) + (gap_acros * 1)) * (acros * 1)) + (gap_acros * 1);
            document.getElementById('ContentPlaceHolder1_txt_die_hight').value = (die_hyt * 1).toFixed(3);
            //// area of die  =============(die width * diw hyt )/100===========  
            die_area = ((die_width * 1) * (die_hyt * 1)) / 100;
            document.getElementById('ContentPlaceHolder1_txtdie_area').value = (die_area * 1).toFixed(3);
            ///total die cost formula=====txtdiecost  =  area of die x unit rate of die x no of die reqd 
            die_unit_rate = fill_zero(document.getElementById("ContentPlaceHolder1_txtdierate").value);
            no_of_die_reqd = fill_zero(document.getElementById("ContentPlaceHolder1_txtdie_reqd").value);
            die_cost = (die_area * 1) * (die_unit_rate * 1) * (no_of_die_reqd * 1);
            document.getElementById('ContentPlaceHolder1_txtdiecost').value = (die_cost * 1).toFixed(3);
            ///======================EMBOSSING VARNISH====txt_tot_embas_varnish_Val = txt_tot_sqm X (txtemb_area_varnish/100) X txt_consum_varnish         
            emb_var = fill_zero(document.getElementById("ContentPlaceHolder1_txtemb_area_varnish").value);
            unit_consum_var = fill_zero(document.getElementById("ContentPlaceHolder1_txt_consum_varnish").value);
            tot_emb_var = (tot_sqm * 1) * ((emb_var * 1) / 100) * (unit_consum_var * 1);
            document.getElementById('ContentPlaceHolder1_txt_tot_embas_varnish_Val').value = (tot_emb_var * 1).toFixed(3);
            //total rate for embossing varnish==== txt_totrate_emb_varnish = (txt_tot_embas_varnish_Val /1000) x txtembvarnish_val + txt_screen_exposing_chg        
            screen_emb_chg = fill_zero(document.getElementById("ContentPlaceHolder1_txt_screen_exposing_chg").value);
            emb_var1 = fill_zero(document.getElementById("ContentPlaceHolder1_txtembvarnish_val").value);
            tot_rt_emb_var = ((tot_emb_var * 1) / 1000) * (emb_var1 * 1) + (screen_emb_chg * 1);
            document.getElementById('ContentPlaceHolder1_txt_totrate_emb_varnish').value = (tot_rt_emb_var * 1).toFixed(3);
            document.getElementById('ContentPlaceHolder1_txttot_rt_For_emb_varnish').value = (tot_rt_emb_var * 1).toFixed(3);
            //================embossing varnish white/screen printing
            ///==========total consumtion of embossing ====== txtemb_Var_conum_white = txtinkval x (txtarea_embosing_white/100) x txtunit_conum_white_var
            area_embosing_white = fill_zero(document.getElementById("ContentPlaceHolder1_txtarea_embosing_white").value);
            unit_conum_white_var = fill_zero(document.getElementById("ContentPlaceHolder1_txtunit_conum_white_var").value);
            emb_Var_conum_white = (t5 * 1) * ((area_embosing_white * 1) / 100) * (unit_conum_white_var * 1);
            document.getElementById('ContentPlaceHolder1_txtemb_Var_conum_white').value = (emb_Var_conum_white * 1).toFixed(3);
            ///=-----tot rate embosing white-------------txt_totrate_emb_white===txtemb_Var_conum_white x txtembossing_var_rate x txtscreen_exposing
            v1 = fill_zero(document.getElementById("ContentPlaceHolder1_txtembossing_var_rate").value);
            v2 = fill_zero(document.getElementById("ContentPlaceHolder1_txtscreen_exposing").value);
            v3 = (emb_Var_conum_white * 1) * (v1 * 1) + (v2 * 1);
            document.getElementById('ContentPlaceHolder1_txt_totrate_emb_white').value = (v3 * 1).toFixed(3);
            document.getElementById('ContentPlaceHolder1_txt_screen_print').value = (v3 * 1).toFixed(3);
            ///totAL PRODUCTION COST ==== txtprod_cost = txtcost_matl + txt_tot_ink_cost + txt_tot_plate_cost + txt_tot_varnish_cost + txtdiecost + txtmch1_cost + txttot_rt_For_emb_varnish + txt_screen_print
            mch1_cost = fill_zero(document.getElementById("ContentPlaceHolder1_txtmch1_cost").value);
            tot_rt_For_emb_varnish = fill_zero(document.getElementById("ContentPlaceHolder1_txttot_rt_For_emb_varnish").value);
            screen_print = fill_zero(document.getElementById("ContentPlaceHolder1_txt_screen_print").value);
            prod_cost = (tot_unit_cost * 1) + (t7 * 1) + (document.getElementById('ContentPlaceHolder1_txt_tot_plate_cost').value * 1) + (tot_varnish_cost1 * 1) + (die_cost * 1) + (mch1_cost * 1) + (tot_rt_For_emb_varnish * 1) + (screen_print * 1);

            del_desp = fill_zero(document.getElementById("ContentPlaceHolder1_txt_del_desp").value);
            document.getElementById('ContentPlaceHolder1_txtprod_cost').value = ((prod_cost * 1) + (del_desp * 1)).toFixed(3);

            //margin considered (B) =====formula ===== prod_Cost x (margin cost /100)
            margin_considered = fill_zero(document.getElementById("ContentPlaceHolder1_txt_margin_considered").value);
            margin_cost_AED = (prod_cost * 1) * ((margin_considered * 1) / 100);
            document.getElementById('ContentPlaceHolder1_txtmargin_cost_AED').value = (margin_cost_AED * 1).toFixed(3);
            ////formula for TOTal=== txttotal  = prod_cost + margin_cost_AED
            debugger;
            total = (prod_cost * 1) + (margin_cost_AED * 1) + (del_desp * 1);
            document.getElementById('ContentPlaceHolder1_txttotal').value = (total * 1).toFixed(3);
            ///vat value=====  vat vale === total x (vat %/100) 
            vat_percent = fill_zero(document.getElementById("ContentPlaceHolder1_txtvat_percent").value);
            val_Value = (total * 1) * ((vat_percent * 1) / 100);
            document.getElementById('ContentPlaceHolder1_txtval_Value').value = (val_Value * 1).toFixed(3);
            ///formula for grand total  -==== txt_grand_tot = total + val value
            //  gd_tot = (total * 1) + (val_Value * 1);//old            
            gd_tot = (total * 1) + (val_Value * 1); //add del and desp amount....28.04.2020
            document.getElementById('ContentPlaceHolder1_txt_grand_tot').value = (gd_tot * 1).toFixed(3);
        }
        function fill_zero(val) { if (isNaN(val)) return 0; if (isFinite(val)) return val; }

        $(document).ready(function () {
            cal();
        });

    </script>

    <asp:HiddenField ID="TabName" runat="server" />
</asp:Content>
