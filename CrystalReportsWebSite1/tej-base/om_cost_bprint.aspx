<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="om_cost_bprint" CodeFile="om_cost_bprint.aspx.cs" %>

<%--  --%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../tej-base/Scripts/gridviewScroll.min.js" type="text/javascript"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>

    <style>
        .Textbox {
            min-width: 500px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper">
        <section class="content-header">
            <table style="width: 100%">
                <tr>
                   
                    <td style="text-align: left">
                        <button type="submit" id="btnnew" class="btn btn-info" style="width: 100px;" runat="server" accesskey="N" onserverclick="btnnew_ServerClick"><u>N</u>ew</button>
                        <button type="submit" id="btnedit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="i" onserverclick="btnedit_ServerClick">Ed<u>i</u>t</button>
                        <button type="submit" id="btnsave" class="btn btn-info" style="width: 100px;" runat="server" accesskey="s" onserverclick="btnsave_ServerClick"><u>S</u>ave</button>
                        <button type="submit" id="btnCal" class="btn btn-info" style="width: 100px;" runat="server" accesskey="y" onserverclick="btnCal_ServerClick">C<u>a</u>lculate</button>
                        <button type="submit" id="btnprint" class="btn btn-info" style="width: 100px;" runat="server" accesskey="P" onserverclick="btnprint_ServerClick"><u>P</u>rint</button>
                        <button type="submit" id="btndel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="l" onserverclick="btndel_ServerClick">De<u>l</u>ete</button>
                        <button type="submit" id="btnlist" class="btn btn-info" style="width: 100px;" runat="server" accesskey="t" onserverclick="btnlist_ServerClick">Lis<u>t</u></button>
                        <button type="submit" id="btncancel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="c" onserverclick="btncancel_ServerClick"><u>C</u>ancel</button>
                        <button type="submit" id="btnexit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="X" onserverclick="btnexit_ServerClick">E<u>x</u>it</button>
                    </td>
                     <td>
                        <asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label></td>
                </tr>
            </table>
        </section>

        <section class="content">
            <div class="row">

                <div class="col-md-6">
                    <div>
                        <%--<div class="box-body">--%>
                        <div class="box-body">
                            <div class="form-group">
                                <div class="col-sm-3">
                                    <asp:Label ID="lbl1" runat="server" Text="Voucher_No." CssClass="col-sm-2 control-label" Style="text-align: center; font-weight: 700"></asp:Label>
                                </div>
                                <div class="col-sm-1">
                                    <asp:Label ID="lbl1a" runat="server" Text="TC" Style="width: 22px; float: right;" CssClass="col-sm-2 control-label"></asp:Label>
                                </div>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtvchnum" runat="server" Width="100px" ReadOnly="true" Height="30px"></asp:TextBox>
                                </div>

                                <%--   <div class="col-sm-4">
                                    <asp:Label ID="Label47" runat="server" Text="Date" CssClass="col-sm-2 control-label" Style="text-align: center; font-weight: 700"></asp:Label>
                                </div>--%>

                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtvchdate" placeholder="Date" runat="server" Width="290px" Height="30px"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtvchdate_CalendarExtender" runat="server"
                                        Enabled="True" TargetControlID="txtvchdate"
                                        Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="Maskedit1" runat="server" Mask="99/99/9999"
                                        MaskType="Date" TargetControlID="txtvchdate" />
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-3">
                                    <asp:Label ID="lbl4" runat="server" Text="Customer_Name" CssClass="col-sm-2 control-label" Style="text-align: center; font-weight: 700"></asp:Label>
                                </div>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnlbl4" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl4_Click" />
                                </div>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtlbl4" runat="server" Width="100px" ReadOnly="true" Height="30px"></asp:TextBox>
                                </div>

                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtlbl4a" runat="server" Width="290px" Height="30px" MaxLength="75"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-3">
                                    <asp:Label ID="Label47" runat="server" Text="Item_Name" CssClass="col-sm-2 control-label" Style="text-align: center; font-weight: 700"></asp:Label>
                                </div>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="ImageButton42" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="ImageButton42_Click" />
                                </div>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtIcode" runat="server" Width="100px" ReadOnly="true" Height="30px"></asp:TextBox>
                                </div>

                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtIname" runat="server" Width="290px" Height="30px" MaxLength="120"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <%--<div class="box-body">--%>
                        <div class="box-body">
                            <div class="form-group">
                                <div class="col-sm-2">
                                    <asp:Label ID="lbl2" runat="server" Text="Ent_By" CssClass="col-sm-2 control-label" Style="text-align: center; font-weight: 700"></asp:Label>
                                </div>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtlbl2" runat="server" Height="30px" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col-sm-2">
                                    <asp:Label ID="lbl3" runat="server" Text="Ent_Dt" CssClass="col-sm-2 control-label" Style="text-align: center; font-weight: 700"></asp:Label>
                                </div>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtlbl3" runat="server" Height="30px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-sm-2">
                                    <asp:Label ID="lbl5" runat="server" Text="Edt_By" CssClass="col-sm-2 control-label" Style="text-align: center; font-weight: 700"></asp:Label>
                                </div>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtlbl5" runat="server" Height="30px" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col-sm-2">
                                    <asp:Label ID="lbl6" runat="server" Text="Edt_Dt" CssClass="col-sm-2 control-label" Style="text-align: center; font-weight: 700"></asp:Label>
                                </div>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtlbl6" runat="server" Height="30px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>

                            <table style="width: 100%">
                                <tr>
                                    <td colspan="4">
                                        <asp:Label ID="lblInfo" runat="server" Text="Save Button Will Be Enable After Clicking on Cal Button" Style="font-weight: 700; color: #FF0000; font-size: 22px;"></asp:Label>
                                    </td>
                                </tr>
                                <tr style="display: none">
                                    <td>
                                        <asp:Label ID="lbl8" runat="server" Text="lbl8" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl8" runat="server"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl9" runat="server" Text="lbl9" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl9" runat="server" Height="30px"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <div class="col-sm-12">
                                    <label id="Label48" runat="server" class="col-md-12 control-label" title="lbl1" style="text-align: center; text-decoration: underline; font-style: italic; font-size: medium;">Paper</label>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-1">
                                        <label id="Label1" runat="server" title="Top"></label>
                                    </div>
                                    <div class="col-sm-1">
                                    </div>
                                    <div class="col-sm-3">
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:Label ID="Label3" runat="server" Text="Length" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:Label ID="Label4" runat="server" Text="Width" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:Label ID="Label5" runat="server" Text="GSM" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:Label ID="Label6" runat="server" Text="Rate" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:Label ID="Label7" runat="server" Text="Amt" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-1">
                                        <asp:Label ID="Label9" runat="server" Text="Top" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" OnClick="ImageButton1_Click" />
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtPTop" runat="server" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtPTL" runat="server" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtPTW" runat="server" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtPTGSM" runat="server" Height="30px" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtPTRate" runat="server" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtPTAmt" runat="server" ReadOnly="true" Height="30px"></asp:TextBox>
                                    </div>

                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-1">
                                        <asp:Label ID="Label8" runat="server" Text="Bottom" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" OnClick="ImageButton2_Click" />
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtPBottom" runat="server" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtPBL" runat="server" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtPBW" runat="server" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtPBGSM" runat="server" Height="30px" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtPBRate" runat="server" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtPBAmt" runat="server" ReadOnly="True" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-1">
                                        <asp:Label ID="Label10" runat="server" Text="Trey" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" OnClick="ImageButton3_Click" />
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtPTrey" runat="server" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtPTrL" runat="server" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtPTrW" runat="server" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtPTrGSM" runat="server" Height="30px" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtPTrRate" runat="server" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtPTrAmt" runat="server" ReadOnly="True" Height="30px"></asp:TextBox>
                                    </div>

                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-1">
                                    </div>
                                    <div class="col-sm-1">
                                    </div>
                                    <div class="col-sm-3">
                                    </div>
                                    <div class="col-sm-1">
                                    </div>
                                    <div class="col-sm-1">
                                    </div>
                                    <div class="col-sm-1">
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:Label ID="Label27" runat="server" Text="Total" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtPTot" runat="server" ReadOnly="True" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <div class="col-sm-12">
                                    <label id="Label11" runat="server" class="col-md-12 control-label" title="lbl1" style="text-align: center; font-style: italic; text-decoration: underline; font-size: medium;">Lamination</label>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-1">
                                        <label id="Label12" runat="server" title="Top"></label>
                                    </div>
                                    <div class="col-sm-1">
                                    </div>
                                    <div class="col-sm-2">
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:Label ID="Label18" runat="server" Text="LamType" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:Label ID="Label14" runat="server" Text="Length" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:Label ID="Label15" runat="server" Text="Width" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:Label ID="Label33" runat="server" Text="MRate" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:Label ID="Label17" runat="server" Text="Rate" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-1">
                                        <asp:Label ID="Label19" runat="server" Text="Top" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:ImageButton ID="ImageButton4" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" OnClick="ImageButton4_Click" />
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtLA_Top" runat="server" Width="190px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtLA_Ts" runat="server" Width="190px" Height="30px" onkeypress="return isDecimalKey(event)" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtLA_TL" runat="server" onkeypress="return isDecimalKey(event)" Height="30px" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtLA_TW" runat="server" onkeypress="return isDecimalKey(event)" Height="30px" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtLA_TMRate" Width="100px" runat="server" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtLA_TRate" runat="server" Width="100px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-1">
                                        <asp:Label ID="Label20" runat="server" Text="Bottom" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:ImageButton ID="ImageButton5" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" OnClick="ImageButton5_Click" />
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtLA_Btm" runat="server" Width="190px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtLA_Bs" runat="server" Width="190px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtLA_BL" runat="server" onkeypress="return isDecimalKey(event)" Height="30px" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtLA_BW" runat="server" onkeypress="return isDecimalKey(event)" Height="30px" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtLA_BMRate" runat="server" Width="100px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtLA_BRate" runat="server" Width="100px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-1">
                                        <asp:Label ID="Label21" runat="server" Text="Trey" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:ImageButton ID="ImageButton6" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" OnClick="ImageButton6_Click" />
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtLA_Trey" runat="server" Width="190px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtLA_TrS" runat="server" Width="190px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtLA_TrL" runat="server" onkeypress="return isDecimalKey(event)" Height="30px" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtLA_TrW" runat="server" onkeypress="return isDecimalKey(event)" Height="30px" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtLA_TrMRate" runat="server" Width="100px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtLA_TRRate" runat="server" Width="100px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-1">
                                        <label id="Label25" runat="server"></label>
                                    </div>
                                    <div class="col-sm-1">
                                    </div>
                                    <div class="col-sm-2">
                                    </div>
                                    <div class="col-sm-3">
                                    </div>
                                    <div class="col-sm-1">
                                    </div>
                                    <div class="col-sm-1">
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:Label ID="Label24" runat="server" Text="Total" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtLATot" runat="server" ReadOnly="True" Width="100px" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <div class="col-sm-12">
                                    <label id="Label2" runat="server" class="col-md-12 control-label" title="lbl1" style="text-align: center; text-decoration: underline; font-style: italic; font-size: medium;">Printing Section</label>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-1">
                                        <asp:Label ID="Label13" runat="server" Text="PrintingType" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-1">
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:Label ID="Label22" runat="server" Text="Color" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:Label ID="Label34" runat="server" Text="Name" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:Label ID="Label35" runat="server" Text="Rate" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-1">
                                        <asp:Label ID="Label62" runat="server" Text="Top" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:ImageButton ID="ImageButton16" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px;" OnClick="ImageButton16_Click" />
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtPR_TCon" runat="server" Width="190px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtPR_TS" runat="server" Width="190px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtPR_TRate" runat="server" Width="149px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-1">
                                        <asp:Label ID="Label16" runat="server" Text="Bottom" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:ImageButton ID="ImageButton7" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px;" OnClick="ImageButton7_Click" />
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtPR_BCon" runat="server" Width="190px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtPR_BS" runat="server" Width="190px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtPR_BRate" runat="server" Width="149px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-1">
                                        <asp:Label ID="Label23" runat="server" Text="Trey" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:ImageButton ID="ImageButton35" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px;" OnClick="ImageButton35_Click" />
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtPR_TrCon" runat="server" Width="190px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtPR_TrS" runat="server" Width="190px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtPR_TrRate" runat="server" Width="149px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-1">
                                    </div>
                                    <div class="col-sm-1">
                                    </div>
                                    <div class="col-sm-4">
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:Label ID="Label36" runat="server" Text="Total" Style="float: right; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtPRTot" runat="server" Width="149px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <div class="col-sm-12">
                                    <label id="Label26" runat="server" class="col-md-12 control-label" title="lbl1" style="text-align: center; text-decoration: underline; font-style: italic; font-size: medium;">Printing U.V.</label>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-1">
                                        <asp:Label ID="Label28" runat="server" Text="Printing" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-1">
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:Label ID="Label29" runat="server" Text="Color" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:Label ID="Label30" runat="server" Text="Name" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:Label ID="Label103" runat="server" Text="Rate" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-1">
                                        <asp:Label ID="Label104" runat="server" Text="Top" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:ImageButton ID="ImageButton36" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px;" OnClick="ImageButton36_Click" />
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtPU_TCon" runat="server" Width="190px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtPU_TS" runat="server" Width="190px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtPU_TRate" runat="server" Width="149px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-1">
                                        <asp:Label ID="Label105" runat="server" Text="Bottom" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:ImageButton ID="ImageButton37" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px;" OnClick="ImageButton37_Click" />
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtPU_BCon" runat="server" Width="190px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtPU_BS" runat="server" Width="190px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtPU_BRate" runat="server" Width="149px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-1">
                                        <asp:Label ID="Label106" runat="server" Text="Trey" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:ImageButton ID="ImageButton38" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px;" OnClick="ImageButton38_Click" />
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtPU_TrCon" runat="server" Width="190px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtPU_TrS" runat="server" Width="190px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtPU_TrRate" runat="server" Width="149px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-1">
                                    </div>
                                    <div class="col-sm-1">
                                    </div>
                                    <div class="col-sm-4">
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:Label ID="Label37" runat="server" Text="Total" Style="float: right; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtPUTot" runat="server" Width="149px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>

                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <div class="col-sm-12">
                                    <label id="Label107" runat="server" class="col-md-12 control-label" title="lbl1" style="text-align: center; text-decoration: underline; font-style: italic; font-size: medium;">Screen Printing And Micro</label>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-1">
                                        <asp:Label ID="Label108" runat="server" Text="Printing" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-1">
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:Label ID="Label109" runat="server" Text="ScreenPrinting" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:Label ID="Label111" runat="server" Text="Rate" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>


                                    <div class="col-sm-1">
                                    </div>
                                    <div class="col-sm-1">
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:Label ID="Label40" runat="server" Text="Micro" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:Label ID="Label41" runat="server" Text="Rate" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>

                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-1">
                                        <asp:Label ID="Label112" runat="server" Text="Top" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:ImageButton ID="ImageButton39" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px;" OnClick="ImageButton39_Click" />
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtS_TP" runat="server" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtS_TRate" runat="server" Width="140px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>

                                    <div class="col-sm-1">
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:ImageButton ID="ImageButton10" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px;" OnClick="ImageButton10_Click" />
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtM_Top" runat="server" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtM_TRate" runat="server" Width="100px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-1">
                                        <asp:Label ID="Label113" runat="server" Text="Bottom" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:ImageButton ID="ImageButton40" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px;" OnClick="ImageButton40_Click" />
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtS_BP" runat="server" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtS_BRate" runat="server" Width="140px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>

                                    <div class="col-sm-1">
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:ImageButton ID="ImageButton19" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px;" OnClick="ImageButton19_Click" />
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtM_Btm" runat="server" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtM_BRate" runat="server" Width="100px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-1">
                                        <asp:Label ID="Label114" runat="server" Text="Trey" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:ImageButton ID="ImageButton41" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px;" OnClick="ImageButton41_Click" />
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtS_TrP" runat="server" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtS_TrRate" runat="server" Width="140px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>

                                    <div class="col-sm-1">
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:ImageButton ID="ImageButton20" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px;" OnClick="ImageButton20_Click" />
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtM_Trey" runat="server" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtM_TrRate" runat="server" Width="100px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-1">
                                    </div>
                                    <div class="col-sm-1">
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:Label ID="Label46" runat="server" Text="Total" Style="float: right; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtSTot" runat="server" Width="140px" ReadOnly="True" Height="30px"></asp:TextBox>
                                    </div>

                                    <div class="col-sm-1">
                                    </div>
                                    <div class="col-sm-1">
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:Label ID="Label39" runat="server" Text="Total" Style="float: right; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtMTot" runat="server" Width="100px" ReadOnly="True" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <%-- funs--%>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <div class="col-sm-12">
                                    <label id="Label42" runat="server" class="col-md-12 control-label" title="lbl1" style="text-align: center; text-decoration: underline; font-style: italic; font-size: medium;">Drip Off And Gloss Varnish</label>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-1">
                                    </div>
                                    <div class="col-sm-1">
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:Label ID="Label69" runat="server" Text="Name" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:Label ID="Label43" runat="server" Text="Dripp" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:Label ID="Label60" runat="server" Text="DripM" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:Label ID="Label44" runat="server" Text="Amt" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-1">
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:Label ID="Label70" runat="server" Text="GName" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:Label ID="Label61" runat="server" Text="GlossM" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:Label ID="Label31" runat="server" Text="GlossV" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-1">
                                        <asp:Label ID="Label63" runat="server" Text="Top" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:ImageButton ID="ImageButton8" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" OnClick="ImageButton8_Click" />
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtD_TDName" runat="server" Width="100px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtD_TDriff" runat="server" Width="50px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtD_TDM" runat="server" Width="50px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtD_TAmt" runat="server" Width="48px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:ImageButton ID="ImageButton23" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" OnClick="ImageButton23_Click" />
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtD_TGName" runat="server" Width="100px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtD_TGRate" runat="server" Width="50px" ReadOnly="True" Height="30px"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtD_TG" runat="server" Width="50px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-1">
                                        <asp:Label ID="Label32" runat="server" Text="Bottom" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:ImageButton ID="ImageButton11" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" OnClick="ImageButton11_Click" />
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtD_BDName" runat="server" Width="100px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtD_BDriff" runat="server" Width="50px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtD_BDM" runat="server" Width="50px" ReadOnly="True" Height="30px"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtD_BAmt" runat="server" Width="48px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:ImageButton ID="ImageButton24" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" OnClick="ImageButton24_Click" />
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtD_BGName" runat="server" Width="100px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtD_BGRate" runat="server" Width="50px" ReadOnly="True" Height="30px"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtD_BG" runat="server" Width="50px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-1">
                                        <asp:Label ID="Label38" runat="server" Text="Trey" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:ImageButton ID="ImageButton12" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" OnClick="ImageButton12_Click" />
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtD_TrDName" runat="server" Width="100px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtD_TrDriff" runat="server" Width="50px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtD_TrDM" runat="server" Width="50px" ReadOnly="True" Height="30px"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtD_TrAmt" runat="server" Width="48px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:ImageButton ID="ImageButton25" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" OnClick="ImageButton25_Click" />
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtD_TrGName" runat="server" Width="100px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtD_TrGRate" runat="server" Width="50px" ReadOnly="True" Height="30px"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtD_TrG" runat="server" Width="50px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-1">
                                    </div>
                                    <div class="col-sm-1">
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:Label ID="Label45" runat="server" Text="Total" Style="float: right; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-1">
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtDTot" runat="server" ReadOnly="True" Width="96px" Height="30px"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:Label ID="Label64" runat="server" Text="Total" Style="float: right; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtGTot" runat="server" ReadOnly="True" Width="98px" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-4">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <div class="col-sm-12">
                                    <label id="Label65" runat="server" class="col-md-12 control-label" title="lbl1" style="text-align: center; font-size: medium; text-decoration: underline; font-style: italic;">Foiling Punching Top</label>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-2">
                                    </div>
                                    <div class="col-sm-2">
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:Label ID="Label66" runat="server" Text="Top" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:Label ID="Label67" runat="server" Text="Rate" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-2">
                                        <asp:Label ID="Label68" runat="server" Text="SpotU.V." Style="text-align: center; font-weight: 700;"></asp:Label>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:ImageButton ID="ImageButton9" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" OnClick="ImageButton9_Click" />
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtSpot_T" runat="server" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtSpot_TRate" runat="server" Width="125px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-2">
                                        <asp:Label ID="Label80" runat="server" Text="Foil" Style="text-align: center; font-weight: 700;"></asp:Label>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:ImageButton ID="ImageButton17" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" OnClick="ImageButton17_Click" />
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtFoil_T" runat="server" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtFoil_TRate" runat="server" Width="125px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-2">
                                        <asp:Label ID="Label81" runat="server" Text="Punching" Style="text-align: center; font-weight: 700;"></asp:Label>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:ImageButton ID="ImageButton18" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" OnClick="ImageButton18_Click" />
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtPunc_T" runat="server" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtPunc_TRate" runat="server" Width="125px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-2">
                                        <asp:Label ID="Label82" runat="server" Text="Emboss" Style="text-align: center; font-weight: 700;"></asp:Label>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:ImageButton ID="ImageButton26" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" OnClick="ImageButton26_Click" />
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtEmb_T" runat="server" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtEmb_TRate" runat="server" ReadOnly="True" Width="125px" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-4">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <div class="col-sm-12">
                                    <label id="Label83" runat="server" class="col-md-12 control-label" title="lbl1" style="text-align: center; font-size: medium; text-decoration: underline; font-style: italic;">Foiling Punching Bottom</label>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-2">
                                    </div>
                                    <div class="col-sm-2">
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:Label ID="Label84" runat="server" Text="Bottom" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:Label ID="Label85" runat="server" Text="Rate" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-2">
                                        <asp:Label ID="Label86" runat="server" Text="SpotU.V." Style="text-align: center; font-weight: 700;"></asp:Label>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:ImageButton ID="ImageButton27" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" OnClick="ImageButton27_Click" />
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtSpot_B" runat="server" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtSpot_BRate" runat="server" Width="125px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-2">
                                        <asp:Label ID="Label87" runat="server" Text="Foil" Style="text-align: center; font-weight: 700;"></asp:Label>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:ImageButton ID="ImageButton28" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" OnClick="ImageButton28_Click" />
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtFoil_B" runat="server" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtFoil_BRate" runat="server" Width="125px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-2">
                                        <asp:Label ID="Label88" runat="server" Text="Punching" Style="text-align: center; font-weight: 700;"></asp:Label>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:ImageButton ID="ImageButton29" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" OnClick="ImageButton29_Click" />
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtPunc_B" runat="server" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtPunc_BRate" runat="server" Width="125px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-2">
                                        <asp:Label ID="Label89" runat="server" Text="Emboss" Style="text-align: center; font-weight: 700;"></asp:Label>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:ImageButton ID="ImageButton30" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" OnClick="ImageButton30_Click" />
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtEmb_B" runat="server" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtEmb_BRate" runat="server" ReadOnly="True" Width="125px" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-4">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <div class="col-sm-12">
                                    <label id="Label90" runat="server" class="col-md-12 control-label" title="lbl1" style="text-align: center; font-size: medium; text-decoration: underline; font-style: italic;">Foiling Punching Trey</label>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-2">
                                    </div>
                                    <div class="col-sm-2">
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:Label ID="Label91" runat="server" Text="Trey" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:Label ID="Label92" runat="server" Text="Rate" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:Label ID="Label93" runat="server" Text="Total" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-2">
                                        <asp:Label ID="Label94" runat="server" Text="SpotU.V." Style="text-align: center; font-weight: 700;"></asp:Label>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:ImageButton ID="ImageButton31" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" OnClick="ImageButton31_Click" />
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtSpot_Tr" runat="server" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtSpot_TrRate" runat="server" Width="125px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtSpot_Tot" runat="server" ReadOnly="True" Width="65px" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-2">
                                        <asp:Label ID="Label95" runat="server" Text="Foil" Style="text-align: center; font-weight: 700;"></asp:Label>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:ImageButton ID="ImageButton32" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" OnClick="ImageButton32_Click" />
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtFoil_Tr" runat="server" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtFoil_TrRate" runat="server" Width="125px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtFoil_Tot" runat="server" ReadOnly="True" Width="65px" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-2">
                                        <asp:Label ID="Label96" runat="server" Text="Punching" Style="text-align: center; font-weight: 700;"></asp:Label>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:ImageButton ID="ImageButton33" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" OnClick="ImageButton33_Click" />
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtPunc_Tr" runat="server" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtPunc_TrRate" runat="server" Width="125px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtPunc_Tot" runat="server" ReadOnly="True" Width="65px" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-2">
                                        <asp:Label ID="Label97" runat="server" Text="Emboss" Style="text-align: center; font-weight: 700;"></asp:Label>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:ImageButton ID="ImageButton34" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" OnClick="ImageButton34_Click" />
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtEmb_Tr" runat="server" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtEmb_TrRate" runat="server" ReadOnly="True" Width="125px" Height="30px"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtEmb_Tot" runat="server" ReadOnly="True" Width="65px" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <div class="col-sm-12">
                                    <label id="Label49" runat="server" class="col-md-12 control-label" title="lbl1" style="text-align: center; text-decoration: underline; font-style: italic; font-size: medium;">Grand Total</label>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-1">
                                        <asp:Label ID="Label50" runat="server" Text="GrossAmt" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-1">
                                    </div>
                                    <div class="col-sm-4">
                                    </div>
                                    <div class="col-sm-2">
                                    </div>
                                    <div class="col-sm-1">
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtGrossAmt" runat="server" ReadOnly="True" Width="143px" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-1">
                                        <asp:Label ID="Label51" runat="server" Text="Wastage" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:ImageButton ID="ImageButton13" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" OnClick="ImageButton13_Click" />
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtW1" runat="server" ReadOnly="True" Width="190px" Height="30px"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtW2" runat="server" ReadOnly="True" Width="100px" Height="30px"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtWPer" runat="server" ReadOnly="True" Width="50px" Height="30px" Style="text-align: center"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtW3" runat="server" ReadOnly="True" Width="143px" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-1">
                                        <asp:Label ID="Label52" runat="server" Text="OtherValue" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:ImageButton ID="ImageButton14" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" OnClick="ImageButton14_Click" Visible="False" />
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtO1" runat="server" Width="190px" Height="30px" MaxLength="70"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtO2" runat="server" Width="100px" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtOPer" runat="server" ReadOnly="True" Width="50px" Height="30px" Style="text-align: center"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtO3" runat="server" ReadOnly="True" Width="143px" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-1">
                                        <asp:Label ID="Label59" runat="server" Text="DCharges" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:ImageButton ID="ImageButton21" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" OnClick="ImageButton21_Click" />
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtD1" runat="server" ReadOnly="True" Width="190px" Height="30px"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtD2" runat="server" ReadOnly="True" Width="100px" Height="30px"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtDPer" runat="server" ReadOnly="True" Width="50px" Height="30px" Style="text-align: center"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtD3" runat="server" ReadOnly="True" Width="143px" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-1">
                                    </div>
                                    <div class="col-sm-1">
                                    </div>
                                    <div class="col-sm-4">
                                    </div>
                                    <div class="col-sm-2">
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:Label ID="Label53" runat="server" Text="Total" Style="float: right; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtTot" runat="server" ReadOnly="True" Width="143px" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <div class="col-sm-12">
                                    <label id="Label54" runat="server" class="col-md-12 control-label" title="lbl1" style="text-align: center; text-decoration: underline; font-style: italic; font-size: medium;">Grand Total</label>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-1">
                                        <asp:Label ID="Label55" runat="server" Text="PTerms" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:ImageButton ID="ImageButton22" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" OnClick="ImageButton22_Click" />
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtPay1" runat="server" Width="190px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtPay2" runat="server" Width="100px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtPay3" runat="server" Width="50px" Height="30px" ReadOnly="True" Style="text-align: center"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtPay4" runat="server" Width="143px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-1">
                                        <asp:Label ID="Label56" runat="server" Text="Total" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-1">
                                    </div>
                                    <div class="col-sm-4">
                                    </div>
                                    <div class="col-sm-2">
                                    </div>
                                    <div class="col-sm-1">
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtPayTot" runat="server" ReadOnly="True" Width="143px" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-1">
                                        <asp:Label ID="Label57" runat="server" Text="Tax" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:ImageButton ID="ImageButton15" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" OnClick="ImageButton15_Click" Visible="False" />
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtTax1" runat="server" Width="190px" Height="30px" MaxLength="50"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtTax2" runat="server" Width="100px" Height="30px" onkeypress="return isDecimalKey(event)" MaxLength="15"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox ID="txtTax3" runat="server" Width="50px" Height="30px" ReadOnly="True" Style="text-align: center"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtTax4" runat="server" Width="143px" Height="30px" ReadOnly="True"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <div class="col-sm-1">
                                        <asp:Label ID="Label58" runat="server" Text="GrandTotal" Style="text-align: center; font-weight: 700"></asp:Label>
                                    </div>
                                    <div class="col-sm-1">
                                    </div>
                                    <div class="col-sm-4">
                                    </div>
                                    <div class="col-sm-2">
                                    </div>
                                    <div class="col-sm-1">
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtGrandTot" runat="server" ReadOnly="True" Width="143px" Height="30px"></asp:TextBox>
                                    </div>
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
