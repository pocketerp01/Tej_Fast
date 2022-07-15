<%@ Page Title="" Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="frmBoxMasterUpdate" CodeFile="frmBoxMasterUpdate.aspx.cs" %>
<%--<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .auto-style1 {
            width: 131px;
        }
        .auto-style5
        {
            width: 34px;
        }
              
        .auto-style7
        {
            width: 46px;
            font-weight: bold;
        }
              
        .auto-style11
        {
            width: 51px;
        }
        .auto-style12
        {
            width: 55px;
            font-weight: bold;
            height: 20px;
            font-size: xx-small;
        }
        .auto-style13
        {
            width: 131px;
            height: 20px;
        }
        .auto-style18
        {
            font-size: small;
        }
        .auto-style19
        {
            width: 192px;
            font-weight: bold;
            font-size: small;
        }
              
        .auto-style22
        {
            font-size: x-small;
        }
        .auto-style24
        {
            width: 185px;
            font-weight: bold;
            height: 20px;
            font-size: small;
        }
        .auto-style26
        {
            width: 55px;
            font-weight: bold;
            font-size: x-small;
        }
        .auto-style27
        {
            width: 55px;
            font-weight: bold;
            font-size: xx-small;
        }
        .auto-style30
        {
            width: 185px;
            font-weight: bold;
            font-size: small;
        }
      
        .auto-style31
        {
            width: 154px;
            font-weight: bold;
            font-size: small;
        }
              
        .auto-style35
        {
            width: 46px;
            font-weight: bold;
            font-size: xx-small;
        }
        .auto-style36
        {
            width: 46px;
            font-weight: bold;
            font-size: x-small;
        }
      
        </style>
    <script type="text/javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode != 46 && charCode > 31
              && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
    </script>
     
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper">
        <section class="content-header">
            <table style="width: 100%">
                <tr>
                    <td> <button type="submit" class="btn btn-info" style="width: 100px;" runat="server" id="cmdnew" onserverclick="cmdnew_Click" accesskey="N"><u>N</u>ew</button>
                <button type="submit" class="btn btn-info" style="width: 100px;" runat="server" id="cmdedit" onserverclick="cmdedit_Click" accesskey="i">Ed<u>i</u>t</button>
                <button type="submit" class="btn btn-info" style="width: 100px;" runat="server" id="btnsave" onserverclick="btnsave_Click" accesskey="s"><u>S</u>ave</button>
                <button type="submit" class="btn btn-info" style="width: 100px;" runat="server" id="cmdprint" onserverclick="cmdprint_Click" accesskey="P"><u>P</u>rint</button>
                <button type="submit" class="btn btn-info" style="width: 100px;" runat="server" id="cmddel" onserverclick="cmddel_Click" accesskey="l">De<u>l</u>ete</button>
                <button type="submit" id="btnlist" class="btn btn-info" style="width:100px;" runat="server" accesskey="t" onserverclick="btnlist_ServerClick">Lis<u>t</u></button>
                <button type="submit" id="btncancel" class="btn btn-info" style="width:100px;" runat="server" accesskey="c" onserverclick="btncancel_ServerClick"><u>C</u>ancel</button>
                <button type="submit" class="btn btn-info" style="width: 100px;" runat="server" id="cmdexit" accesskey="X" onserverclick="cmdexit_Click">E<u>x</u>it</button>
          </td>
                    <td>
                        <asp:Label ID="lblheader" Text="Costing Master Rate Updation" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label>
                    </td>
                </tr>
                </table>
            <%--<h1>Costing Master Rate Updation
            </h1>
            <div class="box-footer">
               <%-- <button type="submit" class="btn btn-info" style="width: 100px;" runat="server" id="cmdnew" onserverclick="cmdnew_Click" accesskey="N"><u>N</u>ew</button>
                <button type="submit" class="btn btn-info" style="width: 100px;" runat="server" id="cmdedit" onserverclick="cmdedit_Click" accesskey="i">Ed<u>i</u>t</button>
                <button type="submit" class="btn btn-info" style="width: 100px;" runat="server" id="btnsave" onserverclick="btnsave_Click" accesskey="s"><u>S</u>ave</button>
                <button type="submit" class="btn btn-info" style="width: 100px;" runat="server" id="cmdprint" onserverclick="cmdprint_Click" accesskey="P"><u>P</u>rint</button>
                <button type="submit" class="btn btn-info" style="width: 100px;" runat="server" id="cmddel" onserverclick="cmddel_Click" accesskey="l">De<u>l</u>ete</button>
                <button type="submit" id="btnlist" class="btn btn-info" style="width:100px;" runat="server" accesskey="t" onserverclick="btnlist_ServerClick">Lis<u>t</u></button>
                <button type="submit" id="btncancel" class="btn btn-info" style="width:100px;" runat="server" accesskey="c" onserverclick="btncancel_ServerClick"><u>C</u>ancel</button>
                <button type="submit" class="btn btn-info" style="width: 100px;" runat="server" id="cmdexit" accesskey="X" onserverclick="cmdexit_Click">E<u>x</u>it</button>
            </div>--%>
        </section>
        <section class="content">
            <div class="row">
                <!-- left column -->
                <div class="col-md-12" style="display:none" >
                    <div>

                        <%--<input type="text" id="txtRange" runat="server" placeholder="Range" class="form-control" style="width:100px; height:30px; margin-bottom:2px;"/>--%>
                        <div class="box-body">
                            <div class="form-group">
                               <table>
                                    <tr>
                                        <td class="auto-style19">
                                            Party Master
                                            </td>
                                        <td class="auto-style7">
                                            <button id="btnParty" runat="server" onserverclick="btnParty_Click" width="16px" >!</button>
                                        </td>
                                        <td class="auto-style11">

                                            <%--<input type="text" id="txtContact" runat="server" placeholder="Contact Person" class="form-control" style="width:200px; height:30px; margin-bottom:2px;"/>--%>

                                            <asp:TextBox ID="txtPCode" runat="server" Style="margin-bottom: 2px"  Width="64px" MaxLength="10" ReadOnly="True"></asp:TextBox>
                                        </td>
                                        <td class="auto-style1">
                                            <asp:TextBox ID="txtParty" runat="server" Style="margin-bottom: 2px" ReadOnly="True" Width="168px"></asp:TextBox>
                                        </td>
                                    </tr>  
                                    <tr style="display:none">
                                        <td class="auto-style19">
                                            Process Wastage </td>
                                        <td class="auto-style35">
                                            in %</td>
                                        <td class="auto-style1" colspan="2">
                                            <asp:TextBox ID="txtProcess" runat="server" Style="margin-bottom: 2px" onKeypress="return isNumberKey(event);" MaxLength="50" oncontextmenu="return false;" onpaste="return false"></asp:TextBox>
                                            <%--</form>--%>
                                        </td>

                                        <%--<button type="submit" class="btn btn-info" style="width:100px;" runat="server" id="hffield"   ></button>--%>
                                    </tr>
                                    <tr style="display:none">
                                        <td class="auto-style19">
                                            Board Making Charges </td>
                                        <td class="auto-style35">
                                            / Kg.</td>
                                        <td class="auto-style1" colspan="2">
                                            <asp:TextBox ID="txtBoard" runat="server" Style="margin-bottom: 2px" onKeypress="return isNumberKey(event);" MaxLength="50" oncontextmenu="return false;" onpaste="return false"></asp:TextBox>
                                            <%--<button type="submit" class="btn btn-info" style="width:100px;" runat="server" id="edmode"   ></button>--%>
                                        </td>

                                    </tr>
                                    <tr style="display:none">
                                        <td class="auto-style19">
                                            Printing / Slotting</td>
                                        <td class="auto-style35">
                                            in Sq.M.</td>
                                        <td class="auto-style1" colspan="2">
                                            <asp:TextBox ID="txtPrinting" runat="server" Style="margin-bottom: 2px" onKeypress="return isNumberKey(event);" MaxLength="100" oncontextmenu="return false;" onpaste="return false"></asp:TextBox>
                                            <%--<input type="text" id="txtTin" runat="server" placeholder="Sales Tax/Tin No" class="form-control" style="width:100px; height:30px; margin-bottom:2px;"/>--%>
                                        </td>

                                    </tr>
                                    <tr style="display:none">
                                        <td class="auto-style19">
                                            <span class="auto-style18">Water Resistance Coating</span> </td>
                                        <td class="auto-style35">
                                            in Sq. M.</td>
                                        <td class="auto-style1" colspan="2">
                                            <asp:TextBox ID="txtWater" runat="server" Style="margin-bottom: 2px;" onKeypress="return isNumberKey(event);" MaxLength="50" oncontextmenu="return false;" onpaste="return false"></asp:TextBox>
                                            <%--<input type="text" id="txtTin" runat="server" placeholder="Sales Tax/Tin No" class="form-control" style="width:100px; height:30px; margin-bottom:2px;"/>--%>
                                        </td>
                                        
                                    </tr>
                                    <tr style="display:none">
                                        <td class="auto-style19">
                                            Die Cutting</td>
                                        <td class="auto-style35">
                                            / Sq. M.</td>
                                        <td class="auto-style1" colspan="2">
                                            <asp:TextBox ID="txtDie" runat="server" Style="margin-bottom: 2px;" onKeypress="return isNumberKey(event);" MaxLength="50" oncontextmenu="return false;" onpaste="return false"></asp:TextBox>
                                            <%--</form>--%>
                                        </td>
                                    </tr>
                                    <tr style="display:none">
                                        <td class="auto-style19">
                                            <span class="auto-style18">Stitching Or Flap Pasting</span> </td>
                                        <td class="auto-style36">
                                            / Kg.</td>
                                        <td class="auto-style1" colspan="2">
                                            <asp:TextBox ID="txtStitching" runat="server" Style="margin-bottom: 2px;" onKeypress="return isNumberKey(event);" MaxLength="50" oncontextmenu="return false;" onpaste="return false"></asp:TextBox>
                                            <%--<input type="text" id="txtContact" runat="server" placeholder="Contact Person" class="form-control" style="width:200px; height:30px; margin-bottom:2px;"/>--%>
                                        </td>
                                    </tr>
                                    <tr style="display:none">
                                        <td class="auto-style19">
                                            Taping Or Binding Cloth </td>
                                        <td class="auto-style7">
                                            <span class="auto-style22">/Run M.</span></td>
                                        <td class="auto-style1" colspan="2">
                                            <asp:TextBox ID="txtTaping" runat="server" Style="margin-bottom: 2px;" onKeypress="return isNumberKey(event);" MaxLength="50" oncontextmenu="return false;" onpaste="return false"></asp:TextBox>
                                            <%--</form>--%>
                                        </td>
                                    </tr>
                                    <tr style="display:none">
                                        <td class="auto-style19">
                                            Packing </td>
                                        <td class="auto-style35">
                                            in %</td>
                                        <td class="auto-style1" colspan="2">
                                            <asp:TextBox ID="txtPacking" runat="server" Style="margin-bottom: 2px;" onKeypress="return isNumberKey(event);" MaxLength="50" oncontextmenu="return false;" onpaste="return false"></asp:TextBox>
                                            <%-- <asp:BoundField DataField="Acode" HeaderText="Code" ReadOnly="True">
                                <HeaderStyle Width="70px" />
                                    <ItemStyle Width="70px" />
                                    </asp:BoundField>
                                <asp:BoundField DataField="Aname" HeaderText="Party Name" ReadOnly="True">
                                    <HeaderStyle Width="500px" />
                                    <ItemStyle Width="500px" />
                                    </asp:BoundField>--%>
                                        </td>
                                    </tr>
                                    <tr style="display:none">
                                        <td class="auto-style19">
                                            Profit Margin </td>
                                        <td class="auto-style36">
                                            in %</td>
                                        <td class="auto-style1" colspan="2">
                                            <asp:TextBox ID="txtProfit" runat="server" Style="margin-bottom: 2px;" onKeypress="return isNumberKey(event);" MaxLength="100" oncontextmenu="return false;" onpaste="return false"></asp:TextBox>
                                            <%--</form>--%>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <!-- /.box-body -->
                        <%--<button type="submit" class="btn btn-info" style="width:100px;" runat="server" id="hffield"   ></button>--%>
                    </div>
                </div>

                <div class="col-md-6" style="display:none">
                    <div>

                        <%--<button type="submit" class="btn btn-info" style="width:100px;" runat="server" id="edmode"   ></button>--%>
                        <div class="box-body">
                            <div class="form-group">
                                <table>
                                    <tr>
                                        <td class="auto-style30">
                                            Freight </td>
                                        <td class="auto-style26">
                                            in %</td>
                                        <td class="auto-style5">
                                            <asp:TextBox ID="txtFreight" runat="server" Style="margin-bottom: 2px" onKeypress="return isNumberKey(event);" MaxLength="50" oncontextmenu="return false;" onpaste="return false"></asp:TextBox>
                                            <%--</form>--%>
                                        </td>



                                    </tr>
                                    <tr>
                                        <td class="auto-style24">
                                            Payment Terms</td>
                                        <td class="auto-style12">
                                            in %</td>
                                        <td class="auto-style13">
                                            <asp:TextBox ID="txtPymt" runat="server" Style="margin-bottom: 2px" onKeypress="return isNumberKey(event);" MaxLength="150" oncontextmenu="return false;" onpaste="return false"></asp:TextBox>
                                            <%--<button type="submit" class="btn btn-info" style="width:100px;" runat="server" id="hffield"   ></button>--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style30">
                                            Excises </td>
                                        <td class="auto-style27">
                                            in %</td>
                                        <td class="auto-style1">
                                            <asp:TextBox ID="txtExcise" runat="server" Style="margin-bottom: 2px" onKeypress="return isNumberKey(event);" MaxLength="175" oncontextmenu="return false;" onpaste="return false"></asp:TextBox>
                                            <%--<button type="submit" class="btn btn-info" style="width:100px;" runat="server" id="edmode"   ></button>--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style30">
                                            Sales Tax </td>
                                        <td class="auto-style27">
                                            in %</td>
                                        <td class="auto-style1">
                                            <asp:TextBox ID="txtSales" runat="server" Style="margin-bottom: 2px" onKeypress="return isNumberKey(event);" MaxLength="50" oncontextmenu="return false;" onpaste="return false"></asp:TextBox>
                                            <%--<input type="text" id="txtTin" runat="server" placeholder="Sales Tax/Tin No" class="form-control" style="width:100px; height:30px; margin-bottom:2px;"/>--%>               
                                        </td>


                                    </tr>
                                    <tr>
                                        <td class="auto-style30">
                                            Minimum Order Quantity</td>
                                        <td class="auto-style27">
                                            &nbsp;</td>
                                        <td class="auto-style1">
                                            <asp:TextBox ID="txtMinimumQty" runat="server" Style="margin-bottom: 2px" onKeypress="return isNumberKey(event);" MaxLength="15" oncontextmenu="return false;" onpaste="return false"></asp:TextBox>
                                            <%--<input type="text" id="txtTin" runat="server" placeholder="Sales Tax/Tin No" class="form-control" style="width:100px; height:30px; margin-bottom:2px;"/>--%>               
                                        </td>


                                    </tr>

                                   
                                </table>
                            </div>
                        </div>
                        <!-- /.box-body -->
                        <%--</form>--%>
                    </div>
                </div>
                <div class="col-md-12">
                    <div>

                        <%--<input type="text" id="txtContact" runat="server" placeholder="Contact Person" class="form-control" style="width:200px; height:30px; margin-bottom:2px;"/>--%>
                        <div class="box-body">
                            <div class="form-group">
                                <div id="order_details_grid" style="height:250px; max-height:250px; max-width:1290px; overflow:auto; box-shadow:0 2px 4px rgba(127,127,127,.3);box-shadow:inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                <table>
                                   <tr>
                                       <td colspan="4">
                                            <asp:GridView ID="sg1" runat="server" Width="100%" AutoGenerateColumns="False"
                                                onrowcommand="sg1_RowCommand" onrowdatabound="sg1_RowDataBound"
                                style="font-size:smaller;"  CssClass="table table-bordered table-hover dataTable" >        
                                <Columns>
                                <asp:TemplateField>
                                <HeaderTemplate>A</HeaderTemplate>
                                <ItemTemplate>
                                <asp:ImageButton ID="btnadd" runat="server" CommandName="Add" ImageAlign="Middle" ImageUrl="~/images/Btn_addn.png" Width="20px" ToolTip="Insert Item" />
                                    </ItemTemplate>
                                <ItemStyle Width="11px" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                <HeaderTemplate>D</HeaderTemplate>
                                <ItemTemplate>
                                <asp:ImageButton ID="btnrmv" runat="server" CommandName="Rmv" ImageUrl="~/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Item" />
                                    </ItemTemplate>
                                <ItemStyle Width="11px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="srno" HeaderText="Srno" ReadOnly="True" >
                                    <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Icode" HeaderText="Item Code" ReadOnly="True">
                                    <HeaderStyle Width="100px" />
                                    <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Iname" HeaderText="Item Name" ReadOnly="True">
                                    <HeaderStyle Width="1000px" />
                                    <ItemStyle Width="1000px" />
                                    </asp:BoundField>
                                   <asp:BoundField DataField="Acode" HeaderText="Party Code" ReadOnly="True">
                                    <HeaderStyle Width="100px" />
                                    <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                     <asp:BoundField DataField="Aname" HeaderText="Party Name" ReadOnly="True">
                                    <HeaderStyle Width="1000px" />
                                    <ItemStyle Width="1000px" />
                                    </asp:BoundField>  
                                <asp:TemplateField>
                                <HeaderTemplate>Rate</HeaderTemplate>
                                <ItemTemplate>
                                <asp:TextBox ID="txtCol16" runat="server" Width="70px" Text='<%#Eval("Col16") %>'  MaxLength="25" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" style="text-align:right" ReadOnly="true" ></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                <HeaderTemplate>New Rate</HeaderTemplate>
                                <ItemTemplate>
                                <asp:TextBox ID="txtCol17" runat="server" Width="70px" Text='<%#Eval("Col17") %>'  MaxLength="25" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" style="text-align:right" ></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                     <asp:TemplateField>
                                <HeaderTemplate>Costing(Total)</HeaderTemplate>
                                <ItemTemplate>
                                <asp:TextBox ID="txtCost" runat="server" Width="70px" Text='<%#Eval("Costing") %>'  MaxLength="25" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" style="text-align:right" ReadOnly="true" ></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                     <asp:BoundField DataField="Hidden" HeaderText="Hidden" ReadOnly="True">
                                    <HeaderStyle Width="1000px" />
                                    <ItemStyle Width="1000px" />
                                    </asp:BoundField>
                                            </Columns>
                                <HeaderStyle BackColor="#1797c0" ForeColor="White" Height="20px" 
                                                CssClass="GridviewScrollHeader" Font-Bold="True"/>
                                            </asp:GridView>
                                       </td>
                                   </tr>
                                </table>
                            </div>
                        </div>
                        </div> 
                        <!-- /.box-body -->
                        <%--</form>--%>
                    </div>
                </div>
                    
            </div>
        </section>
    </div>
   <asp:Button ID="btnhideF" runat="server" onclick="btnhideF_Click" style="display:none" />
<asp:Button ID="btnhideF_s" runat="server" OnClick="btnhideF_s_Click" style="display:none" />
     <%--<button type="submit" class="btn btn-info" style="width:100px;" runat="server" id="hffield"   ></button>--%>
    <asp:HiddenField ID="hf1" runat="server" />
    <asp:HiddenField ID="hffield" runat="server" />
    <asp:HiddenField ID="edmode" runat="server" />
    <asp:HiddenField ID="popselected" runat="server" />
    <%--<button type="submit" class="btn btn-info" style="width:100px;" runat="server" id="edmode"   ></button>--%>
</asp:Content>