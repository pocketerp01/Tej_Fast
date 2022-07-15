<%@ Page Title="" Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="cost_infi_t" CodeFile="cost_infi_t.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../tej-base/Scripts/gridviewScroll.min.js" type="text/javascript"></script>
    <script type="text/javascript">
       
    </script>

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
                        <button type="submit" id="btnCal" class="btn btn-info" style="width: 100px;" runat="server" accesskey="A" onserverclick="btnCal_ServerClick">C<u>a</u>lculate</button>
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
                        <div class="box-body">
                            <div class="form-group">
                                <label id="Label19" runat="server" class="col-sm-3 control-label">Job No.</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtvchnum" runat="server" Width="100%" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-sm-3">
                                    <asp:Label ID="lbl1a" runat="server" Text="TC" Style="width: 22px; float: right;" CssClass="col-sm-2 control-label" Visible="false"></asp:Label>
                                    <label id="Label42" runat="server">Job Dt.</label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtvchdate" placeholder="Date" runat="server" Width="100%" CssClass="form-control" Font-Size="Small"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtvchdate_CalendarExtender" runat="server"
                                        Enabled="True" TargetControlID="txtvchdate"
                                        Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="MaskedEditExtender2" runat="server" Mask="99/99/9999"
                                        MaskType="Date" TargetControlID="txtvchdate" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label11" runat="server" class="col-sm-3 control-label">Board</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtpt1" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right; background-color: #66FFCC" MaxLength="8" onkeyup="cal()"></asp:TextBox>
                                </div>
                                <label id="Label21" runat="server" class="col-sm-3 control-label">Size</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtpt2" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right; background-color: #66FFCC" MaxLength="8" onkeyup="cal()"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label2" runat="server" class="col-sm-3 control-label">Rate</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtpt5" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right; background-color: #66FFCC" ReadOnly="true"></asp:TextBox>
                                </div>
                                <label id="Label25" runat="server" class="col-sm-3 control-label">GSM</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtpt6" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right; background-color: #66FFCC" MaxLength="8" onkeyup="cal()"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label37" runat="server" class="col-sm-3 control-label">Quantity</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtpt8" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right; background-color: #66FFCC" MaxLength="8" onkeyup="cal()"></asp:TextBox>
                                </div>
                                <label id="Label38" runat="server" class="col-sm-3 control-label">FLAT UV</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtpt11" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right; background-color: #66FFCC" MaxLength="8" onkeyup="cal()"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label39" runat="server" class="col-sm-3 control-label">Pasting</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtpt14" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right; background-color: #66FFCC" MaxLength="8" onkeyup="cal()"></asp:TextBox>
                                </div>
                                <label id="Label40" runat="server" class="col-sm-3 control-label"><u>1</u> for Lock Bottom </label>
                                <div class="col-sm-3">
                                    <label id="Label45" runat="server" style="height: 30px"><u>0</u> for Side Pasting </label>
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label43" runat="server" class="col-sm-3 control-label">Rejection%</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtpt15" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right; background-color: #66FFCC" MaxLength="8" onkeyup="cal()"></asp:TextBox>
                                </div>
                                <label id="Label44" runat="server" class="col-sm-3 control-label">Hybrid UV</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtpt104" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right; background-color: #66FFCC" MaxLength="8" onkeyup="cal()"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label92" runat="server" class="col-sm-3 control-label">No. Of Sheet</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtpt17" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label93" runat="server" class="col-sm-3 control-label" style="border-style: groove; border-width: medium; height: 41px">Total D/Box</label>
                                <div class="col-sm-3" style="border-style: groove; border-width: medium;">
                                    <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <label id="Label13" runat="server" class="col-sm-2 control-label">Item Name</label>
                                <div class="col-sm-1" id="div1" runat="server">
                                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="ImageButton1_Click" />
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txticode" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="true"></asp:TextBox>
                                </div>

                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtiname" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtpt3" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right; background-color: #66FFCC" MaxLength="8" onkeyup="cal()"></asp:TextBox>
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtpt4" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="true"></asp:TextBox>
                                </div>
                                <label id="Label41" runat="server" class="col-sm-3 control-label">UPS</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtpt7" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right; background-color: #66FFCC" MaxLength="8" onkeyup="cal()"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label17" runat="server" class="col-sm-3 control-label">Color</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtpt9" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right; background-color: #66FFCC" MaxLength="8" onkeyup="cal()"></asp:TextBox>
                                </div>
                                <label id="Label46" runat="server" class="col-sm-3 control-label">Varnish</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtpt10" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right; background-color: #66FFCC" MaxLength="8" onkeyup="cal()"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label47" runat="server" class="col-sm-3 control-label">Spot UV</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtpt12" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right; background-color: #66FFCC" MaxLength="8" onkeyup="cal()"></asp:TextBox>
                                </div>
                                <label id="Label48" runat="server" class="col-sm-3 control-label">Lamination</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtpt13" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right; background-color: #66FFCC" MaxLength="8" onkeyup="cal()"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label49" runat="server" class="col-sm-3 control-label">Min Printing</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtpt16" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="true"></asp:TextBox>
                                </div>
                                <label id="Label50" runat="server" class="col-sm-3 control-label">Reg Printing</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtpt18" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label51" runat="server" class="col-sm-3 control-label">Embossing</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtpt105" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right; background-color: #66FFCC" MaxLength="8" onkeyup="cal()"></asp:TextBox>
                                </div>
                                <label id="Label52" runat="server" class="col-sm-3 control-label"><u>1</u> for Require </label>
                                <div class="col-sm-3">
                                    <label id="Label53" runat="server" style="height: 30px"><u>0</u> for not Require </label>
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label94" runat="server" class="col-sm-3 control-label" style="border-style: groove; border-width: medium; height: 41px">Total C/Box</label>
                                <div class="col-sm-3" style="border-style: groove; border-width: medium;">
                                    <asp:TextBox ID="TextBox11" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="true"></asp:TextBox>
                                </div>
                                <label id="Label95" runat="server" class="col-sm-3 control-label" style="border-style: groove; border-width: medium; height: 41px">Final Rate</label>
                                <div class="col-sm-3" style="border-style: groove; border-width: medium;">
                                    <asp:TextBox ID="TextBox12" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <section class="col-lg-12 connectedSortable" id="AllTabs" runat="server">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">
                                <li><a href="#DescTab1" id="tab1" runat="server" aria-controls="DescTab1" role="tab" data-toggle="tab">Duplex Box</a></li>
                                <li><a href="#DescTab2" id="tab2" runat="server" aria-controls="DescTab2" role="tab" data-toggle="tab">Corrugated Box</a></li>
                            </ul>

                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane active" id="DescTab1">
                                    <div class="lbBody" style="height: 310px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <label id="Label64" runat="server" class="col-sm-4 control-label">Board in KG</label>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtpt19" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtpt20" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label65" runat="server" class="col-sm-4 control-label">Plate</label>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtpt21" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" MaxLength="8" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtpt22" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label66" runat="server" class="col-sm-4 control-label">MIN Printing</label>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtpt23" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtpt24" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label67" runat="server" class="col-sm-4 control-label">REG Printing</label>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtpt25" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtpt26" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label68" runat="server" class="col-sm-4 control-label">Lamination</label>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtpt27" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtpt28" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label82" runat="server" class="col-sm-4 control-label">Varnish</label>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtpt29" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtpt30" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label83" runat="server" class="col-sm-4 control-label">Flat UV Make Ready</label>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtpt31" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtpt32" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Labelchk1" runat="server" class="col-sm-4 control-label">Flat UV</label>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtpt33" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtpt34" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label69" runat="server" class="col-sm-4 control-label">Spot UV Make Ready</label>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtpt35" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtpt36" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <label id="Label70" runat="server" class="col-sm-4 control-label">Spot UV</label>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtpt37" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtpt38" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label1" runat="server" class="col-sm-4 control-label">Hybrid UV</label>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtpt106" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtpt109" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label71" runat="server" class="col-sm-4 control-label">Die Cost</label>
                                                        <div class="col-sm-4">
                                                        </div>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtpt103" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" MaxLength="8" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label72" runat="server" class="col-sm-4 control-label">Die Make Ready</label>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtpt39" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtpt40" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label73" runat="server" class="col-sm-4 control-label">Die Cutting</label>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtpt41" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtpt42" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label81" runat="server" class="col-sm-4 control-label">Die Notching</label>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtpt43" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtpt44" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label88" runat="server" class="col-sm-4 control-label">Embossing</label>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtpt107" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtpt108" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label4" runat="server" class="col-sm-4 control-label">Pasting</label>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtpt45" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtpt46" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label96" runat="server" class="col-sm-4 control-label">Sorting</label>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtpt47" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtpt48" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <label id="Label76" runat="server" class="col-sm-4 control-label">Add Margin%</label>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtpt50" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right; background-color: #66FFCC" MaxLength="8" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtpt51" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label77" runat="server" class="col-sm-4 control-label">Cost Per Carton</label>
                                                        <div class="col-sm-8">
                                                            <asp:TextBox ID="txtpt53" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label78" runat="server" class="col-sm-4 control-label">Freight</label>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtpt54" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right; background-color: #66FFCC" MaxLength="8" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-4">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <label id="Label84" runat="server" class="col-sm-4 control-label">Total Cost </label>
                                                        <div class="col-sm-8">
                                                            <asp:TextBox ID="txtpt49" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label85" runat="server" class="col-sm-4 control-label">Total Cost Incl. Margin</label>
                                                        <div class="col-sm-8">
                                                            <asp:TextBox ID="txtpt52" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label86" runat="server" class="col-sm-4 control-label">Freight Per Unit</label>
                                                        <div class="col-sm-8">
                                                            <asp:TextBox ID="txtpt55" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="true"></asp:TextBox>
                                                            <asp:TextBox ID="txtpt56" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right; display: none" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label87" runat="server" class="col-sm-4 control-label">Final Rate</label>
                                                        <div class="col-sm-8">
                                                            <asp:TextBox ID="txtpt57" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="True"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane active" id="DescTab2">
                                    <div class="lbBody" style="height: 310px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <label id="Label6" runat="server" class="col-sm-3 control-label"></label>
                                                        <div class="col-sm-3">
                                                            <label id="Label7" runat="server">Length</label>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <label id="Label8" runat="server">Width</label>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <label id="Label9" runat="server">Height</label>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label3" runat="server" class="col-sm-3 control-label">Plate</label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtpt58" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right; background-color: #66FFCC" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtpt59" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right; background-color: #66FFCC" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtpt60" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right; background-color: #66FFCC" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label74" runat="server" class="col-sm-3 control-label">Size with Shrinkage</label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtpt61" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="True"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtpt62" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="True"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtpt63" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="True"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label75" runat="server" class="col-sm-3 control-label">Sheet_Size_with_Triming</label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtpt64" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="True"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtpt65" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="True"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-3">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <label id="Label10" runat="server" class="col-sm-3 control-label"></label>
                                                        <div class="col-sm-3">
                                                            <label id="Label12" runat="server">Length</label>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <label id="Label14" runat="server">Width</label>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <label id="Label15" runat="server">UPS</label>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label79" runat="server" class="col-sm-3 control-label">Sheet Size(INCHS)</label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtpt66" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="True"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtpt67" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="True"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <label id="Label90" runat="server" style="height: 30px"></label>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label80" runat="server" class="col-sm-3 control-label">Actual Sheet Size</label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="TextBox2" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="True"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="TextBox3" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="True"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="TextBox4" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="True"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label89" runat="server" class="col-sm-3 control-label">Area of Sheet</label>
                                                        <div class="col-sm-3">
                                                        </div>
                                                        <div class="col-sm-3">
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtpt68" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="True"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-12">
                                            <div>
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <label id="Label18" runat="server" class="col-md-12 control-label" title="lbl1" style="text-align: center; text-decoration: underline; font-style: bold; font-size: medium;">Paper Details</label>
                                                    </div>

                                                    <div class="form-group">
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <label id="Label22" runat="server">BF</label>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <label id="Label23" runat="server">BS</label>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <label id="Label24" runat="server">GSM</label>
                                                        </div>
                                                        <div class="col-sm-4">
                                                            <label id="Label27" runat="server" style="height: 30px"></label>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label5" runat="server" class="col-sm-1 control-label">VK</label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtpt69" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right; background-color: #66FFCC" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtpt70" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="True"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtpt71" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="True"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtpt72" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right; background-color: #66FFCC" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtpt73" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtpt74" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right; background-color: #66FFCC" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-4">
                                                            <label id="dd" runat="server" style="height: 30px"></label>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <div class="col-sm-1">
                                                            <asp:DropDownList ID="dd1" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right">
                                                                <asp:ListItem Text="E FLUTE 35%" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="B FLUTE 45%" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="A FLUTE 50%" Value="1"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtpt75" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right; background-color: #66FFCC" MaxLength="8" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtpt76" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="True"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtpt77" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="True"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtpt78" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right; background-color: #66FFCC" MaxLength="8" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtpt79" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="True"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtpt80" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right; background-color: #66FFCC" MaxLength="8" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-4">
                                                            <label id="Label16" runat="server" style="height: 30px"></label>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label20" runat="server" class="col-sm-1 control-label">SK</label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtpt81" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right; background-color: #66FFCC" MaxLength="8" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtpt82" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="True"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtpt83" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="True"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtpt84" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right; background-color: #66FFCC" MaxLength="8" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtpt85" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="True"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtpt86" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right; background-color: #66FFCC" MaxLength="8" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-4">
                                                            <label id="Label26" runat="server" style="height: 30px"></label>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <div class="col-sm-1">
                                                            <label id="Label32" runat="server">Paper_Weight</label>
                                                        </div>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <label id="Label33" runat="server">Area</label>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <label id="Label34" runat="server">Paper GSM</label>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <label id="Label35" runat="server">Paper WG</label>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <label id="Label28" runat="server">Rate</label>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <label id="Label29" runat="server">Amount</label>
                                                        </div>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-4">
                                                            <label id="Label31" runat="server" style="height: 30px"></label>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label30" runat="server" class="col-sm-1 control-label">VK</label>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="TextBox5" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="True"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="TextBox6" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="True"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtpt87" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="True"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtpt88" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right; background-color: #66FFCC" MaxLength="8" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtpt89" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="True"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-4">
                                                            <label id="Label36" runat="server" style="height: 30px"></label>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label54" runat="server" class="col-sm-1 control-label">SK</label>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="TextBox7" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="True"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="TextBox8" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="True"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtpt90" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="True"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtpt91" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right; background-color: #66FFCC" MaxLength="8" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtpt92" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="True"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-4">
                                                            <label id="Label55" runat="server" style="height: 30px"></label>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label56" runat="server" class="col-sm-1 control-label">SK</label>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="TextBox9" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="True"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="TextBox10" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="True"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtpt93" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="True"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtpt94" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right; background-color: #66FFCC" MaxLength="8" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtpt95" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="True"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-4">
                                                            <label id="Label57" runat="server" style="height: 30px"></label>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label58" runat="server" class="col-sm-1 control-label">Total_Paper_WG/Cost</label>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtpt96" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="True"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtpt97" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="True"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-4">
                                                            <label id="Label59" runat="server" style="height: 30px"></label>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label60" runat="server" class="col-sm-1 control-label">Conversion@</label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtpt101" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right; background-color: #66FFCC" MaxLength="8" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtpt98" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="True"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <label id="Label62" runat="server" class="col-sm-1 control-label">Rej@</label>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtpt102" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right; background-color: #66FFCC" MaxLength="8" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtpt99" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="True"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <label id="Label63" runat="server" class="col-sm-1 control-label">Printing</label>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtpt113" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="True"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <label id="Label91" runat="server" class="col-sm-1 control-label">Total_Cost</label>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtpt100" runat="server" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" ReadOnly="True"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <label id="Label61" runat="server" style="height: 30px"></label>
                                                        </div>
                                                    </div>
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
            var tabName = $("[id*=TabName]").val() != "" ? $("[id*=TabName]").val() : "DescTab1";
            $('#Tabs a[href="#' + tabName + '"]').tab('show');
            $("#Tabs a").click(function () {
                $("[id*=TabName]").val($(this).attr("href").replace("#", ""));
            });
        });

        function cal() {
            // PICKED FROM TEXTBOX CHANGED EVENT
            //txtpt5.Text = txtpt1.Text.Trim();
            //if (txtpt10.Text.Trim() == "1")
            //    txtpt13.Text = "0";
            //if (txtpt11.Text.Trim() == "1")
            //    txtpt104.Text = "0";
            //if (txtpt13.Text.Trim() == "1")
            //    txtpt10.Text = "0";
            //if (txtpt104.Text.Trim() == "1") txtpt11.Text = "0";

            document.getElementById('ContentPlaceHolder1_txtpt5').value = document.getElementById('ContentPlaceHolder1_txtpt1').value;
            if (fill_zero(document.getElementById('ContentPlaceHolder1_txtpt10').value * 1) == 1)
                document.getElementById('ContentPlaceHolder1_txtpt13').value = "0";
            if (fill_zero(document.getElementById('ContentPlaceHolder1_txtpt11').value * 1) == 1)
                document.getElementById('ContentPlaceHolder1_txtpt104').value = "0";
            if (fill_zero(document.getElementById('ContentPlaceHolder1_txtpt13').value * 1) == 1)
                document.getElementById('ContentPlaceHolder1_txtpt10').value = "0";
            if (fill_zero(document.getElementById('ContentPlaceHolder1_txtpt104').value * 1) == 1) document.getElementById('ContentPlaceHolder1_txtpt11').value = "0";
            //------------------------------------------------------------------------

            // PICKED FROM CAL FUNCTION
            var b17 = 0, e5 = 0, e7 = 0, n32 = 0, n33 = 0, n34 = 0, j14 = 0, j15 = 0, A52 = 0;
            var element = document.getElementById("ContentPlaceHolder1_dd1");
            //txtpt4.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt2.Text.Trim()) * Convert.ToDouble(txtpt3.Text.Trim()), 2));
            document.getElementById('ContentPlaceHolder1_txtpt4').value = fill_zero(((document.getElementById('ContentPlaceHolder1_txtpt2').value * 1) * (document.getElementById('ContentPlaceHolder1_txtpt3').value * 1)).toFixed(2));

            //  txtpt17.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt8.Text.Trim()) / Convert.ToDouble(txtpt7.Text.Trim()), 2));

            document.getElementById('ContentPlaceHolder1_txtpt17').value = fill_zero(((document.getElementById('ContentPlaceHolder1_txtpt8').value * 1) / (document.getElementById('ContentPlaceHolder1_txtpt7').value * 1)).toFixed(2));
            //if (Convert.ToDouble(txtpt17.Text.Trim()) < 4000) txtpt16.Text = "1";
            //else txtpt16.Text = "0";

            if (fill_zero(document.getElementById('ContentPlaceHolder1_txtpt17').value * 1) < 4000) document.getElementById('ContentPlaceHolder1_txtpt16').value = "1";
            else document.getElementById('ContentPlaceHolder1_txtpt16').value = "0";

            //if (Convert.ToDouble(txtpt17.Text.Trim()) >= 4000) txtpt18.Text = "1";
            //else txtpt18.Text = "0";

            if (fill_zero(document.getElementById('ContentPlaceHolder1_txtpt17').value * 1) >= 4000) document.getElementById('ContentPlaceHolder1_txtpt18').value = "1";
            else document.getElementById('ContentPlaceHolder1_txtpt18').value = "0";

            //e5 = Convert.ToDouble(txtpt4.Text.Trim());

            e5 = (document.getElementById('ContentPlaceHolder1_txtpt4').value * 1);

            //if (e5 <= 600) { txtpt31.Text = "700"; txtpt35.Text = "700"; n33 = 1; b17 = 300; }
            //else { txtpt31.Text = "1000"; txtpt35.Text = "1000"; n33 = 1.5; b17 = 600; }

            if (e5 <= 600) { document.getElementById('ContentPlaceHolder1_txtpt31').value = "700"; document.getElementById('ContentPlaceHolder1_txtpt35').value = "700"; n33 = 1; b17 = 300; }
            else { document.getElementById('ContentPlaceHolder1_txtpt4').value = "1000"; document.getElementById('ContentPlaceHolder1_txtpt35').value = "1000"; n33 = 1.5; b17 = 600; }

            //txtpt19.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt4.Text.Trim()) * Convert.ToDouble(txtpt6.Text.Trim()) * Convert.ToDouble(txtpt17.Text.Trim()) * (1 + (Convert.ToDouble(txtpt15.Text.Trim()) / 100))) / 1550000, 2));
            //txtpt20.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt19.Text.Trim()) * Convert.ToDouble(txtpt5.Text.Trim()), 2));

            document.getElementById('ContentPlaceHolder1_txtpt19').value = fill_zero((((document.getElementById('ContentPlaceHolder1_txtpt4').value * 1) * (document.getElementById('ContentPlaceHolder1_txtpt6').value * 1) * (document.getElementById('ContentPlaceHolder1_txtpt17').value * 1) * (1 + ((document.getElementById('ContentPlaceHolder1_txtpt15').value * 1) / 100))) / 1550000).toFixed(2));


            //document.getElementById('ContentPlaceHolder1_txtpt19').value = fill_zero(((((document.getElementById('ContentPlaceHolder1_txtpt4').value * 1) * (document.getElementById('ContentPlaceHolder1_txtpt6').value * 1) * (document.getElementById('ContentPlaceHolder1_txtpt17').value * 1) * (1 + ((document.getElementById('ContentPlaceHolder1_txtpt15').value * 1) / 100))) / 1550000)).toFixed(2));
            document.getElementById('ContentPlaceHolder1_txtpt20').value = fill_zero(((document.getElementById('ContentPlaceHolder1_txtpt19').value * 1) * (document.getElementById('ContentPlaceHolder1_txtpt5').value * 1)).toFixed(2));

            // if (txtpt21.Text == null || txtpt21.Text == "0") txtpt21.Text = Convert.ToString(Math.Round(b17 * Convert.ToDouble(txtpt9.Text.Trim()), 2));

            if (fill_zero(document.getElementById('ContentPlaceHolder1_txtpt21').value * 1) == 0) document.getElementById('ContentPlaceHolder1_txtpt21').value = fill_zero((b17 * (document.getElementById('ContentPlaceHolder1_txtpt9').value * 1)).toFixed(2));

            //txtpt22.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt21.Text.Trim()) * 1, 2));
            document.getElementById('ContentPlaceHolder1_txtpt22').value = fill_zero(((document.getElementById('ContentPlaceHolder1_txtpt21').value * 1)).toFixed(2));

            e7 = fill_zero((document.getElementById('ContentPlaceHolder1_txtpt9').value * 1));

            if (e7 == 0) n32 = 0;
            else if (e7 == 1) n32 = 300;
            else if (e7 == 2) n32 = 500;
            else if (e7 == 3) n32 = 650;
            else if (e7 == 4) n32 = 800;
            else if (e7 == 5) n32 = 1000;
            else if (e7 == 6) n32 = 1200;
            else if (e7 == 7) n32 = 1500;
            else if (e7 == 8) n32 = 1700;

            //txtpt23.Text = Convert.ToString(Math.Round(n32 * n33 * 4000 * Convert.ToDouble(txtpt16.Text.Trim()) * (1 + (Convert.ToDouble(txtpt15.Text.Trim()) / 100)) / 1000, 2));
            //txtpt24.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt23.Text.Trim()) * 1, 2));

            document.getElementById('ContentPlaceHolder1_txtpt23').value = fill_zero((n32 * n33 * 4000 * (document.getElementById('ContentPlaceHolder1_txtpt16').value * 1) * (1 + (document.getElementById('ContentPlaceHolder1_txtpt15').value / 100)) / 1000).toFixed(2));
            document.getElementById('ContentPlaceHolder1_txtpt24').value = fill_zero((document.getElementById('ContentPlaceHolder1_txtpt23').value * 1).toFixed(2));

            //txtpt25.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt17.Text.Trim()) * n32 * n33 * Convert.ToDouble(txtpt18.Text.Trim()) * (1 + (Convert.ToDouble(txtpt15.Text.Trim()) / 100)) / 1000, 2));
            //txtpt26.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt25.Text.Trim()) * 1, 2));

            document.getElementById('ContentPlaceHolder1_txtpt25').value = fill_zero(((document.getElementById('ContentPlaceHolder1_txtpt17').value * 1) * n32 * n33 * document.getElementById('ContentPlaceHolder1_txtpt18').value * (1 + (document.getElementById('ContentPlaceHolder1_txtpt15').value / 100)) / 1000).toFixed(2));
            document.getElementById('ContentPlaceHolder1_txtpt26').value = fill_zero((document.getElementById('ContentPlaceHolder1_txtpt25').value * 1).toFixed(2));

            // txtpt27.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt4.Text.Trim()) / 3) * (Convert.ToDouble(txtpt17.Text.Trim()) / 100), 2));
            //txtpt28.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt27.Text.Trim()) * Convert.ToDouble(txtpt13.Text.Trim()), 2));

            document.getElementById('ContentPlaceHolder1_txtpt27').value = fill_zero((((document.getElementById('ContentPlaceHolder1_txtpt4').value * 1) / 3) * ((document.getElementById('ContentPlaceHolder1_txtpt17').value * 1) / 100)).toFixed(2));
            document.getElementById('ContentPlaceHolder1_txtpt28').value = fill_zero(((document.getElementById('ContentPlaceHolder1_txtpt27').value * 1) * (document.getElementById('ContentPlaceHolder1_txtpt13').value * 1)).toFixed(2));

            //j14 = Math.Round(Convert.ToDouble(txtpt17.Text.Trim()) * (Convert.ToDouble(txtpt15.Text.Trim()) / 100), 2);

            j14 = fill_zero(((document.getElementById('ContentPlaceHolder1_txtpt17').value * 1) * ((document.getElementById('ContentPlaceHolder1_txtpt15').value * 1) / 100)).toFixed(2));

            if (j14 > 200) j15 = 200;
            else j15 = j14;

            //txtpt29.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt4.Text.Trim()) * (Convert.ToDouble(txtpt17.Text.Trim()) - j15) * (1 + (Convert.ToDouble(txtpt15.Text.Trim()) / 100) - 0.02) * 0.09) / 100, 1));
            //txtpt30.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt29.Text.Trim()) * Convert.ToDouble(txtpt10.Text.Trim()), 2));

            document.getElementById('ContentPlaceHolder1_txtpt29').value = fill_zero((((document.getElementById('ContentPlaceHolder1_txtpt4').value * 1) * ((document.getElementById('ContentPlaceHolder1_txtpt17').value * 1) - j15) * (1 + ((document.getElementById('ContentPlaceHolder1_txtpt15').value * 1) / 100) - 0.02) * 0.09) / 100).toFixed(1));
            document.getElementById('ContentPlaceHolder1_txtpt30').value = fill_zero(((document.getElementById('ContentPlaceHolder1_txtpt29').value * 1) * (document.getElementById('ContentPlaceHolder1_txtpt10').value * 1)).toFixed(2));

            //  txtpt32.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt31.Text.Trim()) * Convert.ToDouble(txtpt11.Text.Trim()), 2));
            document.getElementById('ContentPlaceHolder1_txtpt32').value = fill_zero(((document.getElementById('ContentPlaceHolder1_txtpt31').value * 1) * (document.getElementById('ContentPlaceHolder1_txtpt11').value * 1)).toFixed(2));

            //txtpt33.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt4.Text.Trim()) / 4.5 * Convert.ToDouble(txtpt17.Text.Trim()) / 100, 1));
            //txtpt34.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt33.Text.Trim()) * Convert.ToDouble(txtpt11.Text.Trim()), 2));

            document.getElementById('ContentPlaceHolder1_txtpt33').value = fill_zero(((document.getElementById('ContentPlaceHolder1_txtpt4').value * 1) / 4.5 * (document.getElementById('ContentPlaceHolder1_txtpt17').value * 1) / 100).toFixed(1));
            document.getElementById('ContentPlaceHolder1_txtpt34').value = fill_zero(((document.getElementById('ContentPlaceHolder1_txtpt33').value * 1) * (document.getElementById('ContentPlaceHolder1_txtpt11').value * 1)).toFixed(2));

            //txtpt106.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt4.Text.Trim()) / 4.5 * Convert.ToDouble(txtpt17.Text.Trim()) / 100, 1));
            //txtpt109.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt106.Text.Trim()) * Convert.ToDouble(txtpt104.Text.Trim()), 2));

            document.getElementById('ContentPlaceHolder1_txtpt106').value = fill_zero(((document.getElementById('ContentPlaceHolder1_txtpt4').value * 1) / 4.5 * (document.getElementById('ContentPlaceHolder1_txtpt17').value * 1) / 100).toFixed(1));
            document.getElementById('ContentPlaceHolder1_txtpt109').value = fill_zero(((document.getElementById('ContentPlaceHolder1_txtpt106').value * 1) * (document.getElementById('ContentPlaceHolder1_txtpt104').value * 1)).toFixed(2));

            // txtpt36.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt35.Text.Trim()) * Convert.ToDouble(txtpt12.Text.Trim()), 2));

            document.getElementById('ContentPlaceHolder1_txtpt36').value = fill_zero(((document.getElementById('ContentPlaceHolder1_txtpt35').value * 1) * (document.getElementById('ContentPlaceHolder1_txtpt12').value * 1)).toFixed(2));

            //txtpt37.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt17.Text.Trim()) - j15) * Convert.ToDouble(txtpt4.Text.Trim()) * (1 + (Convert.ToDouble(txtpt15.Text.Trim()) / 100)) * 4.5 / 1000, 2));
            //txtpt38.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt37.Text.Trim()) * Convert.ToDouble(txtpt12.Text.Trim()), 2));

            document.getElementById('ContentPlaceHolder1_txtpt37').value = fill_zero((((document.getElementById('ContentPlaceHolder1_txtpt17').value * 1) - j15) * (document.getElementById('ContentPlaceHolder1_txtpt4').value * 1) * (1 + ((document.getElementById('ContentPlaceHolder1_txtpt15').value * 1) / 100)) * 4.5 / 1000).toFixed(2));
            document.getElementById('ContentPlaceHolder1_txtpt38').value = fill_zero(((document.getElementById('ContentPlaceHolder1_txtpt37').value * 1) * (document.getElementById('ContentPlaceHolder1_txtpt12').value * 1)).toFixed(2));

            //txtpt39.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt7.Text.Trim()) * 100, 2));
            //txtpt40.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt39.Text.Trim()) * 1, 2));

            document.getElementById('ContentPlaceHolder1_txtpt39').value = fill_zero(((document.getElementById('ContentPlaceHolder1_txtpt7').value * 1) * 100).toFixed(2));
            document.getElementById('ContentPlaceHolder1_txtpt40').value = fill_zero(((document.getElementById('ContentPlaceHolder1_txtpt39').value * 1) * 1).toFixed(2));

            //txtpt41.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt17.Text.Trim()) * 195 / 1000, 1));
            //txtpt42.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt41.Text.Trim()) * 1, 2));

            document.getElementById('ContentPlaceHolder1_txtpt41').value = fill_zero(((document.getElementById('ContentPlaceHolder1_txtpt17').value * 1) * 195 / 1000).toFixed(1));
            document.getElementById('ContentPlaceHolder1_txtpt42').value = fill_zero(((document.getElementById('ContentPlaceHolder1_txtpt41').value * 1) * 1).toFixed(2));

            //txtpt43.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt8.Text.Trim()) * 6 / 1000, 2));
            //txtpt44.Text = txtpt43.Text.Trim();

            document.getElementById('ContentPlaceHolder1_txtpt43').value = fill_zero(((document.getElementById('ContentPlaceHolder1_txtpt8').value * 1) * 6 / 1000).toFixed(2));
            document.getElementById('ContentPlaceHolder1_txtpt44').value = fill_zero(document.getElementById('ContentPlaceHolder1_txtpt43').value * 1);

            //txtpt107.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt17.Text.Trim()) * 195 / 1000, 1));
            //txtpt108.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt107.Text.Trim()) * Convert.ToDouble(txtpt105.Text.Trim()), 2));

            document.getElementById('ContentPlaceHolder1_txtpt107').value = fill_zero(((document.getElementById('ContentPlaceHolder1_txtpt17').value * 1) * 195 / 1000).toFixed(2));
            document.getElementById('ContentPlaceHolder1_txtpt108').value = fill_zero(((document.getElementById('ContentPlaceHolder1_txtpt107').value * 1) * (document.getElementById('ContentPlaceHolder1_txtpt105').value * 1)).toFixed(2));

            //if (Convert.ToDouble(txtpt14.Text.Trim()) == 1) n34 = 80;
            //else n34 = 50;

            if ((document.getElementById('ContentPlaceHolder1_txtpt14').value * 1) == 1) n34 = 80;
            else n34 = 50;

            //txtpt45.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt8.Text.Trim()) * n34 / 1000, 2));
            //txtpt46.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt45.Text.Trim()) * 1, 2));

            document.getElementById('ContentPlaceHolder1_txtpt45').value = fill_zero(((document.getElementById('ContentPlaceHolder1_txtpt8').value * 1) * n34 / 1000).toFixed(2));
            document.getElementById('ContentPlaceHolder1_txtpt46').value = fill_zero((document.getElementById('ContentPlaceHolder1_txtpt45').value * 1).toFixed(2));

            //txtpt47.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt8.Text.Trim()) * 25 / 1000, 3));
            //txtpt48.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt47.Text.Trim()) * 1, 2));

            document.getElementById('ContentPlaceHolder1_txtpt47').value = fill_zero(((document.getElementById('ContentPlaceHolder1_txtpt8').value * 1) * 25 / 1000).toFixed(3));
            document.getElementById('ContentPlaceHolder1_txtpt48').value = fill_zero((document.getElementById('ContentPlaceHolder1_txtpt47').value * 1).toFixed(2));

            //txtpt49.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt20.Text.Trim()) + Convert.ToDouble(txtpt22.Text.Trim()) + Convert.ToDouble(txtpt24.Text.Trim()) + Convert.ToDouble(txtpt26.Text.Trim()) + Convert.ToDouble(txtpt28.Text.Trim())
            //    + Convert.ToDouble(txtpt30.Text.Trim()) + Convert.ToDouble(txtpt32.Text.Trim()) + Convert.ToDouble(txtpt34.Text.Trim()) + Convert.ToDouble(txtpt36.Text.Trim()) + Convert.ToDouble(txtpt38.Text.Trim()) + Convert.ToDouble(txtpt40.Text.Trim())
            //    + Convert.ToDouble(txtpt42.Text.Trim()) + Convert.ToDouble(txtpt44.Text.Trim()) + Convert.ToDouble(txtpt46.Text.Trim()) + Convert.ToDouble(txtpt48.Text.Trim()) + Convert.ToDouble(txtpt108.Text.Trim()) + Convert.ToDouble(txtpt109.Text.Trim()) + Convert.ToDouble(txtpt103.Text.Trim()), 2));

            document.getElementById('ContentPlaceHolder1_txtpt49').value = fill_zero(((document.getElementById('ContentPlaceHolder1_txtpt20').value * 1) + (document.getElementById('ContentPlaceHolder1_txtpt22').value * 1) + (document.getElementById('ContentPlaceHolder1_txtpt24').value * 1) + (document.getElementById('ContentPlaceHolder1_txtpt26').value * 1) + (document.getElementById('ContentPlaceHolder1_txtpt28').value * 1)
    + (document.getElementById('ContentPlaceHolder1_txtpt30').value * 1) + (document.getElementById('ContentPlaceHolder1_txtpt32').value * 1) + (document.getElementById('ContentPlaceHolder1_txtpt34').value * 1) + (document.getElementById('ContentPlaceHolder1_txtpt36').value * 1) + (document.getElementById('ContentPlaceHolder1_txtpt38').value * 1) + (document.getElementById('ContentPlaceHolder1_txtpt40').value * 1)
    + (document.getElementById('ContentPlaceHolder1_txtpt42').value * 1) + (document.getElementById('ContentPlaceHolder1_txtpt44').value * 1) + (document.getElementById('ContentPlaceHolder1_txtpt46').value * 1) + (document.getElementById('ContentPlaceHolder1_txtpt48').value * 1) + (document.getElementById('ContentPlaceHolder1_txtpt108').value * 1) + (document.getElementById('ContentPlaceHolder1_txtpt109').value * 1) + (document.getElementById('ContentPlaceHolder1_txtpt103').value * 1)).toFixed(2));

            //if (Convert.ToDouble(txtpt50.Text.Trim()) == 0) txtpt51.Text = "0";
            //else txtpt51.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt49.Text.Trim()) * (Convert.ToDouble(txtpt50.Text.Trim()) / 100), 2));

            if ((document.getElementById('ContentPlaceHolder1_txtpt50').value * 1) == 0) document.getElementById('ContentPlaceHolder1_txtpt51').value = "0";
            else document.getElementById('ContentPlaceHolder1_txtpt51').value = fill_zero(((document.getElementById('ContentPlaceHolder1_txtpt49').value * 1) * ((document.getElementById('ContentPlaceHolder1_txtpt50').value * 1) / 100)).toFixed(2));

            //txtpt52.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt49.Text.Trim()) + Convert.ToDouble(txtpt51.Text.Trim()), 2));
            //txtpt53.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt52.Text.Trim()) / Convert.ToDouble(txtpt8.Text.Trim()), 2));

            document.getElementById('ContentPlaceHolder1_txtpt52').value = fill_zero(((document.getElementById('ContentPlaceHolder1_txtpt49').value * 1) + (document.getElementById('ContentPlaceHolder1_txtpt51').value * 1)).toFixed(2));
            document.getElementById('ContentPlaceHolder1_txtpt53').value = fill_zero(((document.getElementById('ContentPlaceHolder1_txtpt52').value * 1) / (document.getElementById('ContentPlaceHolder1_txtpt8').value * 1)).toFixed(2));

            //txtpt55.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt54.Text.Trim()) / Convert.ToDouble(txtpt8.Text.Trim()), 2));
            document.getElementById('ContentPlaceHolder1_txtpt55').value = fill_zero(((document.getElementById('ContentPlaceHolder1_txtpt54').value * 1) / (document.getElementById('ContentPlaceHolder1_txtpt8').value * 1)).toFixed(2));

            //txtpt61.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt58.Text.Trim()) * 0.03) + Convert.ToDouble(txtpt58.Text.Trim()), 2));
            //txtpt62.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt59.Text.Trim()) * 0.03) + Convert.ToDouble(txtpt59.Text.Trim()), 2));
            //txtpt63.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt60.Text.Trim()) * 0.03) + Convert.ToDouble(txtpt60.Text.Trim()), 2));

            document.getElementById('ContentPlaceHolder1_txtpt61').value = fill_zero((((document.getElementById('ContentPlaceHolder1_txtpt58').value * 1) * 0.03) + (document.getElementById('ContentPlaceHolder1_txtpt58').value) * 1).toFixed(2));
            document.getElementById('ContentPlaceHolder1_txtpt62').value = fill_zero((((document.getElementById('ContentPlaceHolder1_txtpt59').value * 1) * 0.03) + (document.getElementById('ContentPlaceHolder1_txtpt59').value) * 1).toFixed(2));
            document.getElementById('ContentPlaceHolder1_txtpt63').value = fill_zero((((document.getElementById('ContentPlaceHolder1_txtpt60').value * 1) * 0.03) + (document.getElementById('ContentPlaceHolder1_txtpt60').value) * 1).toFixed(2));

            //txtpt64.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt62.Text.Trim()) + Convert.ToDouble(txtpt63.Text.Trim()) + 20, 2));
            //txtpt65.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt61.Text.Trim()) + Convert.ToDouble(txtpt62.Text.Trim()) + 50 + 20, 2));

            document.getElementById('ContentPlaceHolder1_txtpt64').value = fill_zero(((document.getElementById('ContentPlaceHolder1_txtpt62').value * 1) + (document.getElementById('ContentPlaceHolder1_txtpt63').value * 1) + 20).toFixed(2));
            document.getElementById('ContentPlaceHolder1_txtpt65').value = fill_zero(((document.getElementById('ContentPlaceHolder1_txtpt61').value * 1) + (document.getElementById('ContentPlaceHolder1_txtpt62').value * 1) + 50 + 20).toFixed(2));

            //txtpt66.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt64.Text.Trim()) / 25.4, 2));
            //txtpt67.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt65.Text.Trim()) / 25.4, 2));

            document.getElementById('ContentPlaceHolder1_txtpt66').value = fill_zero(((document.getElementById('ContentPlaceHolder1_txtpt64').value * 1) / 25.4).toFixed(2));
            document.getElementById('ContentPlaceHolder1_txtpt67').value = fill_zero(((document.getElementById('ContentPlaceHolder1_txtpt65').value * 1) / 25.4).toFixed(2));

            //Area of Sheet
            //TextBox2.Text = txtpt2.Text.Trim(); TextBox3.Text = txtpt3.Text.Trim(); TextBox4.Text = txtpt7.Text.Trim();
            //txtpt68.Text = Convert.ToString(Math.Round(((Convert.ToDouble(TextBox2.Text.Trim()) * Convert.ToDouble(TextBox3.Text.Trim())) / 1550) / Convert.ToDouble(TextBox4.Text.Trim()), 4));

            document.getElementById('ContentPlaceHolder1_TextBox2').value = fill_zero((document.getElementById('ContentPlaceHolder1_txtpt2').value * 1)); document.getElementById('ContentPlaceHolder1_TextBox3').value = fill_zero((document.getElementById('ContentPlaceHolder1_txtpt3').value * 1)); document.getElementById('ContentPlaceHolder1_TextBox4').value = fill_zero((document.getElementById('ContentPlaceHolder1_txtpt7').value * 1));
            document.getElementById('ContentPlaceHolder1_txtpt68').value = fill_zero(((((document.getElementById('ContentPlaceHolder1_TextBox2').value * 1) * (document.getElementById('ContentPlaceHolder1_TextBox3').value * 1)) / 1550) / (document.getElementById('ContentPlaceHolder1_TextBox4').value * 1)).toFixed(4));
            //Paper Detail

            //txtpt70.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt74.Text.Trim()) / 1000, 4));
            //txtpt76.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt80.Text.Trim()) / 1000, 4));
            //txtpt82.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt86.Text.Trim()) / 1000, 4));

            document.getElementById('ContentPlaceHolder1_txtpt70').value = fill_zero(((document.getElementById('ContentPlaceHolder1_txtpt74').value * 1) / 1000).toFixed(4));
            document.getElementById('ContentPlaceHolder1_txtpt76').value = fill_zero(((document.getElementById('ContentPlaceHolder1_txtpt80').value * 1) / 1000).toFixed(4));
            document.getElementById('ContentPlaceHolder1_txtpt82').value = fill_zero(((document.getElementById('ContentPlaceHolder1_txtpt86').value * 1) / 1000).toFixed(4));

            // txtpt71.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt69.Text.Trim()) * Convert.ToDouble(txtpt70.Text.Trim()), 4));
            document.getElementById('ContentPlaceHolder1_txtpt71').value = fill_zero(((document.getElementById('ContentPlaceHolder1_txtpt69').value * 1) * (document.getElementById('ContentPlaceHolder1_txtpt70').value * 1)).toFixed(4));

            if (element.options[element.selectedIndex].value == 0) A52 = 0.35;
            else if (element.options[element.selectedIndex].value == 1) A52 = 0.45;
            else if (element.options[element.selectedIndex].value == 2) A52 = 0.50;

            //txtpt77.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt75.Text.Trim()) * Convert.ToDouble(txtpt76.Text.Trim())) * A52 + (Convert.ToDouble(txtpt75.Text.Trim()) * Convert.ToDouble(txtpt76.Text.Trim())), 4));
            //txtpt83.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt81.Text.Trim()) * Convert.ToDouble(txtpt82.Text.Trim()), 4));

            document.getElementById('ContentPlaceHolder1_txtpt77').value = fill_zero((((document.getElementById('ContentPlaceHolder1_txtpt75').value * 1) * (document.getElementById('ContentPlaceHolder1_txtpt76').value * 1)) * A52 + ((document.getElementById('ContentPlaceHolder1_txtpt75').value * 1) * (document.getElementById('ContentPlaceHolder1_txtpt76').value * 1))).toFixed(4));
            document.getElementById('ContentPlaceHolder1_txtpt83').value = fill_zero(((document.getElementById('ContentPlaceHolder1_txtpt81').value * 1) * (document.getElementById('ContentPlaceHolder1_txtpt82').value * 1)).toFixed(4));

            //BS
            //txtpt73.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt71.Text.Trim()) * Convert.ToDouble(txtpt72.Text.Trim()), 4));
            //txtpt79.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt75.Text.Trim()) * Convert.ToDouble(txtpt76.Text.Trim()) * Convert.ToDouble(txtpt78.Text.Trim())) / 2, 4));
            //txtpt85.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt83.Text.Trim()) * Convert.ToDouble(txtpt84.Text.Trim()), 4));

            document.getElementById('ContentPlaceHolder1_txtpt73').value = fill_zero(((document.getElementById('ContentPlaceHolder1_txtpt71').value * 1) * (document.getElementById('ContentPlaceHolder1_txtpt72').value * 1)).toFixed(4));
            document.getElementById('ContentPlaceHolder1_txtpt79').value = fill_zero((((document.getElementById('ContentPlaceHolder1_txtpt75').value * 1) * (document.getElementById('ContentPlaceHolder1_txtpt76').value * 1) * (document.getElementById('ContentPlaceHolder1_txtpt78').value * 1)) / 2).toFixed(4));
            document.getElementById('ContentPlaceHolder1_txtpt85').value = fill_zero(((document.getElementById('ContentPlaceHolder1_txtpt83').value * 1) * (document.getElementById('ContentPlaceHolder1_txtpt84').value * 1)).toFixed(4));

            //TextBox5.Text = txtpt68.Text.Trim(); TextBox7.Text = txtpt68.Text.Trim(); TextBox9.Text = txtpt68.Text.Trim();
            //TextBox6.Text = txtpt71.Text.Trim(); TextBox8.Text = txtpt77.Text.Trim(); TextBox10.Text = txtpt83.Text.Trim();

            document.getElementById('ContentPlaceHolder1_TextBox5').value = fill_zero((document.getElementById('ContentPlaceHolder1_txtpt68').value * 1)); document.getElementById('ContentPlaceHolder1_TextBox7').value = fill_zero((document.getElementById('ContentPlaceHolder1_txtpt68').value * 1)); document.getElementById('ContentPlaceHolder1_TextBox9').value = fill_zero((document.getElementById('ContentPlaceHolder1_txtpt68').value * 1));
            document.getElementById('ContentPlaceHolder1_TextBox6').value = fill_zero((document.getElementById('ContentPlaceHolder1_txtpt71').value * 1)); document.getElementById('ContentPlaceHolder1_TextBox8').value = fill_zero((document.getElementById('ContentPlaceHolder1_txtpt77').value * 1)); document.getElementById('ContentPlaceHolder1_TextBox10').value = fill_zero((document.getElementById('ContentPlaceHolder1_txtpt83').value * 1));

            //txtpt87.Text = Convert.ToString(Math.Round(Convert.ToDouble(TextBox5.Text.Trim()) * Convert.ToDouble(TextBox6.Text.Trim()), 4));
            //txtpt90.Text = Convert.ToString(Math.Round(Convert.ToDouble(TextBox7.Text.Trim()) * Convert.ToDouble(TextBox8.Text.Trim()), 4));
            //txtpt93.Text = Convert.ToString(Math.Round(Convert.ToDouble(TextBox9.Text.Trim()) * Convert.ToDouble(TextBox10.Text.Trim()), 4));
            document.getElementById('ContentPlaceHolder1_txtpt87').value = fill_zero(((document.getElementById('ContentPlaceHolder1_TextBox5').value * 1) * (document.getElementById('ContentPlaceHolder1_TextBox6').value * 1)).toFixed(4));
            document.getElementById('ContentPlaceHolder1_txtpt90').value = fill_zero(((document.getElementById('ContentPlaceHolder1_TextBox7').value * 1) * (document.getElementById('ContentPlaceHolder1_TextBox8').value * 1)).toFixed(4));
            document.getElementById('ContentPlaceHolder1_txtpt93').value = fill_zero(((document.getElementById('ContentPlaceHolder1_TextBox9').value * 1) * (document.getElementById('ContentPlaceHolder1_TextBox10').value * 1)).toFixed(4));

            //txtpt89.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt87.Text.Trim()) * Convert.ToDouble(txtpt88.Text.Trim()), 4));
            //txtpt92.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt90.Text.Trim()) * Convert.ToDouble(txtpt91.Text.Trim()), 4));
            //txtpt95.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt92.Text.Trim()) * Convert.ToDouble(txtpt93.Text.Trim()), 4));

            document.getElementById('ContentPlaceHolder1_txtpt89').value = fill_zero(((document.getElementById('ContentPlaceHolder1_txtpt87').value * 1) * (document.getElementById('ContentPlaceHolder1_txtpt88').value * 1)).toFixed(4));
            document.getElementById('ContentPlaceHolder1_txtpt92').value = fill_zero(((document.getElementById('ContentPlaceHolder1_txtpt90').value * 1) * (document.getElementById('ContentPlaceHolder1_txtpt91').value * 1)).toFixed(4));
            document.getElementById('ContentPlaceHolder1_txtpt95').value = fill_zero(((document.getElementById('ContentPlaceHolder1_txtpt92').value * 1) * (document.getElementById('ContentPlaceHolder1_txtpt93').value * 1)).toFixed(4));

            //Total Paper WG Cost
            //txtpt96.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt87.Text.Trim()) + Convert.ToDouble(txtpt90.Text.Trim()) + Convert.ToDouble(txtpt93.Text.Trim()), 4));
            //txtpt97.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt89.Text.Trim()) + Convert.ToDouble(txtpt92.Text.Trim()) + Convert.ToDouble(txtpt95.Text.Trim()), 4));

            document.getElementById('ContentPlaceHolder1_txtpt96').value = fill_zero(((document.getElementById('ContentPlaceHolder1_txtpt87').value * 1) + (document.getElementById('ContentPlaceHolder1_txtpt90').value * 1) + (document.getElementById('ContentPlaceHolder1_txtpt93').value * 1)).toFixed(4));
            document.getElementById('ContentPlaceHolder1_txtpt97').value = fill_zero(((document.getElementById('ContentPlaceHolder1_txtpt89').value * 1) + (document.getElementById('ContentPlaceHolder1_txtpt92').value * 1) + (document.getElementById('ContentPlaceHolder1_txtpt95').value * 1)).toFixed(4));

            //Conversion , Rej
            //txtpt98.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt96.Text.Trim()) * Convert.ToDouble(txtpt101.Text.Trim()), 4));
            //txtpt99.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt97.Text.Trim()) + Convert.ToDouble(txtpt98.Text.Trim())) * (Convert.ToDouble(txtpt102.Text.Trim()) / 100), 4));

            document.getElementById('ContentPlaceHolder1_txtpt98').value = fill_zero(((document.getElementById('ContentPlaceHolder1_txtpt96').value * 1) * (document.getElementById('ContentPlaceHolder1_txtpt101').value * 1)).toFixed(4));
            document.getElementById('ContentPlaceHolder1_txtpt99').value = fill_zero((((document.getElementById('ContentPlaceHolder1_txtpt97').value * 1) + (document.getElementById('ContentPlaceHolder1_txtpt98').value * 1)) * ((document.getElementById('ContentPlaceHolder1_txtpt102').value * 1) / 100)).toFixed(4));

            //Total Cost in second tab 0.00 is for print cost
            //txtpt100.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt97.Text.Trim()) + Convert.ToDouble(txtpt98.Text.Trim()) + Convert.ToDouble(txtpt99.Text.Trim()) + 0.00, 2));
            //TextBox11.Text = txtpt100.Text.Trim();
            //TextBox1.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt53.Text.Trim()) + Convert.ToDouble(txtpt55.Text.Trim()), 2));

            document.getElementById('ContentPlaceHolder1_txtpt100').value = fill_zero(((document.getElementById('ContentPlaceHolder1_txtpt97').value * 1) + (document.getElementById('ContentPlaceHolder1_txtpt98').value * 1) + (document.getElementById('ContentPlaceHolder1_txtpt99').value * 1) + 0.00).toFixed(2));
            document.getElementById('ContentPlaceHolder1_TextBox11').value = fill_zero((document.getElementById('ContentPlaceHolder1_txtpt100').value * 1));
            document.getElementById('ContentPlaceHolder1_TextBox1').value = fill_zero(((document.getElementById('ContentPlaceHolder1_txtpt53').value * 1) + (document.getElementById('ContentPlaceHolder1_txtpt55').value * 1)).toFixed(2));

            //Grand Total
            //txtpt57.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt53.Text.Trim()) + Convert.ToDouble(txtpt55.Text.Trim()) + Convert.ToDouble(txtpt100.Text.Trim()) + Convert.ToDouble(txtpt103.Text.Trim()), 2));
            //TextBox12.Text = txtpt57.Text.Trim();

            document.getElementById('ContentPlaceHolder1_txtpt57').value = fill_zero(((document.getElementById('ContentPlaceHolder1_txtpt53').value * 1) + (document.getElementById('ContentPlaceHolder1_txtpt55').value * 1) + (document.getElementById('ContentPlaceHolder1_txtpt100').value * 1) + (document.getElementById('ContentPlaceHolder1_txtpt103').value * 1)).toFixed(2));
            document.getElementById('ContentPlaceHolder1_TextBox12').value = fill_zero((document.getElementById('ContentPlaceHolder1_txtpt57').value * 1));
        }
        function fill_zero(val) { if (isNaN(val)) return 0; if (isFinite(val)) return val; }
    </script>
    <asp:HiddenField ID="TabName" runat="server" />
</asp:Content>
