<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="om_vacation_req" CodeFile="om_vacation_req.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../tej-base/Scripts/gridviewScroll.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            gridviewScroll('#<%=sg1.ClientID%>', gridDiv, 1, 5);
            //calculateSum();
        });
        function gridviewScroll(gridId, gridDiv, headerFreeze, rowFreeze) {
            $(gridId).gridviewScroll({
                width: gridDiv.offsetWidth,
                height: gridDiv.offsetHeight,
                headerrowcount: headerFreeze,
                freezesize: rowFreeze,
                barhovercolor: "#3399FF",
                barcolor: "#3399FF",
                startVertical: $("#<%=hfGridView1SV.ClientID%>").val(),
                startHorizontal: $("#<%=hfGridView1SH.ClientID%>").val(),
                onScrollVertical: function (delta) {
                    $("#<%=hfGridView1SV.ClientID%>").val(delta);
                },
                onScrollHorizontal: function (delta) {
                    $("#<%=hfGridView1SH.ClientID%>").val(delta);
                }
            });
            }
<%--            function calculateSum($element) {

                var toth1 = 0;
                var toth2 = 0;
                var toth3 = 0;
                var goth_tot = 0;
                var row_bas = 0;
                var row_pack = 0;
                var row_frt = 0;
                var row_amtz = 0;
                var row_ed = 0;

                var row_tax = 0;
                var row_atax = 0;

                var row_tax_rt = 0;
                var row_atax_rt = 0;

                var grid_bas = 0;
                var grid_pack = 0;
                var grid_frt = 0;
                var grid_ed = 0;

                var grid_tax = 0;
                var grid_atax = 0;

                var grid_tot = 0;
                var addl_Tx_Mthd = 0;
                var gridSg1 = document.getElementById("<%= sg1.ClientID%>");
                addl_Tx_Mthd = document.getElementById('ContentPlaceHolder1_doc_addl').value

                for (var i = 0; i < gridSg1.rows.length - 1; i++) {

                    var qty = $("input[id*=sg1_t3]");
                    var rate = $("input[id*=sg1_t4]");
                    var disc_per = $("input[id*=sg1_t5]");
                    var disc_val = 0;

                    var pack_perc = $("input[id*=sg1_t6]");
                    var pack_peru = $("input[id*=sg1_t7]");
                    var frt_peru = $("input[id*=sg1_t8]");

                    var tool_Amtz = $("input[id*=sg1_t14]");

                    var ed_per = $("input[id*=sg1_t9]");
                    var ed_val = 0;
                    var tax_per = $("input[id*=sg1_t11]");
                    var atax_per = $("input[id*=sg1_t16]");

                    row_tax_rt = fill_zero(tax_per[i].value);
                    row_atax_rt = fill_zero(atax_per[i].value);

                    row_bas = fill_zero(qty[i].value) * (fill_zero(rate[i].value) * ((100 - fill_zero(disc_per[i].value)) / 100));
                    row_amtz = fill_zero(qty[i].value) * (fill_zero(tool_Amtz[i].value));

                    if (pack_perc[i].value > 0) {
                        row_pack = row_bas * ((fill_zero(pack_perc[i].value)) / 100);
                    }

                    else {
                        row_pack = fill_zero(qty[i].value) * (fill_zero(pack_peru[i].value));
                    }

                    row_frt = fill_zero(qty[i].value) * (fill_zero(frt_peru[i].value));

                    row_ed = (row_bas + row_pack + row_amtz) * ((fill_zero(ed_per[i].value)) / 100);

                    row_tax = ((row_bas + row_pack + row_ed) * ((fill_zero(row_tax_rt)) / 100));

                    if (addl_Tx_Mthd === "1") {
                        row_atax = ((row_tax) * ((fill_zero(row_atax_rt)) / 100));
                    }
                    else {
                        row_atax = ((row_bas + row_pack + row_ed) * ((fill_zero(row_atax_rt)) / 100));
                    }


                    grid_bas = grid_bas + row_bas;
                    grid_pack = grid_pack + row_pack;
                    grid_ed = grid_ed + row_ed;
                    grid_frt = grid_frt + row_frt;
                    grid_tax = grid_tax + row_tax;
                    grid_atax = grid_atax + row_atax;

                }

                document.getElementById('ContentPlaceHolder1_txtlbl20b').value = fill_zero(grid_frt).toFixed(3);

                toth1 = fill_zero(document.getElementById('ContentPlaceHolder1_txtlbl20b').value);
                toth2 = fill_zero(document.getElementById('ContentPlaceHolder1_txtlbl21b').value);
                toth3 = fill_zero(document.getElementById('ContentPlaceHolder1_txtlbl22b').value);

                goth_tot = fill_zero(toth1 * 1) + fill_zero(toth2 * 1) + fill_zero(toth3 * 1);
                grid_tot = fill_zero(grid_bas * 1) + fill_zero(grid_pack * 1) + fill_zero(grid_ed * 1) + fill_zero(grid_tax * 1) + fill_zero(grid_atax * 1) + fill_zero(goth_tot * 1);

                document.getElementById('ContentPlaceHolder1_txtlbl24').value = fill_zero(grid_bas).toFixed(3);
                document.getElementById('ContentPlaceHolder1_txtlbl25').value = fill_zero(grid_pack).toFixed(3);

                document.getElementById('ContentPlaceHolder1_txtlbl26').value = fill_zero(grid_ed).toFixed(3);
                document.getElementById('ContentPlaceHolder1_txtlbl28').value = fill_zero(grid_tax).toFixed(3);

                document.getElementById('ContentPlaceHolder1_txtlbl29').value = fill_zero(grid_atax).toFixed(3);
                document.getElementById('ContentPlaceHolder1_txtlbl30').value = fill_zero(goth_tot).toFixed(3);

                document.getElementById('ContentPlaceHolder1_txtlbl31').value = fill_zero(grid_tot).toFixed(3);
            }--%>
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
                    <td><asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label></td>
                    <td style="text-align:right">
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
                        <div class="box-body">
                            <div class="form-group">
                                <label id="Label1" runat="server" class="col-sm-2 control-label" title="lbl1">Vac_Req_No</label>
                                  <div class="col-sm-1">                      
                                    <asp:Label ID="lbl1a" runat="server" Text="TC" Style="width: 22px;" ></asp:Label>
                                      </div>                        
                                <div class="col-sm-9">
                                    <input id="txtvchnum" type="text" class="form-control" runat="server" readonly="readonly" />                                    
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label8" runat="server" class="col-sm-3 control-label" title="lbl1">Vac_Req_Dt</label>
                                <div class="col-sm-9">
                                    <input id="txtvchdate" type="text" class="form-control" runat="server" readonly="readonly" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label9" runat="server" class="col-sm-2 control-label" title="lbl1">Emp_Code</label>
                                <div class="col-sm-1" id="divCocd" runat="server">
                                    <asp:ImageButton ID="btnCocd" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnCocd_Click" />
                                </div>
                                <div class="col-sm-2">
                                    <input id="txtlbl4" type="text" class="form-control" runat="server" maxlength="4" readonly="true" />
                                </div>
                                <label id="Label14" runat="server" class="col-sm-2 control-label" title="lbl1">Emp_Name</label>
                                <div class="col-sm-5">
                                    <input id="txtlbl7" type="text" class="form-control" runat="server" maxlength="30" readonly="true" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label6" runat="server" class="col-sm-3 control-label" title="lbl1">Card_No</label>
                                <div class="col-sm-9">
                                    <input id="txtlbl8" type="text" class="form-control" runat="server" maxlength="30" readonly="true" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label7" runat="server" class="col-sm-3 control-label" title="lbl1">Reason</label>
                                <div class="col-sm-9">
                                    <select id="txtlbl9" runat="server" class="form-control">
                                        <option value="Official">OFFICIAL</option>
                                        <option value="Personal">PERSONAL</option>
                                        <option value="Family">FAMILY</option>
                                    </select>
                                </div>
                            </div>
                    </div>
                </div></div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">

                            <%--<div class="form-group">
                                <label id="Label7" runat="server" class="col-sm-3 control-label" title="lbl1">Reason</label>
                                <div class="col-sm-9">
                                    <select id="txtlbl9" runat="server" class="form-control">
                                        <option value="Official">Official</option>
                                        <option value="Personal">Personal</option>
                                        <option value="Family">Family</option>
                                    </select>
                                </div>
                            </div>--%>

                            <div class="form-group">
                                <label id="lvfromlb" runat="server" class="col-sm-3 control-label" title="lbl1">Vacation_Date</label>
                                <div class="col-sm-4">
                                    <input id="lvfrom" type="date" class="form-control"  onkeyup="cal()" runat="server"   maxlength="10" />
                                                              
                                </div>
                                
                                  <label id="Label2" runat="server" class="col-sm-2 control-label" title="lbl1">Vacation_Time</label>
                                <div class="col-sm-3">
                                    <input id="lblfromtime" type="time" class="form-control"  runat="server"  maxlength="10" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="lvuptolb" runat="server" class="col-sm-3 control-label" title="lbl1">Return_Date</label>
                                <div class="col-sm-4">
                                    <input id="lvupto" type="date" class="form-control" onkeyup="cal()" runat="server"  maxlength="10" />
                                
                                </div>

                                 <label id="Label10" runat="server" class="col-sm-2 control-label" title="lbl1">Return_Time</label>
                                <div class="col-sm-3">
                                    <input id="lbluptime" type="time" class="form-control"  runat="server"  maxlength="10" />
                                </div>
                            </div>

                             <div class="form-group">
                                <label id="Label5" runat="server" class="col-sm-3 control-label" title="lbl1">Total_Days</label>
                                <div class="col-sm-4">
                                    <input id="lbldays" class="form-control" readonly="true" runat="server"  maxlength="10" />                                
                                </div>

                                 <label id="Label15" runat="server" class="col-sm-2 control-label" title="lbl1">Time_In_Hrs</label>
                                <div class="col-sm-3">
                                    <input id="lblhrs"  class="form-control" readonly="true" placeholder="Hours : Minutes" runat="server"  maxlength="10" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label3" runat="server" class="col-sm-3 control-label" title="lbl1">Alt_Contact_Person</label>
                                <div class="col-sm-9">
                                    <input id="alt_cont_per" type="text" class="form-control" runat="server"  maxlength="20" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label4" runat="server" class="col-sm-3 control-label" title="lbl1">Alt_Contact_Number</label>
                                <div class="col-sm-9">
                                    <input id="alt_cont_no" type="text" class="form-control" onkeypress="return isDecimalKey(event)" runat="server"  maxlength="20" />
                                </div>
                            </div>


                        </div>
                    </div>
                </div>  
                
                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <label id="Label16" runat="server" class="col-sm-3 control-label" title="lbl1">Section</label>
                                  
                                <div class="col-sm-9">
                                    <input id="txtSectn" type="text" class="form-control" runat="server" />                                    
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label18" runat="server" class="col-sm-3 control-label" title="lbl1">Service_Year_No.</label>
                                  
                                <div class="col-sm-9">
                                    <input id="txtservcyr" type="text" class="form-control" runat="server" />                                    
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label20" runat="server" class="col-sm-3 control-label" title="lbl1">Airline_Name</label>
                                  
                                <div class="col-sm-9">
                                    <input id="txtairln" type="text" class="form-control" runat="server" />                                    
                                </div>
                            </div>

                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            
                            <div class="form-group">
                                <label id="Label21" runat="server" class="col-sm-3 control-label" title="lbl1">Dest_From</label>
                                <div class="col-sm-4">
                                    <input id="txtdesFrm" type="text" class="form-control" runat="server" />                                    
                                                              
                                </div>
                                
                                  <label id="Label22" runat="server" class="col-sm-2 control-label" title="lbl1">To</label>
                                <div class="col-sm-3">
                                    <input id="txtdesto" type="text" class="form-control" runat="server" />                                    
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label19" runat="server" class="col-sm-3 control-label" title="lbl1">Ticket_No</label>
                                  
                                <div class="col-sm-9">
                                    <input id="txttickts" type="text" class="form-control" runat="server" />                                    
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label17" runat="server" class="col-sm-3 control-label" title="lbl1">Leave_Add</label>
                                  
                                <div class="col-sm-9">
                                    <input id="txtleaveadd" type="text" class="form-control" runat="server" />                                    
                                </div>
                            </div>

                            


                        </div>
                    </div>
                </div>   
                             
                <div class="col-md-12">
                    <div>
                        <div class="box-body">
                            <asp:Label ID="lbltxtrmk" runat="server" Text="Remarks" CssClass="col-sm-2 control-label"></asp:Label>                            
                            <asp:TextBox ID="txtrmk" runat="server" Width="99%" TextMode="MultiLine" onkeyup="max_length(this,150)" ></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="col-md-12">
                    <div>
                        <div class="box-body">
                            <asp:Label ID="Label23" runat="server" Text="" CssClass="col-sm-4 control-label">Exit - Re - Entry Visa Required</asp:Label>
                            For the Employee &nbsp;<asp:CheckBox ID="chkvisaEmp" runat="server" />&nbsp;&nbsp;&nbsp;
                            For the Family &nbsp;<asp:CheckBox ID="chkvisafam" runat="server" />
                        </div>
                    </div>
                </div>

                <div class="col-md-12">
                    <div>
                        <div class="box-body">
                            <table>
                                <tr>
                                    <td>
                                        <asp:FileUpload ID="Attch" runat="server" Visible="true" onchange="submitFile()" /></td>
                                    <td>
                                        <asp:TextBox ID="txtAttch" runat="server"></asp:TextBox></td>
                                </tr>
                            </table>
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
                </div>


                <section class="col-lg-12 connectedSortable" id="AllTabs" runat="server">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">
                                <li><a href="#DescTab" id="tab1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab">UDF Data</a></li>
                                <li><a href="#DescTab2" id="tab2" runat="server" aria-controls="DescTab2" role="tab" data-toggle="tab">Item Details</a></li>
                                <li><a href="#DescTab3" id="tab3" runat="server" aria-controls="DescTab3" role="tab" data-toggle="tab">Comm.Terms</a></li>
                                <li><a href="#DescTab4" id="tab4" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Addl.Terms</a></li>
                                <li><a href="#DescTab5" id="tab5" runat="server" aria-controls="DescTab5" role="tab" data-toggle="tab">Delv.Sch</a></li>
                                <li><a href="#DescTab6" id="tab6" runat="server" aria-controls="DescTab6" role="tab" data-toggle="tab">Inv.Dtl</a></li>

                            </ul>

                            <div class="tab-content">


                                <div role="tabpanel" class="tab-pane active" id="DescTab">
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
                                            <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader" />
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


                                                <%--                                                        <asp:TemplateField>
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
                                            <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader" />
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
                                            <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader" />
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
                                            <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White"
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
    <asp:HiddenField ID="TabName" runat="server" />

       <script type="text/javascript">
           $(function () {
               var tabName = $("[id*=TabName]").val() != "" ? $("[id*=TabName]").val() : "DescTab";
               $('#Tabs a[href="#' + tabName + '"]').tab('show');
               $("#Tabs a").click(function () {
                   $("[id*=TabName]").val($(this).attr("href").replace("#", ""));
               });
           });

           function cal() {
               var lvfrom = new Date('ContentPlaceHolder1_lvfrom');
               var lvupto = new Date('ContentPlaceHolder1_lvupto');
               // time difference
               var timeDiff = Math.abs(lvupto.getTime() - lvfrom.getTime());
               // days difference
               var diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24));

               document.getElementById('ContentPlaceHolder1_lbldays').value = fill_zero(diffDays);
           }
           function fill_zero(val) { if (isNaN(val)) return 0; if (isFinite(val)) return val; }

           <%-- <script>
        var dateFirst = new Date("11/25/2017");
           var dateSecond = new Date("11/28/2017");

         
           var timeDiff = Math.abs(dateSecond.getTime() - dateFirst.getTime());

          

           // difference
           alert(diffDays);--%>
     

    </script>

</asp:Content>