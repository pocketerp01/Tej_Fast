<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="om_fixed_asset_pur" CodeFile="om_fixed_asset_pur.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../tej-base/Scripts/gridviewScroll.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {

        });
        function fill_zero(val) { if (isNaN(val)) return 0; if (isFinite(val)) return val; }
    </script>
    <script type="text/javascript">
        function openfileDialog() {
            $("#Attch").click();
        }
        function submitFile() {
            $("#<%= btnAtt.ClientID%>").click();
        };
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper">
        <section class="content-header">
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label></td>
                    <td style="text-align:right">
                        <button type="submit" id="btnnew" class="btn btn-info" style="width:100px;" runat="server" accesskey="N" onserverclick="btnnew_ServerClick"><u>N</u>ew</button>
                        <button type="submit" id="btnedit" class="btn btn-info"  style="width: 100px;" runat="server" accesskey="i" onserverclick="btnedit_ServerClick">Ed<u>i</u>t</button>
                        <button type="submit" id="btnsave" class="btn btn-info" style="width: 100px;" runat="server" accesskey="s" onserverclick="btnsave_ServerClick"><u>S</u>ave</button>
                        <button type="submit" id="btnprint" class="btn btn-info" style="width: 100px;" runat="server" accesskey="P" onserverclick="btnprint_ServerClick"><u>P</u>rint</button>
                        <button type="submit" id="btndel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="l" onserverclick="btndel_ServerClick">De<u>l</u>ete</button>
                        <button type="submit" id="btnlist" class="btn btn-info" style="width: 100px;" runat="server" accesskey="t" onserverclick="btnlist_ServerClick">Lis<u>t</u></button>
                        <button type="submit" id="btncancel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="c" onserverclick="btncancel_ServerClick"><u>C</u>ancel</button>
                        <button type="submit" id="btnexit" class="btn btn-info"  style="width: 100px;" runat="server" accesskey="X" onserverclick="btnexit_ServerClick">E<u>x</u>it</button>
                        <button type="submit" id="btn_tag" class="btn btn-info" style="width: 100px;" runat="server" accesskey="X" onserverclick="btn_tag_ServerClick">Asset Ta<u>g</u></button>
                        
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
                                <label id="Label1" runat="server" class="col-sm-4 control-label" title="lbl1">Entry_No</label>
                                <div class="col-sm-3">
                                    <input id="txtvchnum" type="text" class="form-control" style="width: 105px;" runat="server" placeholder="Entry_No" readonly="readonly" />
                                 </div>
                                    <label id="Label8" runat="server" class="col-sm-2 control-label" title="lbl1">Entry Date</label>
                                <div class="col-sm-3">
                                    <input id="txtvchdate" type="date" style="font-size:small" class="form-control" runat="server" placeholder=" Voucher Date" />
                                </div>
                            </div>

                            <div class="form-group">
                                 <label id="Label38" runat="server" class="col-sm-3 control-label"  title="">Block(Optional)</label>
                                <div class="col-sm-1" id="div5" runat="server">
                                   <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/tej-base/css/images/bdsearch5.png" OnClick="btnblock_Click" />
                                </div>
                                  <div class="col-sm-2">
                                    <input id="Textblockg" type="text" class="form-control" style="width: 130px;" runat="server" placeholder="Block Code" maxlength="5"  readonly="readonly"/>
                                </div>
                                 <div class="col-sm-6">
                                    <input id="txtblock" type="text" class="form-control" runat="server" placeholder="Block as per IT Act" maxlength="30"  readonly="readonly"/>
                                </div>

                            </div>

                                                         
                            <div class="form-group">
                                <label id="Label6" runat="server" class="col-sm-3 control-label" title="lbl1">Group of Asset</label>
                                 <div class="col-sm-1" id="divCocd" runat="server">
                                   <asp:ImageButton ID="btnCocd" runat="server" ImageUrl="~/tej-base/css/images/bdsearch5.png" OnClick="btnCocd_Click" />
                                </div>
                                <div class="col-sm-2">
                                    <input id="txtlbl8" type="text" class="form-control" style="width: 130px;" runat="server" placeholder="Code" maxlength="5"  readonly="readonly"/>
                                </div>
                                 <div class="col-sm-6">
                                    <input id="txtlbl8a" type="text" class="form-control" runat="server" placeholder="Group of Asset" maxlength="10" readonly="readonly" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label17" runat="server" class="col-sm-3 control-label" title="lbl1">Location</label>
                               
                                 <div class="col-sm-1" id="div6" runat="server">
                                   <asp:ImageButton ID="btnlocation" runat="server" ImageUrl="~/tej-base/css/images/bdsearch5.png" OnClick="btnlocation_Click" />
                                </div>

                                <div class="col-sm-2">
                                    <input id="txtlocationc" type="text" class="form-control" style="width: 130px;" runat="server" placeholder="Code" maxlength="5"  readonly="readonly"/>
                                </div>
                                
                                <div class="col-sm-6">
                                    <input id="txtlocation" type="text" class="form-control" readonly="true" runat="server" placeholder="Location" maxlength="30"/>
                                </div>
                            </div>

                              <div class="form-group">
                                  <label id="Label36" runat="server" class="col-sm-3 control-label" title="">Department</label>
                                <div class="col-sm-1" id="div1" runat="server">
                                   <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/tej-base/css/images/bdsearch5.png" OnClick="btndepartment_Click" />
                                </div>

                                  <div class="col-sm-2">
                                    <input id="txtdepartc" type="text" class="form-control" style="width: 130px;" runat="server" placeholder="Code" maxlength="5"  readonly="readonly"/>
                                </div>

                                  <div class="col-sm-6">
                                    <input id="txt_department" type="text" class="form-control" runat="server" placeholder="Department" maxlength="30"  readonly="readonly"/>
                                </div>
                            </div>  


                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div>
                        <div class="box-body">

                            <div class="form-group">
                                <label id="Label7" runat="server" class="col-sm-3 control-label" title="">Supplied By</label>
                                <div class="col-sm-1" id="div4" runat="server">
                                   <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/tej-base/css/images/bdsearch5.png" OnClick="btnSup_Click" />
                                </div>
                                
                                 <div class="col-sm-8">
                                      <input id="txtSup_by" type="text" class="form-control" runat="server" placeholder="Supplied By"  maxlength="75" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label10" runat="server" class="col-sm-4 control-label" title="">Supplier Address</label>
                                <div class="col-sm-8">
                                    <input id="txtSup_Address" type="text" class="form-control" runat="server" placeholder="Supplier Address" maxlegth="150"/>
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label5" runat="server" class="col-sm-4 control-label" title="lbl1">Name of Asset </label>
                                <div class="col-sm-8">
                                    <input id="txtaname" type="text" class="form-control" runat="server" placeholder="Name of Asset" maxlength="100" />
                                </div>
                            </div>


                            <div class="form-group">
                                <label id="Label9" runat="server" class="col-sm-4 control-label" title="lbl1">Asset Id</label>
                               
                                <div class="col-sm-8">
                                    <input id="txtlbl4" type="text" class="form-control" runat="server" placeholder="Asset Id" maxlength="20" />
                                </div>
                            </div>
                              

                            <div class="form-group">
                                <label id="Label2" runat="server" class="col-sm-4 control-label" title="">Invoice_No.</label>
                                <div class="col-sm-3">
                                    <input id="txtlbl2" type="text" class="form-control" runat="server" placeholder="Invoice No." maxlength="25" />
                                </div>
                                 <label id="Label4" runat="server" class="col-sm-2 control-label" title="">Invoice_Date</label>
                                <div class="col-sm-3">
                                    <input id="txtlbl5" type="date" style="font-size:small" class="form-control" runat="server" placeholder=" Invoice Date" maxlength="30" />
                                </div>
                            </div>

                        
                        </div>
                    </div>
                </div>                
                <div class="col-md-12">
                      <div class="row">
                            <div class="col-md-6">
                    <div>
                        <div class="box-body">
                          

                               <div class="form-group">
                                <label id="Label3" runat="server" class="col-sm-4 control-label" title="lbl1">Basic Cost/OP Bal.</label>
                                <div class="col-sm-3">
                                    <input id="txtlbl3" type="text" class="form-control" runat="server" placeholder="Basic Cost" maxlength="13" onkeyup="caloriginalcost()" />
                                </div>
                                    <label id="Label28" runat="server" class="col-sm-2 control-label" title="lbl1">Install.Cost</label>
                                <div class="col-sm-3">
                                    <input id="txt_installCost" type="text" class="form-control" runat="server" placeholder="Install. Cost" maxlength="11" onkeyup="caloriginalcost()" />
                                </div>
                            </div>


                              <div class="form-group">
                                <label id="Label29" runat="server" class="col-sm-4 control-label" title="lbl1">Custom Duty</label>
                                <div class="col-sm-3">
                                    <input id="txt_CustomDuty" type="text" class="form-control" runat="server" placeholder="Custom Duty" maxlength="11" onkeyup="caloriginalcost()" />
                                </div>
                                  <label id="Label30" runat="server" class="col-sm-2 control-label" title="lbl1">Other Chrg</label>
                                <div class="col-sm-3">
                                    <input id="txt_otherchrges" type="text" class="form-control" runat="server" placeholder="Other Charges" maxlength="11" onkeyup="caloriginalcost()"  />
                                </div>
                               </div>

                              <div class="form-group">
                                <label id="Label20" runat="server" class="col-sm-4 control-label" title="lbl1">Opening.Depr.</label>
                                <div class="col-sm-3">
                                    <input id="txtOpDep" type="text" class="form-control" runat="server" placeholder="Op.Depriciation" maxlength="13"  onkeyup="caloriginalcost()"/>
                                </div>
                                <label id="Label31" runat="server" class="col-sm-2 control-label" title="lbl1">Orig_Cost</label>
                                <div class="col-sm-3">
                                    <input id="txt_originalcost" type="text" class="form-control" runat="server" placeholder="Original Cost" maxlength="11" onkeyup="caloriginalcost()" readonly="readonly" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label15" runat="server" class="col-sm-4 control-label" title="lbl1">Depriciable.Value</label>
                               <div class="col-sm-3">
                                    <input id="txtdeprab_val" type="text" class="form-control" runat="server" placeholder="Deprab.Val" maxlength="13" readonly="readonly"  />
                                </div>
                                <label id="Label27" runat="server" class="col-sm-2 control-label" title="lbl1">Depr/Day</label>
                                <div class="col-sm-3">
                                    <input id="txtdepr_perday" type="text" class="form-control" runat="server" placeholder="Depr/Day" maxlength="8" readonly="readonly" />
                                </div>
                            </div>

                           <div class="form-group">
                                <label id="Label14" runat="server" class="col-sm-4 control-label" title="lbl1">Residual Value</label>
                                <div class="col-sm-3">
                                     <input id="txtresidual_value" type="text" class="form-control" runat="server" placeholder="Residual Value" maxlength="13" />
                                </div>
                                <label id="Label21" runat="server" class="col-sm-2 control-label" title="lbl1">Quantity</label>
                                <div class="col-sm-3">
                                    <input id="txtQuantity" type="text" class="form-control" runat="server" placeholder="Quantity" maxlength="5" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label33" runat="server" class="col-sm-4 control-label" title="lbl1">Warranty</label>
                                <div class="col-sm-3">
                                         <select id="ddwarrantydays" runat="server" class="form-control">
                                        <option value="Y">Yes</option>
                                        <option value="N" selected="selected">No</option>
                                    </select>
                                </div>
                                <label id="Label34" runat="server" class="col-sm-2 control-label" title="lbl1">Warranty Date</label>
                                <div class="col-sm-3">
                                    <input id="txt_warranty_date" type="Date" style="font-size:small" class="form-control" runat="server" placeholder="Warranty Date" maxlength="10"/>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>

                   <div class="col-md-6">
                    <div>
                        <div class="box-body">

                           <div class="form-group">
                                <label id="Label24" runat="server" class="col-sm-3 control-label" title="lbl1">Total Life in Yrs</label>
                                <div class="col-sm-1" id="div2" runat="server">
                                    <input id="txt_life" type="text" class="form-control" runat="server" placeholder="Years" maxlength ="4"/>
                                </div>
                                <div class="col-sm-3">
                                    <input id="txttotal_life" type="text" class="form-control" runat="server" placeholder="Tot Life days" maxlength="6" readonly="readonly"/>
                                </div>
                            
                                 <label id="Label25" runat="server" class="col-sm-2 control-label" title="lbl1">Balance Life</label>
                                <div class="col-sm-3">
                                    <input id="txtbal_life" type="text" class="form-control" runat="server" placeholder="B.Life days" maxlength="6" readonly="readonly"/>
                                 </div>
                              
                              <div class="form-group">
                                <label id="Label19" runat="server" class="col-sm-4 control-label" title="lbl1">Install Dt.</label>
                                <div class="col-sm-3">
                                       <input id="txtinstalldate" type="date" style="font-size:small" class="form-control" runat="server" placeholder="Install Date"  />
                                 </div>
                                 <label id="Label26" runat="server" class="col-sm-2 control-label" title="lbl1">End Date</label>
                                <div class="col-sm-3">
                                    <input id="txtlife_end" type="text" class="form-control" runat="server" placeholder="Life End Date" maxlength="10"  readonly="readonly" />
                                </div>
                             </div>

                              <div class="form-group">
                                <label id="Label32" runat="server" class="col-sm-4 control-label" title="lbl1">Used Life Days</label>
                                <div class="col-sm-3">
                                    <input id="txt_usedlife" type="text" class="form-control" runat="server" placeholder="Used Life Days" maxlength="6" />
                                </div>
                                <label id="Label35" runat="server" class="col-sm-2 control-label" title="lbl1">Old_Tag </label>
                                <%--<div class="col-sm-3">
                                    <select id="ddadddep" runat="server" class="form-control">
                                        <option value="N">No</option>
                                        <option value="Y">Yes</option>
                                    </select>      
                                </div>--%>
                                  <div class="col-sm-3">
                                    <input id="Txtoldtag" type="text" class="form-control" runat="server" placeholder="Old_Tag" maxlength="30" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label37" runat="server" class="col-sm-4 control-label" title="lbl1">OwnerShip Type</label>
                                <div class="col-sm-3">
                                    <select id="ddOwner" runat="server" class="form-control">
                                        <option value="Y">Owned</option>
                                        <option value="N">Leased</option>
                                        <option value="H">Hire_Purchase</option>
                                        <option value="C">Co_owned</option>

                                    </select>
                                </div>
                                  <label id="Label39" runat="server" class="col-sm-2 control-label" title="lbl1">Add_Dep%CA</label>
                                <div class="col-sm-3">
                                    <input id="txtadddepp" type="text" class="form-control" runat="server" placeholder="Add_Dep_%" maxlength="30"/>
                            </div>
                            </div>
                             
                            <div class="form-group">
                                <label id="Label18" runat="server" class="col-sm-4 control-label" title="lbl1">Dom/Imp</label>
                                <div class="col-sm-3">
                                  <select id="ddDomImp" runat="server" class="form-control">
                                        <option value="D">Domestic</option>
                                        <option value="I">Imported</option>
                                    </select>
                                
                                </div>
                                <label id="Label22" runat="server" class="col-sm-2 control-label" title="lbl1">Tangible</label>
                                <div class="col-sm-3">
                                    <select id="ddtangible" runat="server" class="form-control" >
                                        <option value="Y">Yes</option>
                                        <option value="N">No</option>
                                      
                                    </select>
                                </div>
                            </div>

                               <div class="form-group">
                                <label id="Label16" runat="server" class="col-sm-4 control-label" title="lbl1">Voucher Link</label>
                                <div class="col-sm-8">
                                    <input id="Voucherlink" type="text" class="form-control" runat="server" placeholder="Voucher Link" maxlength="50" />
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
           </div>

                            <asp:Label ID="lbltxtrmk" runat="server" Text="Client Remarks" CssClass="col-sm-2 control-label" Visible="false"></asp:Label>                            
                            <asp:TextBox ID="txtrmk" runat="server" Width="99%" TextMode="MultiLine" onkeyup="max_length(this,200)" placeholder="Remarks" Visible="false"></asp:TextBox>
                        </div>
                    </div>
              
                <div class="col-md-12">
                    <div>
                        <div class="box-body">
                          
                            <asp:TextBox ID="TextBox1" runat="server" CssClass="col-lg-12 control-label" TextMode="SingleLine" MaxLength="100" placeholder="Remarks upto 100 Char" ></asp:TextBox>
                          
                            <div class="form-group">
                                        <asp:FileUpload ID="Attch" CssClass="col-sm-1" runat="server" Visible="true" onchange="submitFile()" ></asp:FileUpload ><%--</td>--%>
                              <div class="col-sm-2">
                                        <asp:TextBox ID="txtAttch" runat="server" Width="110%" ReadOnly="true"  MaxLength="100" placeholder="File Name 100 Char" ></asp:TextBox>
                                     </div>
                                       <div class="col-sm-9"> 
                                    <asp:TextBox ID="txtAttchPath" runat="server" Width="101%" ReadOnly="true" MaxLength="250" placeholder="Path Upto 250 Char"  ></asp:TextBox><%--</td>--%>
                                           
                                    </div>
                               </div> 
                            <asp:Button ID="btnAtt" runat="server" Text="Attachment" OnClick="btnAtt_Click" Width="134px" Style="display: none" />
                            <asp:Label ID="lblShow" runat="server"></asp:Label>
                            <asp:Label ID="lblUpload" runat="server" Style="display: none"></asp:Label>

                            <asp:ImageButton ID="btnView1" runat="server" ImageUrl="~/tej-base/images/preview-file.png" OnClick="btnView1_Click" Visible="false" />
                            <asp:ImageButton ID="btnDwnld1" runat="server" ImageUrl="~/tej-base/images/Save.png" OnClick="btnDwnld1_Click" Visible="false" />
                        </div>
                        
                    </div>
                   
                    
                          
                </div>

                <div class="col-md-12">
                    <div>
                        <div class="box-body">
                            <asp:Label ID="Label11" runat="server" Text="1.Please Mention Icon/Path of the option Correctly." CssClass="col-sm-2 control-label"></asp:Label>
                            <asp:Label ID="Label13" runat="server" Text="2.Remarks should be restricted to 225 Characters." CssClass="col-sm-2 control-label"></asp:Label>
                            <asp:Label ID="Label12" runat="server" Text="3.Please restrict Attachment Size to 3 MB Max." CssClass="col-sm-2 control-label"></asp:Label>
                        </div>
                    </div>
                        <div class="form-group" style="visibility:hidden;">
                                <label id="Label23" runat="server" class="col-sm-4 control-label" title="lbl1">Purch Entry</label>
                                <div class="col-sm-8">
                                    <%--<input id="Text1" type="text" class="form-control" runat="server" placeholder="First Time/New" />--%>
                                    <select id="ddpurchentry" runat="server" class="form-control">
                                        <option value="YES">Yes</option>
                                        <option value="NO">No</option>
                                    </select>
                                </div>
                            </div>
                </div>

                <div style="width:1px; height:1px;">
                <section class="col-lg-12 connectedSortable" id="AllTabs" runat="server" style="visibility:hidden;">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">
                                <li><a href="#DescTab" id="tab1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab" style="visibility:hidden;">UDF Data</a></li>
                                <li><a href="#DescTab2" id="tab2" runat="server" aria-controls="DescTab2" role="tab" data-toggle="tab">Item Details</a></li>
                                <li><a href="#DescTab3" id="tab3" runat="server" aria-controls="DescTab3" role="tab" data-toggle="tab">Comm.Terms</a></li>
                                <li><a href="#DescTab4" id="tab4" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Addl.Terms</a></li>
                                <li><a href="#DescTab5" id="tab5" runat="server" aria-controls="DescTab5" role="tab" data-toggle="tab">Delv.Sch</a></li>
                                <li><a href="#DescTab6" id="tab6" runat="server" aria-controls="DescTab6" role="tab" data-toggle="tab">Inv.Dtl</a></li>

                            </ul>

                            <div class="tab-content">


                                <div role="tabpanel" class="tab-pane active" id="DescTab" style="visibility:hidden;">
                                    <div class="lbBody" style="height: 150px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <asp:GridView ID="sg4" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="1200px" Font-Size="13px"
                                            AutoGenerateColumns="False" OnRowDataBound="sg4_RowDataBound"
                                            OnRowCommand="sg4_RowCommand">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Add</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg4_btnadd" runat="server" CommandName="SG4_ROW_ADD" ImageAlign="Middle" ImageUrl="../tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Del</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg4_btnrmv" runat="server" CommandName="SG4_RMV" ImageUrl="../tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="sg4_srno" HeaderText="Sr.No." />

                                                <asp:TemplateField>
                                                    <HeaderTemplate>UDF_Field</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg4_t1" runat="server" Text='<%#Eval("sg4_t1") %>' Width="100%" ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>UDF_Value</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg4_t2" runat="server" Text='<%#Eval("sg4_t2") %>' Width="100%" MaxLength="40"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#1797C0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        </asp:GridView>
                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane active" id="DescTab2">
                                    <div class="lbBody" id="gridDiv" style="color: White; height: 150px; overflow: hidden; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <asp:GridView ID="sg1" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="1500px" Font-Size="Smaller"
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
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btnadd" runat="server" CommandName="SG1_ROW_ADD" ImageAlign="Middle" ImageUrl="../tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Del</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btnrmv" runat="server" CommandName="SG1_RMV" ImageUrl="../tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="sg1_srno" HeaderText="sg1_srno" />
                                                <asp:BoundField DataField="sg1_f1" HeaderText="sg1_f1" />
                                                <asp:BoundField DataField="sg1_f2" HeaderText="sg1_f2" />
                                                <asp:BoundField DataField="sg1_f3" HeaderText="sg1_f3" />
                                                <asp:BoundField DataField="sg1_f4" HeaderText="sg1_f4" />
                                                <asp:BoundField DataField="sg1_f5" HeaderText="sg1_f5" />

                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t1</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t1" runat="server" Text='<%#Eval("sg1_t1") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Dt</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btndt" runat="server" CommandName="SG1_ROW_DT" ImageAlign="Middle" ImageUrl="../tej-base/images/Btn_addn.png" Width="20px" ToolTip="Date" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t2</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t2" runat="server" Text='<%#Eval("sg1_t2") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%-- <asp:TemplateField>
                                                            <HeaderTemplate>sg1_t2</HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="sg1_t2" runat="server" Text='<%#Eval("sg1_t2") %>' Width="100%"></asp:TextBox>
                                                                <asp:MaskedEditExtender ID="Maskedit2" runat="server" Mask="99/99/9999"
                                                                    MaskType="Date" TargetControlID="sg1_t2" />
                                                                <asp:CalendarExtender ID="txtvchdate_CalendarExtender2" runat="server"
                                                                    Enabled="True" TargetControlID="sg1_t2"
                                                                    Format="dd/MM/yyyy">
                                                                </asp:CalendarExtender>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>--%>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t3</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t3" runat="server" Text='<%#Eval("sg1_t3") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t4</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t4" runat="server" Text='<%#Eval("sg1_t4") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t5</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t5" runat="server" Text='<%#Eval("sg1_t5") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t6</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t6" runat="server" Text='<%#Eval("sg1_t6") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t7</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t7" runat="server" Text='<%#Eval("sg1_t7") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t8</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t8" runat="server" Text='<%#Eval("sg1_t8") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t9</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t9" runat="server" Text='<%#Eval("sg1_t9") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Tcode</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btntax" runat="server" CommandName="SG1_ROW_TAX" ImageAlign="Middle" ImageUrl="../tej-base/images/Btn_addn.png" Width="20px" ToolTip="Choose Tax" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t10</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t10" runat="server" Text='<%#Eval("sg1_t10") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t11</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t11" runat="server" Text='<%#Eval("sg1_t11") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t12</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t12" runat="server" Text='<%#Eval("sg1_t12") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t13</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t13" runat="server" Text='<%#Eval("sg1_t13") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t14</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t14" runat="server" Text='<%#Eval("sg1_t14") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t15</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t15" runat="server" Text='<%#Eval("sg1_t15") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t16</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t16" runat="server" Text='<%#Eval("sg1_t16") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                            </Columns>
                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#1797C0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        </asp:GridView>
                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane active" id="DescTab3">
                                    <div class="lbBody" style="height: 150px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
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

                                                                <asp:TextBox ID="txtlbl10" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl11" runat="server" Text="lbl11" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl11" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl11_Click" /></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl11" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl12" runat="server" Text="lbl12" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl12" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl12_Click" /></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl12" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl13" runat="server" Text="lbl13" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl13" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl13_Click" /></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl13" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl14" runat="server" Text="lbl14" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl14" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl14_Click" /></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl14" runat="server" Width="350px"></asp:TextBox>

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

                                                                <asp:TextBox ID="txtlbl15" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl16" runat="server" Text="lbl16" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl16" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl16_Click" /></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl16" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl17" runat="server" Text="lbl17" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl17" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl17_Click" /></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl17" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl18" runat="server" Text="lbl18" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl18" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl18_Click" /></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl18" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl19" runat="server" Text="lbl19" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl19" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl19_Click" /></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl19" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane active" id="DescTab4">
                                    <div class="lbBody" style="height: 150px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
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
                                            <HeaderStyle BackColor="#1797C0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        </asp:GridView>
                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane active" id="DescTab5">
                                    <div class="lbBody" style="height: 150px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
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
                                            <HeaderStyle BackColor="#1797C0" Font-Bold="True" ForeColor="White"
                                                CssClass="GridviewScrollHeader" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        </asp:GridView>
                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane active" id="DescTab6">
                                    <div class="lbBody" style="height: 150px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl40" runat="server" Text="lbl40" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl40" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl41" runat="server" Text="lbl41" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl41" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl42" runat="server" Text="lbl42" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl42" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl43" runat="server" Text="lbl43" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl43" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl44" runat="server" Text="lbl44" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl44" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl45" runat="server" Text="lbl45" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl45" runat="server" Width="350px"></asp:TextBox>

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

                                                                <asp:TextBox ID="txtlbl46" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl47" runat="server" Text="lbl47" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl47" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl48" runat="server" Text="lbl48" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl48" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl49" runat="server" Text="lbl49" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl49" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl50" runat="server" Text="lbl50" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl50" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl51" runat="server" Text="lbl51" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl51" runat="server" Width="350px"></asp:TextBox>

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
    <script type="text/javascript">
        $(function () {
            var tabName = $("[id*=TabName]").val() != "" ? $("[id*=TabName]").val() : "DescTab";
            $('#Tabs a[href="#' + tabName + '"]').tab('show');
            $("#Tabs a").click(function () {
                $("[id*=TabName]").val($(this).attr("href").replace("#", ""));
            });
        });
    </script>

    <script>
        function calculate() {
          
            debugger;
            var value1 = fill_zero(document.getElementById("ContentPlaceHolder1_txt_life").value);
            var result = new Date(document.getElementById("ContentPlaceHolder1_txtinstalldate").value);
            result.setDate(result.getDate() + value1);

            alert (result);
            document.getElementById("ContentPlaceHolder1_txttotal_life").value = result;
        }
        function fill_zero(val) { if (isNaN(val)) return 0; if (isFinite(val)) return val; }
    </script>


    <script>
        function caloriginalcost()
         {   
            var value1 = fill_zero(document.getElementById("ContentPlaceHolder1_txtlbl3").value),
            value2 = fill_zero(document.getElementById("ContentPlaceHolder1_txt_installCost").value),

            value3 = fill_zero(document.getElementById("ContentPlaceHolder1_txt_CustomDuty").value),
            value4 = fill_zero(document.getElementById("ContentPlaceHolder1_txt_otherchrges").value),
            value5 = fill_zero(document.getElementById("ContentPlaceHolder1_txtOpDep").value);

            var valuetot = fill_zero(parseInt(value1) + Number(value2) + Number(value3) + Number(value4));
            document.getElementById("ContentPlaceHolder1_txt_originalcost").value = valuetot;

            var valuerem = fill_zero(parseInt(valuetot) - Number(value5));
            document.getElementById("ContentPlaceHolder1_txtdeprab_val").value = valuerem;
        }

        function fill_zero(val) { if (isNaN(val)) return 0; if (isFinite(val)) return val; }
    </script>

    <asp:HiddenField ID="TabName" runat="server" />
   
</asp:Content>
