<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" EnableEventValidation="true" Inherits="urights" Title="Tejaxo" CodeFile="urights.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../tej-base/Scripts/gridviewScroll.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            gridviewScroll('#<%=sg1.ClientID%>', gridDiv, 1, 1);
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
            function checkAll(Checkbox, colIndex) {
                var GridView = document.getElementById("<%=sg1.ClientID %>");
                for (var i = 1; i < GridView.rows.length; i++) {
                    GridView.rows[i].cells[colIndex].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;
                }
            }
    </script>
    <script type="text/javascript">
        function colorChanged(sender) {
            sender.get_element().style.color = "#" + sender.get_selectedColor();

        }</script>

    <script type="text/javascript">
        function Search_Gridview(strKey, strGV) {
            var strData = strKey.value.toLowerCase().split(" ");
            var tblData = document.getElementById(strGV);
            var rowData;
            for (var i = 1; i < tblData.rows.length; i++) {
                rowData = tblData.rows[i].innerHTML;
                var styleDisplay = 'none';
                for (var j = 0; j < strData.length; j++) {
                    if (rowData.toLowerCase().indexOf(strData[j]) >= 0)
                        styleDisplay = '';
                    else {
                        styleDisplay = 'none';
                        break;
                    }
                }
                tblData.rows[i].style.display = styleDisplay;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper">
        <section class="content-header">
            <%--<div class="box-footer">--%>
            <table style="width: 100%">
                <tr>
                    <td>
                        <button id="btnnew" runat="server" accesskey="N" onserverclick="btnnew_ServerClick" class="btn btn-info" style="width: 100px;"><u>N</u>ew</button>
                        <button id="btnedit" runat="server" accesskey="i" class="btn btn-info" onserverclick="btnedit_ServerClick" style="width: 100px;">Ed<u>i</u>t</button>
                        <button id="btnsave" runat="server" accesskey="S" class="btn btn-info" onserverclick="btnsave_ServerClick" style="width: 100px;"><u>S</u>ave</button>
                        <button id="btnprint" runat="server" accesskey="t" class="btn btn-info" onserverclick="btnprint_ServerClick" style="width: 100px">Lis<u>t</u></button>
                        <button id="btndel" runat="server" accesskey="l" class="btn btn-info" onserverclick="btndel_ServerClick" style="width: 100px;">De<u>l</u>ete</button>
                        <asp:Button ID="btnext" runat="server" Text="Exit" class="btn btn-info" OnClick="btnext_Click" Style="width: 100px;" />
                    </td>
                    <td style="float: right">
                        <asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large" Text="User Rights"></asp:Label>
                    </td>
                </tr>
            </table>
            <%--</div>--%>
        </section>
        <section class="content">
            <div class="row">
                <div class="col-md-9">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <label id="Label1" runat="server" class="col-sm-2 control-label" title="lbl1">User Name</label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnuserid" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnuserid_Click" />
                                </div>
                                <div class="col-sm-3">
                                    <input id="txtuserid" type="text" class="form-control" runat="server" placeholder="User ID" readonly="readonly" style="height: 25px" />
                                </div>
                                <div class="col-sm-6">
                                    <input id="txtusername" type="text" class="form-control" runat="server" placeholder="User Name" readonly="readonly" style="height: 25px" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label2" runat="server" class="col-sm-3 control-label" title="lbl1">Search</label>
                                <div class="col-sm-9">
                                    <input id="lblSearch" type="text" class="form-control" runat="server" placeholder="search..." onkeyup="Search_Gridview(this, 'ContentPlaceHolder1_sg1')" />
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-sm-9">
                                    <button id="btnSelectIcon" runat="server" class="btn btn-info" onserverclick="btnSelectIcon_Click">Select Menu</button>
                                    <br />
                                </div>
                            </div>

                            <section class="col-lg-12" runat="server">
                                <div class="lbBody" id="gridDiv" style="height: 400px; width: 900px; overflow: hidden; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                    <asp:GridView ID="sg1" runat="server" ForeColor="#333333"
                                        Style="background-color: #FFFFFF; color: White;" Width="850px" Font-Size="13px"
                                        AutoGenerateColumns="False" OnRowDataBound="sg1_RowDataBound" OnRowCommand="sg1_RowCommand">
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <Columns>

                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    Rmv 
                                                    <br />
                                                    <asp:CheckBox ID="chkappall" runat="server" onclick="checkAll(this,0);" Style="display: none" />
                                                </HeaderTemplate>
                                                <ItemStyle Width="30px" />
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chk1" runat="server" Style="display: none" />
                                                    <asp:ImageButton ID="rmv" runat="server" ImageUrl="~/tej-base/images/Btn_remn.png" Style="width: 22px;" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ID" HeaderText="Form ID" />
                                            <asp:BoundField DataField="text" HeaderText="Form Name" />
                                            <asp:BoundField DataField="mlevel" HeaderText="Id Level" />
                                            <asp:BoundField DataField="ALLOW_LEVEL" HeaderText="ALLOW_LEVEL" />
                                            <asp:BoundField DataField="WEB_ACTION" HeaderText="WEB_ACTION" />
                                            <asp:BoundField DataField="SEARCH_KEY" HeaderText="SEARCH_KEY" />
                                            <asp:BoundField DataField="SUBMENU" HeaderText="SUBMENU" />
                                            <asp:BoundField DataField="SUBMENUID" HeaderText="SUBMENUID" />
                                            <asp:BoundField DataField="FORM" HeaderText="FORM" />
                                            <asp:BoundField DataField="PARAM" HeaderText="PARAM" />

                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    Can Save                                                    
                                                    <br />
                                                    <asp:CheckBox ID="chkappall2" runat="server" onclick="checkAll(this,11);" />
                                                </HeaderTemplate>
                                                <ItemStyle Width="60px" />
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chk2" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    Can Edit    
                                                    <br />
                                                    <asp:CheckBox ID="chkappall3" runat="server" onclick="checkAll(this,12);" />
                                                </HeaderTemplate>
                                                <ItemStyle Width="60px" />
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chk3" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    Can Del                                                                            
                                                    <br />
                                                    <asp:CheckBox ID="chkappall4" runat="server" onclick="checkAll(this,13);" />
                                                </HeaderTemplate>
                                                <ItemStyle Width="60px" />
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chk4" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:BoundField DataField="CSS" HeaderText="CSS" />
                                            <asp:BoundField DataField="BRN" HeaderText="BRN" />
                                            <asp:BoundField DataField="PRD" HeaderText="PRD" />

                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    Can Print                                                                       
                                                    <br />
                                                    <asp:CheckBox ID="chkappall5" runat="server" onclick="checkAll(this,17);" />
                                                </HeaderTemplate>
                                                <ItemStyle Width="60px" />
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chk5" runat="server" />
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
                            </section>
                        </div>
                    </div>
                </div>
                <div class="col-md-3" style="display: none">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <div class="box box-solid">
                                    <div class="box-body no-padding">
                                        <table id="layout-skins-list" class="table table-striped bring-up nth-2-center">
                                            <thead>
                                                <tr>
                                                    <th style="width: 210px;">Skin Class</th>
                                                    <th>Preview</th>
                                                    <th>S</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr>
                                                    <td><code>skin-blue</code></td>
                                                    <td><a href="#" data-skin="skin-blue" class="btn btn-danger btn-xs"><i class="fa fa-eye"></i></a></td>
                                                    <td>
                                                        <asp:CheckBox ID="chk1" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td><code>skin-blue-light</code></td>
                                                    <td><a href="#" data-skin="skin-blue-light" class="btn btn-danger btn-xs"><i class="fa fa-eye"></i></a></td>
                                                    <td>
                                                        <asp:CheckBox ID="chk2" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td><code>skin-yellow</code></td>
                                                    <td><a href="#" data-skin="skin-yellow" class="btn btn-warning btn-xs"><i class="fa fa-eye"></i></a></td>
                                                    <td>
                                                        <asp:CheckBox ID="chk3" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td><code>skin-yellow-light</code></td>
                                                    <td><a href="#" data-skin="skin-yellow-light" class="btn btn-warning btn-xs"><i class="fa fa-eye"></i></a></td>
                                                    <td>
                                                        <asp:CheckBox ID="chk4" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td><code>skin-green</code></td>
                                                    <td><a href="#" data-skin="skin-green" class="btn btn-danger btn-xs"><i class="fa fa-eye"></i></a></td>
                                                    <td>
                                                        <asp:CheckBox ID="chk5" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td><code>skin-green-light</code></td>
                                                    <td><a href="#" data-skin="skin-green-light" class="btn btn-danger btn-xs"><i class="fa fa-eye"></i></a></td>
                                                    <td>
                                                        <asp:CheckBox ID="chk6" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td><code>skin-purple</code></td>
                                                    <td><a href="#" data-skin="skin-purple" class="btn bg-purple btn-xs"><i class="fa fa-eye"></i></a></td>
                                                    <td>
                                                        <asp:CheckBox ID="chk7" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td><code>skin-purple-light</code></td>
                                                    <td><a href="#" data-skin="skin-purple-light" class="btn bg-purple btn-xs"><i class="fa fa-eye"></i></a></td>
                                                    <td>
                                                        <asp:CheckBox ID="chk8" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td><code>skin-red</code></td>
                                                    <td><a href="#" data-skin="skin-red" class="btn btn-danger btn-xs"><i class="fa fa-eye"></i></a></td>
                                                    <td>
                                                        <asp:CheckBox ID="chk9" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td><code>skin-red-light</code></td>
                                                    <td><a href="#" data-skin="skin-red-light" class="btn btn-danger btn-xs"><i class="fa fa-eye"></i></a></td>
                                                    <td>
                                                        <asp:CheckBox ID="chk10" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td><code>skin-black</code></td>
                                                    <td><a href="#" data-skin="skin-black" class="btn bg-black btn-xs"><i class="fa fa-eye"></i></a></td>
                                                    <td>
                                                        <asp:CheckBox ID="chk11" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td><code>skin-black-light</code></td>
                                                    <td><a href="#" data-skin="skin-black-light" class="btn bg-black btn-xs"><i class="fa fa-eye"></i></a></td>
                                                    <td>
                                                        <asp:CheckBox ID="chk12" runat="server" />
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                    <!-- /.box-body -->
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
    <asp:HiddenField ID="hfcode" runat="server" />
    <asp:HiddenField ID="hf1" runat="server" />
    <asp:HiddenField ID="edmode" runat="server" />
    <asp:HiddenField runat="server" ID="TreeViewTextValues" />
    <asp:HiddenField ID="hfGridView1SV" runat="server" />
    <asp:HiddenField ID="hfGridView1SH" runat="server" />
    <%--<td colspan="6">
        <table>
            <tr>
                <td style="border-bottom-style: groove; border-width: thin">
                    <asp:TextBox ID="txtsrch" runat="server" CssClass="txtsrch" Width="300px"></asp:TextBox></td>
                <td style="border-bottom-style: groove; border-width: thin">
                    <asp:ImageButton ID="btnsrch" runat="server" Width="120px" Height="27px" Style="margin-left: 2px"
                        ImageUrl="~/tej-base/images/search-button.png" /></td>
            </tr>
            <tr>
                <td colspan="2" style="border-style: groove; border-width: thin;"></td>
            </tr>
        </table>
    </td>--%>
</asp:Content>
