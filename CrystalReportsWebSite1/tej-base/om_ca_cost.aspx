<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="om_ca_cost" CodeFile="om_ca_cost.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../tej-base/Scripts/gridviewScroll.min.js" type="text/javascript"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>

    <style>
        .Textbox {
            min-width: 500px;
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
                        <button type="submit" id="btnCal" class="btn btn-info" style="width: 100px;" runat="server" accesskey="y" onserverclick="btnCal_ServerClick">C<u>a</u>lculate</button>
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
                        <%--<div class="box-body">--%>
                        <div class="box-body">
                            <div class="form-group">
                                <div class="col-sm-3">
                                    <asp:Label ID="lbl1" runat="server" Text="Voucher_No." Style="text-align: center; font-weight: 700"></asp:Label>
                                </div>
                                <div class="col-sm-1">
                                    <asp:Label ID="lbl1a" runat="server" Text="TC" Style="width: 22px; float: right;" CssClass="col-sm-2 control-label"></asp:Label>
                                </div>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtvchnum" runat="server" Width="100px" ReadOnly="true" Height="30px"></asp:TextBox>
                                </div>

                                <%--   <div class="col-sm-4">
                                    <asp:Label ID="Label47" runat="server" Text="Date" CssClass="col-sm-2 control-label" Style="text-align: center; font-weight: 700"></asp:Label>
                                </div>--%>

                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtvchdate" placeholder="Date" runat="server" Width="290px" Height="30px"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtvchdate_CalendarExtender" runat="server"
                                        Enabled="True" TargetControlID="txtvchdate"
                                        Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="Maskedit1" runat="server" Mask="99/99/9999"
                                        MaskType="Date" TargetControlID="txtvchdate" />
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-3">
                                    <asp:Label ID="Label87" runat="server" Text="RFQ_No"  Style="text-align: center; font-weight: 700"></asp:Label>
                                </div>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnRFQ" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="ImageButton5_Click" />
                                </div>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtRFQ" runat="server" Width="100px" ReadOnly="true" Height="30px"></asp:TextBox>
                                </div>

                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtRFQDt" runat="server" Width="290px" Height="30px" MaxLength="75" ReadOnly="True"></asp:TextBox>
                                </div>
                            </div>


                            <div class="form-group">
                                <div class="col-sm-3">
                                    <asp:Label ID="lbl4" runat="server" Text="Customer_Name"  Style="text-align: center; font-weight: 700"></asp:Label>
                                </div>
                                <div class="col-sm-1">
                                   <asp:ImageButton ID="btnlbl4" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl4_Click" Visible="false" />
                                </div>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtlbl4" runat="server" Width="100px" ReadOnly="true" Height="30px"></asp:TextBox>
                                </div>

                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtlbl4a" runat="server" Width="290px" Height="30px" MaxLength="75" ReadOnly="True"></asp:TextBox>
                                </div>
                            </div>

                           <div class="form-group">
                                <div class="col-sm-3">
                                    <asp:Label ID="Label47" runat="server" Text="Item_Name"  Style="text-align: center; font-weight: 700"></asp:Label>
                                </div>
                                <div class="col-sm-1">
                                </div>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtIcode" runat="server" Width="100px" ReadOnly="true" Height="30px"></asp:TextBox>
                                </div>

                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtIname" runat="server" Width="290px" Height="30px" MaxLength="120" ReadOnly="True"></asp:TextBox>
                                </div>
                            </div>                                

                            <div class="form-group">
                                <div class="col-sm-3">
                                    <asp:Label ID="Label86" runat="server" Text="BoxSize" Style="text-align: center; font-weight: 700"></asp:Label>
                                </div>
                                 <div class="col-sm-1">
                                    <asp:ImageButton ID="ImageButton4" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="ImageButton4_Click" />
                                </div>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtLength" runat="server" Height="30px" Width="100px" ReadOnly="true" ></asp:TextBox>
                                </div>
                                <div class="col-sm-2">
                                   <asp:TextBox ID="txtWidth" runat="server" Height="30px" Width="100px" ReadOnly="true" ></asp:TextBox>
                                </div>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtHeight" runat="server" Height="30px" ReadOnly="true" width="100%"></asp:TextBox>
                                </div>
                            </div>                          
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body" >                            
                              <div class="form-group">
                                <div class="col-sm-2">
                                    <asp:Label ID="Label88" runat="server" Text="Part_No." CssClass="col-sm-2 control-label" Style="text-align: center; font-weight: 700"></asp:Label>
                                </div>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="txtCpart" runat="server" Height="30px" Width="100%" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                             <div class="form-group">
                                <div class="col-sm-2"  runat="server">
                                   <asp:Label ID="Label99" runat="server" Text="SF Code" Style="text-align: center; font-weight: 700"></asp:Label>                                   
                                </div>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtChildCode" runat="server" ReadOnly="true" Height="30px" MaxLength="8"></asp:TextBox>
                                </div>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtChildName" runat="server"  ReadOnly="true" Height="30px" Width="100%"></asp:TextBox>
                                    <asp:TextBox ID="txtParentChild" runat="server" Width="100%" ReadOnly="true" Height="30px" Visible="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-sm-2">
                                    <asp:Label ID="lbl2" runat="server" Text="Ent_By" CssClass="col-sm-2 control-label" Style="text-align: center; font-weight: 700"></asp:Label>
                                </div>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtlbl2" runat="server" Height="30px" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col-sm-2">
                                    <asp:Label ID="lbl3" runat="server" Text="Ent_Dt" CssClass="col-sm-2 control-label" Style="text-align: center; font-weight: 700"></asp:Label>
                                </div>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtlbl3" runat="server" Height="30px" Width="100%" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group" style="display:none">
                                <div class="col-sm-2">
                                    <asp:Label ID="lbl5" runat="server" Text="Edt_By" CssClass="col-sm-2 control-label" Style="text-align: center; font-weight: 700"></asp:Label>
                                </div>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtlbl5" runat="server" Height="30px" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col-sm-2">
                                    <asp:Label ID="lbl6" runat="server" Text="Edt_Dt" CssClass="col-sm-2 control-label" Style="text-align: center; font-weight: 700"></asp:Label>
                                </div>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtlbl6" runat="server" Height="30px" Width="100%" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>

                             
                            <div class="form-group">
                                <div class="col-sm-2">
                                    <asp:Label ID="Label85" runat="server" Text="Material(SG/GI)"  Style="text-align: center; font-weight: 700"></asp:Label>
                                </div>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="txtMaterial" runat="server" Width="100%" Height="30px" MaxLength="50"></asp:TextBox>
                                </div>                               
                            </div>

                            <table style="width:100%">
                                <tr>
                                    <td colspan="4">
                                        <asp:Label ID="lblInfo" runat="server" Text="Save Button Will Be Enable After Clicking on Calculate Button" Style="font-weight: 700; color: #FF0000; font-size: 22px;"></asp:Label>
                                    </td>
                                </tr>
                                <tr style="display: none">
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
                                            <asp:TextBox ID="txtlbl9" runat="server" Height="30px"></asp:TextBox>
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
                            <div class="form-group">
                                 <div class="col-sm-12">
                                    <label id="Label48" runat="server" class="col-md-12 control-label"  style="text-align: center; text-decoration: underline; font-style: italic; font-size: medium;">Yield Data</label>
                            </div> </div>

                            <div class="form-group">
                                     <div class="col-sm-3">
                                        <asp:Label ID="Label3" runat="server" Text="Casting_Wt(KG)" ></asp:Label>
                                    </div>                                
                                     <div class="col-sm-3">
                                        <asp:TextBox ID="txtCast" runat="server" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15" Width="100%" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:Label ID="Label4" runat="server" Text="No_Of_Cast_Mould" ></asp:Label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtCast_No" runat="server" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15" Width="100%" ReadOnly="True"></asp:TextBox>
                                    </div>
                                </div>

                            <div class="form-group">
                                  <div class="col-sm-3">
                                        <asp:Label ID="Label5" runat="server" Text="Bunch_Wt" ></asp:Label>
                                    </div>
                                     <div class="col-sm-3">
                                        <asp:TextBox ID="txtBunch" runat="server" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15" Width="100%" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:Label ID="Label6" runat="server" Text="Actual_Finish_Wt(KG)" ></asp:Label>
                                    </div>                                   
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtActual" runat="server" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15" Width="100%" ReadOnly="True"></asp:TextBox>
                                    </div>                                   
                                </div>

                            <div class="form-group">
                                    <div class="col-sm-3">
                                        <asp:Label ID="Label8" runat="server" Text="Pattern_Yield" ></asp:Label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtPattern" runat="server" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15" Width="100%" ReadOnly="True"></asp:TextBox>
                                    </div>
                                     <div class="col-sm-3">
                                        <asp:Label ID="Label1" runat="server" Text="Rejection%" ></asp:Label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtRej" runat="server" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"  Width="100%" ReadOnly="True"></asp:TextBox>
                                        </div>
                                </div>

                             <div class="form-group">
                                    <div class="col-sm-3">
                                        <asp:Label ID="Label90" runat="server" Text="Net_Effective_Yield%" ></asp:Label>
                                    </div>
                                    <div class="col-sm-9">
                                        <asp:TextBox ID="txtNet" runat="server" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15" Style="width:100%" ReadOnly="True"></asp:TextBox>
                                    </div>                                 
                                </div>
                        </div>
                    </div>
                </div>

               <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <div class="col-sm-12">
                                    <label id="Label27" runat="server" class="col-md-12 control-label"  style="text-align: center; text-decoration: underline; font-style: italic; font-size: medium;">Moulding Cost</label>
                                </div>
                            </div>

                            <div class="form-group">
                                    <div class="col-sm-3">
                                        <asp:Label ID="Label29" runat="server" Text="Mixer/Muller_Capacity" ></asp:Label>
                                    </div>
                                      <div class="col-sm-3">
                                        <asp:TextBox ID="txtMixer" runat="server" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15" Width="100%"></asp:TextBox>
                                    </div>
                                 <div class="col-sm-3">
                                        <asp:Label ID="Label89" runat="server" Text="Chaplet_Cost" ></asp:Label>
                                    </div>
                                      <div class="col-sm-3">
                                        <asp:TextBox ID="txtChaplet" runat="server" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15" Width="100%"></asp:TextBox>
                                    </div>
                                 </div>
                            <div class="form-group">
                                    <div class="col-sm-3">
                                        <asp:Label ID="Label91" runat="server" Text="Mould_Heating" ></asp:Label>
                                    </div>
                                      <div class="col-sm-3">
                                        <asp:TextBox ID="txtMould_Heating" runat="server" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15" Width="100%"></asp:TextBox>
                                    </div>
                                 <div class="col-sm-3">
                                        <asp:Label ID="Label92" runat="server" Text="Others_Process" ></asp:Label>
                                    </div>
                                      <div class="col-sm-3">
                                        <asp:TextBox ID="txtMouldingOther" runat="server" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15" Width="100%"></asp:TextBox>
                                    </div>
                                 </div>
                            <div class="form-group">
                                    <div class="col-sm-3">
                                        <asp:Label ID="Label93" runat="server" Text="Sleeve_Cost" ></asp:Label>
                                    </div>
                                      <div class="col-sm-3">
                                        <asp:TextBox ID="txtSleeve" runat="server" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15" Width="100%"></asp:TextBox>
                                    </div>
                                 <div class="col-sm-3">
                                        <asp:Label ID="Label94" runat="server" Text="Moulding_Sand_Cost" ></asp:Label>
                                    </div>
                                      <div class="col-sm-3">
                                        <asp:TextBox ID="txtSand" runat="server" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15" Width="100%"></asp:TextBox>
                                    </div>
                                 </div>
                                 <div class="form-group">
                                     <div class="col-sm-3">
                                        <asp:Label ID="Label30" runat="server" Text="Moulding_Rate" ></asp:Label>
                                    </div>
                                    <div class="col-sm-9">
                                        <asp:TextBox ID="txtMould_Rt" runat="server"  Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15" Width="100%"></asp:TextBox>
                                    </div>
                                </div>

                        </div></div></div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <div class="col-sm-12">
                                    <label id="Label2" runat="server" class="col-md-12 control-label"  style="text-align: center; text-decoration: underline; font-style: italic; font-size: medium;">Power Cost</label>
                                </div>
                            </div>
                            <div class="form-group">
                                    <div class="col-sm-3">
                                        <asp:Label ID="Label22" runat="server" Text="Electricity_Rate" onkeypress="return isDecimalKey(event)"></asp:Label>
                                    </div>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="ImageButton2_Click" />
                                </div>
                                 <div class="col-sm-3">
                                        <asp:TextBox ID="txtElect" runat="server" Height="30px" MaxLength="15" Width="100%"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:Label ID="Label34" runat="server" Text="Auxulary_Cons" ></asp:Label>
                                    </div>
                                
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtAux" runat="server"  Height="30px" ReadOnly="True" MaxLength="15" Width="100%"></asp:TextBox>
                                    </div>                                    
                            </div>
                            <div class="form-group">
                                   <div class="col-sm-3">
                                        <asp:Label ID="Label35" runat="server" Text="Melting_Consumption" ></asp:Label>
                                    </div>
                                 <div class="col-sm-1">
                                </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtMetling" runat="server"  Height="30px" ReadOnly="True" MaxLength="15" Width="100%"></asp:TextBox>
                                    </div>
                                <div class="col-sm-2">
                                        <asp:Label ID="Label16" runat="server" Text="Power_Cost" ></asp:Label>
                                    </div>                                    
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtPower" runat="server"  Height="30px" ReadOnly="True" MaxLength="15" Width="100%"></asp:TextBox>
                                    </div>
                                </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <div class="col-sm-12">
                                    <label id="Label107" runat="server" class="col-md-12 control-label" style="text-align: center; text-decoration: underline; font-style: italic; font-size: medium;">Core Cost</label>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-2">
                                    <asp:Label ID="Label109" runat="server" Text="Core_Wt(KG)"></asp:Label>
                                </div>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtCore_Wt" runat="server" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15" Width="100%"></asp:TextBox>
                                </div>
                                <div class="col-sm-2">
                                    <asp:Label ID="Label98" runat="server" Text="Core_Cost_Type"></asp:Label>
                                </div>
                                <div class="col-sm-1">
                          <asp:ImageButton ID="ImageButton6" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px;" OnClick="ImageButton6_Click" />
                                </div>
                                <div class="col-sm-5">
                                    <asp:TextBox ID="txtCore_Type" runat="server" Height="30px" ReadOnly="true" MaxLength="20" Width="100%"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-2">
                                    <asp:Label ID="Label111" runat="server" Text="Core_Rate/KG(Rs)"></asp:Label>
                                </div>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtCore_Rt" runat="server" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15" Width="100%"></asp:TextBox>
                                </div>
                                <div class="col-sm-2">
                                    <asp:Label ID="Label112" runat="server" Text="Rejection%"></asp:Label>
                                </div>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtCore_Rej" runat="server" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15" Width="100%"></asp:TextBox>
                                </div>
                                <div class="col-sm-2">
                                    <asp:Label ID="Label12" runat="server" Text="Core_Cost(Rs/Kg)"></asp:Label>
                                </div>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtCore_Cost" runat="server" Height="30px" onkeypress="return isDecimalKey(event)" ReadOnly="True" MaxLength="15" Width="100%"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                  <div class="col-md-12">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <div class="col-sm-12">
                                    <label id="Label11" runat="server" class="col-md-12 control-label"  style="text-align: center; font-style: italic; text-decoration: underline; font-size: medium;">Conversion Cost</label>
                                </div>
                            </div>

                            <div class="form-group">
                                    <div class="col-sm-2">
                                        <asp:Label ID="Label18" runat="server" Text="Labour_Cost(Rs/Kg)" ></asp:Label>                                   
                                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px;" OnClick="ImageButton1_Click" />
                                </div>
                                <div class="col-sm-2">
                                        <asp:TextBox ID="txtLabour" runat="server"  Height="30px" onkeypress="return isDecimalKey(event)" ReadOnly="True" MaxLength="15" Width="100%"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:Label ID="Label14" runat="server" Text="Maint_Cost(Rs/Kg)" ></asp:Label>
                                    </div>
                                 <div class="col-sm-2">
                                        <asp:TextBox ID="txtMaint" runat="server" onkeypress="return isDecimalKey(event)" Height="30px" MaxLength="15" ReadOnly="True" Width="100%"></asp:TextBox>
                                    </div>                                    
                                                                                                  
                                   <div class="col-sm-2">
                                        <asp:Label ID="Label15" runat="server" Text="Fettling_ShotBlas_Cost(Rs/Kg)" ></asp:Label>
                                    </div>

                                  <div class="col-sm-2">
                                        <asp:TextBox ID="txtFettling" runat="server" onkeypress="return isDecimalKey(event)" Height="30px" MaxLength="15" ReadOnly="True" Width="100%"></asp:TextBox>
                                    </div>
                                                                                                   
                                </div>

                            <div class="form-group">
                                 <div class="col-sm-2">
                                        <asp:Label ID="Label33" runat="server" Text="Painting_Cost" ></asp:Label>
                                    </div>
                                      <div class="col-sm-2">
                                        <asp:TextBox ID="txtPainting"  runat="server" Height="30px" Width="100%" MaxLength="15" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                    </div> 
                                 <div class="col-sm-2">
                                        <asp:Label ID="Label95" runat="server" Text="Int_Cost(Rs/Kg)" ></asp:Label>
                                    </div>

                                      <div class="col-sm-2">
                                        <asp:TextBox ID="txtInterest"  runat="server" Height="30px" ReadOnly="True" Width="100%" MaxLength="15"></asp:TextBox>
                                    </div>
                                                                 
                                <div class="col-sm-2">
                                        <asp:Label ID="Label17" runat="server" Text="Depriciation_Cost(Rs/Kg)" ></asp:Label>
                                    </div>
                                
                                 <div class="col-sm-2">
                                        <asp:TextBox ID="txtDepr" runat="server"  Height="30px" ReadOnly="True" MaxLength="15" Width="100%"></asp:TextBox>
                                    </div>
                            </div>

                            <div class="form-group">
                                 <div class="col-sm-2">
                                     <asp:Label ID="Label7" runat="server" Text="Other_Cost(Rs/Kg)" ></asp:Label>
                                     </div>

                                <div class="col-sm-2">
                                        <asp:TextBox ID="txtOther" runat="server" Height="30px" ReadOnly="True" Width="100%" MaxLength="15"></asp:TextBox>
                                    </div>
                                 <div class="col-sm-2">
                                        <asp:Label ID="Label96" runat="server" Text="Other_Cost" ></asp:Label>
                                    </div>
                                 <div class="col-sm-2">
                                        <asp:TextBox ID="txtConver_Other1" runat="server" Height="30px" Width="100%" MaxLength="15"></asp:TextBox>
                                    </div>

                                 <div class="col-sm-2">
                                     <asp:Label ID="Label97" runat="server" Text="Other_Cost" ></asp:Label>
                                     </div>

                                <div class="col-sm-2">
                                        <asp:TextBox ID="txtConver_Other2" runat="server" Height="30px" Width="100%" MaxLength="15"></asp:TextBox>
                                    </div>
                                 </div>

                            <div class="form-group">
                               <div class="col-sm-2">
                                        <asp:Label ID="Label9" runat="server" Text="SubTotal" ></asp:Label>
                                    </div>

                                 <div class="col-sm-2">
                                        <asp:TextBox ID="txtSubTotal" runat="server"  Height="30px" ReadOnly="True" MaxLength="15" Width="100%"></asp:TextBox>
                                    </div>                
                              
                                    <div class="col-sm-2">
                                        <asp:Label ID="Label24" runat="server" Text="GrandTotal" ></asp:Label>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtGrandTot" runat="server" ReadOnly="True"  Height="30px" Width="100%" MaxLength="15"></asp:TextBox>
                                    </div>
                              
                                    <div class="col-sm-2">
                                        <asp:Label ID="Label10" runat="server" Text="Casting_Rate" onkeypress="return isDecimalKey(event)"></asp:Label>
                                    </div>

                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtCast_Rt" runat="server" ReadOnly="True" placeholder="Casting Rate W/O OH & Profit (Conversion + Metallics)" width="100%" Height="30px" MaxLength="15"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                <div class="col-md-12">
                    <div>
                        <div class="box-body">
                             <div class="form-group">
                                <div class="col-sm-12">
                                    <label id="Label42" runat="server" class="col-md-12 control-label"  style="text-align: center; text-decoration: underline; font-style: italic; font-size: medium;">Metallics & Ferro Additives</label>
                                </div>
                            </div>

                             <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-2">
                                        <asp:Label ID="Label69" runat="server" Text="Metallics" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:Label ID="Label43" runat="server" Text="Consumption%" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:Label ID="Label60" runat="server" Text="Contribution%" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:Label ID="Label44" runat="server" Text="Rate/KG" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:Label ID="Label13" runat="server" Text="Weight-KG" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:Label ID="Label19" runat="server" Text="Si%" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:Label ID="Label20" runat="server" Text="Mn%" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:Label ID="Label21" runat="server" Text="C%" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:Label ID="Label23" runat="server" Text="Moly%" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                </div>
                            </div>
 
                             <div class="form-group">
                                   <div class="col-sm-2">
                                        <asp:Label ID="Label25" runat="server" Text="Foundry_Returns" ></asp:Label>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtFCons" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtFContri" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtFRate" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtFWt" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" ReadOnly="True" MaxLength="15"></asp:TextBox>
                                    </div>
                                    
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtFSi" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" ReadOnly="True" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtFMn" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" ReadOnly="True" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtFC" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" ReadOnly="True" MaxLength="15"></asp:TextBox>
                                    </div>
                                <div class="col-sm-1">
                                        <asp:TextBox ID="txtFMoly" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" ReadOnly="True" MaxLength="15"></asp:TextBox>
                                    </div>
                                </div>

                             <div class="form-group">
                                   <div class="col-sm-2">
                                        <asp:Label ID="Label26" runat="server" Text="Pig_Iron_CI" ></asp:Label>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtPCons" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtPContri" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" ReadOnly="True" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtPRate" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtPWt" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" ReadOnly="True" MaxLength="15"></asp:TextBox>
                                    </div>
                                    
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtPSi" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtPMn" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtPC" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                <div class="col-sm-1">
                                        <asp:TextBox ID="txtPMoly" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                </div>
                             <div class="form-group">
                                   <div class="col-sm-2">
                                        <asp:Label ID="Label28" runat="server" Text="Steel_Scrap_CRCA" ></asp:Label>
                                       <asp:ImageButton ID="Img_Steel" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px;" OnClick="Img_Steel_Click" />
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtSCons" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtSContri" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" ReadOnly="True" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtSRate" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtSWt" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" ReadOnly="True" MaxLength="15"></asp:TextBox>
                                    </div>
                                    
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtSSi" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtSMn" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtSC" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                <div class="col-sm-1">
                                        <asp:TextBox ID="txtSMoly" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                </div>
                             <div class="form-group">
                                   <div class="col-sm-2">
                                        <asp:Label ID="Label31" runat="server" Text="Cast_Iron_Scrap" ></asp:Label>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtCCons" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtCContri" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" ReadOnly="True" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtCRate" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtCWt" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" ReadOnly="True" MaxLength="15"></asp:TextBox>
                                    </div>
                                    
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtCSi" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtCMn" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtCC" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                <div class="col-sm-1">
                                        <asp:TextBox ID="txtCMoly" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                </div>
                             <div class="form-group">
                                   <div class="col-sm-2">
                                        <asp:Label ID="Label32" runat="server" Text="Sub_Total" ></asp:Label>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtSubCons" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" ReadOnly="True" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtSubContri" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" ReadOnly="True" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1" style="visibility:hidden;">
                                        <asp:TextBox ID="TextBox30" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtSubWt" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                    
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtSubSi" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtSubMn" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtSubC" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                <div class="col-sm-1">
                                        <asp:TextBox ID="txtSubMoly" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                </div>
                             <div class="form-group">
                                   <div class="col-sm-2" style="visibility:hidden;">
                                        <asp:TextBox ID="TextBox39" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2" style="visibility:hidden;">
                                        <asp:TextBox ID="TextBox36" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2" style="visibility:hidden;">
                                        <asp:TextBox ID="TextBox37" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1" style="visibility:hidden;">
                                        <asp:TextBox ID="TextBox38" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:Label ID="Label36" runat="server" Text="Required%" ></asp:Label>
                                        
                                    </div>
                                    
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtReqSi" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtReqMn" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtReqC" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                <div class="col-sm-1">
                                        <asp:TextBox ID="txtReqMoly" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                </div>
                             <div class="form-group">
                                   <div class="col-sm-2" >
                                        <asp:Label ID="Label38" runat="server" Text="Metallic Rate/Ton" ></asp:Label>
                                    </div>
                                    <div class="col-sm-2" style="visibility:hidden;">
                                        <asp:TextBox ID="TextBox45" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2"">
                                        <asp:TextBox ID="txtMetContri" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" ReadOnly="True" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1" style="visibility:hidden;">
                                        <asp:TextBox ID="TextBox47" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:Label ID="Label37" runat="server" Text="Difference%" ></asp:Label>
                                        
                                    </div>
                                    
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtDiffSi" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" ReadOnly="True" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtDiffMn" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" ReadOnly="True" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtDiffC" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" ReadOnly="True" MaxLength="15"></asp:TextBox>
                                    </div>
                                <div class="col-sm-1">
                                        <asp:TextBox ID="txtDiffMoly" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" ReadOnly="True" MaxLength="15"></asp:TextBox>
                                    </div>
                                </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-12">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                    <div class="col-sm-2">
                                        <asp:Label ID="Label39" runat="server" Text="Ferro Additives" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:Label ID="Label40" runat="server" Text="Recovery%" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:Label ID="Label41" runat="server" Text="Required_KG" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:Label ID="Label45" runat="server" Text="Rate/KG" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:Label ID="Label55" runat="server" Text="Cost" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                </div>

                             <div class="form-group">
                                   <div class="col-sm-2" >
                                        <asp:Label ID="Label56" runat="server" Text="Fe-Si"></asp:Label>
                                    <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="ImageButton3_Click" />
                                </div>
                                    <div class="col-sm-2" >
                                        <asp:TextBox ID="txtFeSiRec" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15" ></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2"">
                                        <asp:TextBox ID="txtFeSiReq" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2" >
                                        <asp:TextBox ID="txtFeSiRate" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtFeSiCost" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>                                        
                                    </div>                             
                                </div>

                             <div class="form-group">
                                   <div class="col-sm-2" >
                                        <asp:Label ID="Label46" runat="server" Text="Fe-Mn"></asp:Label>
                                </div>
                                    <div class="col-sm-2" >
                                        <asp:TextBox ID="txtFeMnRec" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtFeMnReq" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2" >
                                        <asp:TextBox ID="txtFeMnRate" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtFeMnCost" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>                                        
                                    </div>                             
                                </div>

                              <div class="form-group">
                                   <div class="col-sm-2" >
                                        <asp:Label ID="Label54" runat="server" Text="Carburiser_CSC"></asp:Label>
                                       <asp:ImageButton ID="Img_Carburiser" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="Img_Carburiser_Click" />
                                </div>
                                    <div class="col-sm-2" >
                                        <asp:TextBox ID="txtCSCRec" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2"">
                                        <asp:TextBox ID="txtCSCReq" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2" >
                                        <asp:TextBox ID="txtCSCRate" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtCSCCost" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>                                        
                                    </div>                             
                                </div>

                             <div class="form-group">
                                   <div class="col-sm-2" >
                                        <asp:Label ID="Label57" runat="server" Text="Moly"></asp:Label>
                                </div>
                                    <div class="col-sm-2" >
                                        <asp:TextBox ID="txtMolyRec" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2"">
                                        <asp:TextBox ID="txtMolyReq" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2" >
                                        <asp:TextBox ID="txtMolyRate" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtMolyCost" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>                                        
                                    </div>                             
                                </div>

                             <div class="form-group">
                                   <div class="col-sm-2" >
                                        <asp:Label ID="Label58" runat="server" Text="FeSiMg"></asp:Label>
                                </div>
                                    <div class="col-sm-2" >
                                        <asp:TextBox ID="txtFeSiMGRec" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2"">
                                        <asp:TextBox ID="txtFeSiMGReq" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2" >
                                        <asp:TextBox ID="txtFeSiMGRate" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtFeSiMGCost" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>                                        
                                    </div>                             
                                </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-12">
                    <div>
                        <div class="box-body">
                             <div class="tab-content" >
                                <div role="tabpanel" class="tab-pane active" id="DescTab">
                                    <div class="lbBody" id="gridDiv" style="color: White; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                        <fin:CoolGridView ID="sg1" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="100%" Height="200px" Font-Size="13px"
                                            AutoGenerateColumns="False" OnRowDataBound="sg1_RowDataBound"
                                            OnRowCommand="sg1_RowCommand">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Add</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btnadd" runat="server" CommandName="SG1_ROW_ADD" ImageAlign="Middle" ImageUrl="../tej-base/images/Btn_addn.png" Width="20px" ToolTip="Only 5 Items Will Be Filled In The Grid" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Del</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btnrmv" runat="server" CommandName="SG1_RMV" ImageUrl="../tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                 <asp:BoundField DataField="sg1_srno" HeaderText="SrNo"/>
                                                <asp:BoundField DataField="sg1_f1" HeaderText="Item Code"/>
                                                <asp:BoundField DataField="sg1_f2" HeaderText="Item Name" Visible="false"/>
                                                <asp:TemplateField>
                                                  <HeaderTemplate>Ferro Additives</HeaderTemplate>
                                                 <ItemTemplate>
                                                  <asp:TextBox ID="sg1_t1" runat="server"  Text='<%#Eval("sg1_t1") %>' ReadOnly="true" MaxLength="15" Width="100%"></asp:TextBox>                                                           
                                                </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Recovery%</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t2" runat="server" Text='<%#Eval("sg1_t2") %>' Width="100%" MaxLength="15" onkeypress="return isDecimalKey(event)"></asp:TextBox>                                                   
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                               
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Required_KG</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t3" runat="server" Text='<%#Eval("sg1_t3") %>'  Width="100%" MaxLength="15" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Rate/KG</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t4" runat="server" Text='<%#Eval("sg1_t4") %>'  Width="100%" MaxLength="15" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField >
                                                    <HeaderTemplate>Cost</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t5" runat="server" Text='<%#Eval("sg1_t5") %>'  Width="100%" MaxLength="15" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                
                                                 <asp:TemplateField >
                                                    <HeaderTemplate>Additives1</HeaderTemplate><%--Pig Iron %--%>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t6" runat="server" Text='<%#Eval("sg1_t6") %>'  Width="100%" MaxLength="15" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                
                                                 <asp:TemplateField >
                                                    <HeaderTemplate>Additives2</HeaderTemplate><%--Return %--%>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t7" runat="server" Text='<%#Eval("sg1_t7") %>'  Width="100%" MaxLength="15" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                
                                                 <asp:TemplateField >
                                                    <HeaderTemplate>Req %</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t8" runat="server" Text='<%#Eval("sg1_t8") %>'  Width="100%" MaxLength="15" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                
                                                 <asp:TemplateField >
                                                    <HeaderTemplate>Diff %</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t9" runat="server" Text='<%#Eval("sg1_t9") %>'  Width="100%" MaxLength="15" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                
                                                 <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>Cost</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t10" runat="server" Text='<%#Eval("sg1_t10") %>'  Width="100%" MaxLength="15" onkeypress="return isDecimalKey(event)"></asp:TextBox>
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
                                </div></div></div></div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                    <div class="form-group">
                                   <div class="col-sm-6" >
                                        <asp:Label ID="Label49" runat="server" Text="Total_Ferro_Alloy_Addition_Cost(Rs/Ton)"></asp:Label>
                                    </div>
                                    <div class="col-sm-2"">
                                       
                                    </div>
                                    <div class="col-sm-2" >
                                        
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtFerroTot" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" ReadOnly="True" MaxLength="15"></asp:TextBox>                                        
                                    </div>                             
                                </div>

                              <div class="form-group">
                                   <div class="col-sm-6" >
                                        <asp:Label ID="Label50" runat="server" Text="Total_Metallics_Cost(Rs/Ton)"></asp:Label>
                                    </div>
                                    <div class="col-sm-2"">
                                       
                                    </div>
                                    <div class="col-sm-2" >
                                        
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtMetTot" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" ReadOnly="True" MaxLength="15"></asp:TextBox>                                        
                                    </div>                             
                                </div>

                             <div class="form-group">
                                   <div class="col-sm-6" >
                                        <asp:Label ID="Label51" runat="server" Text="Stage_Weight(KG)"></asp:Label>
                                    </div>
                                    <div class="col-sm-2"">
                                       
                                    </div>
                                    <div class="col-sm-2" >
                                        
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtStgWt" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" ReadOnly="True" MaxLength="15"></asp:TextBox>                                        
                                    </div>                             
                                </div>

                             <div class="form-group">
                                   <div class="col-sm-3" >
                                        <asp:Label ID="Label52" runat="server" Text="Melting_Losses(%)@"></asp:Label>
                                    </div>
                                    <div class="col-sm-3" >
                                        <asp:TextBox ID="txtMelting_Loss" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:Label ID="Label53" runat="server" Text="Stage_Wt_After_Melting_Losses(KG)"></asp:Label>
                                    </div>                                   
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtMelting_Loss_Wt" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" ReadOnly="True" MaxLength="15"></asp:TextBox>                                        
                                    </div>                             
                                </div>

                            <div class="form-group">
                                   <div class="col-sm-3" >
                                        <asp:Label ID="Label59" runat="server" Text="Master_Alloy%@"></asp:Label>
                                    </div>
                                    <div class="col-sm-3" >
                                       <asp:TextBox ID="txtMasterAlloy1" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2"">
                                       <asp:Label ID="Label61" runat="server" Text="Rate/KG"></asp:Label>
                                    </div>
                                    <div class="col-sm-2" >
                                       <asp:TextBox ID="txtMasterAlloy2" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15" ></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtMasterAlloy" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" ReadOnly="True" MaxLength="15"></asp:TextBox>
                                    </div>                             
                                </div>

                             <div class="form-group">
                                   <div class="col-sm-6" >
                                        <asp:Label ID="Label62" runat="server" Text="Stage_Weight_With_Master_Alloy(KG)"></asp:Label>
                                    </div>
                                    <div class="col-sm-2"">
                                       
                                    </div>
                                    <div class="col-sm-2" >
                                        
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtMasterAlloy_Wt" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" ReadOnly="True" MaxLength="15"></asp:TextBox>                                        
                                    </div>                             
                                </div>

                            <div class="form-group">
                                   <div class="col-sm-3" >
                                        <asp:Label ID="Label63" runat="server" Text="Innoculation%@"></asp:Label>
                                    </div>
                                    <div class="col-sm-3" >
                                        <asp:TextBox ID="txtInnoculation1" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2"">
                                       <asp:Label ID="Label64" runat="server" Text="Rate/KG"></asp:Label>
                                    </div>
                                    <div class="col-sm-2" >
                                        <asp:TextBox ID="txtInnoculation2" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtInnoculation3" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" ReadOnly="True" MaxLength="15"></asp:TextBox>                                        
                                    </div>                             
                                </div>

                             <div class="form-group">
                                   <div class="col-sm-6" >
                                        <asp:Label ID="Label65" runat="server" Text="Stage_Weight_With_Innoculation(KG)"></asp:Label>
                                    </div>
                                    <div class="col-sm-2"">
                                       
                                    </div>
                                    <div class="col-sm-2" >
                                        
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtInnoculationWt" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" ReadOnly="True" MaxLength="15"></asp:TextBox>                                        
                                    </div>                             
                                </div>

                             <div class="form-group">
                                   <div class="col-sm-6" >
                                        <asp:Label ID="Label66" runat="server" Text="Net_Yield_After_Return(KG)"></asp:Label>
                                    </div>
                                    <div class="col-sm-2"">
                                       
                                    </div>
                                    <div class="col-sm-2" >
                                        
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtNetYield" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" ReadOnly="True" MaxLength="15"></asp:TextBox>                                        
                                    </div>                             
                                </div>

                             <div class="form-group">
                                   <div class="col-sm-6" >
                                        <asp:Label ID="Label67" runat="server" Text="Total_Metallic_Rate(KG)"></asp:Label>
                                    </div>
                                    <div class="col-sm-2"">
                                       
                                    </div>
                                    <div class="col-sm-2" >
                                        
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtTotMetRate" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" ReadOnly="True" MaxLength="15"></asp:TextBox>                                        
                                    </div>                             
                                </div>
                </div></div></div>

                 <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <div class="col-sm-12">
                                    <label id="Label68" runat="server" class="col-md-12 control-label"  style="text-align: center; text-decoration: underline; font-style: italic; font-size: medium;">Casting Rate With OH & Profit</label>
                                </div>
                                 <div class="form-group">
                                   <div class="col-sm-3" >
                                        <asp:Label ID="Label70" runat="server" Text="Profit%"></asp:Label>
                                    </div>
                                    <div class="col-sm-3">
                                       <asp:Label ID="Label73" runat="server" Text="@"></asp:Label>
                                    </div>
                                    <div class="col-sm-2" >
                                        
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtProfit1" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>                                        
                                    </div> 
                                      <div class="col-sm-2">
                                        <asp:TextBox ID="txtProfit2" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>                                        
                                    </div>                             
                                </div>

                                 <div class="form-group">
                                   <div class="col-sm-3" >
                                        <asp:Label ID="Label71" runat="server" Text="Over_Head%"></asp:Label>
                                    </div>
                                    <div class="col-sm-3">
                                       <asp:Label ID="Label72" runat="server" Text="@"></asp:Label>
                                    </div>
                                    <div class="col-sm-2" >
                                        
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtOver1" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>                                        
                                    </div>
                                     <div class="col-sm-2">
                                        <asp:TextBox ID="txtOver2" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>                                        
                                    </div>                      
                                </div>

                                <div class="form-group">
                                <div class="col-sm-10">
                                    <label id="Label74" runat="server" class="col-md-12 control-label"  style="text-align: center; text-decoration: underline; font-style: italic; font-size: medium;">Total Casting Rate (Rs/Kg) (With OH and Profit)</label>
                                </div>
                                     <div class="col-sm-2">
                                        <asp:TextBox ID="txtCastTot_OH" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>                                        
                                    </div> 
                                </div>

                                 <div class="form-group">
                                   <div class="col-sm-3" >
                                        <asp:Label ID="Label77" runat="server" Text="Casting_Cost(Rs)"></asp:Label>
                                    </div>
                                    <div class="col-sm-3">
                                      
                                    </div>
                                    <div class="col-sm-2" >
                                       
                                    </div>
                                    <div class="col-sm-2">
                                                                               
                                    </div>
                                     <div class="col-sm-2">
                                        <asp:TextBox ID="txtCast_Rs" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" ReadOnly="True" MaxLength="15"></asp:TextBox>                                        
                                    </div>                      
                                </div>

                                 <div class="form-group">
                                   <div class="col-sm-3" >
                                        <asp:Label ID="Label75" runat="server" Text="Transportation_Cost(Rs)"></asp:Label>
                                    </div>
                                    <div class="col-sm-3">
                                      
                                    </div>
                                    <div class="col-sm-2" >
                                         <asp:Label ID="Label76" runat="server" Text="For_Casting_Weight(KG)"></asp:Label>
                                    </div>
                                    <div class="col-sm-2">
                                                                               
                                    </div>
                                     <div class="col-sm-2">
                                        <asp:TextBox ID="txtTrans" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>                                        
                                    </div>                      
                                </div>

                                 <div class="form-group">
                                   <div class="col-sm-3" >
                                        <asp:Label ID="Label78" runat="server" Text="Tool_Amortisation(Rs)"></asp:Label>
                                    </div>
                                    <div class="col-sm-3">
                                      
                                    </div>
                                    <div class="col-sm-2" >
                                       
                                    </div>
                                    <div class="col-sm-2">
                                                                               
                                    </div>
                                     <div class="col-sm-2">
                                        <asp:TextBox ID="txtTool" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>                                        
                                    </div>                      
                                </div>

                                 <div class="form-group">
                                   <div class="col-sm-3" >
                                        <asp:Label ID="Label79" runat="server" Text="Total_Machining_Cost(Rs)"></asp:Label>
                                    </div>
                                    <div class="col-sm-3">
                                      
                                    </div>
                                    <div class="col-sm-2" >
                                       
                                    </div>
                                    <div class="col-sm-2">
                                                                               
                                    </div>
                                     <div class="col-sm-2">
                                        <asp:TextBox ID="txtTotMach" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>                                        
                                    </div>                      
                                </div>

                                 <div class="form-group">
                                   <div class="col-sm-3" >
                                        <asp:Label ID="Label80" runat="server" Text="Packing_Cost/PC(Rs)"></asp:Label>
                                    </div>
                                    <div class="col-sm-3">
                                      
                                    </div>
                                    <div class="col-sm-2" >
                                       
                                    </div>
                                    <div class="col-sm-2">
                                                                               
                                    </div>
                                     <div class="col-sm-2">
                                        <asp:TextBox ID="txtPacking" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>                                        
                                    </div>                      
                                </div>

                                 <div class="form-group">
                                   <div class="col-sm-3" >
                                        <asp:Label ID="Label81" runat="server" Text="Heat_Treatment"></asp:Label>
                                    </div>
                                    <div class="col-sm-3">
                                      
                                    </div>
                                    <div class="col-sm-2" >
                                       
                                    </div>
                                    <div class="col-sm-2">
                                                                               
                                    </div>
                                     <div class="col-sm-2">
                                        <asp:TextBox ID="txtHeat" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>                                        
                                    </div>                      
                                </div>

                                 <div class="form-group">
                                <div class="col-sm-10">
                                    <label id="Label83" runat="server" class="col-md-12 control-label"  style="text-align: center; text-decoration: underline; font-style: italic; font-size: medium;">Final Component Cost (Rs)</label>
                                </div>
                                     <div class="col-sm-2">
                                        <asp:TextBox ID="txtFinal" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" ReadOnly="True" MaxLength="15"></asp:TextBox>                                        
                                    </div> 
                                </div>

                                 <div class="form-group">
                                   <div class="col-sm-3" >
                                        <asp:Label ID="Label82" runat="server" Text="Interest%"></asp:Label>
                                    </div>
                                    <div class="col-sm-3">
                                      
                                    </div>
                                    <div class="col-sm-2" >
                                       
                                    </div>
                                    <div class="col-sm-2">
                                   <asp:TextBox ID="txtInterestPer2" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                     <div class="col-sm-2">
                                        <asp:TextBox ID="txtInterestPer" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15" ReadOnly="true"></asp:TextBox>
                                    </div>                      
                                </div>

                                 <div class="form-group">
                                <div class="col-sm-10">
                                    <label id="Label84" runat="server" class="col-md-12 control-label"  style="text-align: center; text-decoration: underline; font-style: italic; font-size: medium;">Net Vendor Cost (Rs)</label>
                                </div>
                                     <div class="col-sm-2">
                                        <asp:TextBox ID="txtVendor_Cost" runat="server" Width="100%" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>                                        
                                    </div> 
                                </div>
                                <div class="form-group" style="display:none;">
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtFstr" runat="server" Width="100%" Height="30px" ></asp:TextBox>
                                        <asp:TextBox ID="txtFstr2" runat="server" Width="100%" Height="30px" ></asp:TextBox>
                                        <asp:TextBox ID="txtTest" runat="server" Width="100%" Height="30px" ></asp:TextBox>
                                    </div>                      
                                </div>
                        </div></div></div>
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
    <asp:HiddenField ID="hfGridView1SV" runat="server" />
    <asp:HiddenField ID="hfGridView1SH" runat="server" />
    <asp:HiddenField ID="hfLine" runat="server" />
    <script type="text/javascript">
        $(function () {
            var tabName = $("[id*=TabName]").val() != "" ? $("[id*=TabName]").val() : "DescTab";
            $('#Tabs a[href="#' + tabName + '"]').tab('show');
            $("#Tabs a").click(function () {
                $("[id*=TabName]").val($(this).attr("href").replace("#", ""));
            });
        });
    </script>
    <asp:HiddenField ID="TabName" runat="server" />
</asp:Content>