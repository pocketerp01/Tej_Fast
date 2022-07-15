﻿<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="om_pap_cut" CodeFile="om_pap_cut.aspx.cs" %>
<%--  --%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../tej-base/Scripts/gridviewScroll.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            gridviewScroll('#<%=sg1.ClientID%>', gridDiv, 1, 5);

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
                    <td>
                        <asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label></td>
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
                                <asp:Label ID="lbl1" runat="server" Text="lbl1" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True"></asp:Label>
                                  <div class="col-sm-1">
                                    <asp:Label ID="lbl1a" runat="server" Text="TC" Style="width: 22px; float: right;" CssClass="col-sm-1 control-label"></asp:Label>
                                </div>
                                <div class="col-sm-2">
                                            <asp:TextBox ID="txtvchnum" runat="server" Width="100px" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                        </div>
                                <div class="col-sm-3">
                                            <asp:TextBox ID="txtvchdate" placeholder="Date" runat="server" Width="159px" CssClass="form-control"></asp:TextBox>
                                            <asp:CalendarExtender ID="txtvchdate_CalendarExtender" runat="server"
                                                Enabled="True" TargetControlID="txtvchdate"
                                                Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="Maskedit1" runat="server" Mask="99/99/9999"
                                                MaskType="Date" TargetControlID="txtvchdate" />
                                          </div>                                
                                        <div class="col-sm-4">                                           
                                        <asp:Button ID="btnProdReport" text="Paper_Cut_Prod_Report" runat="server" Height="34px" style="text-align:right" OnClick="btnProdReport_Click" Width="183px" Font-Size="14px" Font-Bold="True"/>
                                        </div>

                            </div>

                            <div class="form-group">
                                <asp:Label ID="lbl4" runat="server" Text="lbl4" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True"></asp:Label>

                                 <div class="col-sm-1">
                                    <asp:ImageButton ID="btnlbl4" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl4_Click" />
                                 </div>
                                  <div class="col-sm-2">
                                        <asp:TextBox ID="txtlbl4" runat="server" Width="100px" ReadOnly="true" CssClass="form-control"></asp:TextBox>  
                                        </div>
                                 <div class="col-sm-7">
                                           <asp:TextBox ID="txtlbl4a" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                        </div>
                                   
                            </div>

                            <div class="form-group">
                               <asp:Label ID="lbl7" runat="server" Text="lbl7" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True"></asp:Label>
                                <div class="col-sm-1">
                               <asp:ImageButton ID="btnlbl7" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl7_Click" />
                                </div>
                                <div class="col-sm-2">
                                 <asp:TextBox ID="txtlbl7" runat="server" Width="100px" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                        </div>
                                 
                                        <div class="col-sm-7">
                                        <asp:TextBox ID="txtlbl7a" runat="server"  ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                        </div>
                            </div>                           
                        </div></div></div>

                 <div class="col-md-6">
                    <div>
                        <div class="box-body">
                             <div class="form-group">
                                 <asp:Label ID="lbl2" runat="server" Text="lbl2" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True"></asp:Label>
                                 <div class="col-sm-4">
                                           <asp:TextBox ID="txtlbl2" runat="server" CssClass="form-control" Width="150px" ></asp:TextBox>
                                        </div>
                                 <asp:Label ID="lbl3" runat="server" Text="lbl3" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True"></asp:Label>
                                        <div class="col-sm-4">
                              <asp:TextBox ID="txtlbl3" runat="server" CssClass="form-control" Width="150px" ></asp:TextBox>
                                        </div>

                             </div>

                            <div class="form-group">
                                 <asp:Label ID="lbl5" runat="server" Text="lbl5" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True"></asp:Label>
                                    
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl5" runat="server" CssClass="form-control" Width="150px" ></asp:TextBox>
                                        </div>
                                   <asp:Label ID="lbl6" runat="server" Text="lbl6" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True"></asp:Label>
                                       <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl6" runat="server" CssClass="form-control" Width="150px"></asp:TextBox>
                                        </div>
                            </div>

                            <div class="form-group">                                                                  
                                        <div class="col-sm-4 hidden">
                                            <asp:TextBox ID="TextBox5" runat="server" CssClass="form-control" Width="150px"></asp:TextBox>
                                        </div>                                                                                                             
                                        <div class="col-sm-4 hidden" >
                                            <asp:TextBox ID="TextBox6" runat="server" CssClass="form-control" Width="150px"></asp:TextBox>
                                        </div>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="lbl9" runat="server" Text="lbl9" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True"></asp:Label>
                                   
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl9" runat="server"  EnableViewState="true" Width="150px" CssClass="form-control" ></asp:TextBox>
                                        </div>
                                 <asp:Label ID="lbl8" runat="server" Text="lbl8" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True"></asp:Label>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl8" runat="server"  EnableViewState="true" Width="150px" CssClass="form-control" ></asp:TextBox>
                                        </div>
                            </div>

                             <%--hidden fields   from here --%>
                            <div class="form-group">           <%--hidden fields--%>     
                                               <asp:Label ID="lbl101" runat="server" Text="lbl101" CssClass="col-sm-4 control-label" Font-Size="14px" Font-Bold="True"></asp:Label>
                                       <div class="col-sm-4 hidden">
                                <asp:ImageButton ID="btnlbl101" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" Visible="false" OnClick="btnlbl101_Click"/>
                                </div> 
                                 <div class="col-sm-4 hidden">
                                            <asp:TextBox ID="txtlbl101" runat="server" Width="80px" ReadOnly="true"></asp:TextBox>
                                        </div>  
                                                                            
                                        <div class="col-sm-4 hidden" >
                                             <asp:TextBox ID="txtlbl101a" runat="server" Width="350px" ReadOnly="true"></asp:TextBox>
                                        </div>
                            </div>        <%--hidden fields--%>
                           <div class="form-group">           <%--hidden fields--%>     
                                              <asp:Label ID="lbl102" runat="server" Text="lbl102" CssClass="col-sm-4 control-label" Font-Size="14px" Font-Bold="True"></asp:Label> 
                                       <div class="col-sm-4 hidden">
                                
                                </div> 
                                 <div class="col-sm-4 hidden">
                                <asp:TextBox ID="txtlbl102" runat="server"></asp:TextBox>
                                        </div>  
                                <asp:Label ID="lbl103" runat="server" Text="lbl103" CssClass="col-sm-4 control-label" Font-Size="14px" Font-Bold="True"></asp:Label>
                                            
                                        <div class="col-sm-4 hidden" >
                                 <asp:TextBox ID="txtlbl103" runat="server"></asp:TextBox>
                                        </div>
                            </div>          <%--hidden fields--%>  
                            <%--Upto here   --%>


                        </div></div></div>
        
                <section class="col-lg-12 connectedSortable">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">
                                <li><a href="#DescTab" id="tab1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab">Form Details</a></li>
                                <li><a href="#DescTab2" id="tab2" runat="server" aria-controls="DescTab2" role="tab" data-toggle="tab" style="visibility:hidden;">Other.Terms</a></li>
                                <li><a href="#DescTab3" id="tab3" runat="server" aria-controls="DescTab3" role="tab" data-toggle="tab">Addl.Terms</a></li>
                                <li><a href="#DescTab4" id="tab4" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Delv.Sch</a></li>
                                <li><a href="#DescTab5" id="tab5" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Inv.Dtl</a></li>
                            </ul>

                            <div class="tab-content" >
                                <div role="tabpanel" class="tab-pane active" id="DescTab">
                                    <div class="lbBody" id="gridDiv" style="color: White; height: 300px; overflow: hidden; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <asp:GridView ID="sg1" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="1500px" Font-Size ="13px" 
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
                                                <asp:BoundField DataField="sg1_f1" HeaderText="JobDT"  ItemStyle-Width="80px"/>
                                                <asp:BoundField DataField="sg1_f2" HeaderText="JobNo." ItemStyle-Width="80px"/>
                                                <asp:BoundField DataField="sg1_f3" HeaderText="ERPcode" ItemStyle-Width="80px"/>
                                                <asp:BoundField DataField="sg1_f4" HeaderText="Name"  ItemStyle-Width="80px"/>
                                                <asp:BoundField DataField="sg1_f5" HeaderText="Unit" ItemStyle-Width="80px" />
                                                <asp:BoundField DataField="sg1_f6" HeaderText="QtyIssue" />
                                                 
                                                <asp:TemplateField>
                                                            <HeaderTemplate>StartTime</HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="sg1_t1" runat="server" data-provide="timepicker" CssClass="timepicker" Text='<%#Eval("sg1_t1") %>' Width="100%" TextMode="Time"></asp:TextBox>                                                           
                                                                  </ItemTemplate>
                                                        </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>EndTime</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t2" runat="server" Text='<%#Eval("sg1_t2") %>' Width="100%" TextMode="Time"></asp:TextBox>
                                                   
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--    <asp:TemplateField>
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
                                                <asp:TemplateField ItemStyle-Width="110px">
                                                    <HeaderTemplate>QtyRcvd</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t3" runat="server" Text='<%#Eval("sg1_t3") %>'  Width="100%" onkeypress="return isDecimalKey(event)" maxlength="7"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="80px">
                                                    <HeaderTemplate>No.Of Cuts</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t4" runat="server" Text='<%#Eval("sg1_t4") %>'  Width="100%" onChange="calrejection()" onkeypress="return isDecimalKey(event)" maxlength="7" ></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>CutSheet</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t5" runat="server" Text='<%#Eval("sg1_t5") %>'  Width="100%" onChange="caltotalvalue()" onkeypress="return isDecimalKey(event)" maxlength="7" ></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>RejSheet</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t6" runat="server" Text='<%#Eval("sg1_t6") %>' Width="100%" onChange="caloksheet()" onkeypress="return isDecimalKey(event)" maxlength="7" ></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Remarks</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t7" runat="server" Text='<%#Eval("sg1_t7") %>' Width="100%" maxlength="100"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>OKSheet</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t8" runat="server" Text='<%#Eval("sg1_t8") %>' Width="100%" onChange="caltotalvalue1()" onkeypress="return isDecimalKey(event)" maxlength="7" ></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                               <%-- <asp:TemplateField>
                                                    <HeaderTemplate>TotalQuantity</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t9" runat="server" Text='<%#Eval("sg1_t9") %>' Width="100%" onkeyup="calvariance()" ></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>



                                                <asp:TemplateField>
                                                    <HeaderTemplate>VarianceQty</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t10" runat="server" Text='<%#Eval("sg1_t10") %>' Width="100%" ></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="80px">
                                                    <HeaderTemplate>sg1_t11</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t11" runat="server" Text='<%#Eval("sg1_t11") %>' Width="100%" onkeyup="calrejection()"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="80px">
                                                    <HeaderTemplate >sg1_t12</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t12" runat="server" Text='<%#Eval("sg1_t12") %>' Width="100%" onkeyup="calrejection()"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="80px">
                                                    <HeaderTemplate>sg1_t13</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t13" runat="server" Text='<%#Eval("sg1_t13") %>' Width="100%" onkeyup="calrejection()"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="80px">
                                                    <HeaderTemplate>sg1_t14</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t14" runat="server" Text='<%#Eval("sg1_t14") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%" onkeyup="calrejection()"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="100px">
                                                    <HeaderTemplate>sg1_t15</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t15" runat="server" Text='<%#Eval("sg1_t15") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%" onkeyup="calrejection()"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="80px">
                                                    <HeaderTemplate>sg1_t16</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t16" runat="server" Text='<%#Eval("sg1_t16") %>' Width="100%" onkeyup="calrejection()"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                  <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t17</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t17" runat="server" Text='<%#Eval("sg1_t17") %>' Width="100%" onkeyup="calrejection()"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                  <asp:TemplateField ItemStyle-Width="80px">
                                                    <HeaderTemplate>sg1_t18</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t18" runat="server" Text='<%#Eval("sg1_t18") %>' Width="100%" onkeyup="calrejection()"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                  <asp:TemplateField ItemStyle-Width="80px">
                                                    <HeaderTemplate>sg1_t19</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t19" runat="server" Text='<%#Eval("sg1_t19") %>' Width="100%" onkeyup="calrejection()"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                  <asp:TemplateField ItemStyle-Width="80px">
                                                    <HeaderTemplate>sg1_t20</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t20" runat="server" Text='<%#Eval("sg1_t20") %>' Width="100%" onkeyup="calrejection()"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                 <asp:TemplateField>
                                                    <HeaderTemplate>ICODE</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t21" runat="server" Text='<%#Eval("sg1_t21") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>OKFROMNG</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t22" runat="server" Text='<%#Eval("sg1_t22") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                  <asp:TemplateField>
                                                    <HeaderTemplate>REJFROM</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t23" runat="server" Text='<%#Eval("sg1_t23") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                  <asp:TemplateField>
                                                    <HeaderTemplate>DownTimeReason</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t24" runat="server" Text='<%#Eval("sg1_t24") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                  <asp:TemplateField>
                                                    <HeaderTemplate>Remarks</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t25" runat="server" Text='<%#Eval("sg1_t25") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                  <asp:TemplateField ItemStyle-Width="100px">
                                                    <HeaderTemplate>sg1_t26</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t26" runat="server" Text='<%#Eval("sg1_t26") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField ItemStyle-Width="110px">
                                                    <HeaderTemplate>sg1_t27</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t27" runat="server" Text='<%#Eval("sg1_t27") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField ItemStyle-Width="100px">
                                                    <HeaderTemplate>sg1_t28</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t28" runat="server" Text='<%#Eval("sg1_t28") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField ItemStyle-Width="180px">
                                                    <HeaderTemplate>sg1_t29</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t29" runat="server" Text='<%#Eval("sg1_t29") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField ItemStyle-Width="80px">
                                                    <HeaderTemplate>sg1_t30</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t30" runat="server" Text='<%#Eval("sg1_t30") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField ItemStyle-Width="80px">
                                                    <HeaderTemplate>sg1_t31</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t31" runat="server" Text='<%#Eval("sg1_t31") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField ItemStyle-Width="80px">
                                                    <HeaderTemplate>sg1_t32</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t32" runat="server" Text='<%#Eval("sg1_t32") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField ItemStyle-Width="100px">
                                                    <HeaderTemplate>sg1_t33</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t33" runat="server" Text='<%#Eval("sg1_t33") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField ItemStyle-Width="80px">
                                                    <HeaderTemplate>sg1_t34</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t34" runat="server" Text='<%#Eval("sg1_t34") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField ItemStyle-Width="80px">
                                                    <HeaderTemplate>sg1_t35</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t35" runat="server" Text='<%#Eval("sg1_t35") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField ItemStyle-Width="80px">
                                                    <HeaderTemplate>sg1_t36</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t36" runat="server" Text='<%#Eval("sg1_t36") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                --%>

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
                                    <div class="lbBody" style="height: 200px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl10" runat="server" Text="lbl10" CssClass="col-sm-2 control-label"></asp:Label></td>

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
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl11" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl12" runat="server" Text="lbl12" CssClass="col-sm-2 control-label"></asp:Label></td>

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
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl13" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl14" runat="server" Text="lbl14" CssClass="col-sm-2 control-label"></asp:Label></td>

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
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl15" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl16" runat="server" Text="lbl16" CssClass="col-sm-2 control-label"></asp:Label></td>

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
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl17" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl18" runat="server" Text="lbl18" CssClass="col-sm-2 control-label"></asp:Label></td>

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
                            <asp:TextBox ID="txtrmk" runat="server"  MaxLength="150" Width="99%" CssClass="form-control" TextMode="MultiLine" placeholder="Remarks"></asp:TextBox>
                        </div>
                    </div>
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
        function calrejection() {
            var rowTot = 0;
            var colTot = 0;
            var mul = 0;
            var total = 0;
            var grid = document.getElementById("<%= sg1.ClientID%>");
            for (var i = 0; i < grid.rows.length - 1; i++) {
                colTot = fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg1_sg1_t3_' + i).value));
                mul = fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg1_sg1_t4_' + i).value));

                //row total is total of total_qty field row wise
                rowTot = colTot * mul;
                document.getElementById('ContentPlaceHolder1_sg1_sg1_t5_' + i).value = fill_zero(rowTot);
                total += rowTot;
                document.getElementById('ContentPlaceHolder1_txtlbl8').value = fill_zero(total);
            }
        }
        function fill_zero(val) { if (isNaN(val)) return 0; if (isFinite(val)) return val; }

        function caloksheet() {
            var rowTot = 0;
            var colTot = 0;
            var min = 0;
            var total = 0;
            var grid = document.getElementById("<%= sg1.ClientID%>");
            for (var i = 0; i < grid.rows.length - 1; i++) {
                colTot = fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg1_sg1_t5_' + i).value));
                min = fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg1_sg1_t6_' + i).value));
                // row total is total of total_qty field row wise
                rowTot = colTot - min;
                document.getElementById('ContentPlaceHolder1_sg1_sg1_t8_' + i).value = fill_zero(rowTot);
                total += rowTot;
                document.getElementById('ContentPlaceHolder1_txtlbl9').value = fill_zero(total);
            }

        }
        function fill_zero(val) { if (isNaN(val)) return 0; if (isFinite(val)) return val; }

        function caltotalvalue() {
            var rowTot = 0;
            var colTot = 0;
            var min = 0;
            var grid = document.getElementById("<%= sg1.ClientID%>");
                for (var i = 0; i < grid.rows.length - 1; i++) {
                    colTot += fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg1_sg1_t5_' + i).value));
                    //row total is total of total_qty field row wise
                    document.getElementById('ContentPlaceHolder1_txtlbl8').value = fill_zero(colTot);
                }
            }
            function fill_zero(val) { if (isNaN(val)) return 0; if (isFinite(val)) return val; }
            function caltotalvalue1() {
                var rowTot = 0;
                var colTot = 0;
                var min = 0;
                var grid = document.getElementById("<%= sg1.ClientID%>");

            for (var i = 0; i < grid.rows.length - 1; i++) {

                colTot += fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg1_sg1_t8_' + i).value));
                document.getElementById('ContentPlaceHolder1_txtlbl9').value = fill_zero(colTot);

            }
        }

    </script>
  
    <asp:HiddenField ID="TabName" runat="server" />
</asp:Content>
