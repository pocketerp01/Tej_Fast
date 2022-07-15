<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="om_vch_upload" Title="Tejaxo" CodeFile="om_vch_upload.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
<script src="../tej-base/Scripts/gridviewScroll.min.js" type="text/javascript"></script>
    <script lang="javascript" type="text/javascript">
        var size = 2;
        var id = 0;
        function submitFile() {
            $("#<%= btnupload.ClientID%>").click();
            ProgressBar();
        };
        function ProgressBar() {
            if (document.getElementById('<%=FileUpload1.ClientID %>').value != "") {
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
                    <td><asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label></td>
                    <td style="text-align:right">
                        <button id="btnnew" runat="server" accesskey="N" onserverclick="btnnew_ServerClick" class="btn btn-info" style="width: 100px"><u>N</u>ew</button>
                        <button id="btnedit" runat="server" accesskey="i" class="btn btn-info" onserverclick="btnedit_ServerClick" style="width: 100px">Ed<u>i</u>t</button>
                        <button id="btnsave" runat="server" accesskey="S" class="btn btn-info" onserverclick="btnsave_ServerClick" style="width: 100px"><u>S</u>ave</button>
                        <button id="btnlist" runat="server" accesskey="t" class="btn btn-info" onserverclick="btnlist_ServerClick" style="width: 100px">Lis<u>t</u></button>
                        <button id="btndel" runat="server" accesskey="l" class="btn btn-info" onserverclick="btndel_ServerClick" style="width: 100px">De<u>l</u>ete</button>
                        <button id="btncan" runat="server" accesskey="c" class="btn btn-info" onserverclick="btncan_ServerClick" style="width: 100px"><u>C</u>ancel</button>
                        <button id="btnext" runat="server" accesskey="x" class="btn btn-info" onserverclick="btnext_ServerClick" style="width: 100px">E<u>x</u>it</button>
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
                                <label id="mrheading" runat="server" class="col-sm-3 control-label">Voucher</label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnvchnum" runat="server" ToolTip="Preview MRR"
                                        ImageUrl="~/tej-base\css/images/bdsearch5.png" ReadOnly="true"
                                        Style="width: 25px; height: 25px; float: right" OnClick="btnvchnum_Click" />
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtvchnum" runat="server" Width="100%" placeholder="Voucher No" onfocus="Change(this, event)" onblur="Change(this, event)" ReadOnly="true" Height="30px" CssClass="form-control"></asp:TextBox>

                                </div>
                                <div class="col-sm-5">
                                    <asp:TextBox ID="txtvchdt" runat="server" Width="100%" placeholder="Voucher Date" onfocus="Change(this, event)" onblur="Change(this, event)" ReadOnly="true" Height="30px" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label3" runat="server" class="col-sm-4 control-label">Party Code/ Name</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtacode" runat="server" Width="100%" placeholder="Party Code" onfocus="Change(this, event)" onblur="Change(this, event)" ReadOnly="true" Height="30px" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-sm-5">
                                    <asp:TextBox ID="txtaname" runat="server" Width="100%" placeholder="Name" onfocus="Change(this, event)" onblur="Change(this, event)" ReadOnly="true" Height="30px" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label1" runat="server" class="col-sm-4 control-label">File to Upload</label>
                                <div class="col-sm-5">
                                    <asp:FileUpload ID="FileUpload1" runat="server" CssClass="rounded_corners" onchange="submitFile()" />

                                    <div id="divUpload" style="display: none; align-items: center;" runat="server">
                                        <div id="Div1" style="width: 200pt; height: 15px; border: solid 1pt gray" runat="server">
                                            <div id="divProgress" runat="server" style="width: 1pt; height: 15px; background-color: #1797C0; display: none">
                                            </div>
                                        </div>
                                        <div style="width: 200pt; text-align: center;">
                                            <asp:Label ID="lblPercentage" runat="server" Text="Label"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-2">
                                    <asp:Button ID="btnupload" runat="server" Text="Upload" class="myButton" OnClick="btnupload_Click" Style="display: none" />
                                </div>
                            </div>

                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <label id="Type" runat="server" class="col-sm-3 control-label">Type/Name</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txttype" runat="server" Width="100%" placeholder="Type" onfocus="Change(this, event)" onblur="Change(this, event)" ReadOnly="true" Height="30px" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txttypename" runat="server" Width="100%" placeholder="Name" onfocus="Change(this, event)" onblur="Change(this, event)" ReadOnly="true" Height="30px" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="billdate" runat="server" class="col-sm-3 control-label">Bill Date/No.</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtbilldt" runat="server" Width="100%" placeholder="Date" onfocus="Change(this, event)" onblur="Change(this, event)" ReadOnly="true" Height="30px" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtbillno" runat="server" Width="100%" placeholder="Bill No." onfocus="Change(this, event)" onblur="Change(this, event)" ReadOnly="true" Height="30px" CssClass="form-control"></asp:TextBox>
                                </div>
                                
                            </div>
                            <div class="form-group">
                                <label id="Amount" runat="server" class="col-sm-3 control-label">Amount</label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtamt" runat="server" Width="100%" placeholder="Amount" onfocus="Change(this, event)" onblur="Change(this, event)" ReadOnly="true" Height="30px" CssClass="form-control"></asp:TextBox>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>

                <section class="col-lg-12 connectedSortable">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">
                                <li><a href="#DescTab" id="tab1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab">Details</a></li>
                            </ul>
                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane active" id="DescTab">
                                    <div class="lbBody" style="color: White; max-height: 300px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <asp:GridView ID="sg1" Width="100%" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" AutoGenerateColumns="false" OnRowDataBound="sg1_RowDataBound" OnRowCommand="sg1_RowCommand">
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#1797C0" Font-Bold="True" ForeColor="White"
                                                CssClass="GridviewScrollHeader" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>R</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnrmv" runat="server" CommandName="Rmv" ImageUrl="~/tej-base\images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove It" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="30px" />
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="srno" HeaderText="Srno" ReadOnly="True">
                                                    <ItemStyle Width="10px" />
                                                    <HeaderStyle Width="10px" />
                                                </asp:BoundField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>D</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btndnlwd" runat="server" CommandName="Dwl" CommandArgument='<%# Eval("FILNO") %>' ImageUrl="~/tej-base\images/save.png" Width="22px" ImageAlign="Middle" ToolTip="Download file" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="30px" />
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>V</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnview" runat="server" CommandName="View" CommandArgument='<%# Eval("FILNO") %>' ImageUrl="~/tej-base\images/preview-file.png" Width="20px" ImageAlign="Middle" ToolTip="View file" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="30px" />
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="filno" HeaderText="File Name" ReadOnly="True"></asp:BoundField>

                                            </Columns>
                                        </asp:GridView>
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
    <%--<asp:HiddenField ID="edmode" runat="server" />--%>
    <asp:HiddenField ID="Form_vty" runat="server" />
    <asp:HiddenField ID="hf_form_mode" runat="server" />
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