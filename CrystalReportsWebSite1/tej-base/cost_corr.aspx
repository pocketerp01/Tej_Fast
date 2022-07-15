<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="cost_corr" EnableEventValidation="true" CodeFile="cost_corr.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style2 {
            font-size: 8pt;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="content-wrapper">
        <section class="content-header">
            <table style="width: 100%">
                <tr>
                    <td>
                        <button id="btnnew" runat="server" accesskey="N" class="btn btn-info" onserverclick="btnnew_ServerClick" style="width: 100px"><u>N</u>ew</button>
                        <button id="btnedit" runat="server" accesskey="i" class="btn btn-info" onserverclick="btnedit_ServerClick" style="width: 100px">Ed<u>i</u>t</button>
                        <button id="btnsave" runat="server" accesskey="S" class="btn btn-info" onserverclick="btnsave_ServerClick" style="width: 100px"><u>S</u>ave</button>
                        <button id="btndel" runat="server" accesskey="l" class="btn btn-info" onserverclick="btndel_ServerClick" style="width: 100px">De<u>l</u>ete</button>
                        <button id="btnprnt" runat="server" accesskey="P" class="btn btn-info" onserverclick="btnprnt_ServerClick" style="width: 100px"><u>P</u>rint</button>
                        <asp:Button ID="btnext" runat="server" Text="Exit" OnClick="btnext_Click" class="btn btn-info" Width="100px" />&nbsp;
                    </td>
                    <td>
                        <asp:Label ID="lbheader" runat="server" Text="Pre Costing Sheet" Font-Bold="True" Font-Size="X-Large"></asp:Label>

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
                                <label id="mrheading" runat="server" class="col-sm-2 control-label">Party Name</label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnacode" runat="server" ToolTip="Select Party Name" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnacode_Click" />
                                </div>
                                <div class="col-sm-2">
                                    <asp:TextBox MaxLength="10" ID="txtacode" runat="server" placeholder="Party Code" ReadOnly="true" CssClass="form-control" Height="28px"></asp:TextBox>
                                </div>
                                <div class="col-sm-6">
                                    <asp:TextBox MaxLength="10" ID="txtaname" runat="server" placeholder="Party Name" ReadOnly="true" CssClass="form-control" Height="28px" OnTextChanged="txtaname_TextChanged1"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="ItemNam" runat="server" class="col-sm-2 control-label">Item Name</label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnicode" runat="server" ToolTip="Select Item Name" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnicode_Click" />
                                </div>
                                <div class="col-sm-2">
                                    <asp:TextBox MaxLength="29" ID="txticode" runat="server" placeholder="Item Code" ReadOnly="true" CssClass="form-control" Height="28px"></asp:TextBox>
                                </div>
                                <div class="col-sm-6">
                                    <asp:TextBox MaxLength="29" ID="txtiname" runat="server" placeholder="Item Name" ReadOnly="true" CssClass="form-control" Height="28px"></asp:TextBox>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <label id="Label1" runat="server" class="col-sm-3 control-label">Cost Sheet No</label>
                                <div class="col-sm-3">
                                    <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10" ID="txtvchnum" runat="server" ReadOnly="true" CssClass="form-control" Height="28px"></asp:TextBox>
                                </div>
                                <label id="Label2" runat="server" class="col-sm-3 control-label">Date</label>
                                <div class="col-sm-3">
                                    <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10" ID="txtvchdt" runat="server" CssClass="form-control" Height="28px"> </asp:TextBox>
                                    <asp:CalendarExtender ID="txtvchdt_CalendarExtender" runat="server"
                                        Enabled="True" TargetControlID="txtvchdt" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="Maskedit1" runat="server" Mask="99/99/9999"
                                        MaskType="Date" TargetControlID="txtvchdt" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label3" runat="server" class="col-sm-3 control-label">Type of Box</label>
                                <div class="col-sm-9">
                                    <asp:DropDownList ID="dd1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="dd1_SelectedIndexChanged" Width="100%" CssClass="form-control" Font-Size="Smaller" Height="28px">
                                        <asp:ListItem Text="Universal Type (RSC)" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Over Flap Model" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Tray Model" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Pad & Punching Item" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="HSC Model" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="Z & U Partition" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="Bottom Self Lock Type" Value="6"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-12">
                    <div>
                        <div class="form-group">
                            <div class="col-sm-2">
                                <asp:Label ID="lbl1" runat="server"></asp:Label>
                            </div>
                            <div class="col-sm-1">
                                <asp:Label ID="lbl2" runat="server"></asp:Label>
                            </div>
                            <div class="col-sm-1">
                                <asp:Label ID="lbl3" runat="server"></asp:Label>
                            </div>
                            <div class="col-sm-1">
                                <asp:Label ID="lbl4" runat="server"></asp:Label>
                            </div>
                            <div class="col-sm-1">
                                <asp:Label ID="lbl5" runat="server"></asp:Label>
                            </div>
                            <div class="col-sm-1">
                                <asp:Label ID="lbl6" runat="server"></asp:Label>
                            </div>
                            <div class="col-sm-1">
                                <asp:Label ID="lbl7" runat="server"></asp:Label>
                            </div>
                            <div class="col-sm-1">
                                <asp:Label ID="lbl8" runat="server"></asp:Label>
                            </div>
                            <div class="col-sm-1">
                                <asp:Label ID="lbl9" runat="server"></asp:Label>
                            </div>
                            <div class="col-sm-1">
                                <asp:Label ID="lbl10" runat="server"></asp:Label>
                            </div>
                            <div class="col-sm-1">
                                <asp:Label ID="lbl11" runat="server"></asp:Label>
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="col-sm-2">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt1" runat="server" Width="70px" BackColor="#BDEDFF"
                                    AutoPostBack="true" Style="text-align: right;" Height="24px"></asp:TextBox>
                                <asp:Label ID="lbl12" runat="server"></asp:Label>
                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox
                                    ID="txtpt2" runat="server" Width="80px" BackColor="#BDEDFF" onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    AutoPostBack="true" Style="text-align: right" OnTextChanged="txtpt2_TextChanged" Height="24px"></asp:TextBox>
                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt3" runat="server" Width="80px" BackColor="#BDEDFF"
                                    AutoPostBack="true" Style="text-align: right" OnTextChanged="txtpt3_TextChanged" />
                            </div>

                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt4" runat="server" Width="80px" BackColor="#BDEDFF"
                                    AutoPostBack="true" Style="text-align: right"
                                    OnTextChanged="txtpt4_TextChanged" Height="24px"></asp:TextBox>
                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt5" runat="server" Width="80px" BackColor="#BDEDFF"
                                    AutoPostBack="true" Style="text-align: right"
                                    OnTextChanged="txtpt5_TextChanged" Height="24px"></asp:TextBox>
                            </div>

                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt6" runat="server" Width="80px" BackColor="#BDEDFF"
                                    AutoPostBack="true" Style="text-align: right"
                                    OnTextChanged="txtpt6_TextChanged" Height="24px"></asp:TextBox>
                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt7" runat="server" Width="80px" BackColor="#BDEDFF"
                                    AutoPostBack="true" Style="text-align: right"
                                    OnTextChanged="txtpt7_TextChanged" Height="24px"></asp:TextBox>
                            </div>
                            <div class="col-sm-1">

                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt8" runat="server" Width="80px" BackColor="#BDEDFF"
                                    AutoPostBack="true" Style="text-align: right"
                                    OnTextChanged="txtpt8_TextChanged" Height="24px"></asp:TextBox>
                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt9" runat="server" Width="80px" BackColor="#BDEDFF"
                                    AutoPostBack="true" Style="text-align: right" OnTextChanged="txtpt9_TextChanged" Height="24px"></asp:TextBox>
                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt10" runat="server" Width="80px" ReadOnly="true"
                                    Style="text-align: right" AutoPostBack="true" BackColor="#BDEDFF"
                                    OnTextChanged="txtpt10_TextChanged" Height="24px"></asp:TextBox>
                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)"
                                    MaxLength="10" ID="txtpt11" runat="server" Width="80px"
                                    ReadOnly="true" Style="text-align: right" OnTextChanged="txtpt11_TextChanged" Height="24px"></asp:TextBox>
                                <asp:Label ID="tl1" runat="server"></asp:Label>
                            </div>
                        </div>

                        <div class="form-group">

                            <div class="col-sm-2">
                                <asp:Label ID="lbl13" runat="server" Height="24px"></asp:Label>
                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt12" runat="server" Width="80px" BackColor="#BDEDFF"
                                    AutoPostBack="true" Style="text-align: right" OnTextChanged="txtpt12_TextChanged" Height="24px"></asp:TextBox>
                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt13" runat="server" Width="80px" BackColor="#BDEDFF"
                                    AutoPostBack="true" Style="text-align: right" OnTextChanged="txtpt13_TextChanged" Height="24px"></asp:TextBox>
                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt14" runat="server" Width="80px" BackColor="#BDEDFF"
                                    AutoPostBack="true" Style="text-align: right" OnTextChanged="txtpt14_TextChanged" Height="24px"></asp:TextBox>
                            </div>
                            <label class="col-sm-1 control-label style2">Deckle_Size(mm)</label>

                            <div class="col-sm-1">
                                <asp:TextBox ID="txtpt15" runat="server" Width="80px"
                                    ReadOnly="true" AutoPostBack="true"
                                    Style="text-align: right" Height="24px"></asp:TextBox>
                            </div>

                            <label class="col-sm-1 control-label style2">Cutting_Size(mm)</label>

                            <div class="col-sm-1">
                                <asp:TextBox ID="txtpt16" runat="server" Width="80px"
                                    ReadOnly="true" AutoPostBack="true"
                                    Style="text-align: right" Height="24px"></asp:TextBox>
                            </div>
                            <label class="col-sm-1 control-label style2">Wt.CFC(gms)</label>

                            <div class="col-sm-1">
                                <asp:TextBox ID="txtpt17" runat="server" Width="80px"
                                    ReadOnly="true" AutoPostBack="true"
                                    Style="text-align: right" Height="24px"></asp:TextBox>
                            </div>
                            <div class="col-sm-1">
                                <asp:Label ID="tl2" runat="server" Width="70px" Height="24px">Total GSM</asp:Label>
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="col-sm-2">
                                <%--   <asp:Label ID="tr2" runat="server" Height="24px"></asp:Label> --%>
                                <asp:Label ID="lbl14" runat="server" Height="24px"></asp:Label>
                            </div>

                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt18" runat="server" Width="80px" ReadOnly="true" AutoPostBack="true"
                                    Style="text-align: right" Height="24px"></asp:TextBox>
                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10" AutoPostBack="true"
                                    ID="txtpt19" runat="server" Width="80px" ReadOnly="true"
                                    Style="text-align: right" Height="24px"></asp:TextBox>
                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt20" runat="server" Width="80px" ReadOnly="true" AutoPostBack="true"
                                    Style="text-align: right" Height="24px"></asp:TextBox>
                            </div>

                            <label class="col-sm-1 control-label style2">Deckle_Size(cm)</label>
                            <div class="col-sm-1">
                                <asp:TextBox ID="txtpt21" runat="server" Width="80px"
                                    ReadOnly="true" AutoPostBack="true"
                                    Style="text-align: right" Height="24px"></asp:TextBox>
                            </div>
                            <label class="col-sm-1 control-label style2">Cutting_Size(cm)</label>

                            <div class="col-sm-1">
                                <asp:TextBox ID="txtpt22" runat="server" Width="80px"
                                    ReadOnly="true" AutoPostBack="true"
                                    Style="text-align: right" Height="24px"></asp:TextBox>
                            </div>

                            <label class="col-sm-1 control-label style2">Wt.CFC(Kgs)</label>

                            <div class="col-sm-1">
                                <asp:TextBox ID="txtpt23" runat="server" Width="80px"
                                    ReadOnly="true" AutoPostBack="true"
                                    Style="text-align: right" Height="24px"></asp:TextBox>
                            </div>

                            <div class="col-sm-1">
                                <asp:TextBox ID="txtpt24"
                                    runat="server" Width="80px" AutoPostBack="true"
                                    onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    BackColor="#BDEDFF"
                                    Style="text-align: right" Height="24px"></asp:TextBox>
                            </div>

                        </div>


                        <div class="form-group">
                            <div class="col-sm-1">
                            </div>
                            <label id="Label6" runat="server" class="col-sm-1 control-label">GSM</label>
                            <label id="Label7" runat="server" class="col-sm-1 control-label">BF</label>
                            <label id="trct1" runat="server" class="col-sm-1 control-label">RCT</label>
                            <div class="col-sm-1">
                            </div>
                            <label id="Label8" runat="server" class="col-sm-1 control-label">Take up</label>
                            <div class="col-sm-1">
                                <asp:Label ID="lbl15" runat="server" />
                            </div>
                            <div class="col-sm-1">
                                <asp:Label ID="lbl16" runat="server" Style="font-size: 8pt" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="lbl17" runat="server"></asp:Label>
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="lbl18" runat="server"></asp:Label>
                            </div>
                            <label id="Label9" runat="server" class="col-sm-1 control-label">Cost/Kg</label>

                        </div>
                        <div class="form-group">
                            <div class="col-sm-1">
                                <label id="Label10" runat="server">Liner</label>
                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt25" runat="server" Width="80px" BackColor="#BDEDFF"
                                    AutoPostBack="true" Style="text-align: right" OnTextChanged="txtpt25_TextChanged" Height="24px"></asp:TextBox>
                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt26" runat="server" Width="80px" BackColor="#BDEDFF"
                                    AutoPostBack="true" Style="text-align: right" OnTextChanged="txtpt26_TextChanged" Height="24px"></asp:TextBox>
                            </div>

                            <div class="col-sm-1">
                                <asp:Label ID="trct2" runat="server"></asp:Label>
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt27" runat="server" Width="80px" ReadOnly="true"
                                    Style="text-align: right" Height="24px"></asp:TextBox>
                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10" BackColor="#BDEDFF"
                                    ID="txtpt28" runat="server" Width="80px"
                                    Style="text-align: right" AutoPostBack="true" OnTextChanged="txtpt28_TextChanged" Height="24px"></asp:TextBox>
                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt29" runat="server" Width="80px" ReadOnly="true"
                                    Style="text-align: right" Height="24px"></asp:TextBox>
                            </div>

                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt30" runat="server" Width="80px" ReadOnly="true"
                                    Style="text-align: right" Height="24px"></asp:TextBox>
                            </div>
                            <div class="col-sm-2">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt31" runat="server" Width="80px" BackColor="#BDEDFF"
                                    Style="text-align: right" OnTextChanged="txtpt31_TextChanged" AutoPostBack="true" Height="24px"></asp:TextBox>
                            </div>

                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt32" runat="server" Width="80px" ReadOnly="true"
                                    Style="text-align: right" Height="24px"></asp:TextBox>
                            </div>
                            <div class="col-sm-1">
                                <label id="Label11" runat="server">Fixed</label>
                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt33" runat="server" Width="100%" BackColor="#BDEDFF"
                                    AutoPostBack="true" Style="text-align: right" OnTextChanged="txtpt34_TextChanged" Height="24px"></asp:TextBox>
                            </div>

                        </div>
                        <div class="form-group">
                            <div class="col-sm-1">
                                <label id="Label12" runat="server">Flute</label>
                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt34" runat="server" Width="80px" BackColor="#BDEDFF"
                                    AutoPostBack="true" Style="text-align: right" OnTextChanged="txtpt34_TextChanged" Height="24px"></asp:TextBox>
                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt35" runat="server" Width="80px" BackColor="#BDEDFF"
                                    AutoPostBack="true" Style="text-align: right" OnTextChanged="txtpt35_TextChanged" Height="24px"></asp:TextBox>
                            </div>

                            <div class="col-sm-1">
                                <asp:Label ID="trct3" runat="server"></asp:Label>
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt36" runat="server" Width="80px" ReadOnly="true"
                                    Style="text-align: right" Height="24px"></asp:TextBox>
                            </div>

                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10" BackColor="#BDEDFF"
                                    ID="txtpt37" runat="server" Width="80px"
                                    Style="text-align: right" OnTextChanged="txtpt37_TextChanged" AutoPostBack="true" Height="24px"></asp:TextBox>
                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt38" runat="server" Width="80px" ReadOnly="true"
                                    Style="text-align: right" Height="24px"></asp:TextBox>
                            </div>

                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt39" runat="server" Width="80px" ReadOnly="true"
                                    Style="text-align: right" Height="24px"></asp:TextBox>
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="lbl19" runat="server"></asp:Label>
                                <asp:TextBox ID="txtpt40" runat="server" onkeypress="return isDecimalKey(event)"
                                    MaxLength="10" Width="80px" BackColor="#BDEDFF"
                                    Style="text-align: right" OnTextChanged="txtpt40_TextChanged" AutoPostBack="true" Height="24px"></asp:TextBox>
                            </div>

                            <div class="col-sm-1">
                                <asp:Label ID="lbl20" runat="server"></asp:Label>
                                <asp:TextBox ID="txtpt41" runat="server" onkeypress="return isDecimalKey(event)"
                                    MaxLength="10" Width="80px" ReadOnly="true"
                                    Style="text-align: right" Height="24px"></asp:TextBox>
                            </div>
                            <div class="col-sm-1">
                                <label id="Label13" runat="server">Variable Cost</label>
                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt42" runat="server" Width="100%" BackColor="#BDEDFF"
                                    Style="text-align: right" OnTextChanged="txtpt42_TextChanged" AutoPostBack="true" Height="24px"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="col-sm-1">
                                <label id="Label14" runat="server">Liner</label>
                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt43" runat="server" Width="80px" BackColor="#BDEDFF"
                                    AutoPostBack="true" Style="text-align: right" OnTextChanged="txtpt43_TextChanged" Height="24px"></asp:TextBox>
                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt44" runat="server" Width="80px" BackColor="#BDEDFF"
                                    AutoPostBack="true" Style="text-align: right" OnTextChanged="txtpt44_TextChanged" Height="24px"></asp:TextBox>
                            </div>

                            <div class="col-sm-1">
                                <label id="trct4" runat="server" />
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt45" runat="server" Width="80px" ReadOnly="true"
                                    Style="text-align: right" Height="24px"></asp:TextBox>
                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10" BackColor="#BDEDFF"
                                    ID="txtpt46" runat="server" Width="80px"
                                    Style="text-align: right" OnTextChanged="txtpt46_TextChanged" AutoPostBack="true" Height="24px"></asp:TextBox>
                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt47" runat="server" Width="80px" ReadOnly="true"
                                    Style="text-align: right" Height="24px"></asp:TextBox>
                            </div>

                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt48" runat="server" Width="80px" ReadOnly="true"
                                    Style="text-align: right" Height="24px"></asp:TextBox>
                            </div>
                            <div class="col-sm-2">
                                <asp:TextBox onkeypress="return isDecimalKey(event)"
                                    MaxLength="10" ID="txtpt49" runat="server" Width="80px"
                                    BackColor="#BDEDFF" Style="text-align: right"
                                    OnTextChanged="txtpt49_TextChanged" AutoPostBack="true" Height="24px"></asp:TextBox>
                            </div>

                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt50" runat="server" Width="80px" ReadOnly="true"
                                    Style="text-align: right" Height="24px"></asp:TextBox>
                            </div>
                            <div class="col-sm-1">
                                <label id="Label15" runat="server">Corru. Gum</label>

                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt51" runat="server" Width="100%" BackColor="#BDEDFF"
                                    Style="text-align: right" OnTextChanged="txtpt51_TextChanged" AutoPostBack="true" Height="24px"></asp:TextBox>
                            </div>

                        </div>

                        <div class="form-group">
                            <div class="col-sm-1">
                                <label id="Label16" runat="server">Flute</label>
                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt52" runat="server" Width="80px" BackColor="#BDEDFF" AutoPostBack="true"
                                    Style="text-align: right" OnTextChanged="txtpt43_TextChanged" Height="24px"></asp:TextBox>
                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt53" runat="server" Width="80px" BackColor="#BDEDFF"
                                    AutoPostBack="true" Style="text-align: right" OnTextChanged="txtpt44_TextChanged" Height="24px"></asp:TextBox>
                            </div>

                            <div class="col-sm-1">
                                <label id="trct5" runat="server" />
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt54" runat="server" Width="80px" ReadOnly="true"
                                    Style="text-align: right" Height="24px"></asp:TextBox>
                            </div>

                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10" BackColor="#BDEDFF"
                                    ID="txtpt55" runat="server" Width="80px" AutoPostBack="true"
                                    Style="text-align: right" OnTextChanged="txtpt46_TextChanged" Height="24px"></asp:TextBox>
                            </div>

                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt56" runat="server" Width="80px" ReadOnly="true"
                                    Style="text-align: right" Height="24px"></asp:TextBox>
                            </div>

                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt57" runat="server" Width="80px" ReadOnly="true"
                                    Style="text-align: right" Height="24px"></asp:TextBox>
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="lbl23" runat="server"></asp:Label>
                                <asp:TextBox onkeypress="return isDecimalKey(event)"
                                    MaxLength="10" ID="txtpt58" runat="server" Width="80px"
                                    BackColor="#BDEDFF" Style="text-align: right"
                                    OnTextChanged="txtpt49_TextChanged" AutoPostBack="true" Height="24px"></asp:TextBox>
                            </div>

                            <div class="col-sm-1">
                                <asp:Label ID="lbl24" runat="server"></asp:Label>
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt59" runat="server" Width="80px" ReadOnly="true"
                                    Style="text-align: right" Height="24px"></asp:TextBox>
                            </div>
                            <div class="col-sm-1">
                                <label id="Label18" runat="server">Stitching</label>

                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt60" runat="server" Width="100%" BackColor="#BDEDFF"
                                    Style="text-align: right" OnTextChanged="txtpt51_TextChanged" AutoPostBack="true" Height="24px"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="col-sm-1">
                                <label id="Label17" runat="server">Liner</label>
                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt61" runat="server" Width="80px" BackColor="#BDEDFF"
                                    AutoPostBack="true" Style="text-align: right" OnTextChanged="txtpt43_TextChanged" Height="24px"></asp:TextBox>
                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt62" runat="server" Width="80px" BackColor="#BDEDFF"
                                    AutoPostBack="true" Style="text-align: right" OnTextChanged="txtpt44_TextChanged" Height="24px"></asp:TextBox>
                            </div>

                            <div class="col-sm-1">
                                <label id="trct6" runat="server" />
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt63" runat="server" Width="80px" ReadOnly="true"
                                    Style="text-align: right" Height="24px"></asp:TextBox>
                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10" BackColor="#BDEDFF"
                                    ID="txtpt64" runat="server" Width="80px"
                                    Style="text-align: right" OnTextChanged="txtpt46_TextChanged" AutoPostBack="true" Height="24px"></asp:TextBox>
                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt65" runat="server" Width="80px" ReadOnly="true"
                                    Style="text-align: right" Height="24px"></asp:TextBox>
                            </div>

                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt66" runat="server" Width="80px" ReadOnly="true"
                                    Style="text-align: right" Height="24px"></asp:TextBox>
                            </div>
                            <div class="col-sm-2">
                                <asp:TextBox onkeypress="return isDecimalKey(event)"
                                    MaxLength="10" ID="txtpt67" runat="server" Width="80px"
                                    BackColor="#BDEDFF" Style="text-align: right"
                                    OnTextChanged="txtpt49_TextChanged" AutoPostBack="true" Height="24px"></asp:TextBox>
                            </div>

                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt68" runat="server" Width="80px" ReadOnly="true"
                                    Style="text-align: right" Height="24px"></asp:TextBox>
                            </div>
                            <div class="col-sm-1">
                                <label id="Label20" runat="server">Printing Ink</label>

                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt69" runat="server" Width="100%" BackColor="#BDEDFF"
                                    Style="text-align: right" OnTextChanged="txtpt51_TextChanged" AutoPostBack="true" Height="24px"></asp:TextBox>
                            </div>

                        </div>

                        <div class="form-group">
                            <div class="col-sm-1">
                                <label id="Label19" runat="server">Flute</label>
                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt70" runat="server" Width="80px" BackColor="#BDEDFF"
                                    AutoPostBack="true" Style="text-align: right" OnTextChanged="txtpt43_TextChanged" Height="24px"></asp:TextBox>
                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt71" runat="server" Width="80px" BackColor="#BDEDFF"
                                    AutoPostBack="true" Style="text-align: right" OnTextChanged="txtpt44_TextChanged" Height="24px"></asp:TextBox>
                            </div>

                            <div class="col-sm-1">
                                <label id="trct7" runat="server" />
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt72" runat="server" Width="80px" ReadOnly="true"
                                    Style="text-align: right" Height="24px"></asp:TextBox>
                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10" BackColor="#BDEDFF"
                                    ID="txtpt73" runat="server" Width="80px"
                                    Style="text-align: right" OnTextChanged="txtpt46_TextChanged" AutoPostBack="true" Height="24px"></asp:TextBox>
                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt74" runat="server" Width="80px" ReadOnly="true"
                                    Style="text-align: right" Height="24px"></asp:TextBox>
                            </div>

                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt75" runat="server" Width="80px" ReadOnly="true"
                                    Style="text-align: right" Height="24px"></asp:TextBox>
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="lbl25" runat="server"></asp:Label>
                                <asp:TextBox onkeypress="return isDecimalKey(event)"
                                    MaxLength="10" ID="txtpt76" runat="server" Width="80px"
                                    BackColor="#BDEDFF" Style="text-align: right"
                                    OnTextChanged="txtpt49_TextChanged" AutoPostBack="true" Height="24px"></asp:TextBox>
                            </div>

                            <div class="col-sm-1">
                                <asp:Label ID="lbl26" runat="server"></asp:Label>
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt77" runat="server" Width="80px" ReadOnly="true"
                                    Style="text-align: right" Height="24px"></asp:TextBox>
                            </div>
                            <div class="col-sm-1">
                                <label id="Label24" runat="server">Cloth</label>

                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt78" runat="server" Width="100%" BackColor="#BDEDFF"
                                    Style="text-align: right" OnTextChanged="txtpt51_TextChanged" AutoPostBack="true" Height="24px"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="col-sm-1">
                                <label id="Label21" runat="server">Liner</label>
                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt93" runat="server" Width="80px" BackColor="#BDEDFF"
                                    AutoPostBack="true" Style="text-align: right" OnTextChanged="txtpt43_TextChanged" Height="24px"></asp:TextBox>
                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt94" runat="server" Width="80px" BackColor="#BDEDFF"
                                    AutoPostBack="true" Style="text-align: right" OnTextChanged="txtpt44_TextChanged" Height="24px"></asp:TextBox>
                            </div>

                            <div class="col-sm-1">
                                <label id="trct8" runat="server" />
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt95" runat="server" Width="80px" ReadOnly="true"
                                    Style="text-align: right" Height="24px"></asp:TextBox>
                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10" BackColor="#BDEDFF"
                                    ID="txtpt96" runat="server" Width="80px"
                                    Style="text-align: right" OnTextChanged="txtpt46_TextChanged" AutoPostBack="true" Height="24px"></asp:TextBox>
                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt97" runat="server" Width="80px" ReadOnly="true"
                                    Style="text-align: right" Height="24px"></asp:TextBox>
                            </div>

                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt98" runat="server" Width="80px" ReadOnly="true"
                                    Style="text-align: right" Height="24px"></asp:TextBox>
                            </div>
                            <div class="col-sm-2">
                                <asp:TextBox onkeypress="return isDecimalKey(event)"
                                    MaxLength="10" ID="txtpt99" runat="server" Width="80px"
                                    BackColor="#BDEDFF" Style="text-align: right"
                                    OnTextChanged="txtpt49_TextChanged" AutoPostBack="true" Height="24px"></asp:TextBox>
                            </div>

                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt100" runat="server" Width="80px" ReadOnly="true"
                                    Style="text-align: right" Height="24px"></asp:TextBox>
                            </div>
                            <div class="col-sm-1">
                                <label id="Label23" runat="server">Packing Cost</label>

                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt82" runat="server" Width="100%" BackColor="#BDEDFF"
                                    Style="text-align: right" OnTextChanged="txtpt51_TextChanged" AutoPostBack="true" Height="24px"></asp:TextBox>
                            </div>

                        </div>

                        <div class="form-group">
                            <div class="col-sm-1">
                                <label id="Label22" runat="server">Total</label>
                            </div>
                            <div class="col-sm-1"></div>
                            <div class="col-sm-1"></div>
                            <div class="col-sm-1"></div>
                            <div class="col-sm-1"></div>
                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10" ReadOnly="true"
                                    ID="txtpt79" runat="server" Width="80px" Style="text-align: right" Height="24px"></asp:TextBox>
                            </div>

                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt80" runat="server" Width="80px"
                                    ReadOnly="true" Style="text-align: right" Height="24px"></asp:TextBox>
                            </div>
                            <div class="col-sm-2">
                                <label id="Label25" runat="server">Cost per Box</label>
                            </div>

                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt81" runat="server" Width="80px" ReadOnly="true"
                                    Style="text-align: right" Height="24px"></asp:TextBox>
                            </div>
                            <div class="col-sm-1">
                                <label id="Label26" runat="server">Labour Cost</label>

                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt83" runat="server" Width="100%" BackColor="#BDEDFF"
                                    Style="text-align: right" OnTextChanged="txtpt51_TextChanged" AutoPostBack="true" Height="24px"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="col-sm-1">
                                <label id="Label27" runat="server">Deckle</label>
                            </div>
                            <div class="col-sm-1">
                                <label id="Label30" runat="server">Length(mm)</label>
                            </div>
                            <div class="col-sm-2">
                                <label id="Label31" runat="server">Box_GR.Wt(g)</label>
                            </div>

                            <div class="col-sm-1">
                            </div>
                            <div class="col-sm-2">
                                <label id="Label28" runat="server">Last_Rates_Details</label>
                            </div>

                            <div class="col-sm-2">
                                <label id="Label32" style="border-width: thin; font-family: Arial, Helvetica, sans-serif; font-weight: 700; color: #474646; font-size: 14px; background-color: #FFFF99; border-top-style: groove; border-left-style: groove;" runat="server">Cost per Box</label>
                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" ID="txthead"
                                    runat="server" Width="80px" Style="font-weight: bold; text-align: right; font-family: Arial, Helvetica, sans-serif; font-weight: 700; color: #474646; font-size: 14px;"
                                    ReadOnly="true" BackColor="#FFFF99" Height="24px"></asp:TextBox>
                            </div>

                            <div class="col-sm-1">
                                <label id="Label29" runat="server">Power Cost</label>

                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt87" runat="server" Width="100%" BackColor="#BDEDFF"
                                    Style="text-align: right" OnTextChanged="txtpt87_TextChanged" AutoPostBack="true" Height="24px"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="col-sm-1">
                            </div>
                            <div class="col-sm-1">
                            </div>
                            <div class="col-sm-1">
                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt84" runat="server" Width="80px" ReadOnly="true"
                                    AutoPostBack="true" Style="text-align: right" Height="24px"></asp:TextBox>
                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt85" runat="server" Width="80px" ReadOnly="true"
                                    Style="text-align: right" Height="24px"></asp:TextBox>

                            </div>
                            <div class="col-sm-2">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt86" runat="server" Width="80px" ReadOnly="true"
                                    AutoPostBack="true" Style="text-align: right" Height="24px"></asp:TextBox>
                            </div>

                            <div class="col-sm-1">
                            </div>
                            <div class="col-sm-1">
                                <asp:Button ID="btninv" runat="server" Width="64px" Text="Invoice"
                                    OnClick="btninv_Click" />
                            </div>
                            <div class="col-sm-1">
                                <asp:Button ID="btnso" runat="server" Width="65px" Text="SO" OnClick="btnso_Click" />
                            </div>

                            <div class="col-sm-1">
                                <asp:Label ID="lbl27" runat="server"></asp:Label>
                            </div>
                            <div class="col-sm-2">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt101" runat="server" Width="80px" BackColor="#BDEDFF"
                                    Style="text-align: right"
                                    AutoPostBack="true" OnTextChanged="txtpt101_TextChanged" Height="24px"></asp:TextBox>
                            </div>

                            <div class="col-sm-1">
                                <label id="Label35" runat="server">Fuel Cost</label>

                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt88" runat="server" Width="100%" BackColor="#BDEDFF"
                                    Style="text-align: right" OnTextChanged="txtpt88_TextChanged" AutoPostBack="true" Height="24px"></asp:TextBox>
                            </div>

                        </div>

                        <div class="form-group">
                            <div class="col-sm-1">
                            </div>
                            <div class="col-sm-1">
                            </div>
                            <div class="col-sm-2">
                            </div>

                            <div class="col-sm-1">
                            </div>
                            <div class="col-sm-1">
                            </div>
                            <div class="col-sm-1">
                            </div>

                            <div class="col-sm-1">
                                <asp:Label ID="lbl28" runat="server"></asp:Label>
                            </div>
                            <div class="col-sm-2">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt102" runat="server" Width="70px" BackColor="#BDEDFF"
                                    Style="text-align: right"
                                    AutoPostBack="true" OnTextChanged="txtpt102_TextChanged" Height="24px"></asp:TextBox>
                            </div>

                            <div class="col-sm-1">
                                <label id="Label34" runat="server">Delivery Cost</label>

                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt89" runat="server" Width="100%" BackColor="#BDEDFF"
                                    Style="text-align: right" OnTextChanged="txtpt89_TextChanged" AutoPostBack="true" Height="24px"></asp:TextBox>
                            </div>

                        </div>

                        <div class="form-group">
                            <div class="col-sm-1">
                            </div>
                            <div class="col-sm-1">
                            </div>
                            <div class="col-sm-2">
                            </div>

                            <div class="col-sm-1">
                            </div>
                            <div class="col-sm-1">
                            </div>
                            <div class="col-sm-1">
                            </div>

                            <div class="col-sm-1">
                                <asp:Label ID="lbl32" runat="server"></asp:Label>
                            </div>
                            <div class="col-sm-2">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt107" runat="server" Width="70px" BackColor="#BDEDFF"
                                    Style="text-align: right"
                                    AutoPostBack="true" OnTextChanged="txtpt107_TextChanged" Height="24px"></asp:TextBox>
                            </div>

                            <div class="col-sm-1">
                                <label id="Label36" runat="server">Total</label>

                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt90" runat="server" Width="100%" ReadOnly="true"
                                    Style="text-align: right" OnTextChanged="txtpt90_TextChanged" AutoPostBack="true" Height="24px"></asp:TextBox>
                            </div>

                        </div>

                        <div class="form-group">
                            <div class="col-sm-1">
                            </div>
                            <div class="col-sm-1">
                            </div>
                            <div class="col-sm-2">
                            </div>

                            <div class="col-sm-1">
                            </div>
                            <div class="col-sm-1">
                            </div>
                            <div class="col-sm-1">
                            </div>

                            <div class="col-sm-1">
                                <asp:Label ID="lbl29" runat="server"></asp:Label>
                            </div>
                            <div class="col-sm-2">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt103" runat="server" Width="70px"
                                    Style="text-align: right" OnTextChanged="txtpt83_TextChanged"
                                    AutoPostBack="true" ReadOnly="True" Height="24px"></asp:TextBox>
                            </div>

                            <div class="col-sm-1">
                                <label id="Label37" runat="server">Profit / Kg</label>

                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt91" runat="server" Width="100%" ReadOnly="true"
                                    Style="text-align: right" OnTextChanged="txtpt91_TextChanged" AutoPostBack="true" Height="24px"></asp:TextBox>
                            </div>

                        </div>

                        <div class="form-group">
                            <div class="col-sm-1">
                            </div>
                            <div class="col-sm-1">
                            </div>
                            <div class="col-sm-2">
                            </div>

                            <div class="col-sm-1">
                            </div>
                            <div class="col-sm-1">
                            </div>

                            <div class="col-sm-1">
                                <asp:Label ID="lbl30" runat="server"></asp:Label>
                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt106" runat="server" Width="80px"
                                    Style="text-align: right" OnTextChanged="txtpt106_TextChanged"
                                    AutoPostBack="true" ReadOnly="True" Height="24px"></asp:TextBox>
                            </div>
                            <div class="col-sm-2">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt104" runat="server" Width="80px"
                                    Style="text-align: right" OnTextChanged="txtpt83_TextChanged"
                                    AutoPostBack="true" ReadOnly="True" Height="24px"></asp:TextBox>
                            </div>

                            <div class="col-sm-1">
                                <label id="Label38" runat="server">Total Conv Cost</label>

                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox ID="txtpt92" runat="server" Width="100%" ReadOnly="true"
                                    Style="text-align: right" Height="24px"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="col-sm-1">
                            </div>
                            <div class="col-sm-1">
                            </div>
                            <div class="col-sm-2">
                            </div>

                            <div class="col-sm-1">
                            </div>
                            <div class="col-sm-1">
                            </div>
                            <div class="col-sm-1">
                            </div>
                            <div class="col-sm-1">
                                <asp:Label ID="lbl31" runat="server"></asp:Label>
                            </div>

                            <div class="col-sm-2">
                                <asp:TextBox onkeypress="return isDecimalKey(event)" MaxLength="10"
                                    ID="txtpt105" runat="server" Width="80px"
                                    Style="text-align: right" OnTextChanged="txtpt83_TextChanged"
                                    AutoPostBack="true" ReadOnly="True" Height="24px"></asp:TextBox>
                            </div>

                            <div class="col-sm-1">
                            </div>
                            <div class="col-sm-1">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>

    </div>

    <table style="display: none">
        <tr id="tr2i" runat="server">
            <td id="tl3" runat="server"></td>
        </tr>
        <tr>
            <td id="Td1" runat="server"></td>
        </tr>
        <tr>
            <td id="Td2" runat="server"></td>
        </tr>
        <tr>
            <td id="Td3" runat="server"></td>
        </tr>
        <tr>
            <td id="Td4" runat="server"></td>
        </tr>
        <tr>
            <td id="Td5" runat="server"></td>
        </tr>
        <tr>
            <td id="Td6" runat="server"></td>
        </tr>
        <tr>
            <td id="Td7" runat="server"></td>
        </tr>
        <tr>
            <td id="Td8" runat="server"></td>
        </tr>
    </table>



    <asp:Button ID="btnhideF" runat="server" OnClick="btnhideF_Click" Style="display: none" />
    <asp:Button ID="btnhideF_s" runat="server" OnClick="btnhideF_s_Click" Style="display: none" />
    <asp:HiddenField ID="hffield" runat="server" />
    <asp:HiddenField ID="hfname" runat="server" />
    <asp:HiddenField ID="edmode" runat="server" />
</asp:Content>

