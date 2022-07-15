<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="om_disptgt" CodeFile="om_disptgt.aspx.cs" %>

<%--  --%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../tej-base/Scripts/gridviewScroll.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function () {
           <%-- gridviewScroll('#<%=sg1.ClientID%>', gridDiv, 1, 5);--%>
            Cal();
        });
        function fill_zero(val) { if (isNaN(val)) return 0; if (isFinite(val)) return val; }
        function Cal() {
            var edtot = 0, CSTOT = 0, sSTOT = 0;

            var gridCount = $("[id*=sg1].GridviewScrollItem2").length - 0;
            for (var i = 0; i < gridCount; i++) {
                var rowI = i;
                //if (i == 0) rowI = 0;
                //else rowI = i * 2;
                debugger;
                edtot += $("#ContentPlaceHolder1_sg1_sg1_t1_" + rowI + "").val() * 1;
                $("#ContentPlaceHolder1_sg1_sg1_t3_" + rowI + "").val($("#ContentPlaceHolder1_sg1_sg1_t1_" + rowI + "").val() * 1);
                CSTOT += $("#ContentPlaceHolder1_sg1_sg1_t6_" + rowI + "").val() * 1;
                sSTOT += (($("#ContentPlaceHolder1_sg1_sg1_t1_" + rowI + "").val() * 1) - ($("#ContentPlaceHolder1_sg1_sg1_t6_" + rowI + "").val() * 1));
            }            
            $("#ContentPlaceHolder1_lbltot1").html("Tgt Crate : " + edtot);
            $("#ContentPlaceHolder1_lbltot2").html("Ach Crate : " + CSTOT);
            $("#ContentPlaceHolder1_lbltot3").html("Bal Crate : " + edtot);
        }

        function submitFile() {
            $("#<%= btnupload.ClientID%>").click();
            ProgressBar();
        };
        function ProgressBar() {
            if (document.getElementById('<%=fupl.ClientID %>').value != "") {
                document.getElementById("ContentPlaceHolder1_divProgress").style.display = "block";
                document.getElementById("ContentPlaceHolder1_divUpload").style.display = "block";
                id = setInterval("progress()", 20);
                return true;
            }
            else {
                alert("Select a file to upload");
                return false;
            }
        }
        function progress() {
            size = size + 1;
            if (size > 199) {
                clearTimeout(id);
            }
            document.getElementById("ContentPlaceHolder1_divProgress").style.width = size + "pt";
            document.getElementById("<%=lblPercentage.ClientID %>").
                firstChild.data = parseInt(size / 2) + "%";
        }
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
                                <asp:Label ID="lbl1" runat="server" Text="lbl1" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Entry No.</asp:Label>
                                <div class="col-sm-1">
                                    <asp:Label ID="lbl1a" runat="server" Text="TC" Style="width: 22px; float: right;" CssClass="col-sm-1 control-label"></asp:Label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtvchnum" runat="server" Width="100%" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label1" runat="server" Text="lbl1" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Entry_Date</asp:Label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtvchdate" placeholder="Date" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtvchdate_CalendarExtender" runat="server"
                                        Enabled="True" TargetControlID="txtvchdate"
                                        Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="Maskedit1" runat="server" Mask="99/99/9999"
                                        MaskType="Date" TargetControlID="txtvchdate" />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="Label14" runat="server" Text="This sheet is to be used to make the Daily Dispatch Target Sheet" CssClass="col-sm-12"></asp:Label>
                                <div class="col-sm-1" style="display: none">
                                    <asp:ImageButton ID="btnlbl7" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl7_Click" Visible="false" />
                                </div>
                                <div class="col-sm-3" style="display: none">
                                    <asp:TextBox ID="txtlbl7" runat="server" CssClass="form-control" MaxLength="80" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col-sm-6" style="display: none">
                                    <asp:TextBox ID="txtlbl7a" runat="server" CssClass="form-control" Width="100%" MaxLength="25" ReadOnly="true" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <asp:Label ID="lbl4" runat="server" Text="lbl4" CssClass="col-sm-4" Font-Size="14px">Pick Customer</asp:Label>
                                <div class="col-sm-8">
                                    <button id="btnPickCustomer" type="submit" class="btn btn-block btn-success" style="width: 100%;" runat="server" onserverclick="btnPickCustomer_ServerClick">Pick Customers, Items</button>
                                    <br />
                                </div>
                                <div class="col-sm-1" style="display: none">
                                    <asp:ImageButton ID="btnlbl4" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl4_Click" />
                                </div>
                                <div class="col-sm-3" style="display: none">
                                    <asp:TextBox ID="txtlbl4" runat="server" Width="100%" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-sm-6" style="display: none">
                                    <asp:TextBox ID="txtlbl4a" runat="server" ReadOnly="true" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group" style="display: none">
                                <asp:Label ID="Label20" runat="server" Text="EntryNo" Font-Bold="true" CssClass="col-sm-3 control-label"></asp:Label>

                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtEntryNO" runat="server" ReadOnly="true" CssClass="form-control" MaxLength="10"></asp:TextBox>
                                </div>

                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtEntryDt" runat="server" ReadOnly="true" CssClass="form-control" MaxLength="10"></asp:TextBox>
                                </div>

                            </div>
                            <%--<div class="form-group" style="visibility:hidden">
                                           <asp:Label ID="Label2" runat="server" Text="Deactivated Y/N" Font-Bold="true" CssClass="col-sm-3 control-label"></asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtstatus" runat="server" CssClass="form-control" MaxLength="1" Text="N" ForeColor="Red"></asp:TextBox>
                                </div>
                                <asp:Label ID="lbldeac" runat="server" Text="(Y:Close,N:Open)" Font-Size="Small" Font-Bold="true" CssClass="col-sm-6 control-label" ForeColor="Red"></asp:Label>
                            </div>--%>
                        </div>
                    </div>
                </div>

                <section class="col-lg-12 connectedSortable">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">
                                <li><a href="#DescTab" id="tab1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab">Details</a></li>
                                <li><a href="#DescTab2" id="tab2" runat="server" aria-controls="DescTab2" role="tab" data-toggle="tab">Comm.Terms</a></li>
                                <li><a href="#DescTab3" id="tab3" runat="server" aria-controls="DescTab3" role="tab" data-toggle="tab">Reel/Lot Dtl</a></li>
                                <li><a href="#DescTab4" id="tab4" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Lot.Dtl</a></li>
                                <li><a href="#DescTab5" id="tab5" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Inv.Dtl</a></li>
                                <li><a href="#DescTab6" id="tab6" runat="server" aria-controls="DescTab6" role="tab" data-toggle="tab">UDF Data</a></li>
                                <li>&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="lbltot1" runat="server"></asp:Label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="lbltot2" runat="server"></asp:Label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="lbltot3" runat="server"></asp:Label>
                                </li>
                                <li style="display: none">
                                    <asp:FileUpload ID="fupl" runat="server" onchange="submitFile()" />
                                    <asp:Button ID="btnupload" runat="server" Text="Upload" class="myButton" Style="display: none" OnClick="btnupload_Click" />
                                    <asp:Label ID="lblU" runat="server"></asp:Label>
                                    <div id="divUpload" style="display: none" runat="server">
                                        <div id="Div1" style="width: 200pt; height: 15px; border: solid 1pt gray" runat="server">
                                            <div id="divProgress" runat="server" style="width: 1pt; height: 15px; background-color: #1797c0; display: none">
                                            </div>
                                        </div>
                                        <div style="width: 200pt; text-align: center;">
                                            <asp:Label ID="lblPercentage" runat="server" Text="Label"></asp:Label>
                                        </div>
                                    </div>
                                </li>
                            </ul>

                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane active" id="DescTab">
                                    <%--<div class="lbBody" id="gridDiv" style="color: White; height: 300px; overflow: hidden; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <fin:CoolGridView ID="sg1" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="1500px" Font-Size ="13px" 
                                            AutoGenerateColumns="False" OnRowDataBound="sg1_RowDataBound"
                                            OnRowCommand="sg1_RowCommand">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />--%>
                                    <div class="lbBody" id="gridDiv" style="color: White; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                        <fin:CoolGridView ID="sg1" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="100%" Height="250px" Font-Size="13px"
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

                                                <asp:TemplateField ItemStyle-Width="5px" Visible="false">
                                                    <HeaderTemplate>Add</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btnadd" runat="server" CommandName="SG1_ROW_ADD" ImageAlign="Middle" ImageUrl="../tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Row" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>Del</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btnrmv" runat="server" CommandName="SG1_RMV" ImageUrl="../tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Row" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="sg1_srno" HeaderText="SrNo" ItemStyle-Width="80px" />
                                                <asp:BoundField DataField="sg1_f1" HeaderText="Order No" ItemStyle-Width="80px" />
                                                <asp:BoundField DataField="sg1_f2" HeaderText="Order Dt" ItemStyle-Width="80px" />
                                                <asp:BoundField DataField="sg1_f3" HeaderText="Customer" ItemStyle-Width="80px" />
                                                <asp:BoundField DataField="sg1_f4" HeaderText="Item Name" ItemStyle-Width="80px" />
                                                <asp:BoundField DataField="sg1_f5" HeaderText="Part No." ItemStyle-Width="80px" />
                                                <asp:BoundField DataField="sg1_f6" HeaderText="-" />

                                                <asp:TemplateField ItemStyle-Width="15px">
                                                    <HeaderTemplate>Tot Sch</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t1" runat="server" Text='<%#Eval("sg1_t1") %>' onkeyup="Cal()" onChange="Cal()" Width="100%" MaxLength="10" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Tot Desp</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t2" runat="server" Text='<%#Eval("sg1_t2") %>' Width="100%" MaxLength="6" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Bal</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t3" runat="server" Text='<%#Eval("sg1_t3") %>' onChange="Cal()" Width="100%" MaxLength="6" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                        <asp:CheckBox ID="sg1_chk3" runat="server" Visible="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="80px">
                                                    <HeaderTemplate>Target</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t4" runat="server" Text='<%#Eval("sg1_t4") %>' onChange="Cal()" onkeypress="return isDecimalKey(event)" Width="100%" MaxLength="6"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Done</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t5" runat="server" Text='<%#Eval("sg1_t5") %>' onChange="Cal()" Width="100%" onkeypress="return isDecimalKey(event)" MaxLength="6"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>June</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t6" runat="server" Text='<%#Eval("sg1_t6") %>' onChange="Cal()" Width="100%" onkeypress="return isDecimalKey(event)" MaxLength="6"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>July</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t7" runat="server" Text='<%#Eval("sg1_t7") %>' onChange="Cal()" Width="100%" onkeypress="return isDecimalKey(event)" MaxLength="6"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>August</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t8" runat="server" Text='<%#Eval("sg1_t8") %>' onChange="Cal()" Width="100%" onkeypress="return isDecimalKey(event)" MaxLength="6"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>September</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t9" runat="server" Text='<%#Eval("sg1_t9") %>' onChange="Cal()" Width="100%" onkeypress="return isDecimalKey(event)" MaxLength="6"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>October</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t10" runat="server" Text='<%#Eval("sg1_t10") %>' onChange="Cal()" Width="100%" onkeypress="return isDecimalKey(event)" MaxLength="6"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>November</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t11" runat="server" Text='<%#Eval("sg1_t11") %>' onChange="Cal()" Width="100%" onkeypress="return isDecimalKey(event)" MaxLength="6"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>December</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t12" runat="server" Text='<%#Eval("sg1_t12") %>' onChange="Cal()" Width="100%" onkeypress="return isDecimalKey(event)" MaxLength="6"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>January</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t13" runat="server" Text='<%#Eval("sg1_t13") %>' onChange="Cal()" Width="100%" onkeypress="return isDecimalKey(event)" MaxLength="6"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>Febuary</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t14" runat="server" Text='<%#Eval("sg1_t14") %>' onChange="Cal()" Width="100%" onkeypress="return isDecimalKey(event)" MaxLength="6"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>March</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t15" runat="server" Text='<%#Eval("sg1_t15") %>' Width="100%" onChange="Cal()" onkeypress="return isDecimalKey(event)" MaxLength="6"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>Total</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t16" runat="server" Text='<%#Eval("sg1_t16") %>' Width="100%" onkeypress="return isDecimalKey(event)" MaxLength="6" ReadOnly="true"></asp:TextBox>
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
                            </div>
                        </div>
                    </div>
                </section>

                <div class="col-md-12" style="display: none;">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <asp:Label ID="Label11" runat="server" Text="lbl7" CssClass="col-sm-1 control-label" Font-Size="14px" Font-Bold="True">Footer_Notes</asp:Label>
                                <div class="col-sm-11">
                                    <asp:TextBox ID="txtfootnote" MaxLength="200" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
                <div class="col-md-12">
                    <div>
                        <div class="box-body">
                            <asp:TextBox ID="txtrmk" runat="server" MaxLength="80" Width="99%" CssClass="form-control" TextMode="MultiLine" placeholder="Remarks"></asp:TextBox>
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

</asp:Content>
