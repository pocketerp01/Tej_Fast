<%@ Page Language="C#" AutoEventWireup="true" Inherits="om_mul_vch" CodeFile="om_mul_vch.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="shortcut icon" type="image/ico" href="images/finsys _small.jpg" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title></title>
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport" />

    <link rel="stylesheet" href="../tej-base/bootstrap/css/bootstrap.min.css" />

    <link rel="stylesheet" href="../tej-base/font-awesome/4.4.0/css/font-awesome.min.css" />
    <link rel="stylesheet" href="../tej-base/ionicons/2.0.1/css/ionicons.min.css" />

    <link rel="stylesheet" href="../tej-base/dist/css/AdminLTE.min.css" />
    <link rel="stylesheet" href="../tej-base/dist/css/skins/_all-skins.min.css" />

    <script src="../tej-base/Scripts/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../tej-base/Scripts/jquery.colorbox.js" type="text/javascript"></script>
    <script src="../tej-base/Scripts/jquery.colorbox-min.js" type="text/javascript"></script>
    <script src="../tej-base/Scripts/temp.js" type="text/javascript"></script>

    <script src="../tej-base/Scripts/shortcut.js" type="text/javascript"></script>


    <link type="text/css" rel="Stylesheet" href="../tej-base/Scripts/colorbox.css" />

    <script src="../tej-base/Scripts/jquery-ui.min.js" type="text/javascript"></script>

    <script src="../tej-base/Scripts/gridviewScroll.min.js" type="text/javascript"></script>
    <link href="../tej-base/css/GridviewScroll2.css" type="text/css" rel="Stylesheet" />

    <script type="text/javascript">
        $(document).ready(function () {
            //gridviewScroll('#<%=sg1.ClientID%>', gridDiv, 1, 3);
            calculateSum();
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
            function calculateSum() {
                var grid = document.getElementById("<%= sg1.ClientID%>");
                var drAmtTot = 0;
                var crAmtTot = 0;

                for (var i = 0; i < grid.rows.length - 1; i++) {

                    drAmtTot += fill_zero(document.getElementById('sg1_sg1_t1_' + i).value * 1);
                    crAmtTot += fill_zero(document.getElementById('sg1_sg1_t2_' + i).value * 1);
                }

                document.getElementById('totDrAmt').value = fill_zero(drAmtTot).toFixed(3);
                document.getElementById('totCrAmt').value = fill_zero(crAmtTot).toFixed(3);
            }
            function fill_zero(val) { if (isNaN(val)) return 0; if (isFinite(val)) return val; }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("input").not($(":image")).keypress(function (evt) {
                if (evt.keyCode == 13) {

                    iname = $(this).val();
                    if (iname !== 'Submit') {
                        var fields = $(this).parents('form:eq(0),body').find('button,input,textarea,select,image');
                        var index = fields.index(this);
                        if (index > -1 && (index + 1) < fields.length) {
                            fields.eq(index + 1).focus();
                            fields.eq(index + 1).select();
                        }
                        return false;
                    }
                }
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server" style="padding-top: 30px;">
        <section class="content-header">
            <table style="width: 100%">
                <tr>
                    <td>
                        <button type="submit" id="btnnew" class="btn btn-info" style="width: 100px;" runat="server" accesskey="N" onserverclick="btnnew_Click"><u>N</u>ew</button>
                        <button type="submit" id="btnedit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="i" onserverclick="btnedit_Click">Ed<u>i</u>t</button>
                        <button type="submit" id="btnsave" class="btn btn-info" style="width: 100px;" runat="server" accesskey="s" onserverclick="btnsave_Click"><u>S</u>ave</button>
                        <button type="submit" id="btnprint" class="btn btn-info" style="width: 100px;" runat="server" accesskey="P" onserverclick="btnprint_Click"><u>P</u>rint</button>
                        <button type="submit" id="btndel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="l" onserverclick="btndel_Click">De<u>l</u>ete</button>
                        <button type="submit" id="btnlist" class="btn btn-info" style="width: 100px;" runat="server" accesskey="t" onserverclick="btnlist_Click">Lis<u>t</u></button>
                        <button type="submit" id="btncancel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="c" onserverclick="btncancel_Click"><u>C</u>ancel</button>
                        <button type="submit" id="btnexit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="X" onserverclick="btnexit_Click">E<u>x</u>it</button>
                    </td>
                    <td>
                        <asp:Label ID="lblheaderx" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label>
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
                                <label id="lbl1" runat="server" class="col-sm-4 control-label" title="lbl1">Voucher No.</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtvchnum" runat="server" Placeholder="Vch No" CssClass="form-control" Height="28px" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col-sm-5">
                                    <asp:TextBox ID="txtvchdate" runat="server" Placeholder="Vch Date" CssClass="form-control" Height="28px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label1" runat="server" class="col-sm-3 control-label" title="lbl1">TAX/VAT Class</label>                                
                                <div class="col-sm-1">
                                <asp:ImageButton ID="btnGstClass" runat="server" ImageUrl="~/tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnGstClass_Click"  />
                                    </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtGstCode" runat="server" Placeholder="Code" CssClass="form-control" Height="28px" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col-sm-5">
                                    <asp:TextBox ID="txtGstName" runat="server" Placeholder="Name" CssClass="form-control" Height="28px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">                            
                            <div class="form-group">
                                <label id="Label2" runat="server" class="col-sm-4 control-label" title="lbl1">Ref No.</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtRefNo" runat="server" Placeholder="Ref No" CssClass="form-control" Height="28px" ></asp:TextBox>
                                </div>
                                <div class="col-sm-5">
                                    <asp:TextBox ID="txtRefDt" runat="server" Placeholder="Ref Date" CssClass="form-control" Height="28px" ></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label3" runat="server" class="col-sm-4 control-label" title="lbl1">MRR/SRV No.</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtMRRNo" runat="server" Placeholder="MRR/SRV No" CssClass="form-control" Height="28px" ></asp:TextBox>
                                </div>
                                <div class="col-sm-5">
                                    <table>
                                        <tr>
                                            <td>
                                    <asp:TextBox ID="txtMRRDt" runat="server" Placeholder="MRR/SRV Date" CssClass="form-control" Height="28px" Width="80px"></asp:TextBox>
                                                </td>
                                            <td>
                                    <asp:Button ID="btnView" runat="server" Text="View Bill" OnClick="btnView_Click" />
                                                </td>
                                        </tr>
                                        </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">
                                <li><a href="#DescTab" id="tab1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab">Item Details</a></li>
                                <li><a href="#DescTab2" id="tab2" runat="server" aria-controls="DescTab2" role="tab" data-toggle="tab">Comm.Terms</a></li>
                                <li><a href="#DescTab3" id="tab3" runat="server" aria-controls="DescTab3" role="tab" data-toggle="tab">Reel Details</a></li>
                                <li><a href="#DescTab4" id="tab4" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Lot.Dtl</a></li>
                                <li><a href="#DescTab5" id="tab5" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Inv.Dtl</a></li>
                                <li><a href="#DescTab6" id="tab6" runat="server" aria-controls="DescTab6" role="tab" data-toggle="tab">UDF Data</a></li>
                            </ul>

                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane active" id="DescTab">
                                    <div class="lbBody" id="gridDiv" style="box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <fin:CoolGridView ID="sg1" runat="server" Width="100%" AutoGenerateColumns="False"
                                            Style="background-color: #FFFFFF; color: White;" Font-Size="13px" Height="250px"
                                            OnRowCommand="sg1_RowCommand" OnRowDataBound="sg1_RowDataBound" OnRowCreated="sg1_RowCreated">
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
                                                 <asp:TemplateField>
                                                    <HeaderTemplate>Add Cost Center</HeaderTemplate>                                                     
                                                     <HeaderStyle Width="80px" />
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btnadd_CC" runat="server" CommandName="SG1_ROW_ADD_CC" ImageAlign="Middle" ImageUrl="~/tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Acc" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="sg1_h10" HeaderText="sg1_h10" />

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Add</HeaderTemplate>
                                                    <HeaderStyle Width="40px" />
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btnadd" runat="server" CommandName="SG1_ROW_ADD" ImageAlign="Middle" ImageUrl="~/tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Acc" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Del</HeaderTemplate>
                                                    <HeaderStyle Width="40px" />
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btnrmv" runat="server" CommandName="SG1_RMV" ImageUrl="~/tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Acc" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="sg1_srno" HeaderText="sg1_srno" />
                                                <asp:BoundField DataField="sg1_f1" HeaderText="sg1_f1" />
                                                <asp:BoundField DataField="sg1_f2" HeaderText="sg1_f2" />
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Rev Code</HeaderTemplate>
                                                    <HeaderStyle Width="60px" />
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btnadd_Acc" runat="server" CommandName="SG1_ROW_ADD_ACC" ImageAlign="Middle" ImageUrl="~/tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Rev Code" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="sg1_f3" HeaderText="sg1_f3" />
                                                <asp:BoundField DataField="sg1_f4" HeaderText="sg1_f4" />
                                                <asp:BoundField DataField="sg1_f5" HeaderText="sg1_f5" />

                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t1</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t1" runat="server" Text='<%#Eval("sg1_t1") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%" Style="text-align: right" onkeyup="calculateSum()"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t2</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t2" runat="server" Text='<%#Eval("sg1_t2") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%" Style="text-align: right" onkeyup="calculateSum()"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t3</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t3" runat="server" Text='<%#Eval("sg1_t3") %>'  Width="100%"  onkeyup="calculateSum()"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t4</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t4" runat="server" Text='<%#Eval("sg1_t4") %>'  Width="100%"  onkeyup="calculateSum()"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t5</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t5" runat="server" Text='<%#Eval("sg1_t5") %>'  Width="100%"  onkeyup="calculateSum()"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t6</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t6" runat="server" Text='<%#Eval("sg1_t6") %>'  Width="100%"  onkeyup="calculateSum()"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t7</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t7" runat="server" Text='<%#Eval("sg1_t7") %>'  Width="100%"  onkeyup="calculateSum()"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t8</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t8" runat="server" Text='<%#Eval("sg1_t8") %>'  Width="100%"  onkeyup="calculateSum()"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t9</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t9" runat="server" Text='<%#Eval("sg1_t9") %>'  Width="100%" onkeyup="calculateSum()"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t10</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t10" runat="server" Text='<%#Eval("sg1_t10") %>' Width="100%" onkeyup="calculateSum()"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t11</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t11" runat="server" Text='<%#Eval("sg1_t11") %>' Width="100%" onkeyup="calculateSum()"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t12</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t12" runat="server" Text='<%#Eval("sg1_t12") %>' Width="100%" onkeyup="calculateSum()"></asp:TextBox>
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
                                                        <asp:TextBox ID="sg1_t14" runat="server" Text='<%#Eval("sg1_t14") %>'  Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t15</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t15" runat="server" Text='<%#Eval("sg1_t15") %>'  Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t16</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t16" runat="server" Text='<%#Eval("sg1_t16") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t17</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t17" runat="server" Text='<%#Eval("sg1_t17") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t18</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t18" runat="server" Text='<%#Eval("sg1_t18") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t19</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t19" runat="server" Text='<%#Eval("sg1_t19") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t20</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t20" runat="server" Text='<%#Eval("sg1_t20") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t21</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t21" runat="server" Text='<%#Eval("sg1_t21") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--                                                <asp:TemplateField>
                                                    <HeaderTemplate>Tcode</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btntax" runat="server" CommandName="SG1_ROW_TAX" ImageAlign="Middle" ImageUrl="~/tej-base/images/Btn_addn.png" Width="20px" ToolTip="Choose Tax" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
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
                                <div role="tabpanel" class="tab-pane active" id="DescTab2"></div>
                                <div role="tabpanel" class="tab-pane active" id="DescTab3"></div>
                                <div role="tabpanel" class="tab-pane active" id="DescTab4"></div>
                                <div role="tabpanel" class="tab-pane active" id="DescTab5"></div>
                                <div role="tabpanel" class="tab-pane active" id="DescTab6"></div>
                            </div>
                        </div>
                    </div>
                
                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <label id="Label4" runat="server" class="col-sm-3 control-label" title="lbl1">Tax Type</label>                                
                                <div class="col-sm-1">
                                <asp:ImageButton ID="btnTax" runat="server" ImageUrl="~/tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnTax_Click" />
                                    </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtTaxCode" runat="server" Placeholder="Code" CssClass="form-control" Height="28px" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col-sm-5">
                                    <asp:TextBox ID="txtTaxName" runat="server" Placeholder="Name" CssClass="form-control" Height="28px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label5" runat="server" class="col-sm-4 control-label" title="lbl1">S.T.38 No.</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtSt38" runat="server" Placeholder="S.T.38 No." CssClass="form-control" Height="28px" ></asp:TextBox>
                                </div>
                                <label id="Label6" runat="server" class="col-sm-2 control-label" title="lbl1">Qty</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtSTQty" runat="server" Placeholder="Qty" CssClass="form-control" Height="28px"></asp:TextBox>
                                </div>
                            </div>
                            
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <label id="Label7" runat="server" class="col-sm-6 control-label" title="lbl1">Total Debit Amount</label>                                
                                <div class="col-sm-6">
                                    <asp:TextBox ID="totDrAmt" runat="server" Placeholder="Amount" CssClass="form-control" Height="28px" ReadOnly="true" style="text-align:right"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label8" runat="server" class="col-sm-6 control-label" title="lbl1">Total Credit Amount</label>                                
                                <div class="col-sm-6">
                                    <asp:TextBox ID="totCrAmt" runat="server" Placeholder="Amount" CssClass="form-control" Height="28px" ReadOnly="true" style="text-align:right"></asp:TextBox>
                                </div>
                            </div>
                            
                        </div>
                    </div>
                </div>
            </div>                        
            </section>
        <div class="col-md-12">
            <asp:TextBox ID="txtremarks" runat="server" Width="99%" TextMode="MultiLine" onkeyup="max_length(this,200)" placeholder="Add Your Remakrs..."></asp:TextBox>
        </div>
        <label id="lblEdtBy" runat="server" class="col-sm-12 control-label" title="lbl1"></label>
        <asp:Button ID="btnhideF" runat="server" OnClick="btnhideF_Click" Style="display: none" />
        <asp:Button ID="btnhideF_s" runat="server" OnClick="btnhideF_s_Click" Style="display: none" />
        <asp:HiddenField ID="hffield" runat="server" />
        <asp:HiddenField ID="hf1" runat="server" />
        <asp:HiddenField ID="edmode" runat="server" />
        <asp:HiddenField ID="popselected" runat="server" />
        <div class="col-sm-8" style="display: none">
            <label for="exampleInputEmail1">Type</label>
            <asp:Label ID="lbltypename" runat="server"></asp:Label>
        </div>
        <asp:HiddenField ID="hfGridView1SV" runat="server" />
        <asp:HiddenField ID="hfGridView1SH" runat="server" />
    </form>
</body>
</html>
