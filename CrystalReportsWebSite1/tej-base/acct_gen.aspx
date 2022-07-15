<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="acct_gen" CodeFile="acct_gen.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../tej-base/Scripts/gridviewScroll.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            gridviewScroll('#<%=sg1.ClientID%>', gridDiv, 1, 5);
            //calculateSum();
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
        function fill_zero(val) { if (isNaN(val)) return 0; if (isFinite(val)) return val; }
    </script>
    <script type="text/javascript">
        function openfileDialog() {
            $("#Attch").click();
        }
        function submitFile() {
            $("#<%= btnAtt.ClientID%>").click();
        };
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper">
        <section class="content-header">
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label></td>
                    <td style="text-align: right">
                        <button type="submit" id="btnnew" class="btn btn-info" style="width: 100px;" runat="server" accesskey="N" onserverclick="btnnew_ServerClick"><u>N</u>ew</button>
                        <button type="submit" id="btnedit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="i" onserverclick="btnedit_ServerClick">Ed<u>i</u>t</button>
                        <button type="submit" id="btnsave" class="btn btn-info" style="width: 100px;" runat="server" accesskey="s" onserverclick="btnsave_ServerClick"><u>S</u>ave</button>
                        <button type="submit" id="btnprint" class="btn btn-info" style="width: 100px;" runat="server" accesskey="P" onserverclick="btnprint_ServerClick"><u>P</u>rint</button>
                        <button type="submit" id="btndel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="l" onserverclick="btndel_ServerClick">De<u>l</u>ete</button>
                        <button type="submit" id="btnlist" class="btn btn-info" style="width: 100px;" runat="server" accesskey="t" onserverclick="btnlist_ServerClick">Lis<u>t</u></button>
                        <button type="submit" id="btnAtch" class="btn btn-info" style="width: 100px;" runat="server" onserverclick="btnAtch_ServerClick">Attachment</button>
                        <button type="submit" id="btncancel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="c" onserverclick="btncancel_ServerClick"><u>C</u>ancel</button>
                        <button type="submit" id="btnexit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="X" onserverclick="btnexit_ServerClick">E<u>x</u>it</button>

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
                                <label id="Label68" runat="server" class="col-sm-2 control-label" title="lbl1">Account Name ( L-4 )</label>
                                <div class="col-sm-10">
                                    <input id="txt_aname" type="text" class="form-control" runat="server" placeholder="Name of Account being Opened" maxlength="80" />
                                </div>
                            </div>


                        </div>
                    </div>
                </div>




                <div class="col-md-6">
                    <div>
                        <div class="box-body">

                            <div class="form-group">
                                <label id="Label19" runat="server" class="col-sm-4 control-label" title="lbl1">Account Nature(L-1)</label>
                                <div class="col-sm-1" id="div1" runat="server">
                                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btn_acn_Click" />
                                </div>
                                <div class="col-sm-7">
                                    <input id="txt_led_nat" type="text" class="form-control" runat="server" placeholder="Account Nature" readonly="readonly" maxlength="20" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label11" runat="server" class="col-sm-4 control-label" title="lbl1">Ledger Group(L-2)</label>
                                <div class="col-sm-1" id="div3" runat="server">
                                    <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btn_mgr_Click" />
                                </div>
                                <div class="col-sm-7">
                                    <input id="txt_led_grp" type="text" class="form-control" runat="server" placeholder="Ledger Group" readonly="readonly" maxlength="50" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label2" runat="server" class="col-sm-4 control-label" title="lbl1">Ledger Schedule(L-3)</label>
                                <div class="col-sm-1" id="div9" runat="server">
                                    <asp:ImageButton ID="ImageButton9" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btn_sch_Click" />
                                </div>
                                <div class="col-sm-7">
                                    <input id="txt_led_Sch" type="text" class="form-control" runat="server" placeholder="Ledger Schedule" readonly="readonly" maxlength="50" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label71" runat="server" class="col-sm-4 control-label" title="lbl1">Show Only In</label>
                                <div class="col-sm-1" id="div19" runat="server">
                                    <asp:ImageButton ID="ImageButton_br" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btn_br_wise_Click" />
                                </div>
                                <div class="col-sm-5">
                                    <input id="txt_showin" type="text" class="form-control" runat="server" placeholder="Show Only in Selected Branch" readonly="readonly" maxlength="40" />
                                </div>
                                <div id="divAero" runat="server">
                                    <button type="submit" id="btnShowCustReg" class="btn-primary" runat="server" onserverclick="btnShowCustReg_ServerClick">Pick From Registration</button>
                                </div>
                            </div>


                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">

                            <div class="form-group">
                                <label id="Label13" runat="server" class="col-sm-4 control-label" title="lbl1">ERP A/c Code</label>
                                <div class="col-sm-8">
                                    <input id="txt_acode" type="text" class="form-control" runat="server" readonly="readonly" placeholder="A/c Code" maxlength="10" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label17" runat="server" class="col-sm-4 control-label" title="lbl1">Alias Name</label>
                                <div class="col-sm-8">
                                    <input id="txt_alias_name" type="text" class="form-control" runat="server" placeholder="Alias Name(If Reqd)" maxlength="60" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label3" runat="server" class="col-sm-4 control-label" title="lbl1">PAN Number</label>
                                <div class="col-sm-8">
                                    <input id="txt_pan_no" type="text" class="form-control" runat="server" placeholder="PAN Number" maxlength="10" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label69" runat="server" class="col-sm-4 control-label" title="lbl1">Opening Bal.(Home Curr.)</label>
                                <div class="col-sm-8">
                                    <input id="txt_balop" type="text" class="form-control" runat="server" placeholder="Opening Balance" maxlength="15" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>




                <section class="col-lg-12 connectedSortable" id="AllTabs" runat="server">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">

                                <li><a href="#DescTab1" id="tab1" runat="server" aria-controls="DescTab1" role="tab" data-toggle="tab">Address Details</a></li>
                                <li><a href="#DescTab2" id="tab2" runat="server" aria-controls="DescTab2" role="tab" data-toggle="tab">Contact,VAT/GST Details</a></li>
                                <li><a href="#DescTab3" id="tab3" runat="server" aria-controls="DescTab3" role="tab" data-toggle="tab">Bank & Credit Details</a></li>
                                <li><a href="#DescTab4" id="tab4" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">General Info.</a></li>
                                <li><a href="#DescTab5" id="tab5" runat="server" aria-controls="DescTab5" role="tab" data-toggle="tab">Finance Info.</a></li>
                                <li><a href="#DescTab6" id="tab6" runat="server" aria-controls="DescTab6" role="tab" data-toggle="tab">UDF Data</a></li>
                                <li><a href="#DescTab7" id="tab7" runat="server" aria-controls="DescTab7" role="tab" data-toggle="tab">Native_Details</a></li>
                                <li><a href="#DescTab8" id="tab8" runat="server" aria-controls="DescTab8" role="tab" data-toggle="tab">Approval / Activation / Others</a></li>
                                <li>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <span style="font-size: small; color: black; text-align: right">(These Details are Applicable ONLY for Customers/Vendors/Affiliates)</span>
                                </li>


                            </ul>

                            <div class="tab-content">

                                <div role="tabpanel" class="tab-pane active" id="DescTab1">
                                    <div class="lbBody" style="overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">

                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">

                                                    <div class="form-group">
                                                        <label id="Label9" runat="server" class="col-sm-3 control-label" title="lbl1">Continent</label>
                                                        <div class="col-sm-1" id="div4" runat="server">
                                                            <asp:ImageButton ID="ImageButton4" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btn_conti_Click" />
                                                        </div>
                                                        <div class="col-sm-8">
                                                            <input id="txt_cont_name" type="text" class="form-control" runat="server" readonly="readonly" placeholder="Continent" maxlength="30" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label1" runat="server" class="col-sm-3 control-label" title="lbl1">Country</label>
                                                        <div class="col-sm-1" id="div10" runat="server">
                                                            <asp:ImageButton ID="ImageButton10" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btn_ctry_Click" />
                                                        </div>
                                                        <div class="col-sm-8">
                                                            <input id="txt_ctry_name" type="text" class="form-control" runat="server" readonly="readonly" placeholder="Country_Name (Select Continent first)" maxlength="50" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label12" runat="server" class="col-sm-3 control-label" title="lbl1">State /Province*</label>
                                                        <div class="col-sm-1" id="div2" runat="server">
                                                            <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btn_stat_Click" />
                                                        </div>
                                                        <div class="col-sm-6">
                                                            <input id="txt_stat_name" type="text" class="form-control" runat="server" placeholder="State_Name (Select Country first)" readonly="readonly" maxlength="50" />
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <input id="txt_stat_code" type="text" class="form-control" runat="server" placeholder="Code" readonly="readonly" maxlength="50" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label25" runat="server" class="col-sm-3 control-label" title="lbl1">District</label>
                                                        <div class="col-sm-1" id="div8" runat="server">
                                                            <asp:ImageButton ID="ImageButton8" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btn_dist_Click" />
                                                        </div>
                                                        <div class="col-sm-8">
                                                            <input id="txt_dist_name" type="text" class="form-control" runat="server" placeholder="District, Pls Select State" readonly="readonly" maxlength="30" />
                                                        </div>
                                                    </div>


                                                    <div class="form-group">
                                                        <label id="Label21" runat="server" class="col-sm-3 control-label" title="lbl1">Industry</label>
                                                        <div class="col-sm-1" id="div7" runat="server">
                                                            <asp:ImageButton ID="ImageButton7" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btn_zone_Click" />
                                                        </div>
                                                        <div class="col-sm-8">
                                                            <input id="txt_zone_name" type="text" class="form-control" runat="server" readonly="readonly" placeholder="Industry" maxlength="30" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label72" runat="server" class="col-sm-3 control-label" title="lbl1">Sales Segment</label>
                                                        <div class="col-sm-1" id="div6" runat="server">
                                                            <asp:ImageButton ID="ImageButton5" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btn_segm_Click" />
                                                        </div>
                                                        <div class="col-sm-8">
                                                            <input id="txt_segm_name" type="text" class="form-control" runat="server" readonly="readonly" placeholder="Sales Segments(Govt/Pvt/NGO/Edu/FMCG)" maxlength="30" />
                                                        </div>
                                                    </div>


                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">

                                                    <div class="form-group">
                                                        <label id="Label18" runat="server" class="col-sm-4 control-label" title="lbl1">Address (Line1)</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_addr_1" type="text" class="form-control" runat="server" placeholder="Address (Line1)" maxlength="90" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label22" runat="server" class="col-sm-4 control-label" title="lbl1">Address (Line2)</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_addr_2" type="text" class="form-control" runat="server" placeholder="Address (Line1)" maxlength="60" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label23" runat="server" class="col-sm-4 control-label" title="lbl1">Address (Line3)</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_addr_3" type="text" class="form-control" runat="server" placeholder="Address (Line3)" maxlength="60" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label4" runat="server" class="col-sm-4 control-label" title="lbl1">Address (Line4)</label>
                                                        <div class="col-sm-3">
                                                            <input id="txt_addr_4" type="text" class="form-control" runat="server" placeholder="Address (Line4) max length 30 char" maxlength="30" />
                                                        </div>

                                                        <label id="Label84" runat="server" class="col-sm-2 control-label" title="lbl1">Pin Code</label>
                                                        <div class="col-sm-3">
                                                            <input id="txtPinCode" type="text" class="form-control" runat="server" placeholder="Pin Code" maxlength="30" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label64" runat="server" class="col-sm-4 control-label" title="lbl1">Native_Language_Name</label>
                                                        <div class="col-sm-8">
                                                            <input id="txtnlaname" type="text" class="form-control" runat="server" placeholder="Party Name in Native Language" maxlength="90" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label65" runat="server" class="col-sm-4 control-label" title="lbl1">Native_Language_Add</label>
                                                        <div class="col-sm-8">
                                                            <input id="txtnladdr" type="text" class="form-control" runat="server" placeholder="Party Address in Native Language" autocomplete="off" maxlength="90" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane active" id="DescTab2">
                                    <div class="lbBody" style="overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">

                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">

                                                    <div class="form-group">
                                                        <label id="Label24" runat="server" class="col-sm-3 control-label" title="lbl1">Telephone #</label>

                                                        <div class="col-sm-8">
                                                            <input id="txt_tel_no" type="text" class="form-control" runat="server" placeholder="Telephone #" maxlength="20" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label26" runat="server" class="col-sm-3 control-label" title="lbl1">Email-ID (Accts)*</label>

                                                        <div class="col-sm-8">
                                                            <input id="txt_mail_1" type="text" class="form-control" runat="server" placeholder="Email-id(1)" maxlength="120" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label20" runat="server" class="col-sm-3 control-label" title="lbl1">Email-ID (Sales)</label>

                                                        <div class="col-sm-8">
                                                            <input id="txt_mail_2" type="text" class="form-control" runat="server" placeholder="Email-id(2)" maxlength="120" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label8" runat="server" class="col-sm-3 control-label" title="lbl1">Contact Person*</label>

                                                        <div class="col-sm-8">
                                                            <input id="txt_cont_pers" type="text" class="form-control" runat="server" placeholder="Contact Person" maxlength="30" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label14" runat="server" class="col-sm-3 control-label" title="lbl1">Contact Mobile</label>

                                                        <div class="col-sm-8">
                                                            <input id="txt_cont_no" type="text" class="form-control" runat="server" placeholder="Contact Mobile" maxlength="10" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label87" runat="server" class="col-sm-2 control-label" title="lbl1">Payment Terms</label>
                                                        <div class="col-sm-1" id="div17" runat="server">
                                                            <asp:ImageButton ID="btnPayTerms" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnPayTerms_Click" />
                                                        </div>
                                                        <div class="col-sm-8">
                                                            <input id="txtPaymentTerms" type="text" class="form-control" runat="server" placeholder="Contact Terms" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">

                                                    <div class="form-group">
                                                        <label id="Label32" runat="server" class="col-sm-4 control-label" title="lbl1">VAT/GST NO.*</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_gst_no" type="text" class="form-control" runat="server" placeholder="VAT/GST NO." maxlength="15" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label31" runat="server" class="col-sm-4 control-label" title="lbl1">CIN NO.</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_cin_no" type="text" class="form-control" runat="server" placeholder="CIN NO." maxlength="21" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label10" runat="server" class="col-sm-4 control-label" title="lbl1">Web Site</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_web_site" type="text" class="form-control" runat="server" placeholder="Web Site" maxlength="40" />
                                                        </div>
                                                    </div>


                                                    <div class="form-group">
                                                        <label id="Label33" runat="server" class="col-sm-4 control-label" title="lbl1">Over Seas A/c </label>
                                                        <div class="col-sm-3">
                                                            <select id="txt_over_sea" runat="server" class="form-control">
                                                                <option value="Y">Y</option>
                                                                <option value="N">N</option>
                                                            </select>
                                                        </div>

                                                        <label id="Label15" runat="server" class="col-sm-3 control-label" title="lbl1">Reverse charge</label>
                                                        <div class="col-sm-2">
                                                            <select id="txt_rev_chg" runat="server" class="form-control">
                                                                <option value="Y">Y</option>
                                                                <option value="N">N</option>
                                                            </select>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label85" runat="server" class="col-sm-4 control-label" title="lbl1">TDS</label>
                                                        <div class="col-sm-8">
                                                            <select id="txt_tds_codes" runat="server" class="form-control">
                                                                <option value="193">193</option>
                                                                <option value="194">194</option>
                                                                <option value="194A">194A</option>
                                                                <option value="194B">194B</option>
                                                                <option value="194BB">194BB</option>
                                                                <option value="194C">194C</option>
                                                                <option value="194D">194D</option>
                                                                <option value="194Q">194Q</option>
                                                            </select>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label88" runat="server" class="col-sm-3 control-label" title="lbl1">Contract Terms</label>
                                                        <div class="col-sm-1" id="div20" runat="server">
                                                            <asp:ImageButton ID="btnContTerms" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnContTerms_Click" />
                                                        </div>
                                                        <div class="col-sm-8">
                                                            <input id="txtContrTerms" type="text" class="form-control" runat="server" placeholder="Contact Terms" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane active" id="DescTab3">
                                    <div class="lbBody" style="overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">

                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">

                                                    <div class="form-group">
                                                        <label id="Label34" runat="server" class="col-sm-4 control-label" title="lbl1">Bank Name</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_bank_name" type="text" class="form-control" runat="server" placeholder="Bank Name" maxlength="50" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label35" runat="server" class="col-sm-4 control-label" title="lbl1">Nature of A/c</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_ac_nat" type="text" class="form-control" runat="server" placeholder="Max length 2 Characters" maxlength="2" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label36" runat="server" class="col-sm-4 control-label" title="lbl1">Bank Address</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_bank_addr" type="text" class="form-control" runat="server" placeholder="Bank Address" maxlength="50" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label6" runat="server" class="col-sm-4 control-label" title="lbl1">Bank A/c No.</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_bank_acno" type="text" class="form-control" runat="server" placeholder="Bank A/c No." maxlength="30" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label28" runat="server" class="col-sm-4 control-label" title="lbl1">Bank IFSC Code</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_bank_ifsc" type="text" class="form-control" runat="server" placeholder="Bank IFSC Code" maxlength="25" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label29" runat="server" class="col-sm-4 control-label" title="lbl1">Bank Swift Code</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_bank_swift" type="text" class="form-control" runat="server" placeholder="Bank Swift Code" maxlength="25" />
                                                        </div>
                                                    </div>


                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">

                                                    <div class="form-group">
                                                        <label id="Label30" runat="server" class="col-sm-4 control-label" title="lbl1">Bank Contact No.</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_bank_tel" type="text" class="form-control" runat="server" placeholder="Bank Contact No." maxlength="30" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label5" runat="server" class="col-sm-4 control-label" title="lbl1">Payment Days</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_pymt_days" type="text" class="form-control" runat="server" placeholder="Payment Terms (Days)" maxlength="4" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label16" runat="server" class="col-sm-4 control-label" title="lbl1">Grace Days</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_grc_days" type="text" class="form-control" runat="server" placeholder="Grace Days Over Payment terms" maxlength="4" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label37" runat="server" class="col-sm-4 control-label" title="lbl1">Credit Limit</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_cred_lmt" type="text" class="form-control" runat="server" placeholder="Credit Limit (Amount)" maxlength="10" />
                                                        </div>
                                                    </div>


                                                    <div class="form-group">
                                                        <label id="Label70" runat="server" class="col-sm-4 control-label" title="lbl1">Opening Bal.(Forex)</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_balop_fx" type="text" class="form-control" runat="server" placeholder="Opening Balance(Forex)" maxlength="15" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label7" runat="server" class="col-sm-4 control-label" title="lbl1">Composition A/c</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_comp_act" type="text" class="form-control" runat="server" placeholder="Composition A/c (Y/N)" maxlength="1" />
                                                        </div>
                                                    </div>


                                                </div>
                                            </div>
                                        </div>


                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane active" id="DescTab4">
                                    <div class="lbBody" style="overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">

                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">
                                                    <%--    <div class="form-group">
                                                    <label id="Label99" runat="server" class="col-sm-4 control-label" title="lbl1">Service Vendor</label>
                                                        
                                                    <div class="col-sm-8">
                                                        Contractor/Job Worker/Transporter
                                                           <input id="chkactype" type="checkbox" class="checkbox" runat="server" />
                                                    </div>
                                                </div>--%>
                                                    <div class="form-group">
                                                        <label id="Label54" runat="server" class="col-sm-4 control-label" title="lbl1">Freight Delivery Terms</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_dlv_term" type="text" class="form-control" runat="server" placeholder="Delivery Terms" maxlength="50" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label55" runat="server" class="col-sm-4 control-label" title="lbl1">COD Terms</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_cod_term" type="text" class="form-control" runat="server" placeholder="COD Terms max len 20 char" maxlength="20" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label56" runat="server" class="col-sm-4 control-label" title="lbl1">Important Notes</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_imp_note" type="text" class="form-control" runat="server" placeholder="Important Notes" maxlength="50" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label57" runat="server" class="col-sm-4 control-label" title="lbl1">Way Bill Reqd</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_way_bill" type="text" class="form-control" runat="server" placeholder="Way Bill Reqd" maxlength="1" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label58" runat="server" class="col-sm-4 control-label" title="lbl1">Other Notes</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_oth_note" type="text" class="form-control" runat="server" placeholder="Other Notes" maxlength="50" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label66" runat="server" class="col-sm-4 control-label" title="lbl1">Owner Details</label>
                                                        <div class="col-sm-8">
                                                            <input id="txtowner" type="text" class="form-control" runat="server" placeholder="Owner Name" maxlength="50" />
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">


                                                    <div class="form-group">
                                                        <label id="Label59" runat="server" class="col-sm-4 control-label" title="lbl1">D/License No.</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_drg_lic" type="text" class="form-control" runat="server" placeholder="D/License No." maxlength="30" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label60" runat="server" class="col-sm-4 control-label" title="lbl1">Vendor Code</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_vend_code" type="text" class="form-control" runat="server" placeholder="Vendor Code" maxlength="15" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label61" runat="server" class="col-sm-4 control-label" title="lbl1">Old Code</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_old_code" type="text" class="form-control" runat="server" placeholder="Old Code" maxlength="10" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label62" runat="server" class="col-sm-3 control-label" title="lbl1">Salesman_linked_to_this_party</label>
                                                        <div class="col-sm-1" id="div15" runat="server">
                                                            <asp:ImageButton ID="btnSalesgrp" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnSalesgrp_Click" />
                                                        </div>
                                                        <div class="col-sm-8">
                                                            <input id="txt_sal_Grp" type="text" class="form-control" runat="server" maxlength="20" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label63" runat="server" class="col-sm-4 control-label" title="lbl1">Cust Group</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_cust_grp" type="text" class="form-control" runat="server" placeholder="Customer Grp" maxlength="30" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label67" runat="server" class="col-sm-4 control-label" title="lbl1">Owner Email ID</label>
                                                        <div class="col-sm-8">
                                                            <input id="txtownerid" type="text" class="form-control" runat="server" placeholder="Owner Email ID" maxlength="50" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label86" runat="server" class="col-sm-4 control-label" title="lbl1">Old_branch</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_affiliate" type="text" class="form-control" runat="server" placeholder="Old branch" maxlength="10" />
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>


                                    </div>
                                </div>


                                <div role="tabpanel" class="tab-pane active" id="DescTab5">
                                    <div class="lbBody" style="overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">

                                        <div class="col-md-12" id="divCan" runat="server">

                                            <div>
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <label id="Label38" runat="server" class="col-sm-2 control-label" title="lbl1">TDS %</label>
                                                        <div class="col-sm-1">
                                                            <input id="txt_TDS_perc" type="text" class="form-control" runat="server" placeholder="TDS %" maxlength="5" />
                                                        </div>
                                                        <label id="Label39" runat="server" class="col-sm-1 control-label" title="lbl1">TCS %</label>
                                                        <div class="col-sm-1">
                                                            <select id="tcsApplicable" runat="server" class="form-control">
                                                                <option value="Y">Y</option>
                                                                <option value="N">N</option>
                                                            </select>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <input id="txt_TCS_perc" type="text" class="form-control" runat="server" placeholder="TCS %" maxlength="5" />
                                                        </div>
                                                        <label id="Label40" runat="server" class="col-sm-2 control-label" title="lbl1">Cash Disc %</label>
                                                        <div class="col-sm-1">
                                                            <input id="txt_cash_disc" type="text" class="form-control" runat="server" placeholder="Cash Disc %" maxlength="5" />
                                                        </div>
                                                        <label id="Label41" runat="server" class="col-sm-2 control-label" title="lbl1">Sales Disc %</label>
                                                        <div class="col-sm-1">
                                                            <input id="txt_sale_disc" type="text" class="form-control" runat="server" placeholder="Sales Disc %" maxlength="5" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label42" runat="server" class="col-sm-1 control-label" title="lbl1">TDS_A/c_Code</label>
                                                        <div class="col-sm-1" id="div14" runat="server">
                                                            <asp:ImageButton ID="btnTDSAccount" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnTDSAccount_Click" />
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <input id="txt_tds_Ac" type="text" class="form-control" runat="server" placeholder="000000" maxlength="6" />
                                                        </div>
                                                        <label id="Label43" runat="server" class="col-sm-2 control-label" title="lbl1">VAT/GST Rating</label>
                                                        <div class="col-sm-1">
                                                            <input id="txt_gst_rating" type="text" class="form-control" runat="server" placeholder="1-100" maxlength="6" />
                                                        </div>
                                                        <label id="Label44" runat="server" class="col-sm-2 control-label" title="lbl1">NON GST A/c</label>
                                                        <div class="col-sm-1">
                                                            <input id="txt_non_gst" type="text" class="form-control" runat="server" placeholder="Y/N" maxlength="1" />
                                                        </div>
                                                        <label id="Label45" runat="server" class="col-sm-2 control-label" title="lbl1">GST Expense</label>
                                                        <div class="col-sm-1">
                                                            <input id="txt_gst_Exp" type="text" class="form-control" runat="server" placeholder="Y/N" maxlength="1" />
                                                        </div>

                                                    </div>
                                                </div>


                                            </div>


                                        </div>

                                        <div class="col-md-12" id="div5" runat="server">
                                            <div>
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <label id="Label46" runat="server" class="col-sm-2 control-label" title="lbl1">Cost Center</label>
                                                        <div class="col-sm-1">
                                                            <input id="txt_cost_cent" type="text" class="form-control" runat="server" placeholder="Cost Cent(Y/N)" maxlength="1" />
                                                        </div>
                                                        <label id="Label47" runat="server" class="col-sm-2 control-label" title="lbl1">Delv Days</label>
                                                        <div class="col-sm-1">
                                                            <input id="txt_dlv_days" type="text" class="form-control" runat="server" placeholder="Dlv Days" maxlength="5" />
                                                        </div>
                                                        <label id="Label48" runat="server" class="col-sm-2 control-label" title="lbl1">% Intt on Due Bills</label>
                                                        <div class="col-sm-1">
                                                            <input id="txt_intt_bill" type="text" class="form-control" runat="server" placeholder="%" maxlength="6" />
                                                        </div>
                                                        <label id="Label49" runat="server" class="col-sm-2 control-label" title="lbl1">% Extra on SO Qty</label>
                                                        <div class="col-sm-1">
                                                            <input id="txt_so_tolr" type="text" class="form-control" runat="server" placeholder="%" maxlength="6" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label50" runat="server" class="col-sm-2 control-label" title="lbl1">Sales Mail</label>
                                                        <div class="col-sm-1">
                                                            <input id="txt_sale_mail" type="text" class="form-control" runat="server" placeholder="Y/N" maxlength="1" />
                                                        </div>
                                                        <label id="Label51" runat="server" class="col-sm-2 control-label" title="lbl1">Hub Stock</label>
                                                        <div class="col-sm-1">
                                                            <select id="txt_hub_stk" runat="server" class="form-control">
                                                                <option value="N">N</option>
                                                                <option value="Y">Y</option>
                                                            </select>
                                                        </div>
                                                        <label id="Label52" runat="server" class="col-sm-2 control-label" title="lbl1">Multi Ord Inv</label>
                                                        <div class="col-sm-1">
                                                            <input id="txt_mult_ord" type="text" class="form-control" runat="server" placeholder="Y/N" maxlength="1" />
                                                        </div>
                                                        <label id="Label53" runat="server" class="col-sm-2 control-label" title="lbl1">Ins.Cover on Inv</label>
                                                        <div class="col-sm-1">
                                                            <input id="txt_ins_conv" type="text" class="form-control" runat="server" placeholder="Y/N" maxlength="1" />
                                                        </div>

                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label90" runat="server" class="col-sm-2 control-label" title="lbl1">Revise P.O</label>
                                                        <div class="col-sm-1">
                                                            <select id="txtRevisePO" runat="server" class="form-control">
                                                                <option value="N">N</option>
                                                                <option value="Y">Y</option>
                                                            </select>
                                                        </div>

                                                        <label id="Label89" runat="server" class="col-sm-2 control-label" title="lbl1">COC No (Print on invoice).</label>
                                                        <div class="col-sm-1">
                                                            <select id="txtCOCNumber" runat="server" class="form-control">
                                                                <option value="N">N</option>
                                                                <option value="Y">Y</option>
                                                            </select>
                                                        </div>

                                                        <label id="Label91" runat="server" class="col-sm-2 control-label" title="lbl1">Is this a Transporter</label>
                                                        <div class="col-sm-1">
                                                            <select id="txtTpt" runat="server" class="form-control">
                                                                <option value="N">N</option>
                                                                <option value="Y">Y</option>
                                                            </select>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane active" id="DescTab6">
                                    <div class="lbBody" style="overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <asp:GridView ID="sg4" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="1200px" Font-Size="13px"
                                            AutoGenerateColumns="False" OnRowDataBound="sg4_RowDataBound"
                                            OnRowCommand="sg4_RowCommand">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Add</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg4_btnadd" runat="server" CommandName="SG4_ROW_ADD" ImageAlign="Middle" ImageUrl="../tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Del</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg4_btnrmv" runat="server" CommandName="SG4_RMV" ImageUrl="../tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="sg4_srno" HeaderText="Sr.No." />

                                                <asp:TemplateField>
                                                    <HeaderTemplate>UDF_Field</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg4_t1" runat="server" Text='<%#Eval("sg4_t1") %>' Width="100%" ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>UDF_Value</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg4_t2" runat="server" Text='<%#Eval("sg4_t2") %>' Width="100%" MaxLength="40"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#1797C0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        </asp:GridView>
                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane active" id="DescTab7">
                                    <div class="lbBody" id="gridDiv" style="color: White; height: 200px; overflow: hidden; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <asp:GridView ID="sg1" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="1500px" Font-Size="Smaller"
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
                                                <asp:BoundField DataField="sg1_h10" HeaderText="sg1_h10" />

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Add</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btnadd" runat="server" CommandName="SG1_ROW_ADD" ImageAlign="Middle" ImageUrl="../tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Del</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btnrmv" runat="server" CommandName="SG1_RMV" ImageUrl="../tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="sg1_srno" HeaderText="sg1_srno" />
                                                <asp:BoundField DataField="sg1_f1" HeaderText="sg1_f1" />
                                                <asp:BoundField DataField="sg1_f2" HeaderText="sg1_f2" />
                                                <asp:BoundField DataField="sg1_f3" HeaderText="sg1_f3" />
                                                <asp:BoundField DataField="sg1_f4" HeaderText="sg1_f4" />
                                                <asp:BoundField DataField="sg1_f5" HeaderText="sg1_f5" />

                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t1</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t1" runat="server" Text='<%#Eval("sg1_t1") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Dt</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btndt" runat="server" CommandName="SG1_ROW_DT" ImageAlign="Middle" ImageUrl="../tej-base/images/Btn_addn.png" Width="20px" ToolTip="Date" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t2</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t2" runat="server" Text='<%#Eval("sg1_t2") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <%--                                                        <asp:TemplateField>
                                                            <HeaderTemplate>sg1_t2</HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="sg1_t2" runat="server" Text='<%#Eval("sg1_t2") %>' Width="100%"></asp:TextBox>
                                                                <asp:MaskedEditExtender ID="Maskedit2" runat="server" Mask="99/99/9999"
                                                                    MaskType="Date" TargetControlID="sg1_t2" />
                                                                <asp:CalendarExtender ID="txtvchdate_CalendarExtender2" runat="server"
                                                                    Enabled="True" TargetControlID="sg1_t2"
                                                                    Format="dd/MM/yyyy">
                                                                </asp:CalendarExtender>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>--%>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t3</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t3" runat="server" Text='<%#Eval("sg1_t3") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t4</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t4" runat="server" Text='<%#Eval("sg1_t4") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t5</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t5" runat="server" Text='<%#Eval("sg1_t5") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t6</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t6" runat="server" Text='<%#Eval("sg1_t6") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t7</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t7" runat="server" Text='<%#Eval("sg1_t7") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t8</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t8" runat="server" Text='<%#Eval("sg1_t8") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t9</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t9" runat="server" Text='<%#Eval("sg1_t9") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Tcode</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btntax" runat="server" CommandName="SG1_ROW_TAX" ImageAlign="Middle" ImageUrl="../tej-base/images/Btn_addn.png" Width="20px" ToolTip="Choose Tax" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t10</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t10" runat="server" Text='<%#Eval("sg1_t10") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t11</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t11" runat="server" Text='<%#Eval("sg1_t11") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t12</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t12" runat="server" Text='<%#Eval("sg1_t12") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t13</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t13" runat="server" Text='<%#Eval("sg1_t13") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t14</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t14" runat="server" Text='<%#Eval("sg1_t14") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t15</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t15" runat="server" Text='<%#Eval("sg1_t15") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t16</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t16" runat="server" Text='<%#Eval("sg1_t16") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                            </Columns>
                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#1797C0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        </asp:GridView>
                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane active" id="DescTab8">
                                    <div class="col-md-6">
                                        <div>
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <label id="Label73" runat="server" class="col-sm-3 control-label" title="lbl1">Approved_By</label>
                                                    <div class="col-sm-1" id="div16" runat="server">
                                                        <asp:ImageButton ID="ImageButton16" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="ImageButton16_Click" />
                                                    </div>
                                                    <div class="col-sm-8">
                                                        <input id="txt_appby" type="text" class="form-control" runat="server" placeholder="Approved By" readonly="readonly" maxlength="15" />
                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <label id="Label74" runat="server" class="col-sm-4 control-label" title="lbl1">Approved_On</label>

                                                    <div class="col-sm-8">
                                                        <input id="txt_appdt" type="text" class="form-control" runat="server" placeholder="Approved On" readonly="readonly" maxlength="10" />
                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <label id="Label75" runat="server" class="col-sm-3 control-label" title="lbl1">Deactivated_By</label>
                                                    <div class="col-sm-1" id="div18" runat="server">
                                                        <asp:ImageButton ID="ImageButton18" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="ImageButton18_Click" />
                                                    </div>
                                                    <div class="col-sm-8">
                                                        <input id="txt_deacby" type="text" class="form-control" runat="server" placeholder="Deactivated By" maxlength="15" readonly="readonly" />
                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <label id="Label76" runat="server" class="col-sm-4 control-label" title="lbl1">Deactivated_On</label>

                                                    <div class="col-sm-8">
                                                        <input id="txt_deacDt" type="text" class="form-control" runat="server" readonly="readonly" placeholder="Deactivated_On" maxlength="10" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-6">
                                        <div>
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <label id="Label77" runat="server" class="col-sm-2 control-label" title="lbl1">Web_Login_Pwd</label>
                                                    <div class="col-sm-10">
                                                        <input id="txtWebLogin" type="text" class="form-control" runat="server" placeholder="Web login Password (Max 15 char)" maxlength="15" />
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label id="Label78" runat="server" class="col-sm-2 control-label" title="lbl1">Markup</label>
                                                    <div class="col-sm-2">
                                                        <input id="txtMarkup" type="text" class="form-control" runat="server" placeholder="Markup" maxlength="15" />
                                                    </div>
                                                    <label id="Label79" runat="server" class="col-sm-2 control-label" title="lbl1">Min_Markup</label>
                                                    <div class="col-sm-2">
                                                        <input id="txtMinMarkup" type="text" class="form-control" runat="server" placeholder="Markup" maxlength="15" />
                                                    </div>
                                                    <label id="Label80" runat="server" class="col-sm-2 control-label" title="lbl1">Max_Markup</label>
                                                    <div class="col-sm-2">
                                                        <input id="txtMaxMarkup" type="text" class="form-control" runat="server" placeholder="Markup" maxlength="15" />
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label id="Label81" runat="server" class="col-sm-2 control-label" title="lbl1">Currency</label>
                                                    <div class="col-sm-1" id="div11" runat="server">
                                                        <asp:ImageButton ID="ImageButton6" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="ImageButton6_Click" />
                                                    </div>
                                                    <div class="col-sm-9">
                                                        <input id="txtCurrency" type="text" class="form-control" runat="server" placeholder="Currency" readonly="readonly" maxlength="15" />
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label id="Label82" runat="server" class="col-sm-2 control-label" title="lbl1">Pay_Terms[Insoft]</label>
                                                    <div class="col-sm-1" id="div12" runat="server">
                                                        <asp:ImageButton ID="ImageButton11" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="ImageButton11_Click" />
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <input id="txtPayTerms" type="text" class="form-control" runat="server" placeholder="Pay Terms" readonly="readonly" maxlength="15" />
                                                    </div>

                                                    <label id="Label83" runat="server" class="col-sm-2 control-label" title="lbl1">Tax_Code</label>
                                                    <div class="col-sm-1" id="div13" runat="server">
                                                        <asp:ImageButton ID="ImageButton12" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="ImageButton12_Click" />
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <input id="txtTaxCode" type="text" class="form-control" runat="server" placeholder="Tax Code" readonly="readonly" maxlength="15" />
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

                <div class="col-md-12">
                    <div>
                        <div class="box-body">
                            <table>
                                <tr>
                                    <td>
                                        <asp:FileUpload ID="Attch" runat="server" Visible="true" onchange="submitFile()" /></td>
                                    <td>
                                        <asp:TextBox ID="txtAttch" runat="server" MaxLength="100" placeholder="Path Upto 100 Char"></asp:TextBox></td>
                                    <td>
                                        <asp:Label ID="lblUpload" runat="server"></asp:Label></td>
                                    <td>
                                        <asp:Button ID="btnView1" runat="server" CssClass="btn-success" Text="View" OnClick="btnView1_Click" Visible="false" /></td>
                                    <td>
                                        <asp:ImageButton ID="btnDwnld1" runat="server" ImageUrl="~/tej-base/images/Save.png" OnClick="btnDwnld1_Click" Visible="false" /></td>
                                    <td>
                                        <asp:Label ID="Label27" runat="server" Text="&nbsp &nbsp  Image Link (Please Link Correct File upto 3MB Size) &nbsp &nbsp"></asp:Label></td>
                                </tr>
                            </table>
                            <asp:Button ID="btnAtt" runat="server" Text="Attachment" OnClick="btnAtt_Click" Width="134px" Style="display: none" />
                            <asp:Label ID="lblShow" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>

                <%--                <div class="col-md-12">
                    <div>
                        <div class="box-body">
                            <table>
                                <tr>
                                    <td><asp:FileUpload ID="Attch" runat="server" Visible="true" onchange="submitFile()" /></td>
                                    <td><asp:TextBox ID="txtAttch" runat="server" MaxLength="100" placeholder="Path Upto 100 Char" ></asp:TextBox></td>
                                    <td><asp:Label ID="lblUpload" runat="server" Style="display: none"></asp:Label></td>
                                    <td><asp:ImageButton ID="btnView1" runat="server" ImageUrl="~/tej-base/images/preview-file.png" OnClick="btnView1_Click" Visible="false" /></td>
                                    <td><asp:ImageButton ID="btnDwnld1" runat="server" ImageUrl="~/tej-base/images/Save.png" OnClick="btnDwnld1_Click" Visible="false" /></td>
                                    <td><asp:Label ID="Label27" runat="server" Text=" Please Link Correct File upto 3MB Size ." ></asp:Label></td>
                                </tr>
                            </table>

                            <asp:Button ID="btnAtt" runat="server" Text="Attachment" OnClick="btnAtt_Click" Width="134px" Style="display: none" />
                            <asp:Label ID="lblShow" runat="server"></asp:Label>

                        </div>
                    </div>
                </div>--%>
            </div>
        </section>
    </div>

    <asp:Button ID="btnhideF" runat="server" OnClick="btnhideF_Click" Style="display: none" />
    <asp:Button ID="btnhideF_s" runat="server" OnClick="btnhideF_s_Click" Style="display: none" />
    <asp:HiddenField ID="hffield" runat="server" />
    <asp:HiddenField ID="doc_nf" runat="server" />
    <asp:HiddenField ID="doc_df" runat="server" />
    <asp:HiddenField ID="doc_vty" runat="server" />
    <asp:HiddenField ID="doc_GST" runat="server" />
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
    </script>
    <asp:HiddenField ID="TabName" runat="server" />
    <asp:HiddenField ID="hf_regis" runat="server" />
</asp:Content>
