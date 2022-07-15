<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="om_frmPmcard" CodeFile="om_frmPmcard.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('#ContentPlaceHolder1_edmode').val() == "Y")
                $('#ContentPlaceHolder1_div1').find('input, textarea, button, select').attr('readonly', true);
            else $('#ContentPlaceHolder1_div2').find('input, textarea, button, select').attr('readonly', true);
        });
        function disableFrame(divID) {
            //$('#ContentPlaceHolder1_' + divID + '').find('input, textarea, button, select').attr('readonly', true);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper">
        <section class="content-header">
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label>
                    </td>
                    <td style="text-align: right">
                        <button type="submit" id="btnnew" class="btn btn-info" style="width: 100px;" runat="server" accesskey="N" onserverclick="btnnew_ServerClick"><u>N</u>ew</button>
                        <button type="submit" id="btnedit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="i" onserverclick="btnedit_ServerClick">Ed<u>i</u>t</button>
                        <button type="submit" id="btnsave" class="btn btn-info" style="width: 100px;" runat="server" accesskey="s" onserverclick="btnsave_ServerClick"><u>S</u>ave</button>
                        <button type="submit" id="btnprint" class="btn btn-info" style="width: 100px;" runat="server" accesskey="P" onserverclick="btnprint_ServerClick"><u>P</u>rint</button>
                        <button type="submit" id="btndel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="l" onserverclick="btndel_ServerClick">De<u>l</u>ete</button>
                        <button type="submit" id="btnlist" class="btn btn-info" style="width: 100px;" runat="server" accesskey="t" onserverclick="btnlist_ServerClick">Lis<u>t</u></button>
                        <button type="submit" id="btnCamera" class="btn btn-info" style="width: 100px;" runat="server" accesskey="A" onserverclick="btnCamera_ServerClick">C<u>a</u>mera</button>
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
                        <div class="box-body" id="div1" runat="server">
                            <%-- <div class="form-group">
                                <label id="Label11" runat="server" class="col-sm-4 control-label" title="lbl1"><u>Request/Complaint Section</u></label>
                            </div>--%>
                            <div class="form-group">

                                <label id="lbl1" runat="server" class="col-sm-4 control-label" title="lbl1">Card No</label>

                                <div class="col-sm-8">
                                    <input style="height: 28px" id="TxtCardNo" type="text" class="form-control" runat="server" placeholder="Card No" readonly />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="lbl2" runat="server" class="col-sm-4 control-label" title="lbl1">Date</label>
                                <div class="col-sm-8">
                                    <input style="height: 28px" id="TxtDate" type="date" class="form-control" runat="server" placeholder="DD/MM/YYYY" />
                                    <%--<asp:TextBox ID="TxtDate" runat="server" class="form-control"></asp:TextBox>
                                     <asp:CalendarExtender ID="TxtDate_CalendarExtender" runat="server"
                                                            Enabled="True" TargetControlID="TxtDate"
                                                            Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                        <asp:MaskedEditExtender ID="Maskedit9" runat="server" Mask="99/99/9999"
                                                            MaskType="Date" TargetControlID="TxtDate" />--%>
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="lbl3" runat="server" class="col-sm-3 control-label" title="lbl1">Shift</label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnshift" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnShift_Click" />
                                </div>
                                <div class="col-sm-8">
                                    <input style="height: 28px; display: none;" id="TxtShiftCode" type="text" runat="server" />
                                    <input style="height: 28px" id="TxtShift" type="text" class="form-control" runat="server" placeholder="Shift" readonly />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label12" runat="server" class="col-sm-3 control-label" title="lbl1">Deptt</label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnDeptt" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnDept_Click" />
                                </div>
                                <div class="col-sm-8">
                                    <input style="height: 28px; display: none;" id="TxtDeptCode" type="text" runat="server" />
                                    <input style="height: 28px" id="Txtdept" type="text" class="form-control" runat="server" placeholder="Department" readonly />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label16" runat="server" class="col-sm-3 control-label" title="lbl1">M/C</label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnMc" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnMc_Click" />
                                </div>
                                <div class="col-sm-8">
                                    <input style="height: 28px; display: none;" id="TxtMCCode" type="text" runat="server" />
                                    <input style="height: 28px" id="TxtMc" type="text" class="form-control" runat="server" placeholder="M/C" readonly="true" />
                                </div>
                            </div>
                            <div class="form-group">

                                <label id="Label13" runat="server" class="col-sm-4 control-label" title="lbl1">Occurance Time Hrs</label>
                                <div class="col-sm-3">
                                    <input style="height: 28px" id="Txthrs" type="number" class="form-control" runat="server" placeholder="Hrs" max="9999" />
                                </div>
                                <label id="Label4" runat="server" class="col-sm-1 control-label" title="lbl1">Mnts</label>
                                <div class="col-sm-4">
                                    <input style="height: 28px" id="TxtMins" type="number" class="form-control" runat="server" placeholder="Mins" max="9999" />
                                </div>
                            </div>

                            <div class="form-group">

                                <label id="Label14" runat="server" class="col-sm-4 control-label" title="lbl1">Production Loss</label>

                                <div class="col-sm-8">
                                    <input style="height: 28px" id="TxtRdProd" type="text" class="form-control" runat="server" placeholder="Y/N" maxlength="1" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label15" runat="server" class="col-sm-4 control-label" title="lbl1">Complaint By</label>
                                <div class="col-sm-8">
                                    <input style="height: 28px" id="TxtComp" type="text" class="form-control" runat="server" placeholder="Complaint By" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label17" runat="server" class="col-sm-4 control-label" title="lbl1">Desc. of Complaint</label>
                                <div class="col-sm-8">
                                    <input style="height: 28px" id="TxtDescComp" type="text" class="form-control" runat="server" placeholder="Description" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label18" runat="server" class="col-sm-4 control-label" title="lbl1">Exp. Date of Completion </label>
                                <div class="col-sm-8">
                                    <input style="height: 28px" id="TxtdateComp" type="date" class="form-control" runat="server" placeholder="dd/mm/yyyy" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label19" runat="server" class="col-sm-3 control-label" title="lbl1">Incharge</label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="BtnIncharge" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnIncharge_Click" />
                                </div>
                                <div class="col-sm-8">
                                    <input style="height: 28px; display: none;" id="TxtInchCode" type="text" runat="server" />
                                    <input style="height: 28px" id="TxtInch" type="text" class="form-control" runat="server" placeholder="Incharge" readonly />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label1" runat="server" class="col-sm-4 control-label" title="lbl1">Remarks</label>
                                <div class="col-sm-8">
                                    <input style="height: 28px" id="TxtRemarks" type="text" class="form-control" runat="server" placeholder="Remarks" />
                                </div>
                            </div>

                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body" id="div2" runat="server">
                            <%--<div class="form-group">
                                <label id="Label4" runat="server" class="col-sm-3 control-label" title="lbl1">Action Taken Section</label>
                            </div>--%>
                            <div class="form-group">
                                <label id="lbl4" runat="server" class="col-sm-3 control-label" title="lbl1">Dept.(Sup/Engr)</label>
                                <div class="col-sm-8">
                                    <input style="height: 28px" id="txtDeptSup" type="text" class="form-control" runat="server" placeholder="Dept.(Sup/Engr)" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label5" runat="server" class="col-sm-3 control-label" title="lbl1">Problm Obsvd</label>
                                <div class="col-sm-8">
                                    <input style="height: 28px" id="TxtProbObs" type="text" class="form-control" runat="server" placeholder="Problem Observed" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label6" runat="server" class="col-sm-3 control-label" title="lbl1">Corrective Action</label>
                                <div class="col-sm-8">
                                    <input style="height: 28px" id="TxtCorrAct" type="text" class="form-control" runat="server" placeholder="Corrective Action" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label7" runat="server" class="col-sm-3 control-label" title="lbl1">Spares/Cons. Used</label>
                                <div class="col-sm-8">
                                    <input style="height: 28px" id="TxtSpCons" type="text" class="form-control" runat="server" placeholder="Spares/Cons Used" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label8" runat="server" class="col-sm-3 control-label" title="lbl1">Containment Action</label>
                                <div class="col-sm-8">
                                    <input style="height: 28px" id="TxtContAct" type="text" class="form-control" runat="server" placeholder="Containment Action" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label9" runat="server" class="col-sm-3 control-label" title="lbl1">Preventive Action</label>
                                <div class="col-sm-8">
                                    <input style="height: 28px" id="TxtPrevAct" type="text" class="form-control" runat="server" placeholder="Preventive Action" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label10" runat="server" class="col-sm-2 control-label" title="lbl1">Nature</label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnNature_Click" />
                                </div>
                                <div class="col-sm-8">
                                    <input style="height: 28px; display: none;" id="TxtNatCode" type="text" runat="server" />
                                    <input style="height: 28px" id="TxtNature" type="text" class="form-control" runat="server" placeholder="Nature" readonly />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label20" runat="server" class="col-sm-3 control-label" title="lbl1">Date of Closure</label>
                                <div class="col-sm-8">
                                    <input style="height: 28px" id="TxtClosureDate" type="date" class="form-control" runat="server" placeholder="DD/MM/YYYY" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label21" runat="server" class="col-sm-3 control-label" title="lbl1">Cost(If Any)</label>
                                <div class="col-sm-8">
                                    <input style="height: 28px" id="TxtCost" type="number" class="form-control" runat="server" placeholder="Cost" max="9999999" />
                                </div>

                            </div>

                            <div class="form-group">
                                <label id="Label22" runat="server" class="col-sm-3 control-label" title="lbl1">Cleared Time Hrs</label>
                                <div class="col-sm-3">
                                    <input style="height: 28px" id="TxtClrHrs" type="number" class="form-control" runat="server" placeholder="Hrs" max="9999" />
                                </div>
                                <label id="Label3" runat="server" class="col-sm-1 control-label" title="lbl1">Mnts</label>
                                <div class="col-sm-4">
                                    <input style="height: 28px" id="TxtClrMins" type="number" class="form-control" runat="server" placeholder="Mins" max="9999" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label24" runat="server" class="col-sm-3 control-label" title="lbl1">Down Time Hrs</label>
                                <div class="col-sm-3">
                                    <input style="height: 28px" id="TxtDownTime" type="number" class="form-control" runat="server" placeholder="Hrs" max="9999" />
                                </div>
                                <label id="Label2" runat="server" class="col-sm-1 control-label" title="lbl1">Mnts</label>
                                <div class="col-sm-4">
                                    <input style="height: 28px" id="TxtDowmMins" type="number" class="form-control" runat="server" placeholder="Mins" max="9999" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label26" runat="server" class="col-sm-2 control-label" title="lbl1">Comp.Type</label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="BtnCompT" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnComp_Click" />
                                </div>
                                <div class="col-sm-8">
                                    <input style="height: 28px" id="TxtCompType" type="text" class="form-control" runat="server" placeholder="Compaint Type" readonly />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label11" runat="server" class="col-sm-3 control-label" title="lbl1">Mark as Cleared</label>
                                <div class="col-sm-1">
                                    <asp:CheckBox ID="chkclr" AutoPostBack="true" runat="server" OnCheckedChanged="chkclr_CheckedChanged" />
                                </div>
                                <div class="col-sm-7">
                                    <input style="height: 28px" id="TxtClr" type="text" class="form-control" runat="server" placeholder="" readonly />
                                </div>
                            </div>

                        </div>
                    </div>
                </div>


                <section class="col-lg-12 connectedSortable" style="display: none">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">
                                <li><a href="#DescTab" id="tab1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab">Form Details</a></li>
                                <li><a href="#DescTab2" id="tab2" runat="server" aria-controls="DescTab2" role="tab" data-toggle="tab">Comm.Terms</a></li>
                                <li><a href="#DescTab3" id="tab3" runat="server" aria-controls="DescTab3" role="tab" data-toggle="tab">Addl.Terms</a></li>
                                <li><a href="#DescTab4" id="tab4" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Delv.Sch</a></li>
                                <li><a href="#DescTab5" id="tab5" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Inv.Dtl</a></li>
                            </ul>

                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane active" id="DescTab">
                                    <div class="lbBody" style="height: 200px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                                <div role="tabpanel" class="tab-pane active" id="DescTab2">
                                    <div class="lbBody" style="height: 200px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                        <div class="lbBody" style="height: 200px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                            <div class="col-md-6">
                                                <div>
                                                    <div class="box-body">
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-6">
                                                <div>
                                                    <div class="box-body">
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                                <div role="tabpanel" class="tab-pane active" id="DescTab3">
                                </div>
                            </div>
                        </div>
                    </div>
                </section>

                <section class="col-lg-12 connectedSortable">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">
                                <li><a href="#DescTab4" id="A4" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Complaint Card Items</a></li>


                            </ul>

                            <div role="tabpanel" class="tab-pane-active" id="DescTab4">
                                <div class="lbBody" style="height: 200px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                    <asp:GridView ID="sg1" runat="server" ForeColor="#333333"
                                        Style="background-color: #FFFFFF; color: White;" Width="1200px" Font-Size="Smaller"
                                        AutoGenerateColumns="False" OnRowDataBound="sg1_RowDataBound"
                                        OnRowCommand="sg1_RowCommand">
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <Columns>

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

                                            <asp:BoundField DataField="sg1_Srno" HeaderText="Sr.No" />
                                            <asp:BoundField DataField="sg1_f1" HeaderText="Code" />
                                            <asp:BoundField DataField="sg1_f2" HeaderText="Name" />

                                            <asp:TemplateField>
                                                <HeaderTemplate>Unit</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="sg1_t1" runat="server" Text='<%#Eval("sg1_t1") %>' MaxLength="10" Width="100%"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField>
                                                <HeaderTemplate>Qty</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="sg1_t2" runat="server" Text='<%#Eval("sg1_t2") %>' MaxLength="10" Width="100%"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>Rate</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="sg1_t3" runat="server" Text='<%#Eval("sg1_t3") %>' MaxLength="10" Width="100%"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>Remarks</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="sg1_t4" runat="server" Text='<%#Eval("sg1_t4") %>' MaxLength="100" Width="100%"></asp:TextBox>
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
    <input id="pwd1" runat="server" style="display: none" />
    <asp:HiddenField ID="hfGridView1SV" runat="server" />
    <asp:HiddenField ID="hfGridView1SH" runat="server" />
    <script type="text/javascript">
        //$(function () {
        //    var tabName = $("[id*=TabName]").val() != "" ? $("[id*=TabName]").val() : "DescTab";
        //    $('#Tabs a[href="#' + tabName + '"]').tab('show');
        //    $("#Tabs a").click(function () {
        //        $("[id*=TabName]").val($(this).attr("href").replace("#", ""));
        //    });
        //});
    </script>
    <asp:HiddenField ID="TabName" runat="server" />
</asp:Content>
