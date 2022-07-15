<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="om_pay_data" CodeFile="om_pay_data.aspx.cs" %>
<%--<%@ Import Namespace="IdeaSparx.CoolControls.Web" %>--%>
<%@ Register TagPrefix="fin" namespace="IdeaSparx.CoolControls.Web" assembly="IdeaSparx.CoolControls.Web"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
   
    <script src="../tej-base/Scripts/gridviewScroll.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            var gridDiv = $("#gridDiv");
            debugger
            //gridviewScroll('#sg1', gridDiv, 1, 4);

        });
        function gridviewScroll(gridId, gridDiv, headerFreeze, rowFreeze) {
            $(gridId).gridviewScroll({
                width: gridDiv.width(),
                height: gridDiv.height(),
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
                    <td style="text-align: right">
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
                            <table style="width: 100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl1" runat="server" Text="Vchnum" CssClass="col-sm-4 control-label" Font-Size="14px"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lbl1a" runat="server" Text="TC" Style="width: 22px; float: right;" CssClass="col-sm-4 control-label" Font-Size="14px"></asp:Label></td>
                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtvchnum" runat="server" Width="80px" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtvchdate" placeholder="Date" runat="server" Width="350px" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            <%--<asp:CalendarExtender ID="txtvchdate_CalendarExtender" runat="server"
                                                Enabled="True" TargetControlID="txtvchdate"
                                                Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="Maskedit1" runat="server" Mask="99/99/9999"
                                                MaskType="Date" TargetControlID="txtvchdate" />--%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl4" runat="server" Text="Grade" CssClass="col-sm-4 control-label" Font-Size="14px"></asp:Label></td>
                                    <td>
                                        <asp:ImageButton ID="btnlbl4" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right; display: none" OnClick="btnlbl4_Click" /></td>
                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl4" runat="server" Width="80px" ReadOnly="true" MaxLength="20" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl4a" runat="server" Width="350px" ReadOnly="true" MaxLength="150" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr style="display: none">
                                    <td>
                                        <asp:Label ID="lbl7" runat="server" Text="lbl7" CssClass="col-sm-4 control-label" Font-Size="14px"></asp:Label></td>
                                    <td>
                                        <asp:ImageButton ID="btnlbl7" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl7_Click" /></td>
                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl7" runat="server" Width="80px" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl7a" runat="server" Width="350px" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr style="display: none">
                                    <td>
                                        <asp:Label ID="lbl101" runat="server" Text="lbl101" CssClass="col-sm-4 control-label" Font-Size="14px"></asp:Label></td>
                                    <td>
                                        <!--<asp:ImageButton ID="btnlbl101" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl101_Click" />-->
                                    </td>
                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl101" runat="server" Width="80px" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl101a" runat="server" Width="350px" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>

                            </table>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <%--<div class="box-body">--%>
                        <div class="box-body">
                            <table style="width: 100%">
                                <tr style="display: none">
                                    <td>
                                        <asp:Label ID="lbl2" runat="server" Text="Ent_By" CssClass="col-sm-2 control-label" Font-Size="14px"></asp:Label></td>
                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl2" runat="server" Width="150px" CssClass="form-control" ReadOnly="True"></asp:TextBox>
                                        </div>
                                    </td>

                                    <td>
                                        <asp:Label ID="lbl3" runat="server" Text="Ent_Dt" CssClass="col-sm-2 control-label" Font-Size="14px"></asp:Label>

                                    </td>
                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl3" runat="server" Width="150px" CssClass="form-control" ReadOnly="True"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr style="display: none">
                                    <td>
                                        <asp:Label ID="lbl5" runat="server" Text="Edt_By" CssClass="col-sm-4 control-label" Font-Size="14px"></asp:Label></td>
                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl5" runat="server" Width="200px" CssClass="form-control" ReadOnly="True"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl6" runat="server" Text="Edt_Dt" CssClass="col-sm-4 control-label" Font-Size="14px"></asp:Label>
                                    </td>

                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl6" runat="server" Width="100%" CssClass="form-control" ReadOnly="True"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>

                                <tr>
                                    <td>
                                        <asp:Label ID="lbl8" runat="server" Text="PayDays" Font-Size="14px"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtlbl8" runat="server" Width="100px" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnAttn" runat="server" OnClick="btnAttn_Click" Text="Pick From Attendance" CssClass="form-control" Width="145px" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnFormat" runat="server" OnClick="btnFormat_Click" Text="Download CSV Format" CssClass="form-control" Width="145px" />
                                    </td>
                                </tr>
                                <tr id="Sal2" runat="server">
                                    <td>
                                        <asp:Button ID="btnImport" runat="server" OnClick="btnImport_Click" Text="Import CSV" CssClass="form-control" Width="100px" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnAtt" runat="server" Text="Attachment" OnClick="btnAtt_Click" Width="134px" Style="display: none" />
                                    </td>
                                    <td>
                                        <asp:FileUpload ID="FileUpload1" runat="server" Visible="false" onchange="submitFile()" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>

                <section class="col-lg-12 connectedSortable">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">
                                <li><a href="#DescTab" id="tab1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab">Emp Details</a></li>
                                <li><a href="#DescTab2" id="tab2" runat="server" aria-controls="DescTab2" role="tab" data-toggle="tab">Leaving Details</a></li>
                                <li><a href="#DescTab3" id="tab3" runat="server" aria-controls="DescTab3" role="tab" data-toggle="tab">Addl.Terms</a></li>
                                <li><a href="#DescTab4" id="tab4" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Delv.Sch</a></li>
                                <li><a href="#DescTab5" id="tab5" runat="server" aria-controls="DescTab5" role="tab" data-toggle="tab">Inv.Dtl</a></li>
                            </ul>

                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane active" id="DescTab">
                                    <div class="lbBody" id="gridDiv" style="color: White; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                        <fin:CoolGridView ID="sg1" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="100%" Height="350px" Font-Size="13px"
                                            AutoGenerateColumns="False" OnRowDataBound="sg1_RowDataBound"
                                            OnRowCommand="sg1_RowCommand">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:BoundField DataField="sg1_h1" HeaderText="sg1_h1" />
                                                <asp:BoundField DataField="sg1_h2" HeaderText="sg1_h2" />
                                                <asp:BoundField DataField="sg1_h3" HeaderText="sg1_h3" />
                                                <asp:BoundField DataField="sg1_h4" HeaderText="sg1_h4" />
                                                <asp:BoundField DataField="sg1_h5" HeaderText="Cardno" />
                                                <asp:BoundField DataField="sg1_h6" HeaderText="Adv.Bal." ItemStyle-HorizontalAlign="Right" />
                                                <%-- <asp:BoundField DataField="sg1_h7" HeaderText="Tot.Days" />--%>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Tot.Days</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_h7" runat="server" Text='<%#Eval("sg1_h7") %>' ReadOnly="true" Width="100%" Style="text-align: right"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="sg1_h8" HeaderText="EmpCode" />
                                                <asp:BoundField DataField="sg1_h9" HeaderText="Name" />
                                                <asp:BoundField DataField="sg1_h10" HeaderText="F/H.Name" />

                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>Add</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btnadd" runat="server" CommandName="SG1_ROW_ADD" ImageAlign="Middle" ImageUrl="../tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>Del</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btnrmv" runat="server" CommandName="SG1_RMV" ImageUrl="../tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="sg1_srno" HeaderText="Sr.No." />
                                                <asp:BoundField DataField="sg1_f1" HeaderText="Ecode" />
                                                <asp:BoundField DataField="sg1_f2" HeaderText="-" Visible="false" />
                                                <asp:BoundField DataField="sg1_f3" HeaderText="-" Visible="false" />
                                                <asp:BoundField DataField="sg1_f4" HeaderText="-" Visible="false" />
                                                <%-- IT WAS DECIDED LATER ON, THAT AFTER EXT.MIN 10 BLANK COLUMNS REQUIRED THAT'S WHY INDEXING IS DISTURBED  --%>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Present</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t1" runat="server" Text='<%#Eval("sg1_t1") %>' onChange="calAttn()" onkeypress="return isDecimalKey(event)" Width="100%" MaxLength="5" Style="text-align: right"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Absent</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t2" runat="server" Text='<%#Eval("sg1_t2") %>' onkeypress="return isDecimalKey(event)" Width="100%" ReadOnly="true" Style="text-align: right"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Holiday</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t3" runat="server" Text='<%#Eval("sg1_t3") %>' onChange="calAttn()" onkeypress="return isDecimalKey(event)" Width="100%" MaxLength="5" Style="text-align: right"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Wrk_Hr</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t4" runat="server" Text='<%#Eval("sg1_t4") %>' onkeypress="return isDecimalKey(event)" Width="100%" MaxLength="5" Style="text-align: right"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Wk/Off</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t5" runat="server" Text='<%#Eval("sg1_t5") %>' onChange="calAttn()" onkeypress="return isDecimalKey(event)" Width="100%" MaxLength="3" Style="text-align: right"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>EL</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t6" runat="server" Text='<%#Eval("sg1_t6") %>' onChange="calAttn()" onkeypress="return isDecimalKey(event)" Width="100%" MaxLength="4" Style="text-align: right"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>CL</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t7" runat="server" Text='<%#Eval("sg1_t7") %>' onChange="calAttn()" onkeypress="return isDecimalKey(event)" Width="100%" MaxLength="4" Style="text-align: right"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>SL</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t8" runat="server" Text='<%#Eval("sg1_t8") %>' onChange="calAttn()" Width="100%" onkeypress="return isDecimalKey(event)" MaxLength="4" Style="text-align: right"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Ext_Hr</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t9" runat="server" Text='<%#Eval("sg1_t9") %>' Width="100%" onkeypress="return isDecimalKey(event)" MaxLength="5" Style="text-align: right"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Ext_Min</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t10" runat="server" Text='<%#Eval("sg1_t10") %>' Width="100%" onkeypress="return isDecimalKey(event)" MaxLength="2" Style="text-align: right"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Loan</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t11" runat="server" Text='<%#Eval("sg1_t11") %>' Width="100%" onkeypress="return isDecimalKey(event)" MaxLength="6" Style="text-align: right"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>TDS</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t12" runat="server" Text='<%#Eval("sg1_t12") %>' Width="100%" onkeypress="return isDecimalKey(event)" MaxLength="6" Style="text-align: right"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Adv_Ded</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t13" runat="server" Text='<%#Eval("sg1_t13") %>' MaxLength="7" onkeypress="return isDecimalKey(event)" Width="100%" Style="text-align: right"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Canteen</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t14" runat="server" Text='<%#Eval("sg1_t14") %>' MaxLength="6" onkeypress="return isDecimalKey(event)" Width="100%" Style="text-align: right"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Medical</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t15" runat="server" Text='<%#Eval("sg1_t15") %>' MaxLength="6" onkeypress="return isDecimalKey(event)" Width="100%" Style="text-align: right"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Kpi(Inc)</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t19" runat="server" Text='<%#Eval("sg1_t19") %>' onkeypress="return isDecimalKey(event)" MaxLength="6" Width="100%" Style="text-align: right"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Oth(Inc)</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t20" runat="server" Text='<%#Eval("sg1_t20") %>' onkeypress="return isDecimalKey(event)" MaxLength="6" Width="100%" Style="text-align: right"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Ar.Days</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t21" runat="server" Text='<%#Eval("sg1_t21") %>' onkeypress="return isDecimalKey(event)" MaxLength="5" Width="100%" Style="text-align: right"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Ar.Mth</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t22" runat="server" Text='<%#Eval("sg1_t22") %>' onkeypress="return isDecimalKey(event)" MaxLength="7" Width="100%" Style="text-align: right"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Kpi</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t24" runat="server" Text='<%#Eval("sg1_t24") %>' onkeypress="return isDecimalKey(event)" MaxLength="7" Width="100%" Style="text-align: right"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Ar:Basic</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t16" runat="server" Text='<%#Eval("sg1_t16") %>' MaxLength="7" onkeypress="return isDecimalKey(event)" Width="100%" Style="text-align: right"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Ar:Hra</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t17" runat="server" Text='<%#Eval("sg1_t17") %>' onkeypress="return isDecimalKey(event)" MaxLength="7" Width="100%" Style="text-align: right"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Ar:Conv</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t18" runat="server" Text='<%#Eval("sg1_t18") %>' onkeypress="return isDecimalKey(event)" MaxLength="7" Width="100%" Style="text-align: right"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Ar:</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t23" runat="server" Text='<%#Eval("sg1_t23") %>' onkeypress="return isDecimalKey(event)" MaxLength="7" Width="100%" Style="text-align: right"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Ar:</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t25" runat="server" Text='<%#Eval("sg1_t25") %>' onkeypress="return isDecimalKey(event)" MaxLength="7" Width="100%" Style="text-align: right"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Ar:</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t26" runat="server" Text='<%#Eval("sg1_t26") %>' onkeypress="return isDecimalKey(event)" MaxLength="7" Width="100%" Style="text-align: right"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Ar:</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t27" runat="server" Text='<%#Eval("sg1_t27") %>' onkeypress="return isDecimalKey(event)" Width="100%" MaxLength="7" Style="text-align: right"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Ar:</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t28" runat="server" Text='<%#Eval("sg1_t28") %>' onkeypress="return isDecimalKey(event)" Width="100%" MaxLength="7" Style="text-align: right"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Ar:</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t29" runat="server" Text='<%#Eval("sg1_t29") %>' onkeypress="return isDecimalKey(event)" Width="100%" MaxLength="7" Style="text-align: right"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Ar:</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t30" runat="server" Text='<%#Eval("sg1_t30") %>' onkeypress="return isDecimalKey(event)" Width="100%" MaxLength="7" Style="text-align: right"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="sg1_h11" HeaderText="Deptt" />
                                                <asp:BoundField DataField="sg1_h12" HeaderText="Erpecode" />
                                                <asp:BoundField DataField="sg1_h13" HeaderText="Status" />
                                                <asp:BoundField DataField="sg1_h14" HeaderText="TMJ" />
                                                <asp:BoundField DataField="sg1_h15" HeaderText="Desg" />
                                                <asp:BoundField DataField="sg1_h16" HeaderText="Leaving_Dt" />
                                                <asp:BoundField DataField="sg1_h17" HeaderText="Join_Dt" />
                                                <asp:BoundField DataField="sg1_h18" HeaderText="C/Off" />
                                                <asp:BoundField DataField="sg1_h19" HeaderText="Appr By" />
                                                <asp:BoundField DataField="sg1_h20" HeaderText="Comp" />
                                                <asp:BoundField DataField="sg1_h21" HeaderText="Deptt Code" />
                                                <asp:BoundField DataField="sg1_h22" HeaderText="HrsSet" ItemStyle-HorizontalAlign="Right" />
                                                <asp:BoundField DataField="sg1_h23" HeaderText="Adv_Giv" />
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Ext_Hr2</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t31" runat="server" Text='<%#Eval("sg1_t31") %>' onkeypress="return isDecimalKey(event)" Width="100%" MaxLength="5" Style="text-align: right"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D" ForeColor="White" />
                                            <HeaderStyle BackColor="#1797c0" ForeColor="White" CssClass="GridviewScrollHeader2" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager2" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem2" />
                                            <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" />
                                        </fin:CoolGridView>
                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane active" id="DescTab2">
                                    <div class="lbBody" style="height: 350px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <div class="col-md-4">
                                            <div>
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <label id="Label3" runat="server" class="col-sm-4 control-label">L.T.A (Rs.)</label>
                                                        <div class="col-sm-8">
                                                            <asp:TextBox ID="txtLTA" runat="server" CssClass="form-control" onkeypress="return isDecimalKey(event)" Width="100%" MaxLength="6" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label4" runat="server" class="col-sm-4 control-label">Medical (Rs.)</label>
                                                        <div class="col-sm-8">
                                                            <asp:TextBox ID="txtMedical" runat="server" CssClass="form-control" onkeypress="return isDecimalKey(event)" Width="100%" MaxLength="6" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label5" runat="server" class="col-sm-4 control-label">Gratuity (Yrs)</label>
                                                        <div class="col-sm-8">
                                                            <asp:TextBox ID="txtGratuity_Yrs" runat="server" CssClass="form-control" onkeypress="return isDecimalKey(event)" Width="100%" MaxLength="6" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label6" runat="server" class="col-sm-4 control-label">Gratuity (Rs.)</label>
                                                        <div class="col-sm-8">
                                                            <asp:TextBox ID="txtGratuity_Rs" runat="server" CssClass="form-control" onkeypress="return isDecimalKey(event)" Width="100%" MaxLength="6" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="lblBonus" runat="server" class="col-sm-4 control-label">Bonus (%)</label>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtBonus_Per" runat="server" CssClass="form-control" onkeypress="return isDecimalKey(event)" Width="100%" MaxLength="6" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtBonus_Amt" runat="server" CssClass="form-control" onkeypress="return isDecimalKey(event)" Width="100%" MaxLength="6" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-4">
                                            <div>
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <label id="Label19" runat="server" class="col-sm-4 control-label">E.L. (Days)</label>
                                                        <div class="col-sm-8">
                                                            <asp:TextBox ID="txtEL_Days" runat="server" CssClass="form-control" onkeypress="return isDecimalKey(event)" Width="100%" MaxLength="6" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label11" runat="server" class="col-sm-4 control-label">E.L. (Rs.)</label>
                                                        <div class="col-sm-8">
                                                            <asp:TextBox ID="txtEL_Rs" runat="server" CssClass="form-control" onkeypress="return isDecimalKey(event)" Width="100%" MaxLength="6" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label2" runat="server" class="col-sm-4 control-label">Notice (Days)</label>
                                                        <div class="col-sm-8">
                                                            <asp:TextBox ID="txtNotice_Days" runat="server" CssClass="form-control" onkeypress="return isDecimalKey(event)" Width="100%" MaxLength="6" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="lblMother" runat="server" class="col-sm-4 control-label">Notice_Pay (Rs.)</label>
                                                        <div class="col-sm-8">
                                                            <asp:TextBox ID="txtNotice_Rs" runat="server" CssClass="form-control" onkeypress="return isDecimalKey(event)" Width="100%" MaxLength="6" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="lblOthers" runat="server" class="col-sm-4 control-label">Others (Rs.)</label>
                                                        <div class="col-sm-8">
                                                            <asp:TextBox ID="txtOthers" runat="server" CssClass="form-control" onkeypress="return isDecimalKey(event)" Width="100%" MaxLength="6" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-4">
                                            <div>
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <label id="Label8" runat="server" class="col-sm-5 control-label">Date of Leaving</label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtLeaving_Dt" runat="server" CssClass="form-control" Width="100%" MaxLength="10"></asp:TextBox>
                                                            <asp:CalendarExtender ID="txtLeaving_Dt_CalendarExtender" runat="server" Enabled="True" TargetControlID="txtLeaving_Dt" Format="dd/MM/yyyy">
                                                            </asp:CalendarExtender>
                                                            <asp:MaskedEditExtender ID="Maskedit1" runat="server" Mask="99/99/9999" MaskType="Date" TargetControlID="txtLeaving_Dt" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label9" runat="server" class="col-sm-5 control-label">Reason Of Leaving</label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtReason" runat="server" CssClass="form-control" Width="100%" MaxLength="20"></asp:TextBox>
                                                        </div>
                                                    </div>
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
                                            <FooterStyle BackColor="#5D7B9D" ForeColor="White" />
                                            <HeaderStyle BackColor="#1797c0" ForeColor="White" CssClass="GridviewScrollHeader" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                            <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" />
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
                                            <FooterStyle BackColor="#5D7B9D" ForeColor="White" />
                                            <HeaderStyle BackColor="#1797c0" ForeColor="White"
                                                CssClass="GridviewScrollHeader" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                            <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" />
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

                <div class="col-md-12" style="display: none">
                    <div>
                        <div class="box-body">
                            <asp:TextBox ID="txtrmk" runat="server" MaxLength="150" Width="99%" CssClass="form-control" TextMode="MultiLine" placeholder="Remarks"></asp:TextBox>
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
    <asp:HiddenField ID="hf2" runat="server" />
    <asp:HiddenField ID="grade" runat="server" />
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
        function calAttn() {
            <%--  var Present = 0;
            var Absent = 0;
            var GridTotDays = 0;
            var grid = document.getElementById("<%= sg1.ClientID%>");
            var TotDays = document.getElementById("ContentPlaceHolder1_txtlbl8").value;
            for (var i = 0; i < grid.rows.length - 1; i++) {                
                Present = fill_zero(document.getElementById('ContentPlaceHolder1_sg1_sg1_t1_' + i).value) * 1;
                Absent = TotDays - Present;
                GridTotDays = TotDays - Absent;
                document.getElementById('ContentPlaceHolder1_sg1_sg1_t2_' + i).value = fill_zero(Absent);
            }--%>


            var present = 0; var holiday = 0; var wk_off = 0; var el = 0; var cl = 0; var sl = 0; var tot = 0;
            var grid = document.getElementById("<%= sg1.ClientID%>");
            var TotDays = document.getElementById("ContentPlaceHolder1_txtlbl8").value;
            for (var i = 0; i < grid.rows.length - 1; i++) {
                present = fill_zero(document.getElementById('ContentPlaceHolder1_sg1_sg1_t1_' + i).value) * 1;
                holiday = fill_zero(document.getElementById('ContentPlaceHolder1_sg1_sg1_t3_' + i).value) * 1;
                wk_off = fill_zero(document.getElementById('ContentPlaceHolder1_sg1_sg1_t5_' + i).value) * 1;
                el = fill_zero(document.getElementById('ContentPlaceHolder1_sg1_sg1_t6_' + i).value) * 1;
                cl = fill_zero(document.getElementById('ContentPlaceHolder1_sg1_sg1_t7_' + i).value) * 1;
                sl = fill_zero(document.getElementById('ContentPlaceHolder1_sg1_sg1_t8_' + i).value) * 1;
                tot = present + holiday + wk_off + el + cl + sl;
                if (tot >= 0) {
                    document.getElementById('ContentPlaceHolder1_sg1_sg1_h7_' + i).value = fill_zero(tot);
                }
                document.getElementById('ContentPlaceHolder1_sg1_sg1_t2_' + i).value = fill_zero(TotDays - tot);
            }
            function fill_zero(val) { if (isNaN(val)) return 0; if (isFinite(val)) return val; }
        }
    </script>
    <asp:HiddenField ID="TabName" runat="server" />
</asp:Content>
