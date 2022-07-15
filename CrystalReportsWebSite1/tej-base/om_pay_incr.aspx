<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="om_pay_incr" CodeFile="om_pay_incr.aspx.cs" %>

<%--  --%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../tej-base/Scripts/gridviewScroll.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            // gridviewScroll('#<%=sg1.ClientID%>', gridDiv, 1, 4);

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
                            <div class="form-group">
                                <asp:Label ID="lbl1" runat="server" Text="lbl1" CssClass="col-sm-3 control-label" Font-Size="14px" >Inc No./Date</asp:Label>
                                <div class="col-sm-1" style="display: none">
                                    <asp:Label ID="lbl1a" runat="server" Text="TC" Style="width: 22px; float: right;" CssClass="col-sm-1 control-label"></asp:Label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtvchnum" runat="server" Width="100%" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-sm-5">
                                    <asp:TextBox ID="txtvchdate" placeholder="Date" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtvchdate_CalendarExtender" runat="server"
                                        Enabled="True" TargetControlID="txtvchdate"
                                        Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="Maskedit1" runat="server" Mask="99/99/9999"
                                        MaskType="Date" TargetControlID="txtvchdate" />
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="lbl4" runat="server" Text="Employee_Code" CssClass="col-sm-3 control-label" Font-Size="14px" ></asp:Label>
                                <div class="col-sm-1" style="display: none">
                                    <asp:ImageButton ID="btnlbl4" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl4_Click" />
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtlbl4" runat="server" Width="100%" ReadOnly="true" CssClass="form-control" MaxLength="20"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label17" runat="server" Text="Grade" CssClass="col-sm-2 control-label" Font-Size="14px" ></asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtlbl4a" runat="server" CssClass="form-control" MaxLength="2" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="lbl7" runat="server" Text="Employee_Name" CssClass="col-sm-3 control-label" Font-Size="14px" ></asp:Label>
                                <div class="col-sm-1" style="display: none">
                                    <asp:ImageButton ID="btnlbl7" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right; display: none" OnClick="btnlbl7_Click" />
                                </div>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtlbl7" runat="server" Width="100%" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-sm-5" style="display: none">
                                    <asp:TextBox ID="txtlbl7a" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="Label1" runat="server" Text="Father's_Name" CssClass="col-sm-3 control-label" Font-Size="14px" ></asp:Label>
                                <div class="col-sm-1" style="display: none"></div>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtFather" runat="server" CssClass="form-control" MaxLength="5" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col-sm-8" style="display: none;">
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="lbl104" runat="server" Text="Designation" CssClass="col-sm-3 control-label" Font-Size="14px" ></asp:Label>
                                <div class="col-sm-1" style="display: none">
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtDesCode" runat="server" Width="100%" ReadOnly="true" CssClass="form-control" MaxLength="20"></asp:TextBox>
                                </div>
                                <div class="col-sm-5">
                                    <asp:TextBox ID="txtDesignation" runat="server" CssClass="form-control" MaxLength="100" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>

                           

                        </div>
                    </div>
                </div>

                <div class="col-md-6" style="display: none;">
                    <div>
                        <%--<div class="box-body">--%>
                        <div class="box-body">
                            <table style="width: 100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl2" runat="server" Text="Arrear_Calculate_For_Which_Month" CssClass="col-sm-4 control-label" Font-Size="14px" ></asp:Label></td>
                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl2" runat="server" Width="200px" CssClass="form-control" ReadOnly="True"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td style="display: none">
                                        <asp:Label ID="lbl3" runat="server" Text="Ent_Dt" CssClass="col-sm-4 control-label" Font-Size="14px" ></asp:Label>
                                    </td>
                                    <td style="display: none">
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl3" runat="server" Width="200px" CssClass="form-control" ReadOnly="True"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr style="display: none">
                                    <td>
                                        <asp:Label ID="lbl5" runat="server" Text="Edt_By" CssClass="col-sm-4 control-label" Font-Size="14px" ></asp:Label></td>
                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl5" runat="server" Width="200px" CssClass="form-control" ReadOnly="True"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl6" runat="server" Text="Edt_Dt" CssClass="col-sm-4 control-label" Font-Size="14px" ></asp:Label>
                                    </td>

                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl6" runat="server" Width="200px" CssClass="form-control" ReadOnly="True"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl8" runat="server" Text="(Put_Last_Date_Of_The_Such_Month)" CssClass="col-sm-4 control-label" Font-Size="14px" ></asp:Label></td>
                                    <td style="display: none">
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl8" runat="server" Width="200px" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td style="display: none">
                                        <asp:Label ID="lbl9" runat="server" Text="lbl9" CssClass="col-sm-4 control-label" Font-Size="14px" ></asp:Label></td>
                                    <td style="display: none">
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl9" runat="server" Width="200px" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>

                                <tr>
                                    <td>
                                        <asp:Label ID="lbl102" runat="server" Text="Put last day of month for which you are preparing salary this time, arrear will auto calculate at time of salary based on months selected at that time because salary is calculated as on last date of the month" CssClass="col-sm-4 control-label" Font-Size="14px" ></asp:Label></td>
                                    <td style="display: none">
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl102" runat="server" Width="200px" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td style="display: none">
                                        <asp:Label ID="lbl103" runat="server" Text="lbl103" CssClass="col-sm-4 control-label" Font-Size="14px" ></asp:Label></td>
                                    <td style="display: none">
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl103" runat="server" Width="200px" CssClass="form-control"></asp:TextBox>
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
                                <asp:Label ID="Label2" runat="server" Text="Department" CssClass="col-sm-3 control-label" Font-Size="14px" ></asp:Label>                               
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtDeptCode" runat="server" Width="100%" ReadOnly="true" CssClass="form-control" MaxLength="20"></asp:TextBox>
                                </div>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtDepartment" runat="server" CssClass="form-control" MaxLength="100" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="Label6" runat="server" Text="PF Deduction"  CssClass="col-sm-3 control-label"></asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtPF" runat="server" CssClass="form-control" MaxLength="10" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col-sm-3">
                                    <asp:Label ID="Label7" runat="server" Text="ApplyPFLimit"  CssClass="col-sm-2 control-label"></asp:Label>
                                </div>
                                 <div class="col-sm-3">
                                    <asp:TextBox ID="txtPFLmt" runat="server" CssClass="form-control" MaxLength="10" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="Label14" runat="server" Text="Effective_Date" ToolTip="For Eg: If arrear is to be given from Apr2018, then the salary is to be calculated in the month of June2018. So arrear is to be given for Apr2018,May2018. So enter 30/04/2018 in Effective Date amd 30/06/2018 in Applicable Date."  CssClass="col-sm-3 control-label"></asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtefffrom" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtefffrom_CalendarExtender" runat="server"
                                        Enabled="True" TargetControlID="txtefffrom"
                                        Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="MaskedEdit1" runat="server" Mask="99/99/9999"
                                        MaskType="Date" TargetControlID="txtefffrom" />
                                </div>
                                <asp:Label ID="Label4" runat="server" Text="Mention Last Date of the Month from which arrear to be calculated" Font-Size="Small" ForeColor="Red" CssClass="col-sm-6 control-label"></asp:Label>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="Label3" runat="server" Text="Applicable_From" ToolTip="For Eg: If arrear is to be given from Apr2018, then the salary is to be calculated in the month of June2018. So arrear is to be given for Apr2018,May2018. So enter 30/04/2018 in Effective Date amd 30/06/2018 in Applicable Date."  CssClass="col-sm-3 control-label"></asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtapplfrm" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtapplfrm_CalendarExtender" runat="server"
                                        Enabled="True" TargetControlID="txtapplfrm"
                                        Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="MaskedEditExtender1" runat="server" Mask="99/99/9999"
                                        MaskType="Date" TargetControlID="txtapplfrm" />
                                </div>
                                <asp:Label ID="Label5" runat="server" Text="Mention Last Date of the Month in which arrear to be calculated" ForeColor="Red" Font-Size="Small" CssClass="col-sm-6 control-label"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>

                <section class="col-lg-12 connectedSortable" id="div1" runat="server">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">
                                <li><a href="#DescTab" id="tab1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab">Emp Details</a></li>
                                <li><a href="#DescTab2" id="tab2" runat="server" aria-controls="DescTab2" role="tab" data-toggle="tab">Other.Terms</a></li>
                                <li><a href="#DescTab3" id="tab3" runat="server" aria-controls="DescTab3" role="tab" data-toggle="tab">Addl.Terms</a></li>
                                <li><a href="#DescTab4" id="tab4" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Delv.Sch</a></li>
                                <li><a href="#DescTab5" id="tab5" runat="server" aria-controls="DescTab5" role="tab" data-toggle="tab">Inv.Dtl</a></li>
                            </ul>

                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane active" id="DescTab">
                                    <div class="lbBody" id="gridDiv" style="color: White; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                        <fin:CoolGridView ID="sg1" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="100%" Height="80px" Font-Size="13px"
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

                                                <asp:BoundField DataField="sg1_srno" HeaderText="SrNo" Visible="false"/>
                                                <asp:BoundField DataField="sg1_f1" HeaderText="sg1_f1" Visible="false" />
                                                <asp:BoundField DataField="sg1_f2" HeaderText="-" Visible="false" />
                                                <asp:BoundField DataField="sg1_f3" HeaderText="-" Visible="false" />
                                                <asp:BoundField DataField="sg1_f4" HeaderText="-" Visible="false" />
                                                <asp:TemplateField>
                                                    <HeaderTemplate></HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t1" runat="server" Text='<%#Eval("sg1_t1") %>' onkeypress="return isDecimalKey(event)" Width="100%" ReadOnly="true" ></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate></HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t2" runat="server" Text='<%#Eval("sg1_t2") %>' onkeypress="return isDecimalKey(event)" Width="100%" ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate></HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t3" runat="server" Text='<%#Eval("sg1_t3") %>'  onkeypress="return isDecimalKey(event)" Width="100%" ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate></HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t4" runat="server" Text='<%#Eval("sg1_t4") %>' onkeypress="return isDecimalKey(event)" Width="100%" ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate></HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t5" runat="server" Text='<%#Eval("sg1_t5") %>'  onkeypress="return isDecimalKey(event)" Width="100%"  ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate></HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t6" runat="server" Text='<%#Eval("sg1_t6") %>'  onkeypress="return isDecimalKey(event)"  Width="100%"  ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate></HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t7" runat="server" Text='<%#Eval("sg1_t7") %>'  onkeypress="return isDecimalKey(event)" Width="100%"  ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate></HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t8" runat="server" Text='<%#Eval("sg1_t8") %>' onkeypress="return isDecimalKey(event)" Width="100%"  ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate></HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t9" runat="server" Text='<%#Eval("sg1_t9") %>' onkeypress="return isDecimalKey(event)" Width="100%" ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate></HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t10" runat="server" Text='<%#Eval("sg1_t10") %>'  onkeypress="return isDecimalKey(event)"  Width="100%" ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate></HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t11" runat="server" Text='<%#Eval("sg1_t11") %>'  onkeypress="return isDecimalKey(event)"  Width="100%" ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate></HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t12" runat="server" Text='<%#Eval("sg1_t12") %>'  onkeypress="return isDecimalKey(event)" Width="100%" ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate> </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t13" runat="server" Text='<%#Eval("sg1_t13") %>'  onkeypress="return isDecimalKey(event)"  Width="100%" ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate></HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t14" runat="server" Text='<%#Eval("sg1_t14") %>'  onkeypress="return isDecimalKey(event)" Width="100%" ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate></HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t15" runat="server" Text='<%#Eval("sg1_t15") %>'  onkeypress="return isDecimalKey(event)" Width="100%" ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate></HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t16" runat="server" Text='<%#Eval("sg1_t16") %>' onkeypress="return isDecimalKey(event)" Width="100%" ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate></HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t17" runat="server" Text='<%#Eval("sg1_t17") %>' onkeypress="return isDecimalKey(event)" Width="100%" ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate></HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t18" runat="server" Text='<%#Eval("sg1_t18") %>' onkeypress="return isDecimalKey(event)" Width="100%" ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate></HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t19" runat="server" Text='<%#Eval("sg1_t19") %>' onkeypress="return isDecimalKey(event)" Width="100%" ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate></HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t20" runat="server" Text='<%#Eval("sg1_t20") %>' onkeypress="return isDecimalKey(event)" Width="100%" ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate></HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t21" runat="server" Text='<%#Eval("sg1_t21") %>' onkeypress="return isDecimalKey(event)" Width="100%" ReadOnly="true" ></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D"  ForeColor="White" />
                                            <HeaderStyle BackColor="#1797c0"  ForeColor="White" CssClass="GridviewScrollHeader2" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager2" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem2" />
                                            <SelectedRowStyle BackColor="#E2DED6"  ForeColor="#333333" />
                                        </fin:CoolGridView>
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
                                            <FooterStyle BackColor="#5D7B9D"  ForeColor="White" />
                                            <HeaderStyle BackColor="#1797c0"  ForeColor="White"
                                                CssClass="GridviewScrollHeader" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                            <SelectedRowStyle BackColor="#E2DED6"  ForeColor="#333333" />
                                        </asp:GridView>
                                    </div>
                                </div>
                                <div role="tabpanel" class="tab-pane active" id="DescTab5">
                                    <div class="lbBody" style="height: 200px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                        
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>

                <div class="col-md-12" id="gridhead" runat="server">
                    <div>
                        <div class="box-body" style="background-color:#ff6666;">
                            <div class="form-group">
                            <asp:Label ID="Label8" runat="server" ForeColor="Black" Text="PUT THE CHANGES INT HE GRID SHOWN BELOW!! PUT THE DIFFERENCE AMOUNT ONLY !!" Font-Underline="true" CssClass="col-sm-6 control-label" Font-Size="12px" ></asp:Label>
                            <asp:Label ID="Label9" runat="server" ForeColor="Black" Text="Example. If Current Salary is Rs 5,000/- and increase is Rs 750/- Then Put 750 in below Grid Columns" Font-Underline="true" CssClass="col-sm-6 control-label" Font-Size="12px" ></asp:Label>
                                </div>
                            </div></div></div>

                <section class="col-lg-12 connectedSortable" id="div2" runat="server">
                    <div class="panel panel-default">
                        <div id="Tabs1" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">
                                <li><a href="#DescTab3" id="A1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab">Increment Details</a></li>
                            </ul>
                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane active" id="DescTab3">
                                    <div class="lbBody" style="overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <fin:CoolGridView ID="sg2" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="100%" Height="80px" Font-Size="13px"
                                            AutoGenerateColumns="False" OnRowDataBound="sg1_RowDataBound"
                                            OnRowCommand="sg2_RowCommand">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <%-- <asp:BoundField DataField="sg2_h1" HeaderText="sg2_h1" />
                                                <asp:BoundField DataField="sg2_h2" HeaderText="sg2_h2" />
                                                <asp:BoundField DataField="sg2_h3" HeaderText="sg2_h3" />
                                                <asp:BoundField DataField="sg2_h4" HeaderText="sg2_h4" />
                                                <asp:BoundField DataField="sg2_h5" HeaderText="sg2_h5" />
                                                <asp:BoundField DataField="sg2_h6" HeaderText="sg2_h6" />
                                                <asp:BoundField DataField="sg2_h7" HeaderText="sg2_h7" />
                                                <asp:BoundField DataField="sg2_h8" HeaderText="sg2_h8" />
                                                <asp:BoundField DataField="sg2_h9" HeaderText="sg2_h9" />
                                                <asp:BoundField DataField="sg2_h10" HeaderText="sg2_h10" />--%>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>Add</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg2_btnadd" runat="server" CommandName="SG2_ROW_ADD" ImageAlign="Middle" ImageUrl="../tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>Del</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg2_btnrmv" runat="server" CommandName="SG2_RMV" ImageUrl="../tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="sg2_srno" HeaderText="Sr.No." Visible="false"/>
                                                <asp:BoundField DataField="sg2_f1" HeaderText="Ecode" Visible="false" />
                                                <asp:BoundField DataField="sg2_f2" HeaderText="-" Visible="false" />
                                                <asp:BoundField DataField="sg2_f3" HeaderText="-" Visible="false" />
                                                <asp:BoundField DataField="sg2_f4" HeaderText="-" Visible="false" />
                                                <asp:BoundField DataField="sg2_f5" HeaderText="-" Visible="false" />
                                                <asp:BoundField DataField="sg2_f6" HeaderText="-" Visible="false" />
                                                <asp:BoundField DataField="sg2_f7" HeaderText="-" Visible="false" />
                                                <asp:BoundField DataField="sg2_f8" HeaderText="-" Visible="false" />
                                                <asp:BoundField DataField="sg2_f9" HeaderText="-" Visible="false" />
                                                <asp:BoundField DataField="sg2_f10" HeaderText="-" Visible="false" />

                                                <asp:TemplateField>
                                                    <HeaderTemplate></HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg2_t1" runat="server" Text='<%#Eval("sg2_t1") %>' onChange="Cal()" Width="100%" onkeypress="return isDecimalKey(event)" MaxLength="10"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate></HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg2_t2" runat="server" Text='<%#Eval("sg2_t2") %>' onChange="Cal()" Width="100%" MaxLength="10" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate></HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg2_t3" runat="server" Text='<%#Eval("sg2_t3") %>' onChange="Cal()" Width="100%" MaxLength="10" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate></HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg2_t4" runat="server" Text='<%#Eval("sg2_t4") %>' onChange="Cal()" Width="100%" MaxLength="10" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate></HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg2_t5" runat="server" Text='<%#Eval("sg2_t5") %>' onChange="Cal()" Width="100%" MaxLength="10" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate></HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg2_t6" runat="server" Text='<%#Eval("sg2_t6") %>' onChange="Cal()" Width="100%" MaxLength="10" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate></HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg2_t7" runat="server" Text='<%#Eval("sg2_t7") %>' onChange="Cal()" Width="100%" MaxLength="10" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate></HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg2_t8" runat="server" Text='<%#Eval("sg2_t8") %>' onChange="Cal()" Width="100%" MaxLength="10" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate></HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg2_t9" runat="server" Text='<%#Eval("sg2_t9") %>' onChange="Cal()" Width="100%" MaxLength="10" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate></HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg2_t10" runat="server" Text='<%#Eval("sg2_t10") %>' onChange="Cal()" onkeypress="return isDecimalKey(event)" Width="100%" MaxLength="10"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate></HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg2_t11" runat="server" Text='<%#Eval("sg2_t11") %>' onChange="Cal()" onkeypress="return isDecimalKey(event)" Width="100%" MaxLength="10"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate></HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg2_t12" runat="server" Text='<%#Eval("sg2_t12") %>' onChange="Cal()" onkeypress="return isDecimalKey(event)" Width="100%" MaxLength="10"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate> </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg2_t13" runat="server" Text='<%#Eval("sg2_t13") %>' onChange="Cal()" onkeypress="return isDecimalKey(event)" Width="100%" ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate></HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg2_t14" runat="server" Text='<%#Eval("sg2_t14") %>' onChange="Cal()" onkeypress="return isDecimalKey(event)" Width="100%" MaxLength="10"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate></HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg2_t15" runat="server" Text='<%#Eval("sg2_t15") %>' onChange="Cal()" onkeypress="return isDecimalKey(event)" Width="100%" ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate></HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg2_t16" runat="server" Text='<%#Eval("sg2_t16") %>' onChange="Cal()" onkeypress="return isDecimalKey(event)" Width="100%" MaxLength="10"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate></HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg2_t17" runat="server" Text='<%#Eval("sg2_t17") %>' onChange="Cal()" onkeypress="return isDecimalKey(event)"  Width="100%" MaxLength="10"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate></HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg2_t18" runat="server" Text='<%#Eval("sg2_t18") %>' onChange="Cal()" onkeypress="return isDecimalKey(event)"  Width="100%" MaxLength="10"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate></HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg2_t19" runat="server" Text='<%#Eval("sg2_t19") %>' onChange="Cal()" onkeypress="return isDecimalKey(event)"  Width="100%" MaxLength="10"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate></HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg2_t20" runat="server" Text='<%#Eval("sg2_t20") %>' onChange="Cal()" onkeypress="return isDecimalKey(event)"  Width="100%" MaxLength="10"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D"  ForeColor="White" />
                                            <HeaderStyle BackColor="#1797c0"  ForeColor="White" CssClass="GridviewScrollHeader2" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager2" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem2" />
                                            <SelectedRowStyle BackColor="#E2DED6"  ForeColor="#333333" />
                                        </fin:CoolGridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>

                 <div class="col-md-6" id="Div3" runat="server">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <asp:Label ID="Label10" runat="server" Text="Current Salary" CssClass="col-sm-3 control-label" Font-Size="14px" ></asp:Label>                               
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtcurrsal" runat="server" Width="100%" ReadOnly="true" CssClass="form-control" onChange="Cal2()" MaxLength="20"></asp:TextBox>
                                </div>
                                     <asp:Label ID="Label11" runat="server" Text="Ctc Incl PF" CssClass="col-sm-3 control-label" Font-Size="14px" ></asp:Label>                               
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtctcpf" runat="server" Width="100%" ReadOnly="true" CssClass="form-control" onChange="Cal2()" MaxLength="20"></asp:TextBox>
                                </div>                        
                            </div>
                        </div></div></div>

                 <div class="col-md-6" id="Div4" runat="server">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <asp:Label ID="Label12" runat="server" Text="Increment" CssClass="col-sm-2 control-label" Font-Size="14px" ></asp:Label>                               
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtincr" runat="server" Width="100%" ReadOnly="true" CssClass="form-control" onChange="Cal2()" MaxLength="20"></asp:TextBox>
                                </div>
                                     <asp:Label ID="Label13" runat="server" Text="Curr Ctc" CssClass="col-sm-2 control-label" Font-Size="14px" ></asp:Label>                               
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtcurrctc" runat="server" Width="100%" ReadOnly="true" BackColor="#ff8080" CssClass="form-control" MaxLength="20" ForeColor="Black"></asp:TextBox>
                                </div>
                                <div class="col-sm-2">
                                    <asp:Button ID="cal_ctc" runat="server" OnClick="cal_ctc_Click" Width="100%" Height="30px" Text="Cal CTC" BorderStyle="Groove" BorderColor="WhiteSmoke"/>
                                    </div>
                            </div>
                        </div></div></div>

                <div class="col-md-12" style="display: none">
                    <div>
                        <div class="box-body">
                            <asp:TextBox ID="txtrate" runat="server" MaxLength="150" Width="99%" CssClass="form-control" TextMode="MultiLine" placeholder="Remarks"></asp:TextBox>
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
    <asp:HiddenField ID="Prodrep" runat="server" />
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
    
    <script>
        function Cal() {
            var c1 = 0; var c2 = 0; var c3 = 0; var c4 = 0; var c5 = 0; var c6 = 0; var c7 = 0; var c8 = 0; var c9 = 0; var c10 = 0;
            var c11 = 0; var c12 = 0; var c13 = 0; var c14 = 0; var c15 = 0; var c16 = 0; var c17 = 0; var c18 = 0; var c19 = 0; var c20 = 0;
            var incr = 0; var pfcal = 0;
            var grid = document.getElementById("<%= sg2.ClientID%>");
            var grid1 = document.getElementById("<%= sg1.ClientID%>");
            for (var i = 0; i < grid.rows.length - 1; i++) {
                //alert(grid.rows.length);
                c1 = fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg2_sg2_t1_' + i).value));
                c2 = fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg2_sg2_t2_' + i).value));
                c3 = fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg2_sg2_t3_' + i).value));
                c4 = fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg2_sg2_t4_' + i).value));
                c5 = fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg2_sg2_t5_' + i).value));
                c6 = fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg2_sg2_t6_' + i).value));
                c7 = fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg2_sg2_t7_' + i).value));
                c8 = fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg2_sg2_t8_' + i).value));
                c9 = fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg2_sg2_t9_' + i).value));
                c10 = fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg2_sg2_t10_' + i).value));
                c11 = fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg2_sg2_t11_' + i).value));
                c12 = fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg2_sg2_t12_' + i).value));
                c13 = fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg2_sg2_t13_' + i).value));
                c14 = fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg2_sg2_t14_' + i).value));
                c15 = fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg2_sg2_t15_' + i).value));
                c16 = fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg2_sg2_t16_' + i).value));
                c17 = fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg2_sg2_t17_' + i).value));
                c18 = fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg2_sg2_t18_' + i).value));
                c19 = fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg2_sg2_t19_' + i).value));
                c20 = fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg2_sg2_t20_' + i).value));
                incr += (c1 * 1) + (c2 * 1) + (c3 * 1) + (c4 * 1) + (c5 * 1) + (c6 * 1) + (c7 * 1) + (c8 * 1) + (c9 * 1) + (c10 * 1) + (c11 * 1) + (c12 * 1) + (c13 * 1) + (c14 * 1) + (c15 * 1) + (c16 * 1) + (c17 * 1) + (c18 * 1) + (c19 * 1) + (c20 * 1);
                //txtincr.value = fill_zero(incr);
                document.getElementById('ContentPlaceHolder1_txtincr').value = fill_zero(incr);
            }
            for (var i = 0; i < grid1.rows.length - 1; i++) {
                pfcal = document.getElementById('ContentPlaceHolder1_sg1_sg1_t1_' + i).value;
                //pfrt = document.getElementById('ContentPlaceHolder1_sg1_sg1_t21_' + i).value;
                //alert(pfrt);
            }
            var currsal = 0; var ctcpf = 0; var incr = 0; var currctc = 0;
            currsal = document.getElementById('ContentPlaceHolder1_txtcurrsal').value;
            document.getElementById('ContentPlaceHolder1_txtctcpf').value = 0;
            ctcpf = document.getElementById('ContentPlaceHolder1_txtctcpf').value;
            incr = document.getElementById('ContentPlaceHolder1_txtincr').value;
            currctc += (currsal * 1) + (ctcpf * 1) + (incr * 1);
            document.getElementById('ContentPlaceHolder1_txtcurrctc').value = fill_zero(currctc);

            function fill_zero(val) { if (isNaN(val)) return 0; if (isFinite(val)) return val; }
        }
    </script>
    <asp:HiddenField ID="TabName" runat="server" />
</asp:Content>
