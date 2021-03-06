<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="oldmcdata" Title="Tejaxo" CodeFile="oldmcdata.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
                            <div class="form-group" id="DivParty" runat="server">
                                <label class="col-sm-2 control-label" id="lblParty" runat="server">Party</label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnAcode" runat="server" Height="22px"
                                        ImageUrl="~/tej-base/css/images/bdsearch5.png" ToolTip="Select Invoice" Width="24px"
                                        ImageAlign="Middle" OnClick="btnAcode_Click" />
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtacode" runat="server" placeholder="Code"
                                        ReadOnly="True" CssClass="form-control" Height="32px"></asp:TextBox>
                                </div>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtaname" runat="server" placeholder="Party Name"
                                        ReadOnly="True" CssClass="form-control" Height="32px"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label">Item Name</label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnIcode" runat="server" Height="22px"
                                        ImageUrl="~/tej-base/css/images/bdsearch5.png" ToolTip="Select Invoice" Width="24px"
                                        ImageAlign="Middle" OnClick="btnIcode_Click" />
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txticode" runat="server" placeholder="Code"
                                        ReadOnly="True" CssClass="form-control" Height="32px"></asp:TextBox>
                                </div>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtiname" runat="server" placeholder="Item Name"
                                        ReadOnly="True" CssClass="form-control" Height="32px"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group" id="DivAddress" runat="server">
                                <label class="col-sm-3 control-label">Address</label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtPaddr" runat="server" placeholder="Address"
                                        ReadOnly="True" CssClass="form-control" Height="32px"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="tdinvoice" runat="server" class="col-sm-2 control-label">Invoice No.</label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btninvno" runat="server" Height="22px"
                                        ImageUrl="~/tej-base/css/images/bdsearch5.png" ToolTip="Select Invoice" Width="24px"
                                        ImageAlign="Middle" OnClick="btninvno_Click" Visible="false" />
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtinvno" runat="server" placeholder="Inv No" CssClass="form-control" Height="32px"></asp:TextBox>
                                </div>
                                <label class="col-sm-2 control-label">Date</label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtinvdate" runat="server" placeholder="Inv Date" CssClass="form-control" Height="32px"></asp:TextBox>
                                    <span id="spnjobno" runat="server">Job No.</span>
                                    <asp:TextBox ID="txtjobno" runat="server" ReadOnly="true" Width="70px" Placeholder="Job No." CssClass="form-control" Height="32px"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group" runat="server" id="tdbatch1">
                                <label class="col-sm-3 control-label" id="lblBatch" runat="server">Batch No.</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtinvbtch" runat="server"
                                        ReadOnly="True" CssClass="form-control" Height="32px"></asp:TextBox>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <label id="tdcomplaint" runat="server" class="col-sm-4 control-label">Complaint No.</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtvchnum" runat="server" placeholder="Entry No."
                                        MaxLength="6" ReadOnly="True" CssClass="form-control" Height="32px"></asp:TextBox>
                                </div>
                                <div class="col-sm-5">
                                    <asp:TextBox ID="txtvchdate" runat="server" CssClass="form-control" Height="32px" placeholder="Entry Dt"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group" style="display: none">
                                <label id="tdtypcomplaint" runat="server" class="col-sm-4 control-label">Type of Complaint</label>
                                <div class="col-sm-8">
                                    <asp:DropDownList ID="ddntrofcmlnt" runat="server" CssClass="form-control" Height="32px"></asp:DropDownList>
                                </div>
                            </div>

                            <div class="form-group" style="display: none">
                                <label id="tdnaturcomplaint" runat="server" class="col-sm-4 control-label">Nature of Complaint</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtntrcmpln" runat="server" CssClass="form-control" Height="32px" MaxLength="50" placeholder="Nature of Complaint"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group" style="display: none">
                                <label id="tddivision" runat="server" class="col-sm-4 control-label">Division of Complaint</label>
                                <div class="col-sm-8">
                                    <asp:DropDownList ID="dddivisioncmltn" runat="server" CssClass="form-control" Height="32px"></asp:DropDownList>
                                </div>
                            </div>

                            <div class="form-group">
                                <label class="col-sm-4 control-label">Guaranty/Warranty Terms</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtGur" runat="server"
                                        CssClass="form-control" Height="32px"></asp:TextBox>
                                </div>
                                <label class="col-sm-2 control-label">Date</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtGurDate" runat="server"
                                        CssClass="form-control" Height="32px"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <section class="col-lg-12 connectedSortable" style="display: none">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">
                                <li><a href="#DescTab" id="tab1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab">Details</a></li>
                            </ul>

                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane active" id="DescTab">
                                    <div class="lbBody" style="color: White; max-height: 300px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <asp:GridView ID="sg1" Width="100%" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" AutoGenerateColumns="false">
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White"
                                                CssClass="GridviewScrollHeader" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                            <Columns>
                                                <asp:BoundField DataField="srno" HeaderText="Srno" ReadOnly="True" />
                                                <asp:BoundField DataField="app" HeaderText="Application" ReadOnly="True" />
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Remarks</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtrmk" runat="server" Width="100%" MaxLength="100" Text='<%#Eval("rmk") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>
                <div style="display: none">
                    <tr id="trextraval" runat="server">
                        <td class="style2">Tpt Amt
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="txttpt" runat="server" Width="80px" Style="text-align: right;" placeholder="TPT Amt" onkeyup="calculateSum();" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false"></asp:TextBox>
                        </td>
                        <td class="style2">Lodging Amt</td>
                        <td class="style2">
                            <asp:TextBox ID="txtlodging" runat="server" Width="80px" Style="text-align: right;" placeholder="Lodging Amt" onkeyup="calculateSum();" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false"></asp:TextBox></td>
                        <td class="style2">Fooding Amt</td>
                        <td class="style2">
                            <asp:TextBox ID="txtfooding" runat="server" Width="80px" Style="text-align: right;" placeholder="Fooding Amt" onkeyup="calculateSum();"></asp:TextBox>
                        </td>
                        <td class="style2">Misc Amt</td>
                        <td class="style2">
                            <asp:TextBox ID="txtmisc" runat="server" Width="80px" Style="text-align: right;" placeholder="Misc Amt" onkeyup="calculateSum();" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false"></asp:TextBox>
                        </td>
                        <td class="style2">Total Amt
                        </td>
                        <td>
                            <asp:TextBox ID="txttot" runat="server" Width="80px" Style="text-align: right;" placeholder="Total Amt" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr style="margin-top: 10px; padding-top: 10px; background-color: #CDE8F0">
                        <td colspan="1" class="style2">Remarks</td>
                        <td colspan="9">
                            <asp:TextBox ID="txtrmk" runat="server" TextMode="MultiLine" Width="1000px"></asp:TextBox>
                        </td>
                    </tr>

                    <tr>
                        <td id="tdtechnicalper" runat="server" class="style2">Tech. Person</td>
                        <td colspan="5">
                            <asp:TextBox ID="txttechper" runat="server" Width="200px" MaxLength="50" placeholder="Tecnical Person Name"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>

                        <td colspan="5">&nbsp;
                      Quantity 
                      <asp:TextBox ID="txtinvqty" runat="server" ReadOnly="true" Width="70px"></asp:TextBox>
                            &nbsp;
                      PMFG
                      <asp:TextBox ID="txtpmrg" runat="server" MaxLength="40" Width="100px"></asp:TextBox>
                        </td>
                        <td colspan="2">&nbsp;</td>
                        <td colspan="2">&nbsp;</td>
                    </tr>
                    <tr>
                </div>
            </div>
        </section>
    </div>

    <asp:Button ID="btnhideF" runat="server" OnClick="btnhideF_Click" Style="display: none" />
    <asp:Button ID="btnhideF_s" runat="server" OnClick="btnhideF_s_Click" Style="display: none" />
    <asp:HiddenField ID="hffield" runat="server" />
    <asp:HiddenField ID="edmode" runat="server" />

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
</asp:Content>
