<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="cost_est" Title="Tejaxo" CodeFile="cost_est.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <style type="text/css">
        .hidden {
            display: none;
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
                        <button type="submit" id="btnprint" class="btn btn-info" style="width: 100px;" runat="server" accesskey="P" onserverclick="btnprint_ServerClick"><u>P</u>rint</button>
                        <button type="submit" id="btndel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="l" onserverclick="btndel_ServerClick">De<u>l</u>ete</button>
                        <button type="submit" id="btnlist" visible="false" class="btn btn-info" style="width: 100px;" runat="server" accesskey="t" onserverclick="btnlist_ServerClick">Lis<u>t</u></button>
                        <button type="submit" id="btncancel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="c" onserverclick="btncancel_ServerClick"><u>C</u>ancel</button>
                        <button type="submit" id="btnexit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="X" onserverclick="btnexit_ServerClick">E<u>x</u>it</button>
                        <button type="submit" id="btnrefresh" class="btn btn-info" visible="true" style="width: 100px;" runat="server" accesskey="N" onserverclick="btnrefresh_ServerClick">R<u>e</u>fresh</button>
                    </td>
                    <td>
                        <asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style="font-size:small;color:red;"> <u>(Supporting text is in red color and Alias name is in blue color)</u> </span></td>
                </tr>
            </table>
        </section>

        <section class="content">
            <div class="row">
                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <asp:Label ID="Label20" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Customer</asp:Label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnacode" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnacode_Click" />
                                </div>
                                <div class="col-sm-2" style="display:none;">
                                    <asp:TextBox ID="txtacode" ReadOnly="true" runat="server" CssClass="form-control" Width="100%" Height="28px"></asp:TextBox>
                                </div>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtaname" runat="server" CssClass="form-control" MaxLength="130" Width="100%" Height="28px"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="Label4" runat="server" Text="lbl3" CssClass="col-sm-4 control-label" Font-Size="14px" Font-Bold="True">Item Name</asp:Label>
                                <div class="col-sm-1" style="display:none;">
                                    <asp:ImageButton ID="btnicode" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnicode_Click" />
                                </div>
                                <div class="col-sm-4"style="display:none;">
                                    <asp:TextBox ID="txticode" ReadOnly="true" runat="server" CssClass="form-control" Width="100%" Height="28px"></asp:TextBox>
                                </div>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtiname" runat="server" CssClass="form-control"  MaxLength="130" Width="100%" Height="28px"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="Label5" runat="server" Text="lbl3" CssClass="col-sm-4 control-label" Font-Size="14px"  Font-Bold="True"> Payment_Terms</asp:Label>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtpayterms" MaxLength="4" runat="server" CssClass="form-control" Width="100%" Height="28px" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label6" runat="server" Text="lbl3" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True"> Delv.Locn</asp:Label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtdelvlocn" MaxLength="30" runat="server" CssClass="form-control" Width="100%" Height="28px"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">

                            <div class="form-group">
                                <asp:Label ID="Label1" runat="server" Text="lbl3" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Costing#</asp:Label>
                                <div class="col-sm-1">
                                    <asp:Label ID="lbl1a" runat="server" Text="CF" Style="width: 22px; float: right;" CssClass="col-sm-1 control-label"></asp:Label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtvchnum" ReadOnly="true" runat="server" CssClass="form-control" Width="100%" Height="28px"></asp:TextBox>
                                    <asp:TextBox ID="txtamd" runat="server" Width="20px" ReadOnly="true" Visible="false"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label2" runat="server" Text="lbl3" autocomplete="off" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Dt.</asp:Label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtvchdt" placeholder="Date" runat="server" Width="100%" CssClass="form-control" ReadOnly="true" Height="28px"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtvchdt_CalendarExtender" runat="server" Enabled="True" TargetControlID="txtvchdt" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="Maskedit1" runat="server" Mask="99/99/9999" MaskType="Date" TargetControlID="txtvchdt" />
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <asp:RadioButtonList ID="rdform" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rdform_SelectedIndexChanged" RepeatDirection="Horizontal" Width="186px">
                                        <asp:ListItem Text="Pouch Form" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Roll Form" Value="1"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>                                                                                               
                            </div>


                            <div class="form-group">
                                <asp:Label ID="Label7" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Min order Qty</asp:Label>

                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtprt"  onkeyup="cal()" MaxLength="6" runat="server" CssClass="form-control" ONKEYPRESS="return isDecimalKey(event)" Width="100%" Height="28px"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label8" runat="server" Text="lbl3" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Annual Qty</asp:Label>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtannualqty"  onkeyup="cal()" MaxLength="8" runat="server" CssClass="form-control" ONKEYPRESS="return isDecimalKey(event)" Width="100%" Height="28px"></asp:TextBox>
                                </div>
                                <div class="col-sm-2">
                                    <asp:Button ID="btncal" runat="server" OnClick="btncal_Click" Text="Calculate" Width="100%" />                                  
                                </div>
                            </div>
                        </div>
                    </div>
                </div>



                <section class="col-lg-12 connectedSortable">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">
                                <li><a href="#DescTab1" id="TabPanel1" runat="server" aria-controls="DescTab1" role="tab" data-toggle="tab">Folding Carton</a></li>
                                <li><a href="#DescTab2" id="TabPanel2" runat="server" aria-controls="DescTab2" role="tab" data-toggle="tab"></a></li>
                                <li><a href="#DescTab" id="TabPanel3" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab">Roto</a></li>
                                <li><a href="#DescTab4" id="TabPanel4" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Labels</a></li>
                                <li><a href="#DescTab5" id="TabPanel5" runat="server" aria-controls="DescTab5" role="tab" data-toggle="tab"></a></li>
                            </ul>

                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane active" id="DescTab1" style="display: none;">
                                    <div class="lbBody" style="height: 400px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <div class="col-md-12">
                                            <div>
                                                <div class="box-body">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td class="font_css" colspan="2">
                                                                <asp:TextBox ID="txtff1" runat="server" Width="130px" Style="float: right;"></asp:TextBox>Carton Size</td>
                                                            <td class="font_css" colspan="2">
                                                                <asp:TextBox ID="txtff2" runat="server" Width="130px" Style="float: right;"></asp:TextBox>Carton Style</td>
                                                            <td class="font_css" colspan="2">
                                                                <asp:TextBox ID="txtff3" runat="server" Width="130px" Style="float: right;"></asp:TextBox>Spl Features</td>
                                                        </tr>

                                                        <tr>
                                                            <td class="font_css" colspan="2">
                                                                <asp:TextBox ID="txtff4" runat="server" Width="130px" Style="float: right;"></asp:TextBox>Board Mill</td>
                                                            <td colspan="2" class="font_css">
                                                                <asp:TextBox ID="txtff5" runat="server" Width="130px" Style="float: right;"></asp:TextBox>Board Type</td>
                                                            <td class="font_css">No. of Clr</td>
                                                            <td align="right" class="font_css">
                                                                <asp:TextBox ID="txtff6" runat="server" Style="background: #FFFf99;" Width="70px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="font_css">
                                                                <asp:TextBox ID="txtff7" onkeyup="cal()" runat="server" Style="background: #FFFf99; float: right" Width="90px"></asp:TextBox>No. of Ups</td>
                                                            <td class="font_css">&nbsp;Board GSM</td>
                                                            <td colspan="2" class="font_css">
                                                                <asp:TextBox ID="txtff8" runat="server" onkeyup="cal()" Width="50px" Style="background: #FFFf99;"></asp:TextBox>
                                                                <asp:TextBox ID="txtff9" runat="server"  onkeyup="cal()" Width="50px" Style="background: #FFFf99; float: right;"></asp:TextBox>Board Len.(Cms)</td>
                                                            <td class="font_css">Board Width(cms)
                                                            </td>
                                                            <td align="right" class="font_css">
                                                                <asp:TextBox ID="txtff10"  onkeyup="cal()" runat="server" Width="70px" Style="background: #FFFf99;"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="font_css">Wastage</td>
                                                            <td align="right" class="font_css">
                                                                <asp:TextBox ID="txtff11" runat="server" Width="70px"></asp:TextBox>
                                                            </td>
                                                            <td class="font_css">WB Rate/100 Sq Inch</td>
                                                            <td class="font_css" align="right">
                                                                <asp:TextBox ID="txtff26" runat="server" Width="70px"></asp:TextBox>

                                                            </td>
                                                            <td class="font_css">Embossing Block Cost</td>
                                                            <td align="right" class="font_css">
                                                                <asp:TextBox ID="txtff41" runat="server" Width="70px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="font_css">Board Qty(Kg)/Th.Crtn</td>
                                                            <td align="right" class="font_css">
                                                                <asp:TextBox ID="txtff12" runat="server" Style="background: #FFccff;" Width="70px" ReadOnly="True"></asp:TextBox>
                                                            </td>
                                                            <td class="font_css">WB Cost/Th.Crtn</td>
                                                            <td class="font_css" align="right">
                                                                <asp:TextBox ID="txtff27" runat="server" Width="70px" Style="background: #ffccff" ReadOnly="True"></asp:TextBox>
                                                            </td>
                                                            <td class="font_css">Embossing Cost/Th Crtn</td>
                                                            <td align="right" class="font_css">
                                                                <asp:TextBox ID="txtff42" runat="server" Width="70px" Style="background: #ffccff" ReadOnly="True"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="font_css">Board Rate/Kg</td>
                                                            <td align="right" class="font_css">
                                                                <asp:TextBox ID="txtff13" runat="server" onkeyup="cal()"  Width="70px" Style="background: #FFFf99"></asp:TextBox>
                                                            </td>
                                                            <td class="font_css">Nylo Plate Cost for
                                                                <br />
                                                                Spot/FI WBV</td>
                                                            <td class="font_css" align="right">
                                                                <asp:TextBox ID="txtff28" runat="server" Width="70px"></asp:TextBox>
                                                            </td>
                                                            <td class="font_css">Embossing Rate/Th Sheet</td>
                                                            <td align="right" class="font_css">
                                                                <asp:TextBox ID="txtff43" runat="server" Width="70px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="font_css">Board Cost/Th.Crtn</td>
                                                            <td align="right" class="font_css">
                                                                <asp:TextBox ID="txtff14" runat="server" Style="background: #FFCCFF;"
                                                                    Width="70px" ReadOnly="True"></asp:TextBox>
                                                            </td>
                                                            <td class="font_css">Nylo Cost/Th.Crtn</td>
                                                            <td class="font_css" align="right">
                                                                <asp:TextBox ID="txtff29" runat="server" Width="70px" Style="background: #FFCCFF;" ReadOnly="True"></asp:TextBox>

                                                            </td>
                                                            <td class="font_css">Embossing Rate/Th Crtn</td>
                                                            <td align="right" class="font_css">
                                                                <asp:TextBox ID="txtff44" runat="server" Width="70px" Style="background: #FFCCFF;" ReadOnly="True"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="font_css">Positive Cost/Th.Crtn</td>
                                                            <td align="right" class="font_css">
                                                                <asp:TextBox ID="txtff15" runat="server" Width="70px"></asp:TextBox>
                                                            </td>
                                                            <td class="font_css">Lamination Rate/100 Sq Inch</td>
                                                            <td class="font_css" align="right">
                                                                <asp:TextBox ID="txtff30" runat="server" Width="70px"></asp:TextBox>
                                                            </td>
                                                            <td class="font_css">Window Patch area(SQ cm)</td>
                                                            <td align="right" class="font_css">
                                                                <asp:TextBox ID="txtff45" runat="server" Width="70px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="font_css">Plate Rate</td>
                                                            <td align="right" class="font_css">
                                                                <asp:TextBox ID="txtff16" runat="server" onkeyup="cal()" Width="70px"></asp:TextBox>
                                                            </td>
                                                            <td class="font_css">Lamination Cost/Th.Crtn</td>
                                                            <td class="font_css " align="right">
                                                                <asp:TextBox ID="txtff31" runat="server" Width="70px" Style="background: #FFCCFF;" ReadOnly="True"></asp:TextBox>
                                                            </td>
                                                            <td class="font_css">Thickness of Fim Micr</td>
                                                            <td align="right" class="font_css">
                                                                <asp:TextBox ID="txtff46" runat="server" Width="70px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="font_css">Plate Cost/Th.Crtn</td>
                                                            <td align="right" class="font_css">
                                                                <asp:TextBox ID="txtff17" runat="server" Style="background: #FFCCFF;" Width="70px" ReadOnly="True"></asp:TextBox>
                                                            </td>
                                                            <td class="font_css">Foiling Area in SQ Inch</td>
                                                            <td class="font_css" align="right">
                                                                <asp:TextBox ID="txtff32" onkeyup="cal()"  runat="server" Width="70px"></asp:TextBox>
                                                            </td>
                                                            <td class="font_css">Rate of Film/Kg</td>
                                                            <td align="right" class="font_css">
                                                                <asp:TextBox ID="txtff47"  onkeyup="cal()" runat="server" Width="70px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="font_css">Printing Rate/Color</td>
                                                            <td align="right" class="font_css">
                                                                <asp:TextBox ID="txtff18" onkeyup="cal()" runat="server" Width="70px"></asp:TextBox>
                                                            </td>
                                                            <td class="font_css">Foiling Rate/Sq Inch</td>
                                                            <td class="font_css" align="right">
                                                                <asp:TextBox ID="txtff33"  onkeyup="cal()" runat="server" Width="70px"></asp:TextBox>
                                                            </td>
                                                            <td class="font_css">Labr Cost of Window/Th Crtn</td>
                                                            <td align="right" class="font_css">
                                                                <asp:TextBox ID="txtff48"  onkeyup="cal()" runat="server" Width="70px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="font_css">Printing Cost/Th.Crtn</td>
                                                            <td align="right" class="font_css">
                                                                <asp:TextBox ID="txtff19" runat="server" Style="background: #FFCCFF;" Width="70px" ReadOnly="True"></asp:TextBox>
                                                            </td>
                                                            <td class="font_css">Foiling Cost/Th.Crtn</td>
                                                            <td class="font_css" align="right">
                                                                <asp:TextBox ID="txtff34" runat="server" Width="70px" Style="background: #FFCCFF;" ReadOnly="True"></asp:TextBox>
                                                            </td>
                                                            <td class="font_css">Window Patch Cost/Th Crtn</td>
                                                            <td align="right" class="font_css">
                                                                <asp:TextBox ID="txtff49" runat="server" Width="70px" Style="background: #FFCCFF;" ReadOnly="True"></asp:TextBox>
                                                            </td>

                                                        </tr>
                                                        <tr>
                                                            <td class="font_css">U.V. Blanket Cost for UV</td>
                                                            <td align="right" class="font_css">
                                                                <asp:TextBox ID="txtff20" onkeyup="cal()" runat="server" Width="70px"></asp:TextBox>
                                                            </td>
                                                            <td class="font_css">Foiling Block Cost</td>
                                                            <td class="font_css" align="right">
                                                                <asp:TextBox ID="txtff35"  onkeyup="cal()" runat="server" Width="70px"></asp:TextBox>
                                                            </td>
                                                            <td class="font_css">S.K. Gsm &nbsp; 
                                                            <asp:TextBox ID="txtff50" onkeyup="cal()"  runat="server" Width="90px"></asp:TextBox>&#160; Flute Gsm </td>
                                                            <td align="right" class="font_css">
                                                                <asp:TextBox ID="txtff51"  onkeyup="cal()" runat="server" Width="70px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="font_css">UV Blanket Cost/Th.Crtn</td>
                                                            <td align="right" class="font_css">
                                                                <asp:TextBox ID="txtff21" runat="server" Style="background: #FFCCFF;" Width="70px" ReadOnly="True"></asp:TextBox>
                                                            </td>
                                                            <td class="font_css">Foiling Block Cost/Th.Crtn</td>
                                                            <td class="font_css" align="right">
                                                                <asp:TextBox ID="txtff36" runat="server" Width="70px" Style="background: #FFCCFF;" ReadOnly="True"></asp:TextBox>
                                                            </td>
                                                            <td class="font_css">S.K(KGs) &nbsp; 
                    <asp:TextBox ID="txtff52" runat="server" Width="50px" Style="background: #FFCCFF;" ReadOnly="True"></asp:TextBox>&#160; S.K. Rate </td>
                                                            <td align="right" class="font_css">
                                                                <asp:TextBox ID="txtff53"  onkeyup="cal()" runat="server" Width="70px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="font_css">U.V. Rate/100 Sq Inch</td>
                                                            <td align="right" class="font_css">
                                                                <asp:TextBox ID="txtff22" onkeyup="cal()" runat="server" Width="70px"></asp:TextBox>
                                                            </td>
                                                            <td class="font_css">Die Cost</td>
                                                            <td class="font_css" align="right">
                                                                <asp:TextBox ID="txtff37" onkeyup="cal()"  runat="server" Width="70px"></asp:TextBox>
                                                            </td>
                                                            <td class="font_css">S.K. Cost/Th Crtn</td>
                                                            <td align="right" class="font_css">
                                                                <asp:TextBox ID="txtff54"  onkeyup="cal()" runat="server" Width="70px" Style="background: #FFCCFF;" ReadOnly="True"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="font_css">UV Cost/Th.Crtn</td>
                                                            <td align="right" class="font_css">
                                                                <asp:TextBox ID="txtff23"  onkeyup="cal()" runat="server" Style="background: #FFCCFF;" Width="70px" ReadOnly="True"></asp:TextBox>
                                                            </td>
                                                            <td class="font_css">Die Cost/Th.Crtn</td>
                                                            <td class="font_css" align="right">
                                                                <asp:TextBox ID="txtff38"  onkeyup="cal()" runat="server" Width="70px" Style="background: #FFCCFF;" ReadOnly="True"></asp:TextBox>
                                                            </td>
                                                            <td class="font_css">Paste/Th. &nbsp; 
                    <asp:TextBox ID="txtff55" runat="server" Width="90px"></asp:TextBox>&#160; Packing </td>
                                                            <td align="right" class="font_css">
                                                                <asp:TextBox ID="txtff56"  onkeyup="cal()" runat="server" Width="70px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="font_css">Nylo Plate Cost for Spot/FI UV</td>
                                                            <td align="right" class="font_css">
                                                                <asp:TextBox ID="txtff24" onkeyup="cal()"  runat="server" Width="70px"></asp:TextBox>
                                                            </td>
                                                            <td class="font_css">Die Cutting Rate/Th Sheet</td>
                                                            <td class="font_css" align="right">
                                                                <asp:TextBox ID="txtff39"  onkeyup="cal()" runat="server" Width="70px"></asp:TextBox>
                                                            </td>
                                                            <td class="font_css">Sub Tot. &nbsp; 
                    <asp:TextBox ID="txtff57" runat="server" Width="90px" Style="background: #FFCCFF;" ReadOnly="true"></asp:TextBox>
                                                                &#160; Margin </td>
                                                            <td align="right" class="font_css">
                                                                <asp:TextBox ID="txtff58"  onkeyup="cal()" runat="server" Width="70px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="font_css">Nylo Cost/Th.Crtn</td>
                                                            <td align="right" class="font_css">
                                                                <asp:TextBox ID="txtff25" runat="server" Style="background: #FFCCFF;" ReadOnly="true" Width="70px"></asp:TextBox>
                                                            </td>
                                                            <td class="font_css">Die Cuttong Cost/Th Crtn</td>
                                                            <td class="font_css" align="right">
                                                                <asp:TextBox ID="txtff40" runat="server" Width="70px" Style="background: #FFCCFF;" ReadOnly="true"></asp:TextBox>
                                                            </td>
                                                            <td class="font_css">Cost/Th.Crtn &nbsp; 
                                                            <asp:TextBox ID="txtff59" runat="server" Width="90px" Style="background: #FFCCFF;" ReadOnly="true"></asp:TextBox>
                                                                &#160; Rate/Crtn </td>
                                                            <td align="right" class="font_css">
                                                                <asp:TextBox ID="txtff60" onkeyup="cal()" runat="server" Width="70px" Style="background: #FFCCFF;" ReadOnly="true"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div role="tabpanel" class="tab-pane active" id="DescTab2" style="display: none;">
                                    <div class="lbBody" style="height: 400px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <div class="col-md-12">
                                            <div>
                                                <div class="box-body">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane active" id="DescTab">
                                    <div class="lbBody" style="height: 400px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <div class="col-md-12">
                                            <div>
                                                <div class="box-body">
                                                    <table style="width: 100%">
                                                        <tr style="display: none">
                                                            <td class="font_css">Size(mm)</td>
                                                            <td class="font_css">Length</td>
                                                            <td>
                                                                <asp:TextBox ID="txtrt1" runat="server" Width="90px" MaxLength="30" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox>
                                                            </td>
                                                            <td class="font_css">Width</td>
                                                            <td>
                                                                <asp:TextBox ID="txtrt2" runat="server" Width="90px" MaxLength="30" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox>
                                                            </td>
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="font_css"><asp:Label ID="lbltd1" Font-Bold="true" runat="server"></asp:Label>  </td>                                                                                                                          
                                                            <td class="font_css">  <asp:Label ID="lbltd2" Font-Bold="true" runat="server"></asp:Label> </td>                                                                                                                         
                                                            <td class="font_css">  <asp:Label ID="lbltd3" Font-Bold="true" runat="server"></asp:Label> </td>                                                                                                                         
                                                            <td class="font_css"><asp:Label ID="lbltd4" Font-Bold="true" runat="server"></asp:Label>  </td>                                                                                                                          
                                                            <td class="font_css"> <asp:Label ID="lbltd5" Font-Bold="true" runat="server"></asp:Label>  </td>                                                                                                                         
                                                            <td class="font_css"> <asp:Label ID="lbltd6" Font-Bold="true" runat="server"></asp:Label>  </td>                                                                                                                         
                                                            <td class="font_css"> <asp:Label ID="lbltd7" Font-Bold="true" runat="server"></asp:Label> </td>                                                                                                                          
                                                            <td></td>
                                                            <td></td>
                                                        </tr>

                                                        <tr>
                                                            <td class="font_css">PET_1 (PLN, CTR, CTD, MET, Amorphous)(micron)</td>                                                            
                                                            <td> <asp:TextBox ID="txtrt3" runat="server" onkeyup="cal()" Width="90px" MaxLength="7" ONKEYPRESS="return isDecimalKey(event)" Style="text-align: right;"></asp:TextBox></td>                                                                                                                           
                                                            <td> <asp:TextBox ID="txtrt4" runat="server" onkeyup="cal()" Width="90px" ReadOnly="True" Text="1.4" Style="text-align: right;"></asp:TextBox></td>                                                                                                                                                                                          
                                                            <td> <asp:TextBox ID="txtrt5" runat="server" Width="90px" ReadOnly="True" BackColor="LightGray" Style="text-align: right;"></asp:TextBox></td>                                                                                                                           
                                                            <td><asp:TextBox ID="txtrt6" runat="server" onkeyup="cal()" Width="90px" MaxLength="7" ONKEYPRESS="return isDecimalKey(event)" Style="text-align: right;"></asp:TextBox></td>                                                                                                                                                                                           
                                                            <td> <asp:TextBox ID="txtrt7" runat="server" Width="90px" ReadOnly="True" BackColor="LightGray" Style="text-align: right;"></asp:TextBox></td>                                                                                                                           
                                                            <td><asp:TextBox ID="txtrt8" runat="server" Width="90px" Style="display: none; text-align: right;"></asp:TextBox></td>                                                                                                                           
                                                            <td></td>
                                                            <td></td>
                                                        </tr>

                                                          <tr>
                                                            <td class="font_css">PET_2 (PLN, CTR, CTD, MET, Amorphous) (micron)</td>                                                               
                                                            <td><asp:TextBox ID="txtrt3a" runat="server" onkeyup="cal()" Width="90px" MaxLength="7" ONKEYPRESS="return isDecimalKey(event)" Style="text-align: right;"></asp:TextBox></td>                                                                                                                            
                                                            <td><asp:TextBox ID="txtrt4a" runat="server" onkeyup="cal()" Width="90px" ReadOnly="True" Text="1.4" Style="text-align: right;"></asp:TextBox></td>                                                                                                                                                                                          
                                                            <td><asp:TextBox ID="txtrt5a" runat="server" Width="90px" ReadOnly="True" BackColor="LightGray" Style="text-align: right;"></asp:TextBox></td>                                                                                                                            
                                                            <td> <asp:TextBox ID="txtrt6a" runat="server" onkeyup="cal()" Width="90px" MaxLength="7" ONKEYPRESS="return isDecimalKey(event)" Style="text-align: right;"></asp:TextBox></td>                                                            
                                                            <td><asp:TextBox ID="txtrt7a" runat="server" Width="90px" ReadOnly="True" BackColor="LightGray" Style="text-align: right;"></asp:TextBox></td>                                                                                                                           
                                                            <td><asp:TextBox ID="txtrt8a" runat="server" Width="90px" Style="display: none; text-align: right;"></asp:TextBox></td>                                                                                                                            
                                                            <td></td>
                                                            <td></td>
                                                        </tr>

                                                        <tr>
                                                            <td class="font_css">PE NAT</td>
                                                            <td><asp:TextBox ID="txtrt9" runat="server" onkeyup="cal()" Width="90px" MaxLength="7" Style="text-align: right;" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox></td>                                                                                                                            
                                                            <td><asp:TextBox ID="txtrt10" runat="server" onkeyup="cal()" Width="90px" ReadOnly="True" Style="text-align: right;" Text="0.92"></asp:TextBox></td>                                                                                                                                                                                           
                                                            <td><asp:TextBox ID="txtrt11" runat="server" Width="90px" ReadOnly="True" BackColor="LightGray" Style="text-align: right;"></asp:TextBox></td>                                                                                                                            
                                                            <td><asp:TextBox ID="txtrt12" runat="server" onkeyup="cal()" Width="90px" MaxLength="7" Style="text-align: right;" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox></td>                                                                                                                                                                                            
                                                            <td> <asp:TextBox ID="txtrt13" runat="server" Width="90px" ReadOnly="True" BackColor="LightGray" Style="text-align: right;"></asp:TextBox></td>                                                                                                                           
                                                            <td> <asp:TextBox ID="txtrt14" runat="server" Width="90px" Style="display: none; text-align: right;"></asp:TextBox></td>                                                               
                                                            <td></td>
                                                            <td></td>
                                                        </tr>

                                                        <tr>
                                                            <td class="font_css">PE WOP (micron)</td>
                                                            <td><asp:TextBox ID="txtrt15" runat="server" Width="90px" onkeyup="cal()" MaxLength="7" Style="text-align: right;" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox></td>                                                            
                                                            <td> <asp:TextBox ID="txtrt16" runat="server" Width="90px" onkeyup="cal()" ReadOnly="True" Style="text-align: right;" Text="0.95"></asp:TextBox></td>                                                                                                                        
                                                            <td><asp:TextBox ID="txtrt17" runat="server" Width="90px" ReadOnly="True" BackColor="LightGray" Style="text-align: right;"></asp:TextBox></td>                                                            
                                                            <td><asp:TextBox ID="txtrt18" runat="server" Width="90px" onkeyup="cal()" MaxLength="7" Style="text-align: right;" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox></td>                                                                                                                                                                                          
                                                            <td><asp:TextBox ID="txtrt19" runat="server" Width="90px" ReadOnly="True" BackColor="LightGray" Style="text-align: right;"></asp:TextBox></td>                                                                                                                            
                                                            <td> <asp:TextBox ID="txtrt20" runat="server" Width="90px" Style="display: none; text-align: right;"></asp:TextBox></td>                                                            
                                                            <td></td>
                                                            <td></td>
                                                        </tr>

                                                        <tr>
                                                            <td class="font_css">BOPP (PLN, MET, HS) (micron)</td>
                                                            <td><asp:TextBox ID="txtrt21" runat="server" Width="90px" MaxLength="7" onkeyup="cal()" Style="text-align: right;" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox></td>                                                                                                                                                                                          
                                                            <td><asp:TextBox ID="txtrt22" runat="server" Width="90px" ReadOnly="True" onkeyup="cal()" Style="text-align: right;" Text="0.905"></asp:TextBox></td>                                                                                                                                                                                            
                                                            <td><asp:TextBox ID="txtrt23" runat="server" Width="90px" ReadOnly="True" BackColor="LightGray" Style="text-align: right;"></asp:TextBox></td>                                                                
                                                            <td><asp:TextBox ID="txtrt24" runat="server" Width="90px" MaxLength="7" onkeyup="cal()" Style="text-align: right;" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox></td>                                                                                                                                                                   
                                                            <td><asp:TextBox ID="txtrt25" runat="server" Width="90px" ReadOnly="True" BackColor="LightGray" Style="text-align: right;"></asp:TextBox></td>                                                                                                                            
                                                            <td><asp:TextBox ID="txtrt26" runat="server" Width="90px" Style="display: none; text-align: right;"></asp:TextBox> </td>                                                                                                                           
                                                            <td></td>
                                                            <td></td>
                                                        </tr>

                                                        <tr>
                                                            <td class="font_css">BOPP WOP (micron)</td>
                                                            <td><asp:TextBox ID="txtrt27" runat="server" Width="90px" onkeyup="cal()" MaxLength="7" Style="text-align: right;" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox></td>                                                                                                                                                                                          
                                                            <td><asp:TextBox ID="txtrt28" runat="server" Width="90px" onkeyup="cal()" ReadOnly="True" Style="text-align: right;" Text="0.905"></asp:TextBox>  </td>                                                                                                                                                                                          
                                                            <td><asp:TextBox ID="txtrt29" runat="server" Width="90px" ReadOnly="True" BackColor="LightGray" Style="text-align: right;"></asp:TextBox></td>                                                                                                                            
                                                            <td><asp:TextBox ID="txtrt30" runat="server" Width="90px" onkeyup="cal()" MaxLength="7" Style="text-align: right;" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox> </td>                                                            
                                                            <td> <asp:TextBox ID="txtrt31" runat="server" Width="90px" ReadOnly="True" BackColor="LightGray" Style="text-align: right;"></asp:TextBox>  </td>                                                                                                                         
                                                            <td><asp:TextBox ID="txtrt32" runat="server" Width="90px" Style="display: none; text-align: right;"></asp:TextBox>  </td>                                                            
                                                            <td></td>
                                                            <td></td>
                                                        </tr>

                                                        <tr>
                                                            <td class="font_css">CPP (NAT, MET) (micron)</td>
                                                            <td><asp:TextBox ID="txtrt48" runat="server" Width="90px" onkeyup="cal()" MaxLength="7" Style="text-align: right;" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox></td>                                                                                                                                                                                           
                                                            <td><asp:TextBox ID="txtrt49" runat="server" Width="90px" onkeyup="cal()" ReadOnly="True" Style="text-align: right;" Text="0.905"></asp:TextBox></td>                                                                                                                                                                                           
                                                            <td><asp:TextBox ID="txtrt50" runat="server" Width="90px" ReadOnly="True" BackColor="LightGray" Style="text-align: right;"></asp:TextBox></td>                                                                                                                            
                                                            <td>  <asp:TextBox ID="txtrt51" runat="server" Width="90px" onkeyup="cal()" MaxLength="7" Style="text-align: right;" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox></td>                                                                                                                                                                                          
                                                            <td> <asp:TextBox ID="txtrt52" runat="server" Width="90px" ReadOnly="True" BackColor="LightGray" Style="text-align: right;"></asp:TextBox> </td>                                                            
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>

                                                        <tr>
                                                            <td class="font_css">CPP W OPQ (micron)</td>
                                                            <td><asp:TextBox ID="txtrt53" runat="server" Width="90px" MaxLength="7" onkeyup="cal()" Style="text-align: right;" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox> </td>                                                                                                                          
                                                            <td><asp:TextBox ID="txtrt54" runat="server" Width="90px" ReadOnly="True" onkeyup="cal()" Style="text-align: right;" Text="0.94"></asp:TextBox>     </td>                                                                                                                                                                                       
                                                            <td> <asp:TextBox ID="txtrt55" runat="server" Width="90px" ReadOnly="True" BackColor="LightGray" Style="text-align: right;"></asp:TextBox> </td>                                                            
                                                            <td><asp:TextBox ID="txtrt56" runat="server" Width="90px" MaxLength="7" onkeyup="cal()" Style="text-align: right;" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox> </td>                                                                                                                                      
                                                            <td><asp:TextBox ID="txtrt57" runat="server" Width="90px" ReadOnly="True" BackColor="LightGray" Style="text-align: right;"></asp:TextBox> </td>                                                            
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>

                                                        <tr>
                                                            <td class="font_css">FOIL (micron)</td>
                                                            <td><asp:TextBox ID="txtrt58" runat="server" Width="90px" MaxLength="7" onkeyup="cal()" Style="text-align: right;" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox></td>                                                                                                                                                                                           
                                                            <td><asp:TextBox ID="txtrt59" runat="server" Width="90px" ReadOnly="True" onkeyup="cal()" Style="text-align: right;" Text="2.67"></asp:TextBox></td>                                                                                                                                                                                          
                                                            <td><asp:TextBox ID="txtrt60" runat="server" Width="90px" ReadOnly="True" BackColor="LightGray" Style="text-align: right;"></asp:TextBox></td>                                                                                                                            
                                                            <td><asp:TextBox ID="txtrt61" runat="server" Width="90px" MaxLength="7" onkeyup="cal()" Style="text-align: right;" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox></td>                                                                                                                               
                                                            <td><asp:TextBox ID="txtrt62" runat="server" Width="90px" ReadOnly="True" BackColor="LightGray" Style="text-align: right;"></asp:TextBox> </td>                                                            
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>

                                                        <tr>
                                                            <td class="font_css">Shrink PVC (micron)</td>
                                                            <td><asp:TextBox ID="txtrt63" runat="server" Width="90px" MaxLength="7" onkeyup="cal()" Style="text-align: right;" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox></td>                                                                                                                            
                                                            <td><asp:TextBox ID="txtrt64" runat="server" Width="90px" ReadOnly="True" onkeyup="cal()" Style="text-align: right;" Text="1.35"></asp:TextBox></td>                                                                                                                                                                                            
                                                            <td><asp:TextBox ID="txtrt65" runat="server" Width="90px" ReadOnly="True" BackColor="LightGray" Style="text-align: right;"></asp:TextBox></td>                                                                                                                            
                                                            <td><asp:TextBox ID="txtrt66" runat="server" Width="90px" MaxLength="7" onkeyup="cal()" Style="text-align: right;" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox></td>                                                                                                                            
                                                            <td> <asp:TextBox ID="txtrt67" runat="server" Width="90px" ReadOnly="True" BackColor="LightGray" Style="text-align: right;"></asp:TextBox>  </td>                                                            
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>

                                                        <tr>
                                                            <td class="font_css">Shrink PET (micron)</td>
                                                            <td><asp:TextBox ID="txtrt68" runat="server" Width="90px" MaxLength="7" onkeyup="cal()" Style="text-align: right;" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox>     </td>                                                                                                                            
                                                            <td><asp:TextBox ID="txtrt69" runat="server" Width="90px" ReadOnly="True" onkeyup="cal()" Style="text-align: right;" Text="1.4"></asp:TextBox> </td>                                                                                                                            
                                                            <td><asp:TextBox ID="txtrt70" runat="server" Width="90px" ReadOnly="True" BackColor="LightGray" Style="text-align: right;"></asp:TextBox></td>                                                                                                                            
                                                            <td><asp:TextBox ID="txtrt71" runat="server" Width="90px" MaxLength="7" onkeyup="cal()" Style="text-align: right;" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox></td>                                                            
                                                            <td> <asp:TextBox ID="txtrt72" runat="server" Width="90px" ReadOnly="True" BackColor="LightGray" Style="text-align: right;"></asp:TextBox>  </td>                                                            
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>

                                                        <tr>
                                                            <td class="font_css">PE NAT Nylon (micron)</td>
                                                            <td><asp:TextBox ID="txtrt73" runat="server" Width="90px" MaxLength="7" onkeyup="cal()" Style="text-align: right;" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox>   </td>                                                            
                                                            <td><asp:TextBox ID="txtrt74" runat="server" Width="90px" ReadOnly="True" ONKEYPRESS="return isDecimalKey(event)" Style="text-align: right;" Text="0.96"></asp:TextBox> </td>                                                            
                                                            <td><asp:TextBox ID="txtrt75" runat="server" Width="90px" ReadOnly="True" BackColor="LightGray" Style="text-align: right;"></asp:TextBox>   </td>                                                            
                                                            <td><asp:TextBox ID="txtrt76" runat="server" Width="90px" MaxLength="7" onkeyup="cal()" Style="text-align: right;" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox>  </td>                                                                                                                            
                                                            <td> <asp:TextBox ID="txtrt77" runat="server" Width="90px" ReadOnly="True" BackColor="LightGray" Style="text-align: right;"></asp:TextBox>   </td>                                                            
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>

                                                        <tr>
                                                            <td class="font_css">PE WOP Nylon (micron)</td>
                                                            <td><asp:TextBox ID="txtrt78" runat="server" Width="90px" MaxLength="7" onkeyup="cal()" Style="text-align: right;" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox> </td>                                                                                                                                                                                          
                                                            <td><asp:TextBox ID="txtrt79" runat="server" Width="90px" ReadOnly="True" onkeyup="cal()" Style="text-align: right;" Text="0.98"></asp:TextBox>  </td>                                                                                                                                                                                          
                                                            <td><asp:TextBox ID="txtrt80" runat="server" Width="90px" ReadOnly="True" BackColor="LightGray" Style="text-align: right;"></asp:TextBox></td>                                                                                                                            
                                                            <td><asp:TextBox ID="txtrt81" runat="server" Width="90px" MaxLength="7" onkeyup="cal()" Style="text-align: right;" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox></td>                                                                                                                                                                                            
                                                            <td> <asp:TextBox ID="txtrt82" runat="server" Width="90px" ReadOnly="True" BackColor="LightGray" Style="text-align: right;"></asp:TextBox> </td>                                                            
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>

                                                        <tr>
                                                            <td class="font_css">NYLON (micron)</td>
                                                            <td><asp:TextBox ID="txtrt83" runat="server" Width="90px" MaxLength="7" onkeyup="cal()" Style="text-align: right;" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox></td>                                                                                                                                                                                         
                                                            <td><asp:TextBox ID="txtrt84" runat="server" Width="90px" ReadOnly="True" onkeyup="cal()" Style="text-align: right;" Text="1.14"></asp:TextBox>   </td>                                                                                                                                                                                      
                                                            <td><asp:TextBox ID="txtrt85" runat="server" Width="90px" ReadOnly="True" BackColor="LightGray" Style="text-align: right;"></asp:TextBox></td>                                                                                                                            
                                                            <td><asp:TextBox ID="txtrt86" runat="server" Width="90px" MaxLength="7" onkeyup="cal()" Style="text-align: right;" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox>  </td>                                                                                                                            
                                                            <td><asp:TextBox ID="txtrt87" runat="server" Width="90px" ReadOnly="True" BackColor="LightGray" Style="text-align: right;"></asp:TextBox></td>                                                                                                                            
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>

                                                        <tr>
                                                            <td class="font_css">PAPER (micron)</td>
                                                            <td><asp:TextBox ID="txtrt88" runat="server" Width="90px" Style="text-align: right;" MaxLength="7" onkeyup="cal()" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox>  </td>                                                                                                                            
                                                            <td><asp:TextBox ID="txtrt89" runat="server" Width="90px" ReadOnly="True" Text="1" onkeyup="cal()" Style="text-align: right;"></asp:TextBox> </td>                                                                                                                            
                                                            <td><asp:TextBox ID="txtrt90" runat="server" Width="90px" ReadOnly="True" BackColor="LightGray" Style="text-align: right;"></asp:TextBox></td>                                                                                                                            
                                                            <td><asp:TextBox ID="txtrt91" runat="server" Width="90px" MaxLength="7" onkeyup="cal()" Style="text-align: right;" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox>  </td>                                                                                                                                                                   
                                                            <td><asp:TextBox ID="txtrt92" runat="server" Width="90px" ReadOnly="True" BackColor="LightGray" Style="text-align: right;"></asp:TextBox>  </td>                                                            
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>

                                                        <tr>
                                                            <td class="font_css">PEARL BOPP (micron)</td>
                                                            <td><asp:TextBox ID="txtrt93" runat="server" Width="90px" MaxLength="7" onkeyup="cal()" Style="text-align: right;" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox></td>                                                                                                                                                                                          
                                                            <td> <asp:TextBox ID="txtrt94" runat="server" Width="90px" ReadOnly="True" onkeyup="cal()" Style="text-align: right;" Text="0.67"></asp:TextBox> </td>                                             
                                                            <td><asp:TextBox ID="txtrt95" runat="server" Width="90px" ReadOnly="True" BackColor="LightGray" Style="text-align: right;"></asp:TextBox></td>                                                                                                                            
                                                            <td><asp:TextBox ID="txtrt96" runat="server" Width="90px" MaxLength="7" onkeyup="cal()" Style="text-align: right;" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox></td>                                                                                                                                                                                            
                                                            <td> <asp:TextBox ID="txtrt97" runat="server" Width="90px" ReadOnly="True" BackColor="LightGray" Style="text-align: right;"></asp:TextBox>  </td>                                                            
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>

                                                        <tr>
                                                            <td class="font_css">ADH SOLVENTLESS (gsm)</td>
                                                            <td><asp:TextBox ID="txtrt98" runat="server" Width="90px" MaxLength="7" onkeyup="cal()" Style="text-align: right;" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox>  </td>                                                            
                                                            <td><asp:TextBox ID="txtrt99" runat="server" Width="90px" ReadOnly="True" onkeyup="cal()" Text="1" Style="text-align: right;"></asp:TextBox>  </td>                                                            
                                                            <td> <asp:TextBox ID="txtrt100" runat="server" Width="90px" ReadOnly="True" BackColor="LightGray" Style="text-align: right;"></asp:TextBox>   </td>                                                                                                                        
                                                            <td><asp:TextBox ID="txtrt101" runat="server" Width="90px" MaxLength="7" onkeyup="cal()" Style="text-align: right;" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox> </td>                                                            
                                                            <td> <asp:TextBox ID="txtrt102" runat="server" Width="90px" ReadOnly="True" BackColor="LightGray" Style="text-align: right;"></asp:TextBox>  </td>                                                            
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>

                                                        <tr>
                                                            <td class="font_css">ADH SOLVENT BASE (gsm)</td>
                                                            <td> <asp:TextBox ID="txtrt103" runat="server" Width="90px" MaxLength="7" onkeyup="cal()" Style="text-align: right;" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox>  </td>                                                                                                                           
                                                            <td><asp:TextBox ID="txtrt104" runat="server" Width="90px" ReadOnly="True" onkeyup="cal()" Text="1" Style="text-align: right;"></asp:TextBox> </td>                                                                                                                                                                                          
                                                            <td><asp:TextBox ID="txtrt105" runat="server" Width="90px" ReadOnly="True" BackColor="LightGray" Style="text-align: right;"></asp:TextBox></td>                                                                                                                            
                                                            <td><asp:TextBox ID="txtrt106" runat="server" Width="90px" MaxLength="7" onkeyup="cal()" Style="text-align: right;" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox>   </td>                                                                                                                                                                                        
                                                            <td><asp:TextBox ID="txtrt107" runat="server" Width="90px" ReadOnly="True" BackColor="LightGray" Style="text-align: right;"></asp:TextBox> </td>                                                                                                                           
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>

                                                        <tr>
                                                            <td class="font_css">Ink (gsm)</td>
                                                            <td><asp:TextBox ID="txtrt108" runat="server" Width="90px" MaxLength="7" onkeyup="cal()" Style="text-align: right;" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox>   </td>                                                                                                                            
                                                            <td><asp:TextBox ID="txtrt109" runat="server" Width="90px" ReadOnly="True" onkeyup="cal()" Text="1" Style="text-align: right;"></asp:TextBox>  </td>                                                                                                                            
                                                            <td><asp:TextBox ID="txtrt110" runat="server" Width="90px" ReadOnly="True" BackColor="LightGray" Style="text-align: right;"></asp:TextBox></td>                                                                                                                            
                                                            <td><asp:TextBox ID="txtrt111" runat="server" Width="90px" MaxLength="7" onkeyup="cal()" Style="text-align: right;" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox>     </td>                                                                                                                            
                                                            <td> <asp:TextBox ID="txtrt112" runat="server" Width="90px" ReadOnly="True" BackColor="LightGray" Style="text-align: right;"></asp:TextBox> </td>                                                                                                                          
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>

                                                        <tr>
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td class="font_css">RMC, Rs/Kg</td>
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>                                                            
                                                        </tr>

                                                        <tr>                                                           
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td>Total_Gsm <span style="color:blue;"> (A)</span></td>
                                                            <td> <asp:TextBox ID="txtrt113" runat="server" Width="90px" ReadOnly="True" BackColor="LightGray" Style="text-align: right;"></asp:TextBox> </td>
                                                            <td>Total_Sqm <span style="color:blue;"> (B)</span></td>
                                                            <td> <asp:TextBox ID="txtrt114" runat="server" Width="90px" ReadOnly="True" BackColor="LightGray" Style="text-align: right;"></asp:TextBox> </td>
                                                            <td> <asp:TextBox ID="txtrt34" runat="server" Width="80px" Style="display: none; text-align: right;"></asp:TextBox></td>                                                                                                                                                                                     
                                                        </tr>

                                                        <tr>
                                                            <td colspan="2" style="color:red;">Total Formula : <span style="font-size:small">[Tot_sqm/Tot_gsm * 1000]</span></td>             
                                                            <td></td>
                                                            <td></td>
                                                            <td class="font_css">Total</td>
                                                            <td> <asp:TextBox ID="txtrt33" runat="server" Width="90px" BackColor="LightGray" ReadOnly="True" Style="text-align: right;"></asp:TextBox> </td>                                                                                                                          
                                                            <td class="font_css">Wstg(2-ply 12% to 4-ply 18%)(%) <span style="color:blue;">(C)</span></td>
                                                            <td><asp:TextBox ID="txtrt115" runat="server" Width="90px" MaxLength="7" onkeyup="cal()" Style="text-align: right;" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox>  </td>                                                                                                                                                                                        
                                                            <td></td>
                                                        </tr>

                                                        <tr>                                                         
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td class="font_css">Other_Input_Costs(Rs/Kg)<span style="color:blue;">(D)</span></td>
                                                            <td> <asp:TextBox ID="txtrt116" runat="server" Width="90px" MaxLength="7" onkeyup="cal()" ONKEYPRESS="return isDecimalKey(event)" Style="text-align: right;"></asp:TextBox>  </td>                                                                                                                                                                                         
                                                            <td class="font_css">VA (%)<span style="color:blue;">(E)</span></td>
                                                            <td> <asp:TextBox ID="txtrt117" runat="server" Width="90px" MaxLength="7" onkeyup="cal()" ONKEYPRESS="return isDecimalKey(event)" Style="text-align: right;"></asp:TextBox> </td>                                                            
                                                            <td></td>                            
                                                        </tr>

                                                        <tr>
                                                            <td colspan="2"><span style="font-size:small;color:red;">Tot_RMC Formula : [ (B/A*1000) *(1+(C%)+D) ]  &nbsp;&nbsp;&nbsp;&&nbsp;&nbsp;&nbsp; (G) : [ (F*A)/1000 ] </span></td>                                                         
                                                            <td></td>
                                                            <td></td>                                                          
                                                            <td class="font_css">Total RMC <span style="font-size:small;color:blue;"> (F)</span></td>                                                        
                                                            <td> <asp:TextBox ID="txtrt118" runat="server" Width="90px" ReadOnly="True" BackColor="LightGray" Style="text-align: right;"></asp:TextBox></td>                                                  
                                                            <td><span style="font-size:small;color:blue;"> (G)</span>  </td>   
                                                            <td><asp:TextBox ID="txtrt119" runat="server" Width="90px" ReadOnly="True" BackColor="LightGray" Style="text-align: right;"></asp:TextBox> </td>                                                                                                   
                                                            <td></td>                                                       
                                                        </tr>

                                                        <tr>
                                                            <td colspan="2"><span style="font-size:small;color:red;">Indicative_SP : [ F+(F*(D/100)) ]   &nbsp;&nbsp;&nbsp;&&nbsp;&nbsp;&nbsp; I : [ (((F*A)/1000))/(1-(D/100)) ]</span></td>                                                           
                                                            <td></td>  
                                                            <td></td>                                                                                                                       
                                                            <td class="font_css">Indicative_Selling_Price<span style="color:blue;">(H)</span></td>
                                                            <td><asp:TextBox ID="txtrt35" runat="server" Width="90px" ReadOnly="True" BackColor="LightGray" Style="text-align: right;"></asp:TextBox>   </td>                                                                                                                         
                                                             <td><span style="font-size:small;color:blue;"> (I)</span>  </td>   
                                                            <td><asp:TextBox ID="txtrt36" runat="server" Width="90px" ReadOnly="True" BackColor="LightGray" Style="text-align: right;"></asp:TextBox>    </td>                                                                                                                      
                                                           
                                                            <td></td>      
                                                        </tr>

                                                               <tr>
                                                          <td><span style="font-size:small;color:red;"> VA : [ L -F ] &nbsp;&nbsp;&nbsp;&&nbsp;&nbsp;&nbsp; K : [ M-G ] </span></td>
                                                            <td></td>        
                                                            <td></td> 
                                                            <td></td>                                                                  
                                                            <td class="font_css">VA<span style="color:blue;">(J)</span></td>
                                                            <td><asp:TextBox ID="txtrt37" runat="server" BackColor="LightGray" Width="90px" ReadOnly="True" Style="text-align: right;"></asp:TextBox>    </td>                                                                                                                        
                                                            <td><span style="font-size:small;color:blue;"> (K)</span>  </td>   
                                                            <td> <asp:TextBox ID="txtrt38" runat="server" BackColor="LightGray" Width="90px" ReadOnly="True" Style="text-align: right;"></asp:TextBox></td>                                                                                                                                                                                                                                                 
                                                            <td></td> 
                                                        </tr>

                                                        <tr id="pch2_" runat="server">
                                                            <td></td>
                                                            <td></td>        
                                                            <td></td>  
                                                            <td></td>  
                                                             <td></td>   
                                                            <td id="Td2" class="font_css" runat="server">Coil Width(mm)<span style="color:blue;">(L)</span></td>
                                                            <td id="Td3" class="font_css" runat="server">Reel Length(mm)<span style="color:blue;">(M)</span></td>
                                                            <td></td>
                                                            <td></td>     
                                                        </tr>

                                                 <tr id="pch2" runat="server">
                                                            <td id="Td16" runat="server"></td>
                                                            <td id="Td17" runat="server"></td>
                                                            <td id="Td18" runat="server"></td>
                                                            <td></td>
                                                            <td id="Td10" class="font_css" runat="server">Pouch Dimensions</td>                                                         
                                                           <td id="Td11" runat="server"> <asp:TextBox ID="txtrt120" runat="server" Width="90px" MaxLength="7" onkeyup="cal()" ONKEYPRESS="return isDecimalKey(event)" Style="text-align: right;"></asp:TextBox></td>                                                                                                                                                                                           
                                                            <td id="Td12" runat="server"> <asp:TextBox ID="txtrt121" runat="server" Width="90px" Style="text-align: right;" MaxLength="7" onkeyup="cal()" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox></td>                                                                                                                                                                                      
                                                     <td></td>
                                                     <td></td>
                                                 </tr>

                                                        <tr id="pch1" runat="server">
                                                            <td id="Td8" runat="server"><span style="font-size:small;color:red;"> Area_sqmtr : [ (L*M)/1000000 ]  &nbsp;&nbsp;&nbsp;&&nbsp;&nbsp;&nbsp; Weight_gms : [ O * A ] </span></td>
                                                            <td id="Td9" runat="server"></td>
                                                            <td id="Td1" class="font_css" runat="server"></td>
                                                            <td id="Td7" class="font_css" runat="server"></td>
                                                            <td id="Td7_1" class="font_css" runat="server"></td>
                                                            <td id="Td4" class="font_css" runat="server">Gusset(mm)<span style="color:blue;">(N)</span></td>
                                                            <td id="Td5" class="font_css" runat="server">Area(sqmtr)<span style="color:blue;">(O)</span></td>
                                                            <td id="Td6" class="font_css" runat="server">Weight(gms)<span style="color:blue;">(P)</span></td>  
                                                            <td></td>                                                                                                                    
                                                        </tr>

                                                        <tr id="pch2_1" runat="server">   
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>                                                                                                                  
                                                            <td id="Td13" runat="server"><asp:TextBox ID="txtrt122" runat="server" Width="90px" MaxLength="7" ONKEYPRESS="return isDecimalKey(event)" onkeyup="cal()" Style="text-align: right;"></asp:TextBox></td>                                                                                                                                                                                          
                                                            <td id="Td14" runat="server"> <asp:TextBox ID="txtrt123" runat="server" BackColor="LightGray" Width="90px" ReadOnly="True" Style="text-align: right;"></asp:TextBox></td>                                                                                                                           
                                                            <td id="Td15" runat="server"><asp:TextBox ID="txtrt124" runat="server" Width="90px" ReadOnly="True" BackColor="LightGray" Style="text-align: right;"></asp:TextBox></td>                                                                                                                            
                                                            <td></td>
                                                        </tr>

                                                        <tr id="pch3" runat="server">
                                                           <td id="Td25" runat="server"></td>
                                                            <td id="Td26" runat="server"></td>
                                                            <td id="Td27" runat="server"></td>
                                                            <td></td>                                                          
                                                            <td id="Td19" runat="server" class="font_css">Pouching_Charges(per 1000)<span style="color:blue;">(Q)</span></td>
                                                            <td id="Td20" runat="server"><asp:TextBox ID="txtrt44" runat="server" Width="90px" MaxLength="7" onkeyup="cal()" Style="text-align: right;"></asp:TextBox></td>
                                                            <td></td>
                                                            <td></td>
                                                              <td></td>
                                                        </tr>

                                                        <tr id="pch3_" runat="server">
                                                            <td><span style="font-size:small;color:red;">Pouching_wstg_chg : [ (AC + R) * (S/100) ]</span></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td id="Td21" runat="server" class="font_css">Other_Pouching_Cost(per 1000)<span style="color:blue;">(R)</span></td>
                                                            <td id="Td22" runat="server"><asp:TextBox ID="txtrt42" runat="server" Width="90px" MaxLength="7" onkeyup="cal()" ONKEYPRESS="return isDecimalKey(event)" onpaste="false" Style="text-align: right;"></asp:TextBox> </td>                                                                                                                                                                                           
                                                            <td id="Td23" class="font_css" runat="server">Pouching_Wastage(%) <asp:TextBox ID="txtrt134" runat="server" Width="20px" Text="2" ONKEYPRESS="return isDecimalKey(event)" onkeyup="cal()" onpaste="false" Style="text-align: right;" MaxLength="5"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style="color:blue;font-size:small;">(S)</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                                            <td id="Td24" runat="server"> <asp:TextBox ID="txtrt43" runat="server"  BackColor="LightGray" Width="90px" ReadOnly="True" Style="text-align: right;"></asp:TextBox> </td>                                                                                                                          
                                                            <td></td>
                                                        </tr>

                                                        <tr id="pch4" runat="server">
                                                            <td id="Td32" runat="server"><span style="font-size:small;color:red;"> Yield(kg) : [ 1000/Weight(gms) ]  &nbsp;&nbsp;&nbsp;&&nbsp;&nbsp;&nbsp; Yield/1000(kg) : [ 1/Weight(gms) ] </span></td>
                                                            <td id="Td33" runat="server"></td>
                                                            <td id="Td34" runat="server"></td>
                                                            <td id="Td35" runat="server"></td>
                                                            <td id="Td28" class="font_css" runat="server">1)Yield/kg<span style="color:blue;">(T)</span> </td>
                                                            <td id="Td29" runat="server"> <asp:TextBox ID="txtrt125" runat="server" BackColor="LightGray" Width="90px" ReadOnly="True" Style="text-align: right;"></asp:TextBox>  </td>                                                                                                                         
                                                           <td id="Td30" class="font_css" runat="server">2)Yield/1000 kgs<span style="color:blue;">(U)</span> </td>
                                                            <td id="Td31" runat="server"><asp:TextBox ID="txtrt126" runat="server" Width="90px" BackColor="LightGray" ReadOnly="True" Style="text-align: right;"></asp:TextBox></td>                                                                                                                           
                                                            <td id="Td36" runat="server"> <asp:TextBox ID="txtrt45" runat="server" Width="90px" Style="display: none; text-align: right;"></asp:TextBox>   </td>                                                                                                                                                                                  
                                                        </tr>

                                                        <tr id="trexise" runat="server">
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td class="font_css">Excisable_other_Charges<span style="font-size:small;color:blue;">(V)</span></td>
                                                            <td><asp:TextBox ID="txtrt127" runat="server" Width="90px" onkeyup="cal()" Style="text-align: right;" ONKEYPRESS="return isDecimalKey(event)" MaxLength="7"></asp:TextBox> </td>
                                                            <td class="font_css">Excise % <span style="font-size:small;color:blue;">(W)</span></td>
                                                            <td><asp:TextBox ID="txtrt128" runat="server" Width="90px" onkeyup="cal()" ONKEYPRESS="return isDecimalKey(event)" Style="text-align: right;" MaxLength="9"></asp:TextBox></td>                                                                                                                           
                                                            <td></td>
                                                        </tr>

                                                           <tr id="trexise_1" runat="server">
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td class="font_css">Cess % <span style="font-size:small;color:blue;">(X)</span></td>                                                          
                                                            <td class="font_css"><asp:TextBox ID="txtrt129" runat="server" Width="90px" MaxLength="7" ONKEYPRESS="return isDecimalKey(event)" onkeyup="cal()" Style="text-align: right;"></asp:TextBox></td>
                                                            <td>She Cess % <span style="font-size:small;color:blue;">(Y)</span></td>
                                                            <td><asp:TextBox ID="txtrt130" runat="server" Width="90px" MaxLength="7" ONKEYPRESS="return isDecimalKey(event)" onkeyup="cal()" Style="text-align: right;"></asp:TextBox> </td>                                                                                                                            
                                                            <td></td>
                                                        </tr>

                                                        <tr id="saletaxrow" runat="server">
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td class="font_css" id="non_exc" runat="server">Non_Excisable_other_Charges<span style="font-size:small;color:blue;">(Z)</span> </td>                                                           
                                                            <td id="non_exc_val" runat="server"><asp:TextBox ID="txtrt131" runat="server" MaxLength="7" Width="90px" ONKEYPRESS="return isDecimalKey(event)" onkeyup="cal()" Style="text-align: right;"></asp:TextBox></td>                                                                                                                          
                                                            <td class="font_css" id="sale_tax" runat="server">Sales_tax(%)<span style="font-size:small;color:blue;">(AA)</span></td>                                                            
                                                            <td id="sale_tax_Val" runat="server" ><asp:TextBox ID="txtrt132" runat="server" MaxLength="7" Width="90px" ONKEYPRESS="return isDecimalKey(event)" onkeyup="cal()" Style="text-align: right;"></asp:TextBox></td>                                                                                                                                                                             
                                                            <td></td>
                                                        </tr>

                                                        <tr id="saletaxrow_1" runat="server">
                                                            <td><span style="font-size:small;color:red;"> RMC(per 1000) : [ Weight(gms) * Indicative_SP ] </span></td>
                                                            <td></td>
                                                            <td></td>                                                           
                                                            <td></td>
                                                             <td class="font_css">Grand Total<span style="color:blue;">(AB)</span></td>
                                                            <td><asp:TextBox ID="txtrt133" runat="server" Width="90px" BackColor="LightGray" ReadOnly="True" Style="text-align: right;"></asp:TextBox></td>
                                                            <td class="font_css">RMC (per 1000)<span style="color:blue;">(AC)</span></td>
                                                            <td><asp:TextBox ID="txtrt41" runat="server" Width="90px" BackColor="LightGray" ReadOnly="True" Style="text-align: right;"></asp:TextBox></td>        
                                                            <td></td>             
                                                        </tr>


                                                        <tr id="Selling_price" runat="server">
                                                            <td colspan="2" style="color:red;">Formula for Selling Price <span style="font-size:small">[ (((((L*M)/1000000)*A)*H)+R+S+Q) ]</span></td>                                                            
                                                             
                                                            <td></td>
                                                            <td></td>                                            
                                                            <td class="font_css">Selling_Price(per 1000)<span style="color:blue;">(AD)</span></td>
                                                            <td> <asp:TextBox ID="txtrt47" runat="server" Width="90px" BackColor="LightGray" ReadOnly="True" Style="text-align: right;"></asp:TextBox>  </td>                                                                                                                         
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>

                                                        <tr>
                                                            <td colspan="9" style="color:red;">Formula for Grand Total <span style="font-size:small">[ (((((F+V) * (W/100)) * (Y/100)) + (F+V) + ((F+V) * (W/100)) + (((F+V) * (W/100)) * (X/100)) + Z) * (AA/100)) + (F+V) + ((F+V) * (W/100)) + (((F+V) * (W/100)) * (X/100)) + (((F+V) * (W/100)) * (Y/100)) ]</span></td>
                                                        </tr>

                                                        <tr>
                                                            <td></td>
                                                            <td> <asp:TextBox ID="txtrt46" runat="server" Width="90px" Style="display: none; text-align: right;"></asp:TextBox> </td>                                                                                                                          
                                                            <td> <asp:TextBox ID="txtrt39" runat="server" Width="90px" Style="display: none; text-align: right;"></asp:TextBox> </td>                                                          
                                                            <td>   <asp:TextBox ID="txtrt40" runat="server" Width="80px" Style="display: none; text-align: right;"></asp:TextBox> </td>                                                            
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane active" id="DescTab4">
                                    <div class="lbBody" style="height: 400px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <div class="col-md-12">
                                            <div>
                                                <div class="box-body">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td class="font_css">Label Size</td>
                                                            <td class="font_css">Length</td>
                                                            <td class="font_css">
                                                                <asp:TextBox ID="txtlt1" runat="server" Width="70px" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox>
                                                                &nbsp;width</td>
                                                            <td class="font_css">
                                                                <asp:TextBox ID="txtlt2" runat="server" Width="70px" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox>
                                                            </td>
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="font_css">Substrate</td>
                                                            <td></td>
                                                            <td>
                                                                <asp:TextBox ID="txtlt3" runat="server" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox></td>
                                                            <td></td>
                                                            <td class="font_css">RM Cost(Basic Rate)</td>
                                                            <td></td>
                                                            <td>
                                                                <asp:TextBox ID="txtlt4" runat="server" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="font_css">Reel Size</td>
                                                            <td></td>
                                                            <td>
                                                                <asp:TextBox ID="txtlt9" runat="server" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox></td>
                                                            <td></td>
                                                            <td class="font_css">Excise Duty</td>
                                                            <td></td>
                                                            <td>
                                                                <asp:TextBox ID="txtlt5" runat="server" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="font_css">Cylinder Circumference</td>
                                                            <td></td>
                                                            <td>
                                                                <asp:TextBox ID="txtlt10" runat="server" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox></td>
                                                            <td></td>
                                                            <td class="font_css">CST</td>
                                                            <td></td>
                                                            <td>
                                                                <asp:TextBox ID="txtlt6" runat="server" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="font_css">UPS</td>
                                                            <td></td>
                                                            <td>
                                                                <asp:TextBox ID="txtlt11" runat="server" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox></td>
                                                            <td></td>
                                                            <td class="font_css">Freight + Insurance</td>
                                                            <td></td>
                                                            <td>
                                                                <asp:TextBox ID="txtlt7" runat="server" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="font_css">Qty in Sq.Mtr</td>
                                                            <td></td>
                                                            <td>
                                                                <asp:TextBox ID="txtlt12" runat="server" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox></td>
                                                            <td></td>
                                                            <td class="font_css">Total</td>
                                                            <td></td>
                                                            <td>
                                                                <asp:TextBox ID="txtlt8" runat="server" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="font_css">Rate Per Kg</td>
                                                            <td></td>
                                                            <td>
                                                                <asp:TextBox ID="txtlt13" runat="server" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="font_css">Material Cost</td>
                                                            <td class="font_css">Substrate</td>
                                                            <td>
                                                                <asp:TextBox ID="txtlt14" runat="server" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td class="font_css">Ink</td>
                                                            <td>
                                                                <asp:TextBox ID="txtlt15" runat="server" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td class="font_css">Varnish</td>
                                                            <td>
                                                                <asp:TextBox ID="txtlt16" runat="server" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="font_css">Wastage%</td>
                                                            <td>
                                                                <asp:TextBox ID="txtlt17" runat="server" Width="70px" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox></td>
                                                            <td>
                                                                <asp:TextBox ID="txtlt18" runat="server" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="font_css">Printing Color + Varnish</td>
                                                            <td></td>
                                                            <td>
                                                                <asp:TextBox ID="txtlt19" runat="server" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox></td>
                                                            <td></td>
                                                            <td class="font_css">M/c Cost + Utility%</td>
                                                            <td>
                                                                <asp:TextBox ID="txtlt23" runat="server" Width="70px" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox></td>
                                                            <td>
                                                                <asp:TextBox ID="txtlt24" runat="server" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="font_css">Plate Cost</td>
                                                            <td></td>
                                                            <td>
                                                                <asp:TextBox ID="txtlt20" runat="server" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox></td>
                                                            <td></td>
                                                            <td class="font_css">Profit%</td>
                                                            <td>
                                                                <asp:TextBox ID="txtlt25" runat="server" Width="70px" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox></td>
                                                            <td>
                                                                <asp:TextBox ID="txtlt26" runat="server" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="font_css">Die Cost</td>
                                                            <td></td>
                                                            <td>
                                                                <asp:TextBox ID="txtlt21" runat="server" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox></td>
                                                            <td></td>
                                                            <td class="font_css">Grand Total</td>
                                                            <td></td>
                                                            <td>
                                                                <asp:TextBox ID="txtlt27" runat="server" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="font_css">Total Cost</td>
                                                            <td></td>
                                                            <td>
                                                                <asp:TextBox ID="txtlt22" runat="server" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox></td>
                                                            <td></td>
                                                            <td class="font_css">Packing%</td>
                                                            <td>
                                                                <asp:TextBox ID="txtlt28" runat="server" Width="70px" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox></td>
                                                            <td>
                                                                <asp:TextBox ID="txtlt29" runat="server" ONKEYPRESS="return isDecimalKey(event)"></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td class="font_css">Selling Price per Unit</td>
                                                            <td></td>
                                                            <td>
                                                                <asp:TextBox ID="txtlt30" runat="server"></asp:TextBox></td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane active" id="DescTab5" style="display: none;">
                                    <div class="lbBody" style="height: 400px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <div class="col-md-12">
                                            <div>
                                                <div class="box-body">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>
            </div>
        </section>
    </div>

    <asp:HiddenField ID="hffield" runat="server" />
    <asp:HiddenField ID="hfname" runat="server" />
    <asp:HiddenField ID="doc_nf" runat="server" />
    <asp:HiddenField ID="doc_df" runat="server" />
    <asp:HiddenField ID="doc_vty" runat="server" />
    <asp:HiddenField ID="doc_addl" runat="server" />
    <asp:HiddenField ID="edmode" runat="server" />
    <asp:HiddenField ID="hf1" runat="server" />
    <asp:HiddenField ID="hf2" runat="server" />
    <asp:HiddenField ID="hf4" runat="server" />
    <asp:Button ID="btnhideF" runat="server" OnClick="btnhideF_Click" Style="display: none" />
    <asp:Button ID="btnhideF_s" runat="server" OnClick="btnhideF_s_Click" Style="display: none" />


    <script type="text/javascript">

        function cal() {

            var ff12 = 0; var ff14 = 0; var ff17 = 0; var ff19 = 0; var ff21 = 0; var ff23 = 0; var ff25 = 0; var ff27 = 0; var ff29 = 0;
            var ff31 = 0; var ff34 = 0; var ff36 = 0; var ff38 = 0; var ff40 = 0; var ff42 = 0; var ff44 = 0; var ff49 = 0;
            var ff52 = 0; var ff54 = 0; var ff56 = 0; var ff57 = 0; var ff59 = 0; var ff60 = 0;
            var pet_thick = 0; var pet_thick1 = 0; var pet_spgr = 0; var pet_spgr1 = 0; var pet_gsm = 0; var pet_gsm1 = 0; var pet_kgs = 0; var pet_kgs1 = 0; var pet_sqm = 0; var pet_sqm1 = 0; var nat_thick = 0; var nat_spgr = 0; var nat_gsm = 0; var nat_kgs = 0; var nat_sqm = 0;
            var wop_thick = 0; var wop_spgr = 0; var Wop_gsm = 0; var Wop_kgs = 0; var Wop_sqm = 0; var bop_thick = 0; var bop_spgr = 0; var bop_gsm = 0; var bop_kgs = 0; var bop_sqm = 0;
            var bop_thick1 = 0; var bop_spgr1 = 0; var bop_gsm1 = 0; var bop_kgs1 = 0; var bop_sqm1 = 0; var cpp_thick = 0; var cpp_spgr = 0; var cpp_gsm = 0; var cpp_kgs = 0; var cpp_sqm = 0;
            var cpp_thick1 = 0; var cpp_spgr1 = 0; var cpp_gsm1 = 0; var cpp_kgs1 = 0; var cpp_sqm1 = 0; var foil_thick = 0; var foil_spgr = 0; var foil_gsm = 0; var foil_kgs = 0; var foil_sqm = 0;
            var shrnk_thick = 0; var shrnk_spgr = 0; var shrnk_gsm = 0; var shrnk_kgs = 0; var shrnk_sqm = 0;
            var shrnk_thick1 = 0; var shrnk_spgr1 = 0; var shrnk_gsm1 = 0; var shrnk_kgs1 = 0; var shrnk_sqm1 = 0;
            var pe_nat_thick = 0; var pet_nat_spgr = 0; var pe_nat_gsm = 0; var pe_nat_kgs = 0; var pe_nat_sqm = 0;
            var pe_wop_thick = 0; var pe_wop_spgr = 0; var pe_wop_gsm = 0; var pe_wop_kgs = 0; var pe_wop_sqm = 0;
            var nyl_Thick = 0; var nyl_spgr = 0; var nyl_gsm = 0; var nyl_kgs = 0; var nyl_sqm = 0;
            var pap_thick = 0; var pap_spgr = 0; var pap_gsm = 0; var pap_kgs = 0; var pap_sqm = 0;
            var pearl_thick = 0; var pearl_spgr = 0; var pearl_gsm = 0; var pearl_kgs = 0; var pearl_sqm = 0;
            var adh_thick = 0; var adh_spgr = 0; var adh_gsm = 0; var adh_kgs = 0; var adh_sqm = 0;
            var adh1_thick = 0; var adh1_spgr = 0; var adh1_gsm = 0; var adh1_kgs = 0; var adh1_sqm = 0; var tot3 = 0; var tot4 = 0; var tot5 = 0; var tot6 = 0; var tot7 = 0;
            var ink_thick = 0; var ink_spgr = 0; var ink_gsm = 0; var ink_kgs = 0; var ink_sqm = 0; var tot = 0; var tot1 = 0; var tot2 = 0; var tot8 = 0; var tot9 = 0;
            var tot10 = 0; var tot11 = 0; var tot12 = 0; var tot13 = 0; var tot14 = 0; var tot15 = 0; var tot16 = 0; var tot17 = 0; var tot18 = 0; var tot19 = 0; var tot20 = 0;
            var tot21 = 0; var tot22 = 0; var tot23 = 0; var tot24 = 0; var tot25 = 0; var tot26 = 0; var tot27 = 0; var tot28 = 0; var tot29 = 0; var tot30 = 0;
            var shrnk1_gsm = 0;

            //=====================
            document.getElementById('ContentPlaceHolder1_txtrt4').value = "1.4"; document.getElementById('ContentPlaceHolder1_txtrt10').value = "0.92";
            document.getElementById('ContentPlaceHolder1_txtrt16').value = "0.95"; document.getElementById('ContentPlaceHolder1_txtrt22').value = "0.905";
            document.getElementById('ContentPlaceHolder1_txtrt28').value = "0.905"; document.getElementById('ContentPlaceHolder1_txtrt49').value = "0.905";
            document.getElementById('ContentPlaceHolder1_txtrt54').value = "0.94"; document.getElementById('ContentPlaceHolder1_txtrt59').value = "2.67";
            document.getElementById('ContentPlaceHolder1_txtrt64').value = "1.35"; document.getElementById('ContentPlaceHolder1_txtrt69').value = "1.4";
            document.getElementById('ContentPlaceHolder1_txtrt74').value = "0.96"; document.getElementById('ContentPlaceHolder1_txtrt79').value = "0.98";
            document.getElementById('ContentPlaceHolder1_txtrt84').value = "1.14"; document.getElementById('ContentPlaceHolder1_txtrt89').value = "1";
            document.getElementById('ContentPlaceHolder1_txtrt94').value = "0.67"; document.getElementById('ContentPlaceHolder1_txtrt99').value = "1";
            document.getElementById('ContentPlaceHolder1_txtrt104').value = "1"; document.getElementById('ContentPlaceHolder1_txtrt109').value = "1";
            document.getElementById('ContentPlaceHolder1_txtrt134').value = "2";

            //================for PET (1ST ROW)
            pet_thick = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt3").value * 1);
            pet_spgr = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt4").value * 1);
            pet_gsm = (pet_thick * 1) * (pet_spgr * 1);
            document.getElementById('ContentPlaceHolder1_txtrt5').value = pet_gsm.toFixed(6);
            pet_kgs = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt6").value * 1);
            pet_sqm = ((pet_gsm * 1) * (pet_kgs * 1)) / 1000;
            document.getElementById('ContentPlaceHolder1_txtrt7').value = pet_sqm.toFixed(6);
            //==============for PET2 nd row
            pet_thick1 = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt3a").value * 1);
            pet_spgr1 = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt4a").value * 1);
            pet_gsm1 = (pet_thick1 * 1) * (pet_spgr1 * 1);
            document.getElementById('ContentPlaceHolder1_txtrt5a').value = (pet_gsm1 * 1).toFixed(6);
            pet_kgs1 = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt6a").value * 1);
            pet_sqm1 = ((pet_gsm1 * 1) * (pet_kgs1 * 1)) / 1000;
            document.getElementById('ContentPlaceHolder1_txtrt7a').value = (pet_sqm1 * 1).toFixed(6);
            ///=================for NAT (2ND ROW)                             
            nat_thick = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt9").value * 1);
            nat_spgr = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt10").value * 1);
            nat_gsm = nat_thick * nat_spgr;
            document.getElementById('ContentPlaceHolder1_txtrt11').value = nat_gsm.toFixed(6);
            nat_kgs = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt12").value * 1);
            nat_sqm = (nat_gsm * nat_kgs) / 1000;
            document.getElementById('ContentPlaceHolder1_txtrt13').value = nat_sqm.toFixed(6);
            ///PE WOP (MICRON)--------3RD ROW
            wop_thick = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt15").value * 1);
            wop_spgr = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt16").value * 1);
            Wop_gsm = wop_thick * wop_spgr;
            document.getElementById('ContentPlaceHolder1_txtrt17').value = Wop_gsm.toFixed(6);
            Wop_kgs = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt18").value * 1);
            Wop_sqm = (Wop_gsm * Wop_kgs) / 1000;
            document.getElementById('ContentPlaceHolder1_txtrt19').value = Wop_sqm.toFixed(6);
            ///BOPP (PLN, MET, HS) (micron)-----4th ROW               
            bop_thick = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt21").value * 1);
            bop_spgr = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt22").value);
            bop_gsm = bop_thick * bop_spgr;
            document.getElementById('ContentPlaceHolder1_txtrt23').value = bop_gsm.toFixed(6);
            bop_kgs = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt24").value * 1);
            bop_sqm = (bop_gsm * bop_kgs) / 1000;
            document.getElementById('ContentPlaceHolder1_txtrt25').value = bop_sqm.toFixed(6);
            //BOPP WOP (micron) for 5TH ROW          
            bop_thick1 = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt27").value * 1);
            bop_spgr1 = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt28").value * 1);
            bop_gsm1 = bop_thick1 * bop_spgr1;
            document.getElementById('ContentPlaceHolder1_txtrt29').value = bop_gsm1.toFixed(6);
            bop_kgs1 = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt30").value * 1);
            bop_sqm1 = (bop_gsm1 * bop_kgs1) / 1000;
            document.getElementById('ContentPlaceHolder1_txtrt31').value = bop_sqm1.toFixed(6);
            //====CPP (NAT, MET) (micron) for 6 TH ROW            
            cpp_thick = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt48").value * 1);
            cpp_spgr = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt49").value * 1);
            cpp_gsm = cpp_thick * cpp_spgr;
            document.getElementById('ContentPlaceHolder1_txtrt50').value = cpp_gsm.toFixed(6);
            cpp_kgs = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt51").value * 1);
            cpp_sqm = (cpp_gsm * cpp_kgs) / 1000;
            document.getElementById('ContentPlaceHolder1_txtrt52').value = cpp_sqm.toFixed(6);
            //CPP W OPQ (micron) for 7TH ROW              
            cpp_thick1 = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt53").value * 1);
            cpp_spgr1 = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt54").value * 1);
            cpp_gsm1 = cpp_thick1 * cpp_spgr1;
            document.getElementById('ContentPlaceHolder1_txtrt55').value = cpp_gsm1.toFixed(2);
            cpp_kgs1 = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt56").value * 1);
            cpp_sqm1 = (cpp_gsm1 * cpp_kgs1) / 1000;
            document.getElementById('ContentPlaceHolder1_txtrt57').value = cpp_sqm1.toFixed(6);
            //FOIL (micron) FOR 8TH ROW            
            foil_thick = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt58").value * 1);
            foil_spgr = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt59").value * 1);
            foil_gsm = foil_thick * foil_spgr;
            document.getElementById('ContentPlaceHolder1_txtrt60').value = foil_gsm.toFixed(6);
            foil_kgs = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt61").value * 1);
            foil_sqm = (foil_gsm * foil_kgs) / 1000;
            document.getElementById('ContentPlaceHolder1_txtrt62').value = foil_sqm.toFixed(6);
            //Shrink PVC (micron) FOR 9TH ROW              
            shrnk_thick = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt63").value * 1);
            shrnk_spgr = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt64").value * 1);
            shrnk_gsm = shrnk_thick * shrnk_spgr;
            document.getElementById('ContentPlaceHolder1_txtrt65').value = shrnk_gsm.toFixed(6);
            shrnk_kgs = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt66").value * 1);
            shrnk_sqm = (shrnk_gsm * shrnk_kgs) / 1000;
            document.getElementById('ContentPlaceHolder1_txtrt67').value = shrnk_sqm.toFixed(6);
            //Shrink PET (micron) for 10th ROW              
            shrnk_thick1 = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt68").value);
            shrnk_spgr1 = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt69").value * 1);
            shrnk_gsm1 = shrnk_thick1 * shrnk_spgr1;
            document.getElementById('ContentPlaceHolder1_txtrt70').value = shrnk_gsm1.toFixed(6);
            shrnk_kgs1 = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt71").value * 1);
            shrnk_sqm1 = (shrnk_gsm1 * shrnk_kgs1) / 1000;
            document.getElementById('ContentPlaceHolder1_txtrt72').value = shrnk_sqm1.toFixed(6);
            ///   PE NAT Nylon (micron) for 11 th row  
            pe_nat_thick = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt73").value * 1);
            pet_nat_spgr = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt74").value * 1);
            pe_nat_gsm = pe_nat_thick * pet_nat_spgr;
            document.getElementById('ContentPlaceHolder1_txtrt75').value = pe_nat_gsm.toFixed(6);
            pe_nat_kgs = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt76").value * 1);
            pe_nat_sqm = (pe_nat_gsm * pe_nat_kgs) / 1000;
            document.getElementById('ContentPlaceHolder1_txtrt77').value = pe_nat_sqm.toFixed(6);
            //PE WOP Nylon (micron) for 12 th row
            pe_wop_thick = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt78").value * 1);
            pe_wop_spgr = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt79").value * 1);
            pe_wop_gsm = pe_wop_thick * pe_wop_spgr;
            document.getElementById('ContentPlaceHolder1_txtrt80').value = pe_wop_gsm.toFixed(6);
            pe_wop_kgs = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt81").value * 1);
            pe_wop_sqm = (pe_wop_gsm * pe_wop_kgs) / 1000;
            document.getElementById('ContentPlaceHolder1_txtrt82').value = pe_wop_sqm.toFixed(6);
            //NYLON (micron) for 13 th row
            nyl_Thick = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt83").value * 1);
            nyl_spgr = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt84").value * 1);
            nyl_gsm = nyl_Thick * nyl_spgr;
            document.getElementById('ContentPlaceHolder1_txtrt85').value = nyl_gsm.toFixed(6);
            nyl_kgs = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt86").value * 1);
            nyl_sqm = (nyl_gsm * nyl_kgs) / 1000;
            document.getElementById('ContentPlaceHolder1_txtrt87').value = nyl_sqm.toFixed(6);
            //====PAPER (micron) for 14 th ROW
            pap_thick = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt88").value * 1);
            pap_spgr = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt89").value * 1);
            pap_gsm = pap_thick * pap_spgr;
            document.getElementById('ContentPlaceHolder1_txtrt90').value = pap_gsm.toFixed(6);
            pap_kgs = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt91").value * 1);
            pap_sqm = (pap_gsm * pap_kgs) / 1000;
            document.getElementById('ContentPlaceHolder1_txtrt92').value = pap_sqm.toFixed(6);
            //=======PEARL BOPP (micron) FOR 15 TH ROW        
            pearl_thick = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt93").value * 1);
            pearl_spgr = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt94").value * 1);
            pearl_gsm = pearl_thick * pearl_spgr;
            document.getElementById('ContentPlaceHolder1_txtrt95').value = pearl_gsm.toFixed(6);
            pearl_kgs = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt96").value * 1);
            pearl_sqm = (pearl_gsm * pearl_kgs) / 1000;
            document.getElementById('ContentPlaceHolder1_txtrt97').value = pearl_sqm.toFixed(6);
            //=======ADH SOLVENTLESS (gsm) FOR 16 TH ROW
            adh_thick = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt98").value * 1);
            adh_spgr = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt99").value * 1);
            adh_gsm = adh_thick * adh_spgr;
            document.getElementById('ContentPlaceHolder1_txtrt100').value = adh_gsm.toFixed(6);
            adh_kgs = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt101").value * 1);
            adh_sqm = (adh_gsm * adh_kgs) / 1000;
            document.getElementById('ContentPlaceHolder1_txtrt102').value = adh_sqm.toFixed(6);
            //========ADH SOLVENT BASE (gsm) for 17th ROW
            adh1_thick = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt103").value * 1);
            adh1_spgr = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt104").value * 1);
            adh1_gsm = adh1_thick * adh1_spgr;
            document.getElementById('ContentPlaceHolder1_txtrt105').value = adh1_gsm.toFixed(6);
            adh1_kgs = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt106").value * 1);
            adh1_sqm = (adh1_gsm * adh1_kgs) / 1000;
            document.getElementById('ContentPlaceHolder1_txtrt107').value = adh1_sqm.toFixed(6);
            //====================Ink (gsm) for 18th ROW
            ink_thick = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt108").value * 1);
            ink_spgr = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt109").value * 1);
            ink_gsm = ink_thick * ink_spgr;
            document.getElementById('ContentPlaceHolder1_txtrt110').value = ink_gsm.toFixed(6);
            ink_kgs = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt111").value * 1);
            ink_sqm = (ink_gsm * ink_kgs) / 1000;
            document.getElementById('ContentPlaceHolder1_txtrt112').value = ink_sqm.toFixed(6);
            //main tot              
            tot = ((pet_gsm * 1) + (pet_gsm1 * 1) + (nat_gsm * 1) + (Wop_gsm * 1) + (bop_gsm * 1) + (bop_gsm1 * 1) + (cpp_gsm * 1) + (cpp_gsm1 * 1) + (foil_gsm * 1) + (shrnk_gsm * 1) + (shrnk_gsm1 * 1) + (pe_nat_gsm * 1) + (pe_wop_gsm * 1) + (nyl_gsm * 1) + (pap_gsm * 1) + (pearl_gsm * 1) + (adh_gsm * 1) + (adh1_gsm * 1) + (ink_gsm * 1)).toFixed(6);
            tot1 = ((pet_sqm * 1) + (pet_sqm1 * 1) + (nat_sqm * 1) + (Wop_sqm * 1) + (bop_sqm * 1) + (bop_sqm1 * 1) + (cpp_sqm * 1) + (cpp_sqm1 * 1) + (foil_sqm * 1) + (shrnk_sqm * 1) + (shrnk_sqm1 * 1) + (pe_nat_sqm * 1) + (pe_wop_sqm * 1) + (nyl_sqm * 1) + (pap_sqm * 1) + (pearl_sqm * 1) + (adh_sqm * 1) + (adh1_sqm * 1) + (ink_sqm * 1)).toFixed(6);
            document.getElementById('ContentPlaceHolder1_txtrt113').value = (tot * 1).toFixed(6);
            document.getElementById('ContentPlaceHolder1_txtrt114').value = (tot1 * 1).toFixed(6);
            tot2 = ((tot1 * 1) / (tot * 1) * 1000).toFixed(6);
            document.getElementById('ContentPlaceHolder1_txtrt33').value = (tot2 * 1).toFixed(6);
            //==============
            tot3 = (((tot2 * 1) * (1 + fill_zero(document.getElementById("ContentPlaceHolder1_txtrt115").value * 1) / 100) + fill_zero(document.getElementById("ContentPlaceHolder1_txtrt116").value * 1))).toFixed(6);
            document.getElementById('ContentPlaceHolder1_txtrt118').value = (tot3 * 1).toFixed(6);
            //==============       
            tot4 = (((tot3 * 1) * (tot * 1)) / 1000).toFixed(6);
            document.getElementById('ContentPlaceHolder1_txtrt119').value = (tot4 * 1).toFixed(6);
            //==============
            tot5 = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt117").value * 1);
            document.getElementById('ContentPlaceHolder1_txtrt35').value = ((tot3 * 1) + ((tot3 * 1) * ((tot5 * 1) / 100))).toFixed(6);
            //alert(document.getElementById('ContentPlaceHolder1_txtrt35').value);
            //==============
            tot6 = ((tot4 * 1)) / (1 - ((tot5 * 1) / 100)).toFixed(6);
            document.getElementById('ContentPlaceHolder1_txtrt36').value = (tot6 * 1).toFixed(6);
            //==============
            tot7 = ((tot6 * 1) - (tot4 * 1)).toFixed(6);
            document.getElementById('ContentPlaceHolder1_txtrt38').value = (tot7 * 1).toFixed(6);
            //=============DGDFGFDGFDGFG
            //txtrt37.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtrt35.Text.Trim()) - Convert.ToDouble(txtrt118.Text.Trim()), 6)); //FORMULA ON WFINSERP CODE           
            //tot8 = ((tot3 * 1) + ((tot3 * 1) * ((tot5 * 1) / 100))).toFixed(6);       
            // document.getElementById('ContentPlaceHolder1_txtrt37').value = ((tot8 * 1) + (tot3 * 1)).toFixed(6);
            document.getElementById('ContentPlaceHolder1_txtrt37').value = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt35").value * 1) - fill_zero(document.getElementById("ContentPlaceHolder1_txtrt118").value * 1).toFixed(6);
            //==============          
            tot9 = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt120").value * 1);
            tot10 = fill_zero(document.getElementById("ContentPlaceHolder1_txtrt121").value * 1);
            document.getElementById('ContentPlaceHolder1_txtrt123').value = (((tot9 * 1) * (tot10 * 1)) / 1000000).toFixed(6);
            //==============    
            tot11 = (((tot9 * 1) * (tot10 * 1)) / 1000000) * fill_zero(document.getElementById("ContentPlaceHolder1_txtrt113").value * 1).toFixed(6);
            document.getElementById('ContentPlaceHolder1_txtrt124').value = (tot11 * 1).toFixed(6);
            //==============              
            tot12 = ((tot11 * 1) * fill_zero(document.getElementById("ContentPlaceHolder1_txtrt35").value * 1));
            document.getElementById('ContentPlaceHolder1_txtrt41').value = (tot12 * 1).toFixed(6);
            //==============
            // txtrt43.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtrt41.Text.Trim()) + Convert.ToDouble(txtrt42.Text.Trim())) * (Convert.ToDouble(txtrt134.Text.Trim()) / 100), 6)).Replace("Infinity", "0").Replace("NaN", "0"); 
            tot13 = (fill_zero(document.getElementById("ContentPlaceHolder1_txtrt41").value * 1) + fill_zero(document.getElementById("ContentPlaceHolder1_txtrt42").value * 1)) * (fill_zero(document.getElementById("ContentPlaceHolder1_txtrt134").value * 1) / 100);
            document.getElementById('ContentPlaceHolder1_txtrt43').value = (tot13 * 1).toFixed(6);
            //alert("txtrt 41  " + fill_zero(document.getElementById("ContentPlaceHolder1_txtrt41").value * 1));           
            //====================
            tot14 = ((tot12 * 1) + fill_zero(document.getElementById("ContentPlaceHolder1_txtrt42").value * 1) + fill_zero(document.getElementById("ContentPlaceHolder1_txtrt43").value * 1) + fill_zero(document.getElementById("ContentPlaceHolder1_txtrt44").value * 1));
            document.getElementById('ContentPlaceHolder1_txtrt47').value = (tot14 * 1).toFixed(6);
            //====================
            document.getElementById('ContentPlaceHolder1_txtrt126').value = (1 / (tot11 * 1)).toFixed(6);
            document.getElementById('ContentPlaceHolder1_txtrt125').value = (1000 / (tot11 * 1)).toFixed(6);
            //====================
            var rmc_exc = 0, exc = 0, netexc = 0, allexc = 0, saletx = 0;
            rmc_exc = fill_zero((tot3 * 1) + (document.getElementById("ContentPlaceHolder1_txtrt127").value * 1));
            exc = (rmc_exc * 1) * (fill_zero(document.getElementById("ContentPlaceHolder1_txtrt128").value * 1) / 100);
            netexc = (exc * 1) * (fill_zero(document.getElementById("ContentPlaceHolder1_txtrt129").value * 1) / 100);
            allexc = (exc * 1) * (fill_zero(document.getElementById("ContentPlaceHolder1_txtrt130").value * 1) / 100);
            saletx = ((allexc * 1) + (rmc_exc * 1) + (exc * 1) + (netexc * 1) + (fill_zero(document.getElementById("ContentPlaceHolder1_txtrt131").value * 1))) * (fill_zero(document.getElementById("ContentPlaceHolder1_txtrt132").value * 1) / 100);
            document.getElementById('ContentPlaceHolder1_txtrt133').value = ((saletx * 1) + (rmc_exc * 1) + (exc * 1) + (netexc * 1) + (allexc * 1)).toFixed(6);
            /////////////////===================================
            ff12 = fill_zero(document.getElementById("ContentPlaceHolder1_txtff9").value * 1);
            tot15 = fill_zero(document.getElementById("ContentPlaceHolder1_txtff10").value * 1);
            tot16 = fill_zero(document.getElementById("ContentPlaceHolder1_txtff8").value * 1);
            tot17 = (((ff12 * 1) * (tot15 * 1) * (tot16 * 1)) / 10000000) * (fill_zero(document.getElementById("ContentPlaceHolder1_txtprt").value * 1) * fill_zero(document.getElementById("ContentPlaceHolder1_txtff7").value * 1)) / 1;
            document.getElementById('ContentPlaceHolder1_txtff12').value = (tot17 * 1).toFixed(6);
            ////========================     
            ff14 = (fill_zero(document.getElementById("ContentPlaceHolder1_txtff13").value * 1) * (tot17 * 1));
            document.getElementById('ContentPlaceHolder1_txtff14').value = (ff14 * 1).toFixed(6);
            ////========================     
            ff17 = (fill_zero(document.getElementById("ContentPlaceHolder1_txtff6").value * 1) * fill_zero(document.getElementById("ContentPlaceHolder1_txtff16").value * 1));
            document.getElementById('ContentPlaceHolder1_txtff17').value = (ff17 * 1).toFixed(6);
            ///========================           
            ff19 = (fill_zero(document.getElementById("ContentPlaceHolder1_txtff6").value * 1) * fill_zero(document.getElementById("ContentPlaceHolder1_txtff18").value * 1)) * (fill_zero(document.getElementById("ContentPlaceHolder1_txtprt").value * 1) / fill_zero(document.getElementById("ContentPlaceHolder1_txtff7").value * 1) / 1000);
            document.getElementById('ContentPlaceHolder1_txtff19').value = (ff19 * 1).toFixed(6);
            ////==========================         
            ff21 = (fill_zero(document.getElementById("ContentPlaceHolder1_txtff20").value * 1) / fill_zero(document.getElementById("ContentPlaceHolder1_txtannualqty").value * 1)) / 1000;
            document.getElementById('ContentPlaceHolder1_txtff21').value = (ff21 * 1).toFixed(6);
            ////==========================        
            ff23 = (((fill_zero(document.getElementById("ContentPlaceHolder1_txtff22").value * 1)) / 100) * (fill_zero(document.getElementById("ContentPlaceHolder1_txtff9").value * 1) / 2.54 * fill_zero(document.getElementById("ContentPlaceHolder1_txtff10").value * 1) / 2.54)) / fill_zero(document.getElementById("ContentPlaceHolder1_txtff7").value * 1) * 1000;
            document.getElementById('ContentPlaceHolder1_txtff23').value = (ff23 * 1).toFixed(6);
            ////==========================           
            ff25 = (fill_zero(document.getElementById("ContentPlaceHolder1_txtff24").value * 1) / fill_zero(document.getElementById("ContentPlaceHolder1_txtannualqty").value * 1)) / 1000;
            document.getElementById('ContentPlaceHolder1_txtff25').value = (ff25 * 1).toFixed(6);
            //============================           
            ff27 = ((fill_zero(document.getElementById("ContentPlaceHolder1_txtff26").value * 1) / 100) * fill_zero(document.getElementById("ContentPlaceHolder1_txtff9").value * 1) / 2.54 * fill_zero(document.getElementById("ContentPlaceHolder1_txtff10").value * 1) / 2.54) / fill_zero(document.getElementById("ContentPlaceHolder1_txtff7").value * 1) * 1000;
            document.getElementById('ContentPlaceHolder1_txtff27').value = (ff27 * 1).toFixed(6);
            //============================          
            ff29 = (fill_zero(document.getElementById("ContentPlaceHolder1_txtff28").value * 1) / fill_zero(document.getElementById("ContentPlaceHolder1_txtannualqty").value * 1)) / 1000;
            document.getElementById('ContentPlaceHolder1_txtff29').value = (ff29 * 1).toFixed(6);
            //============================         
            ff31 = ((fill_zero(document.getElementById("ContentPlaceHolder1_txtff30").value * 1) / 100) * fill_zero(document.getElementById("ContentPlaceHolder1_txtff9").value * 1) / 2.54 * fill_zero(document.getElementById("ContentPlaceHolder1_txtff10").value * 1) / 2.54) / fill_zero(document.getElementById("ContentPlaceHolder1_txtff7").value * 1) * 1000;
            document.getElementById('ContentPlaceHolder1_txtff31').value = (ff31 * 1).toFixed(6);
            //============================            
            ff34 = fill_zero(document.getElementById("ContentPlaceHolder1_txtff32").value * 1) * fill_zero(document.getElementById("ContentPlaceHolder1_txtff33").value * 1) * 1000;
            document.getElementById('ContentPlaceHolder1_txtff34').value = (ff34 * 1).toFixed(6);
            //=============================       
            ff36 = (fill_zero(document.getElementById("ContentPlaceHolder1_txtff35").value * 1) / fill_zero(document.getElementById("ContentPlaceHolder1_txtannualqty").value * 1)) / 1000;
            document.getElementById('ContentPlaceHolder1_txtff36').value = (ff36 * 1).toFixed(6);
            //=============================         
            ff38 = (fill_zero(document.getElementById("ContentPlaceHolder1_txtff37").value * 1) / fill_zero(document.getElementById("ContentPlaceHolder1_txtannualqty").value * 1)) / 1000;
            document.getElementById('ContentPlaceHolder1_txtff38').value = (ff38 * 1).toFixed(6);
            //===========================           
            ff40 = fill_zero(document.getElementById("ContentPlaceHolder1_txtff39").value * 1) / fill_zero(document.getElementById("ContentPlaceHolder1_txtff7").value * 1);
            document.getElementById('ContentPlaceHolder1_txtff40').value = (ff40 * 1).toFixed(6);
            //============================          
            ff42 = (fill_zero(document.getElementById("ContentPlaceHolder1_txtff41").value * 1) / fill_zero(document.getElementById("ContentPlaceHolder1_txtannualqty").value * 1)) / 1000;
            document.getElementById('ContentPlaceHolder1_txtff42').value = (ff42 * 1).toFixed(6);
            //============================           
            ff44 = fill_zero(document.getElementById("ContentPlaceHolder1_txtff43").value * 1) / fill_zero(document.getElementById("ContentPlaceHolder1_txtff7").value * 1);
            document.getElementById('ContentPlaceHolder1_txtff44').value = (ff44 * 1).toFixed(6);
            //============================          
            ff49 = (((fill_zero(document.getElementById("ContentPlaceHolder1_txtff45").value * 1) / 10000) * (fill_zero(document.getElementById("ContentPlaceHolder1_txtff46").value * 1) * 1.4)) * fill_zero(document.getElementById("ContentPlaceHolder1_txtff47").value * 1)) + fill_zero(document.getElementById("ContentPlaceHolder1_txtff48").value * 1);
            document.getElementById('ContentPlaceHolder1_txtff49').value = (ff49 * 1).toFixed(6);
            //===========================          
            ff52 = ((((fill_zero(document.getElementById("ContentPlaceHolder1_txtff9").value * 1) * fill_zero(document.getElementById("ContentPlaceHolder1_txtff10").value * 1) / 10000)) * (fill_zero(document.getElementById("ContentPlaceHolder1_txtff51").value * 1) * 1.3 + fill_zero(document.getElementById("ContentPlaceHolder1_txtff50").value * 1))) / fill_zero(document.getElementById("ContentPlaceHolder1_txtff7").value * 1) * 1.03);
            document.getElementById('ContentPlaceHolder1_txtff52').value = (ff52 * 1).toFixed(6);
            //===========================         
            ff54 = fill_zero(document.getElementById("ContentPlaceHolder1_txtff52").value * 1) * fill_zero(document.getElementById("ContentPlaceHolder1_txtff53").value * 1);
            document.getElementById('ContentPlaceHolder1_txtff54').value = (ff54 * 1).toFixed(6);
            //=================================          
            ff56 = (fill_zero(document.getElementById("ContentPlaceHolder1_txtff52").value * 1) + fill_zero(document.getElementById("ContentPlaceHolder1_txtff12").value * 1) + fill_zero(document.getElementById("ContentPlaceHolder1_txtff55").value * 1)) * 2;
            document.getElementById('ContentPlaceHolder1_txtff56').value = (ff56 * 1).toFixed(6);
            //=================================
            ff57 = ((ff14 * 1) + (ff17 * 1) + (ff19 * 1) + (ff21 * 1) + (ff23 * 1) + (ff25 * 1) + (ff27 * 1) + (ff29 * 1) + (ff31 * 1) + (ff34 * 1) + (ff36 * 1) + (ff38 * 1) + (ff40 * 1) + (ff42 * 1) + (ff44 * 1) + (ff49 * 1) + (ff54 * 1) + (ff56 * 1));
            document.getElementById('ContentPlaceHolder1_txtff57').value = (ff57 * 1).toFixed(6);
            //=================================          
            ff59 = (ff57 * 1) * ((100 + fill_zero(document.getElementById("ContentPlaceHolder1_txtff58").value * 1)) / 100);
            document.getElementById('ContentPlaceHolder1_txtff59').value = (ff59 * 1).toFixed(6);
            //=================================           
            ff60 = ((ff59 * 1) / 1000).toFixed(8);
            document.getElementById('ContentPlaceHolder1_txtff60').value = (ff60 * 1).toFixed(6);

        }
        function fill_zero(val) { if (isNaN(val)) return 0; if (isFinite(val)) return val; }
    </script>

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
