<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="om_bank_reco"  EnableEventValidation="true" CodeFile="om_bank_reco.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../tej-base/Scripts/gridviewScroll.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            //gridviewScroll('#<%=sg1.ClientID%>', gridDiv, 1, 1);
        });
        function fnAllowNumeric() {
            //if ((event.keyCode < 48 || event.keyCode > 57) && event.keyCode != 8 && event.keyCode != 45) {
            //    event.keyCode = 0;
            //    alert("Please Enter Number Only");
            //    return false;
            //}

            var chqIssued = 0, balanceEntered = 0;

            chqIssued = document.getElementById("ContentPlaceHolder1_txtcalbank").value;
            balanceEntered = document.getElementById("ContentPlaceHolder1_txtbalance").value;

            document.getElementById("ContentPlaceHolder1_txtdiff").value = ((chqIssued) - (balanceEntered)).toFixed(2);

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
                        <asp:Label ID="lblprogress" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                    </td>
                    <td style="text-align: right">
                        <button type="submit" id="btnnew" class="btn btn-info" style="width: 100px;" runat="server" accesskey="N" onserverclick="btnnew_ServerClick"><u>N</u>ew</button>
                        <button type="submit" id="btnlist1" class="btn btn-info" style="width: 120px;" runat="server" onserverclick="btnlist1_ServerClick">Get Server Data</button>
                        <button type="submit" id="btnautofilldt" class="btn btn-info" style="width: 120px;" runat="server" onserverclick="btnautofilldt_ServerClick">Auto fill Dates</button>
                        <button type="submit" id="btnupdate" class="btn btn-info" style="width: 130px;" runat="server" onserverclick="btnupdate_ServerClick">Update Vouchers</button>
                        <button type="submit" id="btnsave" class="btn btn-info" style="width: 100px;" runat="server" accesskey="s" onserverclick="btnsave_ServerClick"><u>S</u>ave</button>
                        <button type="submit" id="btnprint" class="btn btn-info" style="width: 100px;" runat="server" accesskey="P" onserverclick="btnprint_ServerClick"><u>P</u>rint</button>
                        <button type="submit" id="btndel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="l" onserverclick="btndel_ServerClick">De<u>l</u>ete</button>
                        <button type="submit" id="btnlist" class="btn btn-info" style="width: 100px;" runat="server" accesskey="t" onserverclick="btnlist_ServerClick">Lis<u>t</u></button>
                        <button type="submit" id="btncancel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="c" onserverclick="btncancel_ServerClick"><u>C</u>ancel</button>
                        <button type="submit" id="btnexit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="X" onserverclick="btnexit_ServerClick">E<u>x</u>it</button>
                        <button type="submit" id="btnedit" class="btn btn-info" style="width: 120px; display: none;" runat="server" accesskey="i" onserverclick="btnedit_ServerClick">Ed<u>i</u>t</button>
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
                                <label id="Label2" runat="server" class="col-sm-2 control-label" title="lbl1">DocNo</label>
                                <label id="lblVty" runat="server" class="col-sm-1 control-label" title="lbl1">DC</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtvchnum" runat="server" placeholder="DocumentNo" ReadOnly="true" CssClass="form-control" Height="28px"></asp:TextBox>
                                </div>
                                <div class="col-sm-6    ">
                                    <asp:TextBox ID="txtvchdate" placeholder="Date" runat="server" ReadOnly="true" CssClass="form-control" Height="28px"></asp:TextBox>

                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label9" runat="server" class="col-sm-2 control-label" title="lbl1">BankName</label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnAcode" runat="server" ImageUrl="~/tej-base/css/images/bdsearch5.png" OnClick="btnAcode_Click" Style="width: 22px; float: right;" />
                                </div>
                                <div class="col-sm-3">
                                    <input id="txtbankcode" type="text" readonly="true" class="form-control" runat="server" placeholder="Code" maxlength="4" style="height: 28px;" />
                                </div>
                                <div class="col-sm-6">
                                    <input id="txtbankname" type="text" readonly="true" class="form-control" runat="server" placeholder="Name" maxlength="4" style="height: 28px;" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label1" runat="server" class="col-sm-3 control-label" title="lbl1">ReconDate</label>

                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtrecondt" type="text" class="form-control" runat="server" placeholder="Date" Style="height: 28px;"></asp:TextBox>

                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server"
                                        Enabled="True" TargetControlID="txtrecondt"
                                        Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="MaskedEditExtender1" runat="server" Mask="99/99/9999"
                                        MaskType="Date" TargetControlID="txtrecondt" />
                                </div>

                            </div>
                            <div class="form-group">
                                <label id="Label10" runat="server" class="col-sm-3 control-label" title="lbl1">Balance</label>
                                <div class="col-sm-9">
                                    <input id="txtbalance" class="form-control" runat="server" placeholder="Put -ve sign in case of Cr.Bal" style="height: 28px;" onkeyup="fnAllowNumeric()" />
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-sm-6">
                                    Display Cleared Entries &nbsp;&nbsp;
                                    <asp:CheckBox ID="chkdispclear" runat="server" />
                                </div>
                                <div class="col-sm-6">
                                    All Entries Cleared, Ok &nbsp;&nbsp;
                                    <asp:CheckBox ID="Check1" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <label id="Label6" runat="server" class="col-sm-4 control-label" title="lbl1">Balance as per books</label>

                                <div class="col-sm-8">
                                    <input id="txtledgerbal" type="text" readonly="true" class="form-control" runat="server" placeholder="C/B.Balance" maxlength="12" style="height: 28px;" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label8" runat="server" class="col-sm-4 control-label" title="lbl1">Cheque Deposit(Not cleared)</label>


                                <div class="col-sm-8">
                                    <input id="txtdepnotclr" type="text" readonly="true" class="form-control" runat="server" placeholder="Chq.Deposit(Not Cleared)" maxlength="4" style="height: 28px;" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label3" runat="server" class="col-sm-4 control-label" title="lbl1">Cheque Issued(notCleared)</label>
                                <div class="col-sm-8">
                                    <input id="txtissnotclr" type="text" class="form-control" runat="server" placeholder="Chq.Issue (Not Cleared)" style="height: 28px;" readonly="true" />
                                </div>

                            </div>
                            <div class="form-group">
                                <label id="Label7" runat="server" class="col-sm-4 control-label" title="lbl1">Calculated Bank Balance</label>
                                <div class="col-sm-8">
                                    <input id="txtcalbank" type="text" class="form-control" runat="server" placeholder="Calculate Bank Balance" maxlength="8" style="height: 28px;" readonly="true" />
                                </div>

                            </div>
                            <div class="form-group">
                                <label id="Label11" runat="server" class="col-sm-4 control-label" title="lbl1">Difference, If any</label>
                                <div class="col-sm-8">
                                    <input id="txtdiff" type="text" class="form-control" runat="server" placeholder="Difference Value" maxlength="8" style="height: 28px;" readonly="true" />
                                </div>

                            </div>
                        </div>
                    </div>
                </div>

                <section class="col-lg-12 connectedSortable">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">
                                <li><a href="#DescTab" id="tab1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab">Item Details</a></li>
                                <li>
                                    <asp:Label ID="lblRowCount" runat="server"></asp:Label>
                                </li>
                                <li><a href="#DescTab2" id="tab2" runat="server" aria-controls="DescTab2" role="tab" data-toggle="tab">Comm.Terms</a></li>
                                <li><a href="#DescTab3" id="tab3" runat="server" aria-controls="DescTab3" role="tab" data-toggle="tab">Reel Details</a></li>
                                <li><a href="#DescTab4" id="tab4" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Lot.Dtl</a></li>
                                <li><a href="#DescTab5" id="tab5" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Inv.Dtl</a></li>
                                <li><a href="#DescTab6" id="tab6" runat="server" aria-controls="DescTab6" role="tab" data-toggle="tab">UDF Data</a></li>
                            </ul>

                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane active" id="DescTab">
                                    <div class="lbBody" id="gridDiv" style="color: White; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <fin:CoolGridView ID="sg1" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="100%" Height="300px" Font-Size="13px"
                                            AutoGenerateColumns="False" OnRowDataBound="sg1_RowDataBound" AllowSorting="true"
                                            OnRowCommand="sg1_RowCommand" OnSorting="sg1_Sorting">
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

                                                <asp:BoundField DataField="sg1_srno" HeaderText="Srno" SortExpression="sg1_srno" HeaderStyle-Width="30px" />
                                                <asp:BoundField DataField="sg1_f1" HeaderText="Vch_No" ItemStyle-Width="60px" HeaderStyle-Width="60px" />
                                                <asp:BoundField DataField="sg1_f2" HeaderText="Vch_Dt" ItemStyle-Width="80px" SortExpression="sg1_f2" HeaderStyle-Width="80px" />
                                                <asp:BoundField DataField="sg1_f3" HeaderText="Chq_No" ItemStyle-Width="100px" SortExpression="sg1_f3" HeaderStyle-Width="80px" />
                                                <asp:BoundField DataField="sg1_f4" HeaderText="Debit" ItemStyle-Width="80px" SortExpression="sg1_f4" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" />
                                                <asp:BoundField DataField="sg1_f5" HeaderText="Credit" ItemStyle-Width="80px" SortExpression="sg1_f5" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" />

                                                <asp:TemplateField SortExpression="sg1_t1">
                                                    <HeaderTemplate>Date</HeaderTemplate>
                                                    <HeaderStyle Width="150px" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t1" runat="server" Text='<%#Eval("sg1_t1") %>' Width="100%" onKeyUp="updatefield()" onchange="updatefield()" onblur="updatefield()" TextMode="Date"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField SortExpression="sg1_t2">
                                                    <HeaderTemplate>Naration</HeaderTemplate>
                                                    <HeaderStyle Width="200px" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t2" runat="server" Text='<%#Eval("sg1_t2") %>' Width="100%" ReadOnly="true"></asp:TextBox>
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
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Type</HeaderTemplate>
                                                    <HeaderStyle Width="60px" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t3" runat="server" Text='<%#Eval("sg1_t3") %>' Width="100%" ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t4</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t4" runat="server" Text='<%#Eval("sg1_t4") %>' Width="100%" ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField SortExpression="sg1_t5">
                                                    <HeaderTemplate>RCode</HeaderTemplate>
                                                    <HeaderStyle Width="60px" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t5" runat="server" Text='<%#Eval("sg1_t5") %>' Width="100%" ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Y</HeaderTemplate>
                                                    <HeaderStyle Width="40px" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t6" runat="server" Text='<%#Eval("sg1_t6") %>' Width="100%" ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Num Dt</HeaderTemplate>
                                                    <HeaderStyle Width="100px" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t7" runat="server" Text='<%#Eval("sg1_t7") %>' Width="100%" ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Cost CD</HeaderTemplate>
                                                    <HeaderStyle Width="80px" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t8" runat="server" Text='<%#Eval("sg1_t8") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField SortExpression="sg1_t9">
                                                    <HeaderTemplate>BranchCD</HeaderTemplate>
                                                    <HeaderStyle Width="60px" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t9" runat="server" Text='<%#Eval("sg1_t9") %>' Width="100%" ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Ref Date</HeaderTemplate>
                                                    <HeaderStyle Width="100px" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t10" runat="server" Text='<%#Eval("sg1_t10") %>' Width="100%" ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>TfCdr</HeaderTemplate>
                                                    <HeaderStyle Width="80px" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t11" runat="server" Text='<%#Eval("sg1_t11") %>' Width="100%" ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>TfCcr</HeaderTemplate>
                                                    <HeaderStyle Width="80px" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t12" runat="server" Text='<%#Eval("sg1_t12") %>' Width="100%" ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Remarks</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t13" runat="server" Text='<%#Eval("sg1_t13") %>' Width="100%" ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t14</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t14" runat="server" Text='<%#Eval("sg1_t14") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%" ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t15</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t15" runat="server" Text='<%#Eval("sg1_t15") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%" ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t16</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t16" runat="server" Text='<%#Eval("sg1_t16") %>' Width="100%" ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#1797C0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader2" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager2" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem2" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        </fin:CoolGridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>
                <div class="col-md-12" style="display: none">
                    <div>
                        <div class="box-body">
                            <asp:TextBox ID="txtrmk" runat="server" Width="99%" TextMode="MultiLine" onkeyup="max_length(this,200)" placeholder="Remarks" Style="visibility: hidden;"></asp:TextBox>
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
    <asp:HiddenField ID="hfCNote" runat="server" />

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

        function updatefield() {
            var colTot;
            var grid = document.getElementById("<%= sg1.ClientID%>");
            for (var i = 0; i < grid.rows.length - 1; i++) {
                colTot = document.getElementById('ContentPlaceHolder1_sg1_sg1_t1_' + i).value;

                if (colTot.length > 2) {
                    document.getElementById('ContentPlaceHolder1_sg1_sg1_t6_' + i).value = "Y";
                }
                else
                    document.getElementById('ContentPlaceHolder1_sg1_sg1_t6_' + i).value = "";

                //row total is total of total_qty field row wise

            }
        }



    </script>


    <asp:Label ID="lbl1a" runat="server" Text="TC" Style="width: 22px; display: none" CssClass="col-sm-1 control-label"></asp:Label>
    <asp:Label ID="txtcustPo" runat="server" Style="visibility: hidden"></asp:Label>
    <asp:HiddenField ID="TabName" runat="server" />
</asp:Content>
