<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="appr" Title="Tejaxo" CodeFile="appr.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">    
    <script type="text/javascript">
        function checkAll(gvExample, colIndex) {
            var GridView = gvExample.parentNode.parentNode.parentNode;
            for (var i = 1; i < GridView.rows.length; i++) {
                var chb = GridView.rows[i].cells[colIndex].getElementsByTagName("input")[0];
                chb.checked = gvExample.checked;
            }
        }
        function checkItem_All(objRef, colIndex) {
            var GridView = objRef.parentNode.parentNode.parentNode;
            var selectAll = GridView.rows[0].cells[colIndex].getElementsByTagName("input")[0];
            if (!objRef.checked) {
                selectAll.checked = false;
            }
            else {
                var checked = true;
                for (var i = 1; i < GridView.rows.length; i++) {
                    var chb = GridView.rows[i].cells[colIndex].getElementsByTagName("input")[0];
                    if (!chb.checked) {
                        checked = false;
                        break;
                    }
                }
                selectAll.checked = checked;
            }
        }
    </script>
    <style type="text/css">
        .grd {
            margin-bottom: 100px;
            margin-left: 1000px;
            margin-right: 50px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper">
        <section class="content-header">
            <table style="width: 100%">
                <tr>
                    <td>
                        <button id="btnnew" runat="server" class="btn btn-info" onserverclick="btnnew_ServerClick" accesskey="t" style="width: 100px;">S<u>t</u>art</button>
                        <button id="btnsave" runat="server" class="btn btn-info" onserverclick="btnsave_ServerClick" accesskey="S" style="width: 100px;"><u>S</u>ave</button>
                        <asp:Button ID="btnext" runat="server" Text="Exit" class="btn btn-info" OnClick="btnext_Click" Style="width: 100px;" />
                    </td>
                    <td style="float: right">
                        <asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Task Managment"></asp:Label>
                    </td>
                </tr>
            </table>
        </section>
        <section class="content">
            <div class="row">
                <div class="col-md-12">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtsearch" runat="server" TabIndex="1" CssClass="txtsrch" onblur="Change(this, event)" onfocus="Change(this, event)"
                                                placeholder="Enter here to search" ToolTip="Enter here to search" Height="35px" Width="250px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="srch" runat="server" ImageUrl="~/images/search-button.png"
                                                Width="120px" Height="31px" TabIndex="2" ToolTip="Click to Search"
                                                OnClick="srch_Click" /></td>


                                        <td style="float: right">Show Rows</td>
                                        <td></td>
                                        <td>
                                            <asp:TextBox ID="tkrow" runat="server" Width="56px" CssClass="txtcss2" onkeypress="return isDecimalKey(event)"
                                                Style="text-align: right;" Height="30px" Text="20" onblur="Change(this, event)" onfocus="Change(this, event)"></asp:TextBox></td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-12">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <div id="gridDiv" style="height: 400px; max-height: 400px; max-width: 1290px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                    <asp:GridView ID="sg1" runat="server" Width="100%"
                                        OnRowDataBound="sg1_RowDataBound" OnRowCommand="sg1_RowCommand" CellPadding="2" ForeColor="#333333"
                                        GridLines="Both" Style="background-color: #FFFFFF; color: White;">
                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader" />
                                        <EditRowStyle BackColor="#999999" />
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <Columns>
                                            <%--  <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt" runat="server" Visible="false"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>

                                            <asp:TemplateField HeaderText="Ok" HeaderStyle-Width="20px">
                                                <HeaderStyle HorizontalAlign="Center" />

                                                <HeaderTemplate>
                                                    Ok
                                    <br />
                                                    <asp:CheckBox ID="chkappall" runat="server" onclick="checkAll(this,0);" />
                                                </HeaderTemplate>
                                                <ItemStyle Width="20px" />
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkapp" runat="server" onclick="checkItem_All(this,0)" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="No" HeaderStyle-Height="30px">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <HeaderTemplate>
                                                    No
                                    <br />
                                                    <asp:CheckBox ID="chkrejall" runat="server" onclick="checkAll(this,1);" />
                                                </HeaderTemplate>
                                                <ItemStyle />
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkrej" runat="server" onclick="checkItem_All(this,1)" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Completed on" HeaderStyle-Width="90px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtcompdt" TextMode="Date" runat="server" Width="140px" CssClass="textboxStyle" onblur="Change(this, event)" onfocus="Change(this, event)"></asp:TextBox>
                                                    <%--   <cc1:CalendarExtender ID="CalendarExtender1" runat="server" 
                Enabled="True" TargetControlID="txtcompdt" 
                Format="dd/MM/yyyy">
            </cc1:CalendarExtender>
           <cc1:MaskedEditExtender ID="MaskedEditExtender2" runat="server" Mask="99/99/9999" 
                            MaskType="Date" TargetControlID="txtcompdt" />--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="View" HeaderStyle-Width="40px">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btnvw" runat="server" ImageUrl="~/css/images/bdsearch5.png" CommandName="Show" ImageAlign="Middle" Width="20px" ToolTip="Show" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Remarks" HeaderStyle-Width="90px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtreason" runat="server" Width="70px" CssClass="textboxStyle" onblur="Change(this, event)" onfocus="Change(this, event)"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataRowStyle BackColor="White" ForeColor="Red" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <EmptyDataTemplate>
                                            <asp:Image ID="imgdata" runat="server" ImageUrl="~/images/DataNotFound.jpg" AlternateText="No Data Exist" Width="400px" />
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>
    <asp:HiddenField ID="hffield" runat="server" />
    <asp:Button ID="btnhideF" runat="server" OnClick="btnhideF_Click" Style="display: none" />
    <asp:Button ID="btnhideF_s" runat="server" OnClick="btnhideF_s_Click" Style="display: none" />
    <asp:HiddenField ID="hfGridView1SV" runat="server" />
    <asp:HiddenField ID="hfGridView1SH" runat="server" />
</asp:Content>
