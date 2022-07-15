<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="frm_ShowImages" Title="Tejaxo" CodeFile="frm_ShowImages.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style1
        {
            width: 100px;
        }
        .style2
        {
            width: 131px;
        }
        .style3
        {
            font-size: x-small;
            word-spacing: 0;
        }
        .style4
        {
            width: 448px;
        }
        .style5
        {
            width: 80px;
        }
        .auto-style1
        {
            width: 123px;
        }
        .auto-style2
        {
            width: 144px;
        }
        .auto-style3
        {
            width: 23px;
        }
        .auto-style4
        {
            height: 22px;
        }
        .auto-style5
        {
            width: 110px;
        }
        .auto-style6
        {
            width: 26px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server" >
    <section class="content">
            <div class="row">
                <div class="col-md-12">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
<asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label>
<br />
    <asp:Label ID="lbl" runat="server" Style="background-color:white"></asp:Label>
    <a id="A1" runat="server" href='<%#Eval("po") %>' target="_blank">
        <asp:Image ID="Image2" runat="server" ImageAlign="Middle" ToolTip="View PO Attachment"/></a>
                                </div></div></div></div>
     
       <section class="col-lg-12 connectedSortable">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                <div class="tab-content" >
         <div class="lbBody" id="gridDiv" style="color: White; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
        <asp:GridView ID="sg1" runat="server" AutoGenerateColumns="False" OnRowDataBound="sg1_RowDataBound"
            ForeColor="#333333" Style="background-color: #FFFFFF; color: White;" Width="100%"  Height="100%" Font-Size="13px">
            <Columns>
                <asp:BoundField DataField="srno" HeaderText="Sr." ReadOnly="True"></asp:BoundField>
                <asp:BoundField DataField="icode" HeaderText="Item Code" ReadOnly="True"></asp:BoundField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        Item Drawing</HeaderTemplate>
                    <ItemTemplate>
                        <asp:TextBox ID="txttfld4" runat="server" Width="100px" Text='<%#Eval("itemdrg") %>'
                            MaxLength="50" ReadOnly="true"></asp:TextBox>
                        <a id="dd" runat="server" href='<%#Eval("pdf") %>' target="_blank" style="width: 100px">
                            <asp:Image ID="Image1" runat="server" ImageAlign="Middle" ToolTip="View"/></a></ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EditRowStyle BackColor="#999999" />
           <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader2" />
            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager2" />
             <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem2" />
           <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        </asp:GridView>
    </div>
</div>
</div></div></section>
       </div></section>
</asp:Content>
