<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="rfqapp" Title="Tejaxo" CodeFile="rfqapp.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../tej-base/Scripts/gridviewScroll.min.js"></script>
    <script type="text/javascript" language="javascript">

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

       <%-- function submitFile() {
            $("#<%= LnkBtnv.ClientID%>").click();
        };--%>

        function gridviewScroll() {
            $('#<%=GridView1.ClientID%>').gridviewScroll({
                width: 1250,
                height: 600
            });
        }
    </script>

    <style type="text/css">
        .style7 {
            height: 24px;
        }

        .vandana {
            width: 50px;
        }

            .vandana input {
                width: 50px;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper">
        <section class="content-header">
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:Label ID="lblhead" runat="server" Text="" Font-Bold="True" Font-Size="X-Large"></asp:Label>
                    </td>
                    <td style="text-align: right">
                        <button type="submit" id="btntrans" class="btn btn-info" style="width: 100px;" runat="server" accesskey="N" onserverclick="btntrans_Click" visible="false">Up<u>d</u>ate</button>
                        <button type="submit" id="btnexit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="X" onserverclick="btnexit_ServerClick">E<u>x</u>it</button>
                        <%--       <div class="form-group">
                             <asp:Label ID="lblhead" runat="server" CssClass="col-sm-4 control-label" Font-Size="16px" Font-Bold="True"></asp:Label>   
                                  <div class="col-sm-8">                      
                                <div id="alermsg" style="color: #f00; font-weight: bold; font-size: medium; text-align: center;display: none;" runat="server" />
                                      </div>
                            </div>--%>
                    </td>
                </tr>
            </table>
        </section>
        <%--   <div class="bSubBlock brandSecondaryBrd secondaryPalette" align="center">
        <h3 class="lbHeader" align="center">
            <button id="btntrans" accesskey="d" class="frmbtn" runat="server" onserverclick="btntrans_Click">
                Up<u>d</u>ate</button>
            <button id="btnexit" class="frmbtn" runat="server" onserverclick="btnexit_Click">
                Exit</button>
            <br />
        </h3>--%>
        <%--  <div class="toolsContentLeft">
            <div class="bSubBlock brandSecondaryBrd secondaryPalette" style="background-image: url('css/images/bgTop.gif');">
                <div align="left" style="color: #1797c0; background-image: url(images/bgTop.gif);
                    font-size: medium; font-weight: bold;">
                    &nbsp;<asp:Image ID="Image1" runat="server" Height="24px" Width="28px" ImageUrl="~/css/images/app.jpg" />
                    &nbsp;<asp:Label ID="lblhead" runat="server"></asp:Label>
                </div>
                <table style="width: 100%;">
                    <tr>
                        <td colspan="4">
                            <div id="alermsg" style="color: #f00; font-weight: bold; font-size: medium; text-align: center;
                                display: none;" runat="server" />
                        </td>
                    </tr>
                </table>--%>

        <section class="content">
            <div class="row">
                <div class="col-md-12">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div id="alermsg" style="color: #f00; font-weight: bold; font-size: medium; text-align: center; display: none;" runat="server" />
                                </div>
                            </div>

                            <div class="form-group" style="display: none;">
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnfrom" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnfrom_Click" />
                                </div>
                                <div class="col-sm-2">
                                    <%--                                    <asp:TextBox ID="txtfrom" ReadOnly="true" runat="server" CssClass="form-control" Width="100%" ></asp:TextBox>--%>
                                    <asp:TextBox ID="txtfrom" runat="server" CssClass="form-control" onblur="Change(this, event)" onfocus="Change(this, event)" placeholder="User Name" ReadOnly="True" TabIndex="-1"></asp:TextBox>
                                </div>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtto" runat="server" CssClass="form-control" onblur="Change(this, event)" onfocus="Change(this, event)" placeholder="Contact Person" ReadOnly="True" TabIndex="-1"></asp:TextBox>
                                </div>
                                <div class="col-sm-7">
                                    <asp:ImageButton ID="btnto" runat="server" Height="22px" ImageAlign="Middle" ImageUrl="~/css/images/bdsearch5.png" ToolTip="To" Width="24px" OnClick="btnto_Click" />
                                </div>
                            </div>


                            <div class="form-group">
                                <div class="col-sm-1">
                                    <%-- <input id="txtrows" style="width: 70px;" type="text" placeholder="No. of rows" runat="server" />--%>
                                    <asp:TextBox ID="txtrows" Width="100%" runat="server" CssClass="form-control" placeholder="No. of rows"></asp:TextBox>
                                </div>
                                <div class="col-sm-1">
                                    <asp:Button ID="btnshow" runat="server" BackColor="Orange" CssClass="form-control" Text="Show" OnClick="btnshow_Click" />
                                </div>
                                <asp:Label ID="lblshow" CssClass="col-sm-10 control-label" runat="server" Height="35px" Font-Size="Medium" Text="0"></asp:Label>

                            </div>

                            <div class="form-group" id="r1" runat="server" style="display: none;">
                                <asp:TextBox ID="txtbcode" runat="server" Style="display: none;"></asp:TextBox>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtbname" runat="server" CssClass="form-control" onblur="Change(this, event)" onfocus="Change(this, event)" placeholder="Branch Name" ReadOnly="True" TabIndex="-1" Width="773px"></asp:TextBox>
                                </div>
                                <div class="col-sm-10">
                                    <asp:ImageButton ID="btnmbr" runat="server" Height="22px" ImageAlign="Middle" ImageUrl="~/css/images/bdsearch5.png" ToolTip="Branch Name" Width="24px" OnClick="btnmbr_Click" />
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtsearch" runat="server" CssClass="form-control" onblur="Change(this, event)" onfocus="Change(this, event)" placeholder="Search here..." TabIndex="-1" Width="500px"></asp:TextBox>
                                </div>
                                <div class="col-sm-1">
                                    <asp:Button ID="btnsearch" runat="server" CssClass="form-control" BackColor="Orange" Width="60px" Text="Search" OnClick="btnsearch_Click" ToolTip="click here to search" />
                                </div>
                                <div class="col-sm-1">
                                    <asp:Button ID="btnexp" runat="server" CssClass="form-control" BackColor="Orange" Width="60px" OnClick="btnexp_Click" Text="Export" ToolTip="click here to export data" />
                                </div>
                                <div class="col-sm-6">
                                    <asp:Label ID="lblfree" runat="server"></asp:Label>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>

                <div class="col-md-12">
                    <div>
                        <div class="box-body">
                            <div class="toolsContentLeft">
                                <%--                                            <div class="lbBody" style="color: White; box-shadow: 0 2px 4px rgba(127,127,127,.3);box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">--%>
                                <div class="lbBody" id="gridDiv" style="color: White; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                    <%--  <asp:GridView ID="GridView1" Width="100%" runat="server" CellPadding="2" ForeColor="#333333"
                                                    GridLines="Both" Style="border-color: #E2DED6; background-color: #FFFFFF; color: White;"
                                                    AutoGenerateColumns="true" OnRowDataBound="GridView1_RowDataBound">
                                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                    <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                    <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader" />
                                                    <EditRowStyle BackColor="#999999" />
                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />--%>
                                    <fin:CoolGridView ID="GridView1" runat="server" ForeColor="#333333"
                                        Style="background-color: #FFFFFF; color: White;" Width="100%" Height="375px" Font-Size="13px"
                                        AutoGenerateColumns="False" OnRowDataBound="GridView1_RowDataBound" OnRowCommand="GridView1_RowCommand">
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
                                            <%--9 yaha tak--%>

                                            <asp:TemplateField>
                                                <HeaderTemplate>View</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="LnkBtnv" runat="server" Visible="false" ToolTip="View , see the RFQ in Print format" OnClick="LnkBtnv_Click" ForeColor="#1797c0">View</asp:LinkButton>
                                                    <%--   <asp:ImageButton ID="linkimg" runat="server" AlternateText="View" ToolTip="View , see the RFQ in Print format" OnClick="linkimg_Click"  />--%>
                                                    <asp:ImageButton ID="linkimg" runat="server" CommandName="View" ImageAlign="Middle" ImageUrl="../tej-base/images/preview-file.png" Width="20px" />
                                                    <asp:ImageButton ID="linkDwnlod" runat="server" CommandName="DWN" ImageAlign="Middle" ImageUrl="../tej-base/images/Save.png" Width="20px" />
                                                    <%--                                                         <asp:Button ID="LnkBtnv" runat="server" Text="View" ToolTip="View , see the RFQ in Print format"  OnClick="LnkBtnv_Click" Width="50px"  />--%>
                                                                &nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="LnkBtnd" runat="server" ToolTip="Download the link" OnClick="LnkBtnd_Click" ForeColor="#1797c0" Visible="false">Download</asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%--<asp:TemplateField HeaderText="View">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="LnkBtnv" runat="server" ToolTip="View , see the RFQ in Print format"  OnClick="LnkBtnv_Click" ForeColor="#1797c0">View</asp:LinkButton>
                                                                &nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:LinkButton ID="LnkBtnd" runat="server" ToolTip="Download the link" OnClick="LnkBtnd_Click"  ForeColor="#1797c0">Download</asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>--%>

                                            <asp:TemplateField Visible="false">
                                                <HeaderTemplate>Ok</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkok" runat="server" Text='<%#Eval("chkok") %>' BackColor="YellowGreen" ToolTip="Select RFQ for approval" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField Visible="false">
                                                <HeaderTemplate>No</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkno" runat="server" Text='<%#Eval("chkno") %>' BackColor="YellowGreen" ToolTip="Select RFQ for refusal" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%--<asp:TemplateField HeaderText="OK" ItemStyle-Width="50px">
                                                            <ItemStyle BackColor="YellowGreen" />
                                                            <ItemTemplate>
                                                                <div class="vandana">
                                                                    <asp:CheckBox ID="chkok" runat="server" ToolTip="Select RFQ for approval" />
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="NO" ItemStyle-Width="50px">
                                                            <ItemStyle BackColor="Red" />
                                                            <ItemTemplate>
                                                                <div class="vandana">
                                                                    <asp:CheckBox ID="chkno" runat="server" ToolTip="Select RFQ for refusal" />
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>--%>

                                            <asp:TemplateField HeaderText="Date" Visible="false">
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txtdate" placeholder="dd/mm/yyyy" Width="100%"></asp:TextBox>
                                                    <cc1:MaskedEditExtender ID="MEE1" runat="server" Mask="99/99/9999" MaskType="Date"
                                                        TargetControlID="txtdate" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Time Out" Visible="false">
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txttout" placeholder="Time Out" Width="100%" ReadOnly="true"></asp:TextBox>
                                                    <cc1:MaskedEditExtender ID="me1" runat="server" Mask="99:99" MaskType="Time" UserTimeFormat="TwentyFourHour"
                                                        TargetControlID="txttout">
                                                    </cc1:MaskedEditExtender>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField Visible="false">
                                                <HeaderTemplate>Reason</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtrsn" runat="server" Text='<%#Eval("txtrsn") %>' Width="100%"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--15 no pr reasonaaya--%>

                                            <asp:BoundField DataField="sg1_f1" HeaderText="Entry No" />
                                            <asp:BoundField DataField="sg1_f2" HeaderText="Entry Date" />
                                            <asp:BoundField DataField="sg1_f3" HeaderText="Customer" />
                                            <asp:BoundField DataField="sg1_f4" HeaderText="Part Name" />
                                            <asp:BoundField DataField="sg1_f5" HeaderText="Part No" />
                                            <asp:BoundField DataField="sg1_f6" HeaderText="Modal No" />
                                            <asp:BoundField DataField="sg1_f7" HeaderText="ERP Code" />
                                            <asp:BoundField DataField="sg1_f8" HeaderText="Revision No" />
                                            <asp:BoundField DataField="sg1_f9" HeaderText="Revision Dt" />
                                            <asp:BoundField DataField="sg1_f10" HeaderText="ECN_NO" />
                                            <asp:BoundField DataField="sg1_f11" HeaderText="Ent By" />
                                            <asp:BoundField DataField="sg1_f12" HeaderText="Ent Dt" />
                                            <asp:BoundField DataField="sg1_f13" HeaderText="Code" />
                                        </Columns>
                                        <%--  </asp:GridView>--%>
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

                <div class="col-md-12">
                    <div>
                        <div class="box-body">

                            <%--    <div class="lbBody" id="gridDiv" style="color: White; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                        <fin:CoolGridView ID="sg1" runat="server" ForeColor="#333333"
                            Style="background-color: #FFFFFF; color: White;" Width="100%" Height="375px" Font-Size="13px"
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
                                <asp:BoundField DataField="sg1_h10" HeaderText="sg1_h10" />--%>

                            <%--     <asp:TemplateField>
                                    <HeaderTemplate>Add</HeaderTemplate>
                                    <HeaderStyle Width="30px" />
                                    <ItemTemplate>
                                        <asp:ImageButton ID="sg1_btnadd" runat="server" CommandName="SG1_ROW_ADD" ImageAlign="Middle" ImageUrl="../tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Export Invoice" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>Del</HeaderTemplate>
                                    <HeaderStyle Width="30px" />
                                    <ItemTemplate>
                                        <asp:ImageButton ID="sg1_btnrmv" runat="server" CommandName="SG1_RMV" ImageUrl="../tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Export Invoice" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="sg1_srno" HeaderText="SrNo" HeaderStyle-Width="40px" />
                                <asp:BoundField DataField="sg1_f1" HeaderText="Height" />
                                <asp:BoundField DataField="sg1_f2" HeaderText="Width" />
                                <asp:BoundField DataField="sg1_f3" HeaderText="sg1_f3" />
                                <asp:BoundField DataField="sg1_f4" HeaderText="sg1_f4" />
                                <asp:BoundField DataField="sg1_f5" HeaderText="sg1_f5" />
                                <asp:BoundField DataField="sg1_f6" HeaderText="sg1_f6" />
                                <asp:BoundField DataField="sg1_f7" HeaderText="sg1_f7" />
                                <asp:BoundField DataField="sg1_f8" HeaderText="sg1_f8" />
                                <asp:BoundField DataField="sg1_f9" HeaderText="sg1_f9" />
                                <asp:BoundField DataField="sg1_f10" HeaderText="sg1_f10" />
                               
                                  <asp:TemplateField HeaderText="View">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="LnkBtnv" runat="server" ToolTip="View , see the RFQ in Print format"
                                                                    OnClick="LnkBtnv_Click" ForeColor="#1797c0">View</asp:LinkButton>
                                                                &nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:LinkButton ID="LnkBtnd" runat="server" ToolTip="Download the link" OnClick="LnkBtnd_Click"
                                                                    ForeColor="#1797c0">Download</asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="OK" ItemStyle-Width="50px">
                                                            <ItemStyle BackColor="YellowGreen" />
                                                            <ItemTemplate>
                                                                <div class="vandana">
                                                                    <asp:CheckBox ID="chkok" runat="server" ToolTip="Select RFQ for approval" />
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="NO" ItemStyle-Width="50px">
                                                            <ItemStyle BackColor="Red" />
                                                            <ItemTemplate>
                                                                <div class="vandana">
                                                                    <asp:CheckBox ID="chkno" runat="server" ToolTip="Select RFQ for refusal" />
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Date">
                                                            <ItemTemplate>
                                                                <asp:TextBox runat="server" ID="txtdate" placeholder="dd/mm/yyyy" Width="80px"></asp:TextBox>
                                                                <cc1:MaskedEditExtender ID="MEE1" runat="server" Mask="99/99/9999" MaskType="Date"
                                                                    TargetControlID="txtdate" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Time Out">
                                                            <ItemTemplate>
                                                                <asp:TextBox runat="server" ID="txttout" placeholder="Time Out" Width="80px" ReadOnly="true"></asp:TextBox>
                                                                <cc1:MaskedEditExtender ID="me1" runat="server" Mask="99:99" MaskType="Time" UserTimeFormat="TwentyFourHour"
                                                                    TargetControlID="txttout">
                                                                </cc1:MaskedEditExtender>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Reason">
                                                            <ItemTemplate>
                                                                <asp:TextBox runat="server" ID="txtrsn" placeholder="Reason" Width="200px" TextMode="MultiLine"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                             
                            </Columns>
                            <EditRowStyle BackColor="#999999" />
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader2" />
                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager2" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem2" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        </fin:CoolGridView>--%>
                        </div>
                    </div>
                </div>


            </div>

        </section>
    </div>

    <%--                <div class="toolsContentLeft">
                    <div class="bSubBlock brandSecondaryBrd secondaryPalette">
                        <div class="lbBody">--%>
    <%-- <table style="text-align:center;width=100%;">--%>
    <%-- <tr>
                                    <td>
                                        <asp:TextBox ID="txtfrom" runat="server" CssClass="textboxStyle" onblur="Change(this, event)"
                                            onfocus="Change(this, event)" placeholder="User Name" ReadOnly="True" TabIndex="-1"></asp:TextBox>
                                        <asp:ImageButton ID="btnfrom" runat="server" Height="22px" ImageAlign="Middle" ImageUrl="~/css/images/bdsearch5.png"
                                            ToolTip="To" Width="24px" OnClick="btnfrom_Click" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtto" runat="server" CssClass="textboxStyle" onblur="Change(this, event)"
                                            onfocus="Change(this, event)" placeholder="Contact Person" ReadOnly="True" TabIndex="-1"></asp:TextBox>
                                        <asp:ImageButton ID="btnto" runat="server" Height="22px" ImageAlign="Middle" ImageUrl="~/css/images/bdsearch5.png"
                                            ToolTip="To" Width="24px" OnClick="btnto_Click" />
                                        <input id="txtrows" style="width: 70px;" type="text" placeholder="No. of rows" runat="server" />
                                        <asp:Button ID="btnshow" runat="server" CssClass="searchbtn" Text="Show" OnClick="btnshow_Click" />
                                        <asp:Label ID="lblshow" runat="server" Text="0"></asp:Label>
                                    </td>
                                </tr>--%>
    <%--            <tr id="r1" runat="server">
                                    <td colspan="2">
                                        <asp:TextBox ID="txtbcode" runat="server" Style="display: none;"></asp:TextBox>
                                        <asp:TextBox ID="txtbname" runat="server" CssClass="textboxStyle" onblur="Change(this, event)"
                                            onfocus="Change(this, event)" placeholder="Branch Name" ReadOnly="True" TabIndex="-1"
                                            Width="773px"></asp:TextBox>
                                        <asp:ImageButton ID="btnmbr" runat="server" Height="22px" ImageAlign="Middle" ImageUrl="~/css/images/bdsearch5.png"
                                            ToolTip="Branch Name" Width="24px" OnClick="btnmbr_Click" />
                                    </td>
                                </tr>--%>
    <%--<tr>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtsearch" runat="server" CssClass="textboxStyle" onblur="Change(this, event)"
                                            onfocus="Change(this, event)" placeholder="Search here..." TabIndex="-1" Width="700px"></asp:TextBox>
                                        <asp:Button ID="btnsearch" runat="server" CssClass="searchbtn" Text="Search" OnClick="btnsearch_Click"
                                            ToolTip="click here to search" />
                                        <asp:Button ID="btnexp" runat="server" CssClass="searchbtn" OnClick="btnexp_Click"
                                            Text="Export" ToolTip="click here to export data" />
                                    </td>
                                </tr>--%>
    <%--   <div class="toolsContentLeft">
                                            <div class="lbBody" style="color: White; box-shadow: 0 2px 4px rgba(127,127,127,.3);
                                                box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                                <asp:GridView ID="GridView1" Width="100%" runat="server" CellPadding="2" ForeColor="#333333"
                                                    GridLines="Both" Style="border-color: #E2DED6; background-color: #FFFFFF; color: White;"
                                                    AutoGenerateColumns="true" OnRowDataBound="GridView1_RowDataBound">
                                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                    <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                    <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader" />
                                                    <EditRowStyle BackColor="#999999" />
                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="View">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="LnkBtnv" runat="server" ToolTip="View , see the RFQ in Print format"
                                                                    OnClick="LnkBtnv_Click" ForeColor="#1797c0">View</asp:LinkButton>
                                                                &nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:LinkButton ID="LnkBtnd" runat="server" ToolTip="Download the link" OnClick="LnkBtnd_Click"
                                                                    ForeColor="#1797c0">Download</asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="OK" ItemStyle-Width="50px">
                                                            <ItemStyle BackColor="YellowGreen" />
                                                            <ItemTemplate>
                                                                <div class="vandana">
                                                                    <asp:CheckBox ID="chkok" runat="server" ToolTip="Select RFQ for approval" />
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="NO" ItemStyle-Width="50px">
                                                            <ItemStyle BackColor="Red" />
                                                            <ItemTemplate>
                                                                <div class="vandana">
                                                                    <asp:CheckBox ID="chkno" runat="server" ToolTip="Select RFQ for refusal" />
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Date">
                                                            <ItemTemplate>
                                                                <asp:TextBox runat="server" ID="txtdate" placeholder="dd/mm/yyyy" Width="80px"></asp:TextBox>
                                                                <cc1:MaskedEditExtender ID="MEE1" runat="server" Mask="99/99/9999" MaskType="Date"
                                                                    TargetControlID="txtdate" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Time Out">
                                                            <ItemTemplate>
                                                                <asp:TextBox runat="server" ID="txttout" placeholder="Time Out" Width="80px" ReadOnly="true"></asp:TextBox>
                                                                <cc1:MaskedEditExtender ID="me1" runat="server" Mask="99:99" MaskType="Time" UserTimeFormat="TwentyFourHour"
                                                                    TargetControlID="txttout">
                                                                </cc1:MaskedEditExtender>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Reason">
                                                            <ItemTemplate>
                                                                <asp:TextBox runat="server" ID="txtrsn" placeholder="Reason" Width="200px" TextMode="MultiLine"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
    --%>
    <%-- </table>--%>
    <%--                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>--%>
    <asp:Button ID="btnhideF" runat="server" OnClick="btnhideF_Click" Style="display: none" />
    <%--<asp:Button ID="btnhideF_s" runat="server" OnClick="btnhideF_s_Click" Style="display: none" />--%>
    <asp:HiddenField ID="doc_nf" runat="server" />
    <asp:HiddenField ID="doc_df" runat="server" />
    <asp:HiddenField ID="doc_vty" runat="server" />
    <asp:HiddenField ID="doc_addl" runat="server" />
    <asp:HiddenField ID="edmode" runat="server" />
    <asp:HiddenField ID="hffield" runat="server" />
    <asp:HiddenField ID="hf1" runat="server" />
    <asp:HiddenField ID="lbledmode" runat="server" />
    <asp:HiddenField ID="lblname" runat="server" />
    <asp:HiddenField ID="HFOLDDT" runat="server" />
    <asp:HiddenField ID="HFOPT" runat="server" />
    <asp:HiddenField ID="hf2" runat="server" />
    <asp:HiddenField ID="hfdept" runat="server" />
    <asp:HiddenField ID="hfbtnmode" runat="server" />
    <asp:HiddenField ID="TabName" runat="server" />
    <asp:HiddenField ID="hfGridView1SV" runat="server" />
    <asp:HiddenField ID="hfGridView1SH" runat="server" />
    <asp:HiddenField ID="FilePath" runat="server" />
    <CR:CrystalReportViewer ID="CRV1" runat="server" AutoDataBind="true" Style="display: none;" />
    <%-- <asp:HiddenField ID="hfbtnmode" runat="server" />
    <CR:CrystalReportViewer ID="CRV1" runat="server" AutoDataBind="true" Style="display: none;" />
    <input type="button" id="btnhideF" runat="server" onserverclick="btnhideF_Click"
        style="display: none" />--%>
</asp:Content>
