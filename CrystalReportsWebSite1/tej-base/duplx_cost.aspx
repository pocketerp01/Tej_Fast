<%@ Page Title="" Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="duplx_cost" CodeFile="duplx_cost.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <style type="text/css">
        .style1 {
            width: 200px;
        }

        .style2 {
        }

        .style3 {
            width: 125px;
        }

        .style4 {
            width: 148px;
        }

        .style5 {
        }

        .style7 {
            width: 151px;
        }

        .style8 {
            width: 182px;
        }

        .style11 {
            width: 181px;
        }

        .style12 {
            width: 230px;
        }

        .style13 {
            width: 321px;
        }

        .style14 {
            width: 200px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
        <table style="width: 100%; margin:5px">
            <tr>
             
                <td style="text-align: left">
                    <button type="submit" id="btnnew" class="btn btn-info" style="width: 100px;" runat="server" accesskey="N" onserverclick="btnnew_ServerClick"><u>N</u>ew</button>
                    <button type="submit" id="btnedit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="i" onserverclick="btnedit_ServerClick">Ed<u>i</u>t</button>
                    <button type="submit" id="btnsave" class="btn btn-info" style="width: 100px;" runat="server" accesskey="s" onserverclick="btnsave_ServerClick"><u>S</u>ave</button>
                    <button type="submit" id="btnprint" class="btn btn-info" style="width: 100px;" runat="server" accesskey="P" onserverclick="btnprint_ServerClick"><u>P</u>rint</button>
                    <button type="submit" id="btndel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="l" onserverclick="btndel_ServerClick">De<u>l</u>ete</button>
                    <button type="submit" id="btnlist" class="btn btn-info" style="width: 100px;" runat="server" accesskey="t" onserverclick="btnlist_ServerClick">Lis<u>t</u></button>
                    <button type="submit" id="btncan" class="btn btn-info" style="width: 100px;" runat="server" accesskey="c" onserverclick="btncan_ServerClick"><u>C</u>ancel</button>
                    <button type="submit" id="btnext" class="btn btn-info" style="width: 100px;" runat="server" accesskey="X" onserverclick="btnext_ServerClick">E<u>x</u>it</button>
                </td>
                   <td>
                    <asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label></td>
            </tr>
        </table>
    </div>
    <div class="toolsContentLeft">
        <div class="bSubBlock brandSecondaryBrd secondaryPalette" >
            <div class="toolsContentLeft">
                <div class="bSubBlock brandSecondaryBrd secondaryPalette">
                    <div class="lbBody" style="color: black; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                        <table>
                            <tr style="background-color: #CDE8F0">
                                <td class="style1">Party Name
                <asp:ImageButton ID="btnacode" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;"
                    OnClick="btnacode_Click" />
                                </td>
                                <td class="style2" colspan="3">
                                    <asp:TextBox ID="txtacode" runat="server" Width="60px" Placeholder="Code" ReadOnly="true"></asp:TextBox>
                                    <asp:TextBox ID="txtaname" runat="server" Width="250px" Placeholder="Party Name"
                                        MaxLength="30" AutoPostBack="true"
                                        OnTextChanged="txtaname_TextChanged"></asp:TextBox>
                                </td>
                                <td class="style2">Destination</td>
                                <td>
                                    <asp:TextBox ID="tk66" runat="server" Width="100px" BackColor="#BDEDFF"
                                        MaxLength="50"></asp:TextBox>
                                </td>
                                <td>Product Type</td>
                                <td class="style13">
                                    <asp:TextBox ID="tk69" runat="server" Width="70px" BackColor="#BDEDFF"
                                        MaxLength="30"></asp:TextBox>
                                </td>
                                <td class="style14">Sheet No.</td>
                                <td>
                                    <asp:TextBox ID="txtvchnum" runat="server" Width="70px" Placeholder="Sheet No" ReadOnly="true" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="style1">Item Name
                <asp:ImageButton ID="btnicode" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;"
                    OnClick="btnicode_Click" />
                                </td>
                                <td class="style2" colspan="3">
                                    <asp:TextBox ID="txticode" runat="server" Width="60px" Placeholder="Code" ReadOnly="true"></asp:TextBox>
                                    <asp:TextBox ID="txtiname" runat="server" Width="250px" Placeholder="Item Name" MaxLength="30"></asp:TextBox>
                                </td>
                                <td class="style2">Item dimension</td>
                                <td class="style2">
                                    <asp:TextBox ID="tk67" runat="server" Width="100px" BackColor="#BDEDFF"
                                        MaxLength="30"></asp:TextBox>
                                </td>
                                <td>Printing Type</td>
                                <td class="style13">
                                    <asp:TextBox ID="tk68" runat="server" Width="70px" BackColor="#BDEDFF"
                                        MaxLength="30"></asp:TextBox>
                                </td>
                                <td class="style14">Date
                                </td>
                                <td>

                                    <asp:TextBox ID="txtvchdate" runat="server" Width="70px" placeholder="Date" ReadOnly="true"> </asp:TextBox>

                                </td>
                            </tr>
                            <tr style="background-color: #CDE8F0">
                                <td class="style1">Print Sheet Size (L x W)</td>
                                <td class="style5">
                                    <asp:TextBox ID="tk1" runat="server" Width="40px" BackColor="#BDEDFF"
                                        onkeypress="return isDecimalKey(event)" MaxLength="10"
                                        onkeyup="calculateSum();"></asp:TextBox>
                                    <asp:TextBox ID="tk2" runat="server" Width="40px" BackColor="#BDEDFF"
                                        onkeypress="return isDecimalKey(event)" MaxLength="10"
                                        onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                                <td class="style3">No. of Colours</td>
                                <td class="style12">
                                    <asp:TextBox ID="tk3" runat="server" Width="70px" BackColor="#BDEDFF" onkeypress="return isDecimalKey(event)" MaxLength="10" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                                <td class="style11">No. of Ups</td>
                                <td class="style4">
                                    <asp:TextBox ID="tk4" runat="server" Width="70px" BackColor="#BDEDFF" onkeypress="return isDecimalKey(event)" MaxLength="10" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                                <td class="style4">Paper GSM</td>
                                <td class="style13">
                                    <asp:TextBox ID="tk5" runat="server" Width="70px" BackColor="#BDEDFF" onkeypress="return isDecimalKey(event)" MaxLength="10" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                                <td class="style14">Paper Type</td>
                                <td>
                                    <asp:TextBox ID="tk70" runat="server" Width="70px" BackColor="#BDEDFF"
                                        MaxLength="10"
                                        onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="style1">Quantity</td>
                                <td class="style5">
                                    <asp:TextBox ID="tk6" runat="server" Width="70px" BackColor="#BDEDFF" onkeypress="return isDecimalKey(event)" MaxLength="10" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                                <td class="style3">No.of.Impression</td>
                                <td class="style12">
                                    <asp:TextBox ID="tk11" runat="server" Width="70px" ReadOnly="true" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                                <td class="style11">Wstg of Board(%)</td>
                                <td class="style4">
                                    <asp:TextBox ID="tk17" runat="server" Width="70px" BackColor="#BDEDFF" onkeypress="return isDecimalKey(event)" MaxLength="10" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                                <td class="style4">&nbsp;</td>
                                <td class="style13">&nbsp;</td>
                                <td class="style14">&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr style="background-color: #CDE8F0;">
                                <td class="style1" style="border-top-style: groove; border-top-width: thick;">Board Cost <span style="font-size: xx-small">/Kg</span></td>
                                <td style="border-top-style: groove; border-top-width: thick;" class="style5">
                                    <asp:TextBox ID="tk8" runat="server" Width="70px" BackColor="#BDEDFF" onkeypress="return isDecimalKey(event)" MaxLength="10" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                                <td class="style3" style="border-top-style: groove; border-top-width: thick;">Pasting <span style="font-size: xx-small">/1000 Ctrn</span> </td>
                                <td class="style12" style="border-top-style: groove; border-top-width: thick;">
                                    <asp:TextBox ID="tk7" runat="server" Width="70px" BackColor="#BDEDFF" onkeypress="return isDecimalKey(event)" MaxLength="10" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                                <td class="style11"
                                    style="border-top-style: groove; border-top-width: thick;">Punching<span style="font-size: xx-small"> /1000Sht</span></td>
                                <td class="style4" style="border-top-style: groove; border-top-width: thick;">
                                    <asp:TextBox ID="tk14" runat="server" Width="70px" BackColor="#BDEDFF" onkeypress="return isDecimalKey(event)" MaxLength="10" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                                <td class="style4" style="border-top-style: groove; border-top-width: thick;">Embossing<span style="font-size: xx-small"> /1000Sht</span></td>
                                <td class="style13" style="border-top-style: groove; border-top-width: thick;">
                                    <asp:TextBox ID="tk15" runat="server" Width="70px" BackColor="#BDEDFF" onkeypress="return isDecimalKey(event)" MaxLength="10" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                                <td style="border-top-style: groove; border-top-width: thick;" class="style14">Packaging<span style="font-size: xx-small"> /1000Crtn</span></td>
                                <td style="border-top-style: groove; border-top-width: thick;">
                                    <asp:TextBox ID="tk16" runat="server" Width="70px" BackColor="#BDEDFF" onkeypress="return isDecimalKey(event)" MaxLength="10" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="style1" style="border-bottom-style: groove; border-bottom-width: thick;">Plate Chrg<span style="font-size: xx-small"> /Plate/Clr</span> </td>
                                <td class="style5" style="border-bottom-style: groove; border-bottom-width: thick;">
                                    <asp:TextBox ID="tk9" runat="server" Width="70px" BackColor="#BDEDFF" onkeypress="return isDecimalKey(event)" MaxLength="10" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                                <td class="style3" style="border-bottom-style: groove; border-bottom-width: thick;">Positive<span style="font-size: xx-small">/Sq./inch</span></td>
                                <td class="style12" style="border-bottom-style: groove; border-bottom-width: thick;">
                                    <asp:TextBox ID="tk12" runat="server" Width="70px" BackColor="#BDEDFF" onkeypress="return isDecimalKey(event)" MaxLength="10" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                                <td class="style11" style="border-bottom-style: groove; border-bottom-width: thick;">Foil Stamping<span style="font-size: xx-small"> /1000Sht</span></td>
                                <td class="style4" style="border-bottom-style: groove; border-bottom-width: thick;">
                                    <asp:TextBox ID="tk18" runat="server" Width="70px" BackColor="#BDEDFF" onkeypress="return isDecimalKey(event)" MaxLength="10" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                                <td style="border-bottom-style: groove; border-bottom-width: thick;">Aqua/UV/Lami<span style="font-size: xx-small"> /Sq./inch</span> </td>
                                <td class="style13" style="border-bottom-style: groove; border-bottom-width: thick;">

                                    <asp:TextBox ID="tk13" runat="server" Width="70px" BackColor="#BDEDFF" onkeypress="return isDecimalKey(event)" MaxLength="10" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                                <td class="style14" style="border-bottom-style: groove; border-bottom-width: thick;">Printing Chrg<span style="font-size: xx-small"> /1000Sht/Clr</span></td>
                                <td style="border-bottom-style: groove; border-bottom-width: thick;">
                                    <asp:TextBox ID="tk10" runat="server" Width="70px" BackColor="#BDEDFF" onkeypress="return isDecimalKey(event)" MaxLength="10" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="style1">
                                    <u>Calculation</u>
                                </td>
                                <td class="style5">&nbsp;</td>
                                <td class="style3">&nbsp;</td>
                                <td class="style12">&nbsp;</td>
                                <td class="style11">&nbsp;</td>
                                <td class="style4">&nbsp;</td>
                                <td class="style4">&nbsp;</td>
                                <td class="style13">&nbsp;</td>
                                <td class="style14">&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr style="background-color: #CDE8F0">
                                <td class="style1" style="border-top-style: groove; border-left-style: groove; border-width: thin">Paper</td>
                                <td style="border-top-style: groove; border-width: thin" class="style5" colspan="2">Sheet size X GSM</td>
                                <td class="style12" style="border-top-style: groove; border-width: thin">
                                    <asp:TextBox ID="tk19" runat="server" Width="70px" ReadOnly="true" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                                <td class="style11"
                                    style="border-top-style: groove; border-width: thin">
                                    <asp:TextBox ID="tk20" runat="server" Width="70px" ReadOnly="true" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                                <td class="style4" style="width: 150px; border-top-style: groove; border-width: thin">
                                    <asp:TextBox ID="tk21" runat="server" Width="70px" ReadOnly="true" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                                <td class="style4" style="width: 150px; border-top-style: groove; border-width: thin">
                                    <asp:TextBox ID="tk22" runat="server" Width="70px" ReadOnly="true" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                                <td class="style13" style="border-top-style: groove; border-width: thin">
                                    <asp:TextBox ID="tk23" runat="server" Width="70px" ReadOnly="true" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                                <td style="border-top-style: groove; border-width: thin" class="style14">
                                    <asp:TextBox ID="tk24" runat="server" Width="70px" ReadOnly="true" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                                <td style="width: 150px; border-top-style: groove; border-right-style: groove; border-width: thin">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="style1" style="border-left-style: groove; border-width: thin">Board Cost <span style="font-size: xx-small">/Kg</span></td>
                                <td class="style5">
                                    <asp:TextBox ID="tk25" runat="server" Width="70px" ReadOnly="true" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                                <td class="style3">
                                    <asp:TextBox ID="tk26" runat="server" Width="70px" ReadOnly="true" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                                <td class="style12">
                                    <asp:TextBox ID="tk27" runat="server" Width="70px" ReadOnly="true" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                                <td class="style11">
                                    <asp:TextBox ID="tk28" runat="server" Width="70px" ReadOnly="true" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                                <td class="style4">Plate Chrg<span style="font-size: xx-small"> /Plate/Clr</span> </td>
                                <td class="style4">
                                    <asp:TextBox ID="tk29" runat="server" Width="70px" ReadOnly="true" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                                <td class="style13">
                                    <asp:TextBox ID="tk30" runat="server" Width="70px" ReadOnly="true" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                                <td class="style14">&nbsp;</td>
                                <td style="border-right-style: groove; border-width: thin;">
                                    <asp:TextBox ID="tk31" runat="server" Width="70px" ReadOnly="true" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="background-color: #CDE8F0">
                                <td class="style1" style="border-left-style: groove; border-width: thin">Printing Chrg<span style="font-size: xx-small"> /1000Sht/Clr</span></td>
                                <td class="style5">
                                    <asp:TextBox ID="tk32" runat="server" Width="70px" ReadOnly="true" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                                <td class="style3">
                                    <asp:TextBox ID="tk33" runat="server" Width="70px" ReadOnly="true" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                                <td class="style12">&nbsp;</td>
                                <td class="style11">
                                    <asp:TextBox ID="tk34" runat="server" Width="70px" ReadOnly="true" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                                <td class="style4">Positive<span style="font-size: xx-small">/Sq./inch</span></td>
                                <td class="style4">
                                    <asp:TextBox ID="tk35" runat="server" Width="70px" ReadOnly="true" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                                <td class="style13">
                                    <asp:TextBox ID="tk36" runat="server" Width="70px" ReadOnly="true" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                                <td class="style14">
                                    <asp:TextBox ID="tk37" runat="server" Width="70px" ReadOnly="true" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                                <td style="border-right-style: groove; border-width: thin;">
                                    <asp:TextBox ID="tk38" runat="server" Width="70px" ReadOnly="true" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="style1" style="border-left-style: groove; border-width: thin">Aqua/UV/Lami<span style="font-size: xx-small"> /Sq./inch</span></td>
                                <td class="style5">
                                    <asp:TextBox ID="tk39" runat="server" Width="70px" ReadOnly="true" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                                <td class="style3">
                                    <asp:TextBox ID="tk40" runat="server" Width="70px" ReadOnly="true" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                                <td class="style12">
                                    <asp:TextBox ID="tk41" runat="server" Width="70px" ReadOnly="true" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                                <td class="style11">
                                    <asp:TextBox ID="tk42" runat="server" Width="70px" ReadOnly="true" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                                <td class="style4">Embossing<span style="font-size: xx-small"> /1000Sht</span></td>
                                <td class="style4">
                                    <asp:TextBox ID="tk43" runat="server" Width="70px" ReadOnly="true" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                                <td class="style13">
                                    <asp:TextBox ID="tk44" runat="server" Width="70px" ReadOnly="true" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                                <td class="style14">&nbsp;</td>
                                <td style="border-right-style: groove; border-width: thin;">
                                    <asp:TextBox ID="tk45" runat="server" Width="70px" ReadOnly="true" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="background-color: #CDE8F0">
                                <td class="style1" style="border-left-style: groove; border-width: thin">Punching<span style="font-size: xx-small"> /1000Sht</span></td>
                                <td class="style5">
                                    <asp:TextBox ID="tk46" runat="server" Width="70px" ReadOnly="true" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                                <td class="style3">
                                    <asp:TextBox ID="tk47" runat="server" Width="70px" ReadOnly="true" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                                <td class="style12">&nbsp;</td>
                                <td class="style11">
                                    <asp:TextBox ID="tk48" runat="server" Width="70px" ReadOnly="true" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                                <td class="style4">Foil Stamping<span style="font-size: xx-small"> /1000Sht</span></td>
                                <td class="style4">
                                    <asp:TextBox ID="tk49" runat="server" Width="70px" ReadOnly="true" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                                <td class="style13">
                                    <asp:TextBox ID="tk50" runat="server" Width="70px" ReadOnly="true" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                                <td class="style14">&nbsp;</td>
                                <td style="border-right-style: groove; border-width: thin;">
                                    <asp:TextBox ID="tk51" runat="server" Width="70px" ReadOnly="true" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="style1" style="border-bottom-style: groove; border-left-style: groove; border-width: thin">Pasting <span style="font-size: xx-small">/1000 Ctrn</span> </td>
                                <td style="border-bottom-style: groove; border-width: thin" class="style5">
                                    <asp:TextBox ID="tk52" runat="server" Width="70px" ReadOnly="true" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                                <td class="style3" style="border-bottom-style: groove; border-width: thin">
                                    <asp:TextBox ID="tk53" runat="server" Width="70px" ReadOnly="true" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                                <td class="style12" style="border-bottom-style: groove; border-width: thin">&nbsp;</td>
                                <td class="style11" style="border-bottom-style: groove; border-width: thin">
                                    <asp:TextBox ID="tk54" runat="server" Width="70px" ReadOnly="true" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                                <td class="style4" style="border-bottom-style: groove; border-width: thin">Packaging<span style="font-size: xx-small"> /1000Ctrn</span></td>
                                <td class="style4" style="border-bottom-style: groove; border-width: thin">
                                    <asp:TextBox ID="tk55" runat="server" Width="70px" ReadOnly="true" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                                <td class="style13" style="border-bottom-style: groove; border-width: thin">
                                    <asp:TextBox ID="tk56" runat="server" Width="70px" ReadOnly="true" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                                <td style="border-bottom-style: groove; border-width: thin" class="style14">&nbsp;</td>
                                <td style="border-bottom-style: groove; border-right-style: groove; border-width: thin">
                                    <asp:TextBox ID="tk57" runat="server" Width="70px" ReadOnly="true" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="style1" style="border-style: groove; border-width: thin;">Remarks</td>
                                <td class="style5" colspan="7" rowspan="3" valign="top" style="border-style: groove; border-width: thin;">
                                    <asp:TextBox ID="txtrmk" runat="server" TextMode="MultiLine" Width="99%"
                                        Height="82px"></asp:TextBox>

                                </td>
                                <td style="background-color: #CDE8F0; border-left-style: groove; border-width: thin"
                                    class="style14">Total</td>
                                <td style="background-color: #CDE8F0">
                                    <asp:TextBox ID="tk58" runat="server" Width="70px" ReadOnly="true" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="style1">&nbsp;</td>
                                <td style="border-left-style: groove; border-width: thin" class="style14">Gross Profit
                                        <asp:TextBox ID="tk60" runat="server" Width="32px" onkeyup="calculateSum();"></asp:TextBox>
                                    &nbsp;%</td>
                                <td>
                                    <asp:TextBox ID="tk59" runat="server" Width="70px" ReadOnly="true" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="style1">&nbsp;</td>
                                <td style="background-color: #CDE8F0; border-left-style: groove; border-width: thin"
                                    class="style14">Grand Total</td>
                                <td style="background-color: #CDE8F0">
                                    <asp:TextBox ID="tk61" runat="server" Width="70px" ReadOnly="true" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="style1"></td>
                                <td class="style5"></td>
                                <td class="style3"></td>
                                <td class="style12"></td>
                                <td class="style11"></td>
                                <td class="style7"></td>
                                <td class="style7"></td>
                                <td class="style13"></td>
                                <td class="style14" style="border-left-style: groove; border-width: thin">Total cost per carton</td>
                                <td class="style8">
                                    <asp:TextBox ID="tk62" runat="server" Width="70px" ReadOnly="true" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="style1">&nbsp;</td>
                                <td class="style5">&nbsp;</td>
                                <td class="style3">&nbsp;</td>
                                <td class="style12">&nbsp;</td>
                                <td class="style11">&nbsp;</td>
                                <td class="style4">&nbsp;</td>
                                <td class="style4">&nbsp;</td>
                                <td class="style13">&nbsp;</td>
                                <td style="background-color: #CDE8F0; border-left-style: groove; border-width: thin"
                                    class="style14">Freight
                                        <asp:TextBox ID="tk71" runat="server" Width="32px" onkeyup="calculateSum();"></asp:TextBox>
                                    &nbsp;</td>
                                <td style="background-color: #CDE8F0">
                                    <asp:TextBox ID="tk63" runat="server" Width="70px" ReadOnly="true" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="style1">&nbsp;</td>
                                <td class="style5">&nbsp;</td>
                                <td class="style3">&nbsp;</td>
                                <td class="style12">&nbsp;</td>
                                <td class="style11">&nbsp;</td>
                                <td class="style4">&nbsp;</td>
                                <td class="style4">&nbsp;</td>
                                <td class="style13">
                                    <u>45 Days Credit</u>
                                </td>
                                <td style="border-left-style: groove; border-width: thin" class="style14">Total cost with Freight</td>
                                <td>
                                    <asp:TextBox ID="tk64" runat="server" Width="70px" ReadOnly="true" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="style1">&nbsp;</td>
                                <td class="style5">&nbsp;</td>
                                <td class="style3">&nbsp;</td>
                                <td class="style12">&nbsp;</td>
                                <td class="style11">&nbsp;</td>
                                <td class="style4">&nbsp;</td>
                                <td class="style4">&nbsp;</td>
                                <td class="style13">
                                    <u>90 Days Credit</u>
                                </td>
                                <td style="border-left-style: groove; border-width: thin" class="style14">&nbsp;</td>
                                <td>
                                    <asp:TextBox ID="tk65" runat="server" Width="70px" ReadOnly="true" onkeyup="calculateSum();"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </div>
    <asp:Button ID="btnhideF" runat="server" OnClick="btnhideF_Click" Style="display: none" />
    <asp:Button ID="btnhideF_s" runat="server" OnClick="btnhideF_s_Click" Style="display: none" />
    <asp:HiddenField ID="hffield" runat="server" />
    <asp:HiddenField ID="edmode" runat="server" />
    <asp:HiddenField ID="hfname" runat="server" />
</asp:Content>
