<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="Web_mach_mast" CodeFile="Web_mach_mast.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../tej-base/Scripts/gridviewScroll.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            //gridviewScroll('#<%=sg5.ClientID%>', gridDiv, 1, 1);
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

            function fill_zero(val) { if (isNaN(val)) return 0; if (isFinite(val)) return val; }
    </script>

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
                        <button type="submit" id="btnprint" visible="false" class="btn btn-info" style="width: 100px;" runat="server" accesskey="P" onserverclick="btnprint_ServerClick"><u>P</u>rint</button>
                        <button type="submit" id="btndel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="l" onserverclick="btndel_ServerClick">De<u>l</u>ete</button>
                        <button type="submit" id="btnlist" class="btn btn-info" style="width: 100px;" runat="server" accesskey="t" onserverclick="btnlist_ServerClick">Lis<u>t</u></button>
                        <button type="submit" id="btncancel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="c" onserverclick="btncancel_ServerClick"><u>C</u>ancel</button>
                        <button type="submit" id="btnexit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="X" onserverclick="btnexit_ServerClick">E<u>x</u>it</button>
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
                                <label id="Label1" runat="server" class="col-sm-3 control-label" title="lbl1">Opt_No</label>
                                <div class="col-sm-3">
                                    <input id="txtvchnum" type="text" class="form-control" runat="server" placeholder=" " readonly="readonly" />
                                </div>    
                                <label id="Label8" runat="server" class="col-sm-3 control-label" title="lbl1">Opt_Date</label>
                                <div class="col-sm-3">
                                    <input id="txtvchdate" type="text" class="form-control" runat="server" placeholder=" " readonly="readonly" />
                                </div>                           
                            </div>                    

                            <div class="form-group" style="display:none;">
                                <label id="Label5" runat="server" class="col-sm-4 control-label" title="lbl1">Name</label>
                                <div class="col-sm-8">
                                    <input id="txtlbl2" type="text" class="form-control" maxlength="120" runat="server" placeholder="Name" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="lbl3" runat="server" class="col-sm-2 control-label" title="lbl1">Code</label>
                                <div class="col-sm-1" id="divPersonName" runat="server">
                                    <asp:ImageButton ID="btnmachine" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnmachine_Click" />
                                </div>
                                   <div class="col-sm-2">
                                     <%--<asp:TextBox ID="txtcode" ReadOnly="true" runat="server" CssClass="form-control" Width="100%" Height="28px"  placeholder=" " maxlength="10"></asp:TextBox>--%>
                                         <input id="txtcode" type="text" readonly="readonly" class="form-control" runat="server" placeholder=" " />
                                </div>
                                <div class="col-sm-7">                                    
                                    <%--<asp:TextBox ID="txtname" ReadOnly="true" runat="server" CssClass="form-control" Width="100%" Height="28px"   placeholder=" " maxlength="10" ></asp:TextBox>--%>
                                     <input id="txtname" type="text" readonly="readonly" class="form-control" runat="server" placeholder=" " />
                                </div>
                            </div>

                            <div class="form-group" style="display:none;">
                                <label id="lbl4" runat="server" class="col-sm-3 control-label" title="lbl1">Sch Code</label>
                                <div class="col-sm-1" id="div1" runat="server">
                                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnactg_Click" />
                                </div>
                                <div class="col-sm-8">
                                    <input id="txtlbl4" type="text" class="form-control" runat="server" placeholder=" " maxlength="6" />
                                </div>
                            </div>

                                <div class="form-group">
                                <label id="Label9" runat="server" class="col-sm-3 control-label" title="lbl1">Machine_Cost</label>
                                <div class="col-sm-3">                               
                                    <%-- <asp:TextBox  id="txtmch_cost" type="text" class="form-control" BackColor="LightBlue" maxlength="10" onkeypress="return isDecimalKey(event)" runat="server" placeholder=" "></asp:TextBox>--%>
                                     <input  id="txtmch_cost" type="text" class="form-control" style="background-color:lightblue;"  maxlength="10" onkeypress="return isDecimalKey(event)" runat="server" placeholder=" " />
                                </div>
                                     <label id="Label22" runat="server" class="col-sm-3 control-label" title="lbl1">Year_Considered<span style="font-size:x-small;">(years)</span></label>
                                <div class="col-sm-3">
                                    <input id="txt_yr_considered" type="text" class="form-control" onkeyup="cal()"  style="background-color:lightblue;" maxlength="8" onkeypress="return isDecimalKey(event)" runat="server" placeholder=" " />
                                </div>
                            </div>

                        
                         
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">                                
                           
                                   <div class="form-group">
                                <label id="Label10" runat="server" class="col-sm-3 control-label" title="lbl1">Workg_Hr_Pday <span style="font-size:x-small;">(hrs)</span></label>
                                <div class="col-sm-3">
                                    <input id="txt_wrk_pday" type="text" class="form-control" onkeyup="cal()" style="background-color:lightblue;" maxlength="8" onkeypress="return isDecimalKey(event)" runat="server" placeholder=" " />
                                </div>
                                   <label id="Label11" runat="server" class="col-sm-3 control-label" title="lbl1">Days_worked/month<span style="font-size:x-small;">(Days)</span></label>
                                <div class="col-sm-3">
                                    <input id="txt_day_wrked_pm" type="text" class="form-control" onkeyup="cal()" style="background-color:lightblue;"  maxlength="8" onkeypress="return isDecimalKey(event)" runat="server" placeholder=" " />
                                </div>
                            </div>
                                 <div class="form-group">
                                <label id="Label12" runat="server" class="col-sm-3 control-label" title="lbl1">Total_mon/Year<span style="font-size:x-small;">(months)</span></label>
                                <div class="col-sm-3">
                                    <input id="txt_tot_mth_pyear" type="text" class="form-control" onkeyup="cal()" style="background-color:lightblue;"  maxlength="8" onkeypress="return isDecimalKey(event)" runat="server" placeholder=" " />
                                </div>
                                   <label id="Label13" runat="server" class="col-sm-3 control-label" title="lbl1">Tot_hrs<span style="font-size:x-small;">(hr)</span></label>
                                <div class="col-sm-3">
                                    <input id="txttot_hrs" type="text" class="form-control" style="width:100%" readonly="readonly" runat="server" placeholder=" " />
                                </div>                                     
                            </div>

                            <div class="form-group">
                                 <label id="Label14" runat="server" class="col-sm-3 control-label" title="lbl1">Machine_rate/hr</label>
                                <div class="col-sm-9">
                                    <input id="txtmch_rate_ph" type="text" class="form-control" readonly="readonly" maxlength="8" style="width:130px"  onkeypress="return isDecimalKey(event)" runat="server" placeholder=" " />
                                </div>
                            </div>

                                                 
                           

                        </div>
                    </div>
                </div>

                   <div class="col-md-12">
                    <div>
                        <div class="box-body">  
                             <label id="Label25" runat="server" class="col-sm-3 control-label" title="lbl1">Machine Running Time</label>
                            
                        </div>
                    </div>
                </div>
                    <div class="col-md-6">
                    <div>
                        <div class="box-body">  
                              <div class="form-group">
                               
                                   <label id="Label16" runat="server" class="col-sm-4 control-label" title="lbl1">Operator_Salary</label>
                                <div class="col-sm-2">
                                    <input id="txtoper_sal" type="text" class="form-control" onkeyup="cal()"  style="background-color:lightblue;"  maxlength="8" onkeypress="return isDecimalKey(event)" runat="server" placeholder=" " />
                                </div>
                                 
                                   <label id="Label17" runat="server" class="col-sm-4 control-label" title="lbl1">Operator_Salary/hr</label>
                                <div class="col-sm-2">
                                    <input id="txtop_Sal_phr" type="text" class="form-control" readonly="readonly" maxlength="8" onkeypress="return isDecimalKey(event)" runat="server" placeholder=" " />
                                </div> 
                            </div>       

                              <div class="form-group">
                            <label id="Label15" runat="server" class="col-sm-4 control-label" title="lbl1">No_of_Impr./Mnt<span style="font-size:x-small;">(Imp/Mnt)</span></label>   <%--<span style="font-size:small;">(a)</span>--%>
                                <div class="col-sm-2">
                                    <input id="txtno_of_impresion_mnt" type="text" class="form-control" style="background-color:lightblue;"  onkeyup="cal()" maxlength="8" onkeypress="return isDecimalKey(event)" runat="server" placeholder=" " />
                                </div> 
                                   <label id="Label18" runat="server" class="col-sm-4 control-label" title="lbl1">Max_R.Mtr_In_1hr(A)<span style="font-size:x-small;">(mtr/mnt)</span></label>  <%--<span style="font-size:xx-small;">(b)</span>--%>
                                <div class="col-sm-2">
                                    <input id="txtmax_runn_mtr_A" type="text" onkeyup="cal()" class="form-control" maxlength="8" style="background-color:lightblue;"  onkeypress="return isDecimalKey(event)" runat="server" placeholder=" " />
                                </div>
                              </div>

                             <div class="form-group">                               
                                  <label id="Label23" runat="server" class="col-sm-4 control-label" title="lbl1">Max_R.Mtr_In_1hr(B)<span style="font-size:x-small;">(mtr/mnt)</span></label>  <%-- <span style="font-size:xx-small;">(b*a)</span>--%>
                                <div class="col-sm-2">
                                    <input id="txtmax_runn_mtr_hr" type="text" readonly="readonly" class="form-control" maxlength="8" onkeypress="return isDecimalKey(event)" onkeyup="cal()" runat="server" placeholder=" " />
                                </div>
                                  <label id="Label20" runat="server" class="col-sm-4 control-label" title="lbl1">Setting_Time<span style="font-size:x-small;">(mnts)</span></label>
                                <div class="col-sm-2">
                                    <input id="txtsetting_tym" type="text" class="form-control" onkeyup="cal()" style="background-color:lightblue;"  maxlength="8" onkeypress="return isDecimalKey(event)" runat="server" placeholder="  " />
                                </div>  
                            </div>

                            <div class="form-group" style="display:none;">
                                   <label id="Label19" runat="server" class="col-sm-4 control-label" title="lbl1">Time_for_the_job<span style="font-size:x-small;">(mnts)</span></label>
                                <div class="col-sm-2">
                                    <input id="txtjob_time" type="text" onkeyup="cal()" class="form-control" style="background-color:lightblue;"  maxlength="8" onkeypress="return isDecimalKey(event)" runat="server" placeholder=" " />
                                </div>

                                 <label id="Label21" runat="server" class="col-sm-4 control-label" title="lbl1">Total_Time_for_the_job<span style="font-size:x-small;">(hrs)</span></label>
                                <div class="col-sm-2">
                                    <input id="txttot_job_time" type="text" readonly="readonly" class="form-control" maxlength="8" onkeypress="return isDecimalKey(event)" onkeyup="cal()" runat="server" placeholder=" " />
                                </div>
                                                                
                            </div>

                                                     
                            <div class="form-group"  style="display:none;">
                                <asp:Label ID="Label2" runat="server" CssClass="col-sm-4 control-label" title="lbl1" Text="Parameter_1"></asp:Label>
                                <div class="col-sm-8">
                                    <input id="txtlbl5" type="text" class="form-control" runat="server" onkeypress="return isDecimalKey(event)"  placeholder="Parameter 1" />
                                </div>
                            </div>

                            <div class="form-group"  style="display:none;">
                                <asp:Label ID="Label3" runat="server" CssClass="col-sm-4 control-label" title="lbl1" Text="Parameter_2"></asp:Label>
                                <div class="col-sm-8">
                                    <input id="txtlbl6" type="text" class="form-control" runat="server" placeholder="Parameter 2" />
                                </div>
                            </div>

                            <div class="form-group"  style="display:none;">
                                <asp:Label ID="Label4" runat="server" CssClass="col-sm-4 control-label" title="lbl1" Text="Parameter_3"></asp:Label>
                                <div class="col-sm-8">
                                    <input id="txtlbl7" type="text" class="form-control" runat="server" placeholder="Parameter 3" />
                                </div>
                            </div>

                            <div class="form-group"  style="display:none;">
                                <label id="Label6" runat="server" class="col-sm-4 control-label" title="lbl1">Parameter_4</label>
                                <div class="col-sm-8">
                                    <input id="txtlbl8" type="text" class="form-control" runat="server" placeholder="Parameter 4" />
                                </div>
                            </div>

                            <div class="form-group"  style="display:none;">
                                <label id="Label7" runat="server" class="col-sm-4 control-label" title="lbl1">Parameter_5</label>
                                <div class="col-sm-8">
                                    <input id="txtlbl9" type="text" class="form-control" runat="server" placeholder="Parameter 5" />
                                </div>
                            </div>

                              </div>
                    </div>
                </div>

                 <div class="col-md-6">
                    <div>
                        <div class="box-body">  
                              <div class="form-group">                                 
                                 <label id="Label24" runat="server" class="col-sm-3 control-label" title="lbl1">Machine_Cost_for_the_job</label>
                                <div class="col-sm-3">
                                    <input id="mch_Cost" type="text" readonly="readonly" class="form-control" maxlength="8"  onkeyup="cal()" onkeypress="return isDecimalKey(event)" runat="server" placeholder=" " />
                                </div>
                                    <label id="Label26" runat="server" class="col-sm-3 control-label" title="lbl1">Tot_Electricity_Usage<span style="font-size:x-small;">(Amt)</span></label>
                                <div class="col-sm-3">
                                    <input id="txt_elect_use" type="text" class="form-control" style="background-color:lightblue;"  maxlength="8" onkeyup="cal()" onkeypress="return isDecimalKey(event)" runat="server" placeholder=" " />
                                </div>
                                </div>

                               <div class="form-group">                                 
                                 <label id="Label27" runat="server" class="col-sm-3 control-label" title="lbl1">Electric_chg_for_1_hr<span style="font-size:x-small;">(Amt)</span></label>
                                <div class="col-sm-3">
                                    <input id="txt_elec_charge" type="text" readonly="readonly" class="form-control" maxlength="8" onkeypress="return isDecimalKey(event)" runat="server" placeholder=" " />
                                </div>
                                    <label id="Label29" runat="server" class="col-sm-3 control-label" title="lbl1">Total_Elec_Chg_for_job</label>
                                <div class="col-sm-3">
                                    <input id="txt_tot_elec_chg" type="text" readonly="readonly" class="form-control" maxlength="8" onkeypress="return isDecimalKey(event)" runat="server" placeholder=" " />
                                </div>
                                </div>

                             <div class="form-group">         
                                  <label id="Label28" runat="server" class="col-sm-3 control-label" title="lbl1">Total_Machine_Cost</label>
                                <div class="col-sm-3">
                                    <input id="txttot_mch_cost" type="text" readonly="readonly" class="form-control" maxlength="8" onkeypress="return isDecimalKey(event)"  runat="server" placeholder=" " />
                                </div>
                                 </div>
                                </div>

                    </div>
                </div>


                <section class="col-lg-12 connectedSortable" id="AllTabs" runat="server">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">
                                <li><a href="#DescTab" id="tab1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab" style="font-size:15px">List</a></li>
                                <li><a href="#DescTab2" id="tab2" runat="server" aria-controls="DescTab2" role="tab" data-toggle="tab">Item Details</a></li>
                                <li><a href="#DescTab3" id="tab3" runat="server" aria-controls="DescTab3" role="tab" data-toggle="tab">Comm.Terms</a></li>
                                <li><a href="#DescTab4" id="tab4" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Addl.Terms</a></li>
                                <li><a href="#DescTab5" id="tab5" runat="server" aria-controls="DescTab5" role="tab" data-toggle="tab">Delv.Sch</a></li>
                                <li><a href="#DescTab6" id="tab6" runat="server" aria-controls="DescTab6" role="tab" data-toggle="tab">Inv.Dtl</a></li>

                            </ul>

                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane active" id="DescTab">
                                    <div class="lbBody" style="height: 250px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                        <asp:GridView ID="sg5" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="1200px" Font-Size="13px">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        </asp:GridView>

                                        <asp:GridView ID="sg4" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="1200px" Font-Size="13px"
                                            AutoGenerateColumns="False" OnRowDataBound="sg4_RowDataBound"
                                            OnRowCommand="sg4_RowCommand" Visible="false">
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
                                                    <HeaderTemplate>Type</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg4_t1" runat="server" Text='<%#Eval("sg4_t1") %>' Width="100%" ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Name</HeaderTemplate>
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
                                    <div class="lbBody" id="gridDiv" style="color: White; height: 250px; overflow: hidden; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
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
                                                <%--                                            <asp:TemplateField>
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
                                                </asp:TemplateField>--%>
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
                                    <div class="lbBody" style="height: 250px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
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
                                    <div class="lbBody" style="height: 250px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
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
                                    <div class="lbBody" style="height: 250px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
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
                                    <div class="lbBody" style="height: 250px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
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
    <asp:HiddenField ID="doc_GST" runat="server" />
    <asp:HiddenField ID="edmode" runat="server" />
    <asp:HiddenField ID="hf1" runat="server" />
    <asp:HiddenField ID="hfGridView1SV" runat="server" />
    <asp:HiddenField ID="hfGridView1SH" runat="server" />
       <script type="text/javascript">

           function cal() {
               var ff12 = 0; var ff13 = 0; var ff14 = 0; ff15 = 0; ff16 = 0; var ff17 = 0; var ff18 = 0; var ff19 = 0; var ff20 = 0; var ff21 = 0;
               var ff22 = 0; var ff23 = 0; var ff24 = 0; var ff25 = 0; var ff26 = 0; var ff27 = 0; var ff28 = 0; var ff29 = 0; var ff30 = 0;
               var elc_chg = 0; var elc_use = 0; var mch_cost = 0; var tot_mch_cost = 0; var tot_elc_chg = 0;var elec=0;
               ff12 = fill_zero(document.getElementById("ContentPlaceHolder1_txt_yr_considered").value * 1);
               ff13 = fill_zero(document.getElementById("ContentPlaceHolder1_txt_wrk_pday").value * 1);
               ff14 = fill_zero(document.getElementById("ContentPlaceHolder1_txt_day_wrked_pm").value * 1);
               ff15 = fill_zero(document.getElementById("ContentPlaceHolder1_txt_tot_mth_pyear").value * 1);
               ff16 = (ff12 * 1) * (ff13 * 1) * (ff14 * 1) * (ff15*1);
               document.getElementById('ContentPlaceHolder1_txttot_hrs').value = (ff16 * 1).toFixed(2);
               /////================machine rate formula               
               ff17 = fill_zero(document.getElementById("ContentPlaceHolder1_txtmch_cost").value * 1);
               ff18 = (ff17 * 1) / (ff16 * 1);
               document.getElementById('ContentPlaceHolder1_txtmch_rate_ph').value = (ff18 * 1).toFixed(6);
               ///////==================operator salary in hr  
                ff19 = fill_zero(document.getElementById("ContentPlaceHolder1_txtoper_sal").value * 1);              
                ff20 = (ff19 * 1) / ((ff13 * 1) * (ff14 * 1));
               document.getElementById('ContentPlaceHolder1_txtop_Sal_phr').value = (ff20 * 1).toFixed(6);
               ///============time for the job
               //isme formula hai/..........b30/b104..but b30 costing part ka field hai .so abi is field ko user entry kiya hai and jab costing form per 
               //work hoga tab Total Running meter used ko b104 se divide kr denge
               //==============total time for the job       
               ff21 = fill_zero(document.getElementById("ContentPlaceHolder1_txtjob_time").value * 1);
               ff22 = fill_zero(document.getElementById("ContentPlaceHolder1_txtsetting_tym").value * 1);             
               //////=========new formula======txttot_job_time = txtjob_time + txtsetting_tym
               ff23 = (ff21 * 1) + (ff22 * 1);
               document.getElementById('ContentPlaceHolder1_txttot_job_time').value = (ff23 * 1).toFixed(2);
               //==================================================================
               /////max running mtr formula for B      
               ff24 = fill_zero(document.getElementById("ContentPlaceHolder1_txtmax_runn_mtr_A").value * 1);
               ff25 = fill_zero(document.getElementById("ContentPlaceHolder1_txtno_of_impresion_mnt").value * 1);
               ff26 = (ff24 * 1) * (ff25 * 1);
               document.getElementById('ContentPlaceHolder1_txtmax_runn_mtr_hr').value = (ff26 * 1).toFixed(2);
               //==================================================================
               ////=============for final machine cost      mch_Cost              
               ff27 = fill_zero(document.getElementById("ContentPlaceHolder1_txttot_job_time").value * 1);
               ff28 = fill_zero(document.getElementById("ContentPlaceHolder1_txtmch_rate_ph").value * 1);
               ff29 = fill_zero(document.getElementById("ContentPlaceHolder1_txtop_Sal_phr").value * 1);             
               ff30 = (((ff27 * 1) / 60) * (ff28 * 1));
               document.getElementById('ContentPlaceHolder1_mch_Cost').value = (ff30 * 1).toFixed(2);
               //==================================================================              
               elec = fill_zero(document.getElementById("ContentPlaceHolder1_txt_elec_charge").value * 1);
               tot_elc_chg = (elec * 1) * ((ff27 * 1) / 60);           
               document.getElementById('ContentPlaceHolder1_txt_tot_elec_chg').value = (tot_elc_chg * 1).toFixed(2);
               //==================================================================
               //formula for elcetricity charge for 1 hr                               
               elc_use = fill_zero(document.getElementById("ContentPlaceHolder1_txt_elect_use").value * 1);
               elc_chg = (elc_use * 1) / ((ff14 * 1) * (ff13 * 1));
               document.getElementById('ContentPlaceHolder1_txt_elec_charge').value = (elc_chg * 1).toFixed(2);
               //==================================================================
               //total machine cost           
               tot_mch_cost = (tot_elc_chg * 1) + (ff29 * 1) + (ff30 * 1); //and multiply it by no of passes which is coming on label costing form
               document.getElementById('ContentPlaceHolder1_txttot_mch_cost').value = (tot_mch_cost * 1).toFixed(2);
               
              
           }
           function fill_zero(val) { if (isNaN(val)) return 0; if (isFinite(val)) return val; }
    </script>

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
