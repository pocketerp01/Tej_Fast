<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="om_dbd_bpln2" CodeFile="om_dbd_bpln2.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../tej-base/Scripts/gridviewScroll.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            //gridviewScroll('#<%=sg1.ClientID%>', gridDiv, 1, 0);
            //calculateSum();
        });
        function gridviewScroll(gridId, gridDiv, headerFreeze, rowFreeze) {
            $(gridId).gridviewScroll({
                width: gridDiv.offsetWidth,
                height: gridDiv.offsetHeight,
                headerrowcount: headerFreeze,
                //freezesize: rowFreeze,
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
    </script>
    <script type="text/javascript">
        var SelectedRow = null;
        var SelectedRowIndex = null;
        var SelectedCol = null;
        var SelectedColIndex = null;
        var LeftBound = null;
        var RightBound = null;
        var UpperBound = null;
        var LowerBound = null;

        window.onload = function () {
            UpperBound = parseInt('<%= this.sg1.Rows.Count %>') - 1;
            RightBound = parseInt('<%= this.sg1.Columns.Count %>') - 1;
            LowerBound = 0; LeftBound = 0;
            SelectedRowIndex = -1;
            SelectedColIndex = 1;
        }

        function SelectRow(CurrentRow, RowIndex) {
            if (SelectedRow == CurrentRow || RowIndex > UpperBound || RowIndex < LowerBound) return;

            if (SelectedRow != null) {
                SelectedRow.style.backgroundColor = SelectedRow.originalBackgroundColor;
                SelectedRow.style.color = SelectedRow.originalForeColor;
            }

            if (CurrentRow != null) {
                CurrentRow.originalBackgroundColor = CurrentRow.style.backgroundColor;
                CurrentRow.originalForeColor = CurrentRow.style.color;
                CurrentRow.style.backgroundColor = '#DCFC5C';
                CurrentRow.style.color = 'Black';
            }

            SelectedRow = CurrentRow;
            SelectedRowIndex = RowIndex;
            setTimeout("SelectedRow.focus();", 0);
        }

        function SelectSibling(e) {
            var e = e ? e : window.event;
            var KeyCode = e.which ? e.which : e.keyCode;

            if (KeyCode == 40) SelectRow(SelectedRow.nextSibling, SelectedRowIndex + 1);
            else if (KeyCode == 38) SelectRow(SelectedRow.previousSibling, SelectedRowIndex - 1);

            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper">
        <section class="content-header">
            <table style="width: 100%">
                <tr>
                    
                    <td style="text-align: left">
                        <button type="submit" id="btnnew" class="btn btn-info" style="width: 100px;" runat="server" onserverclick="btnnew_ServerClick"><u>S</u>how</button>
                        <button type="submit" id="btnedit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="i" onserverclick="btnedit_ServerClick">Ed<u>i</u>t</button>
                        <button type="submit" id="btnsave" class="btn btn-info" style="width: 100px;" runat="server" accesskey="s" onserverclick="btnsave_ServerClick"><u>S</u>ave</button>
                        <button type="submit" id="btnSF" class="btn btn-info" style="width: 100px;" runat="server" onserverclick="btnSF_ServerClick">SF Req</button>
                        <button type="submit" id="btncancel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="c" onserverclick="btncancel_ServerClick"><u>C</u>ancel</button>
                        <button type="submit" id="btnexit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="X" onserverclick="btnexit_ServerClick">E<u>x</u>it</button>
                        <button type="submit" id="btnprint" class="btn btn-info" style="width: 100px;" runat="server" accesskey="P" onserverclick="btnprint_ServerClick">Ex<u>p</u>ort</button>

                        &nbsp;&nbsp;&nbsp;&nbsp;

                        <button type="submit" id="btn1" class="btn btn-info" style="width: 100px;" runat="server" onserverclick="btn1_ServerClick">PR Pending</button>
                        <button type="submit" id="bt2" class="btn btn-info" style="width: 100px;" runat="server" onserverclick="bt2_ServerClick">PO Pending</button>
                        <button type="submit" id="btn3" class="btn btn-info" style="width: 100px;" runat="server" onserverclick="btn3_ServerClick">QA Pending</button>
                        <button type="submit" id="btn4" class="btn btn-info" style="width: 100px;" runat="server" onserverclick="btn4_ServerClick">Sales Plan</button>
                        <button type="submit" id="btn5" class="btn btn-info" style="width: 100px;" runat="server" visible="false" onserverclick="btn5_ServerClick">Tfr Pend/Acpt</button>
                        
                        <button type="submit" id="btndel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="l" onserverclick="btndel_ServerClick">De<u>l</u>ete</button>
                        <button type="submit" id="btnlist" class="btn btn-info" style="width: 100px;" runat="server" accesskey="t" onserverclick="btnlist_ServerClick">Sta<u>t</u>us</button>
                    </td>
                    <td>
                        <asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label></td>
                </tr>
            </table>
        </section>

        <section class="content">
            <div class="row">
                <div class="col-md-7" id="grid1" runat="server">
                    <div>
                        <div class="box-header" style="display: none">
                            <i class="fa fa-clipboard"></i>
                            <h3 class="box-title">
                                <asp:Label ID="lblSg1" runat="server"></asp:Label>
                            </h3>
                        </div>
                        <div class="box-body">
                            <div class="form-group">
                                <div class="lbBody" id="gridDiv" style="color: White; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                    <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                                    <fin:CoolGridView ID="sg1" runat="server" ForeColor="#333333"
                                        Style="background-color: #FFFFFF; color: White;" Width="100%" Height="450px" Font-Size="11px"
                                        AutoGenerateColumns="false" OnRowDataBound="sg1_RowDataBound" OnSelectedIndexChanged="sg1_SelectedIndexChanged"
                                        OnRowCommand="sg1_RowCommand" OnRowCreated="sg1_RowCreated" PageSize="50" AllowPaging="true" OnPageIndexChanging="sg1_PageIndexChanging">
                                        <Columns>
                                            <asp:CommandField ButtonType="Image" HeaderText="Sel" HeaderStyle-Width="25px" ShowSelectButton="True"
                                                SelectImageUrl="~/tej-base/images/tick.png">
                                                <ItemStyle Width="25px" CssClass="hidden"></ItemStyle>
                                                <HeaderStyle CssClass="hidden" />
                                            </asp:CommandField>

                                            <asp:BoundField HeaderText="sg1_t1" DataField="sg1_t1" />
                                            <asp:BoundField HeaderText="sg1_t2" DataField="sg1_t2" />
                                            <asp:BoundField HeaderText="sg1_t3" DataField="sg1_t3" />
                                            <asp:BoundField HeaderText="sg1_t4" DataField="sg1_t4" />
                                            <asp:BoundField HeaderText="sg1_t5" DataField="sg1_t5" />
                                            <asp:BoundField HeaderText="sg1_t6" DataField="sg1_t6" />
                                            <asp:BoundField HeaderText="sg1_t7" DataField="sg1_t7" />
                                            <asp:BoundField HeaderText="sg1_t8" DataField="sg1_t8" />
                                            <asp:BoundField HeaderText="sg1_t9" DataField="sg1_t9" />
                                            <asp:BoundField HeaderText="sg1_t10" DataField="sg1_t10" />
                                            <asp:BoundField HeaderText="sg1_t11" DataField="sg1_t11" />
                                            <asp:BoundField HeaderText="sg1_t12" DataField="sg1_t12" />
                                            <asp:BoundField HeaderText="sg1_t13" DataField="sg1_t13" />
                                            <asp:BoundField HeaderText="sg1_t14" DataField="sg1_t14" />
                                            <asp:BoundField HeaderText="sg1_t15" DataField="sg1_t15" />
                                            <asp:BoundField HeaderText="sg1_t16" DataField="sg1_t16" />
                                            <asp:BoundField HeaderText="sg1_t17" DataField="sg1_t17" />
                                            <asp:BoundField HeaderText="sg1_t18" DataField="sg1_t18" />
                                            <asp:BoundField HeaderText="sg1_t19" DataField="sg1_t19" />
                                            <asp:BoundField HeaderText="sg1_t20" DataField="sg1_t20" />
                                            <asp:BoundField HeaderText="sg1_t21" DataField="sg1_t21" />
                                            <asp:BoundField HeaderText="sg1_t22" DataField="sg1_t22" />
                                            <asp:BoundField HeaderText="sg1_t23" DataField="sg1_t23" />
                                            <asp:BoundField HeaderText="sg1_t24" DataField="sg1_t24" />
                                            <asp:BoundField HeaderText="sg1_t25" DataField="sg1_t25" />
                                            <asp:BoundField HeaderText="sg1_t26" DataField="sg1_t26" />
                                            <asp:BoundField HeaderText="sg1_t27" DataField="sg1_t27" />
                                            <asp:BoundField HeaderText="sg1_t28" DataField="sg1_t28" />
                                            <asp:BoundField HeaderText="sg1_t29" DataField="sg1_t29" />
                                            <asp:BoundField HeaderText="sg1_t30" DataField="sg1_t30" />
                                            <asp:BoundField HeaderText="sg1_t31" DataField="sg1_t31" />
                                            <asp:BoundField HeaderText="sg1_t32" DataField="sg1_t32" />
                                            <asp:BoundField HeaderText="sg1_t33" DataField="sg1_t33" />
                                            <asp:BoundField HeaderText="sg1_t34" DataField="sg1_t34" />
                                            <asp:BoundField HeaderText="sg1_t35" DataField="sg1_t35" />
                                            <asp:BoundField HeaderText="sg1_t36" DataField="sg1_t36" />
                                            <asp:BoundField HeaderText="sg1_t37" DataField="sg1_t37" />
                                            <asp:BoundField HeaderText="sg1_t38" DataField="sg1_t38" />
                                            <asp:BoundField HeaderText="sg1_t39" DataField="sg1_t39" />
                                            <asp:BoundField HeaderText="sg1_t40" DataField="sg1_t40" />
                                            <asp:BoundField HeaderText="sg1_t41" DataField="sg1_t41" />
                                            <asp:BoundField HeaderText="sg1_t42" DataField="sg1_t42" />
                                            <asp:BoundField HeaderText="sg1_t43" DataField="sg1_t43" />
                                            <asp:BoundField HeaderText="sg1_t44" DataField="sg1_t44" />
                                            <asp:BoundField HeaderText="sg1_t45" DataField="sg1_t45" />
                                            <asp:BoundField HeaderText="sg1_t46" DataField="sg1_t46" />
                                            <asp:BoundField HeaderText="sg1_t47" DataField="sg1_t47" />
                                            <asp:BoundField HeaderText="sg1_t48" DataField="sg1_t48" />
                                            <asp:BoundField HeaderText="sg1_t49" DataField="sg1_t49" />
                                            <asp:BoundField HeaderText="sg1_t50" DataField="sg1_t50" />
                                            <asp:BoundField HeaderText="sg1_t51" DataField="sg1_t51" />
                                            <asp:BoundField HeaderText="sg1_t52" DataField="sg1_t52" />
                                            <asp:BoundField HeaderText="sg1_t53" DataField="sg1_t53" />
                                            <asp:BoundField HeaderText="sg1_t54" DataField="sg1_t54" />
                                            <asp:BoundField HeaderText="sg1_t55" DataField="sg1_t55" />
                                            <asp:BoundField HeaderText="sg1_t56" DataField="sg1_t56" />
                                            <asp:BoundField HeaderText="sg1_t57" DataField="sg1_t57" />
                                            <asp:BoundField HeaderText="sg1_t58" DataField="sg1_t58" />
                                            <asp:BoundField HeaderText="sg1_t59" DataField="sg1_t59" />
                                            <asp:BoundField HeaderText="sg1_t60" DataField="sg1_t60" />
                                            <asp:BoundField HeaderText="sg1_t61" DataField="sg1_t61" />
                                            <asp:BoundField HeaderText="sg1_t62" DataField="sg1_t62" />
                                            <asp:BoundField HeaderText="sg1_t63" DataField="sg1_t63" />
                                            <asp:BoundField HeaderText="sg1_t64" DataField="sg1_t64" />
                                            <asp:BoundField HeaderText="sg1_t65" DataField="sg1_t65" />
                                            <asp:BoundField HeaderText="sg1_t66" DataField="sg1_t66" />
                                            <asp:BoundField HeaderText="sg1_t67" DataField="sg1_t67" />
                                            <asp:BoundField HeaderText="sg1_t68" DataField="sg1_t68" />
                                            <asp:BoundField HeaderText="sg1_t69" DataField="sg1_t69" />
                                            <asp:BoundField HeaderText="sg1_t70" DataField="sg1_t70" />
                                            <asp:BoundField HeaderText="sg1_t71" DataField="sg1_t71" />
                                            <asp:BoundField HeaderText="sg1_t72" DataField="sg1_t72" />
                                            <asp:BoundField HeaderText="sg1_t73" DataField="sg1_t73" />
                                            <asp:BoundField HeaderText="sg1_t74" DataField="sg1_t74" />
                                            <asp:BoundField HeaderText="sg1_t75" DataField="sg1_t75" />
                                            <asp:BoundField HeaderText="sg1_t76" DataField="sg1_t76" />
                                            <asp:BoundField HeaderText="sg1_t77" DataField="sg1_t77" />
                                            <asp:BoundField HeaderText="sg1_t78" DataField="sg1_t78" />
                                            <asp:BoundField HeaderText="sg1_t79" DataField="sg1_t79" />


                                        </Columns>
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <EditRowStyle BackColor="#999999" />
                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader2" />
                                        <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager2" />
                                        <PagerSettings FirstPageImageUrl="~/tej-base/css/images/lefts.png" LastPageImageUrl="~/tej-base/css/images/rights.png" Mode="NumericFirstLast" />
                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem2" />
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                    </fin:CoolGridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-5" id="grid2" runat="server">
                    <div>
                        <div class="box-header">
                            <i class="fa fa-clipboard"></i>
                            <h3 class="box-title">
                                <asp:Label ID="lblSg2" runat="server"></asp:Label>
                            </h3>
                        </div>
                        <div class="box-body">
                            <div class="form-group">
                                <div class="lbBody" id="gridDiv1" runat="server" style="color: White; height: 115px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                    <asp:GridView ID="sg2" runat="server" ForeColor="#333333"
                                        Style="background-color: #FFFFFF; color: White;" Width="400px" Font-Size="Smaller"
                                        AutoGenerateColumns="true" OnRowDataBound="sg2_RowDataBound" OnSelectedIndexChanged="sg2_SelectedIndexChanged">
                                        <Columns>
                                            <asp:CommandField ButtonType="Image" HeaderText="Sel" HeaderStyle-Width="25px" ShowSelectButton="True"
                                                SelectImageUrl="images/tick.png">
                                                <ItemStyle Width="25px"></ItemStyle>
                                            </asp:CommandField>
                                        </Columns>
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <EditRowStyle BackColor="#999999" />
                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader" />
                                        <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-5" id="grid3" runat="server">
                    <div>
                        <div class="box-header">
                            <i class="fa fa-clipboard"></i>
                            <h3 class="box-title">
                                <asp:Label ID="lblSg3" runat="server"></asp:Label>
                            </h3>
                        </div>
                        <div class="box-body">
                            <div class="form-group">
                                <div class="lbBody" id="gridDiv3" runat="server" style="color: White; height: 115px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                    <asp:GridView ID="sg3" runat="server" ForeColor="#333333"
                                        Style="background-color: #FFFFFF; color: White;" Width="400px" Font-Size="Smaller"
                                        AutoGenerateColumns="true" OnRowDataBound="sg3_RowDataBound">
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <EditRowStyle BackColor="#999999" />
                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader" />
                                        <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-5" id="grid4" runat="server">
                    <div>
                        <div class="box-header">
                            <i class="fa fa-clipboard"></i>
                            <h3 class="box-title">
                                <asp:Label ID="lblSg4" runat="server"></asp:Label>
                            </h3>
                        </div>
                        <div class="box-body">
                            <div class="form-group">
                                <div class="lbBody" id="gridDiv4" runat="server" style="color: White; height: 115px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                    <asp:GridView ID="sg4" runat="server" ForeColor="#333333"
                                        Style="background-color: #FFFFFF; color: White;" Width="400px" Font-Size="Smaller"
                                        AutoGenerateColumns="true" OnRowDataBound="sg4_RowDataBound">
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <EditRowStyle BackColor="#999999" />
                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader" />
                                        <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                    </asp:GridView>
                                </div>
                            </div>
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
    <asp:HiddenField ID="TabName" runat="server" />
</asp:Content>
