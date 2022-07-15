<%@ Page Title="" Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="om_crm_Contact" CodeFile="om_crm_Contact.aspx.cs" %>

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
                    <td><asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label></td>
                    <td style="text-align:right">
                        <button type="submit" id="btnnew" class="btn btn-info" style="width: 100px;" runat="server" accesskey="N" onserverclick="btnnew_ServerClick"><u>N</u>ew</button>
                        <button type="submit" id="btnedit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="i" onserverclick="btnedit_ServerClick">Ed<u>i</u>t</button>
                        <button type="submit" id="btnsave" class="btn btn-info" style="width: 100px;" runat="server" accesskey="s" onserverclick="btnsave_ServerClick"><u>S</u>ave</button>
                        <button type="submit" id="btnprint" class="btn btn-info" style="width: 100px;" runat="server" accesskey="P" onserverclick="btnprint_ServerClick"><u>P</u>rint</button>
                        <button type="submit" id="btndel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="l" onserverclick="btndel_ServerClick">De<u>l</u>ete</button>
                        <button type="submit" id="btnlist" class="btn btn-info" style="width: 100px;" runat="server" accesskey="t" onserverclick="btnlist_ServerClick">Lis<u>t</u></button>
                        <button type="submit" id="btncancel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="c" onserverclick="btncancel_ServerClick"><u>C</u>ancel</button>
                        <button type="submit" id="btnexit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="X" onserverclick="btnexit_ServerClick">E<u>x</u>it</button>
                        <button type="submit" id="btnAtch" class="btn btn-info" style="width: 100px;" runat="server" onserverclick="btnAtch_ServerClick">Attachment</button>
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
                                <label id="Label19" runat="server" class="col-sm-4 control-label" title="lbl1">Contact Code</label>

                                <div class="col-sm-8">
                                    <input id="txt_ConsigneeCode" type="text" class="form-control" runat="server" placeholder="Contact Code" maxlength="6" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label15" runat="server" class="col-sm-4 control-label" title="lbl1">Contact Company</label>

                                <div class="col-sm-8">
                                    <input id="txt_consname" type="text" class="form-control" runat="server" placeholder="Contact Company" maxlength="80" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label13" runat="server" class="col-sm-4 control-label" title="lbl1">Contact Person</label>

                                <div class="col-sm-8">
                                    <input id="txt_contactperson" type="text" class="form-control" runat="server" placeholder="Contact Person" maxlength="50" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label17" runat="server" class="col-sm-3 control-label" title="lbl1">Contact Type</label>

                                <div class="col-sm-1" id="div1" runat="server">
                                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btn_type_Click" />
                                </div>

                                <div class="col-sm-8">
                                    <input id="txt_type" type="text" readonly="readonly" class="form-control" runat="server" placeholder="Type of Contact" maxlength="50" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">

                            <div class="form-group">
                                <label id="Label1" runat="server" class="col-sm-4 control-label" title="lbl1">Deals In</label>

                                <div class="col-sm-8">
                                    <input id="txt_dealin" type="text" class="form-control" runat="server" placeholder="Product Dealing In" maxlength="50" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label21" runat="server" class="col-sm-4 control-label" title="lbl1">Contact Of</label>

                                <div class="col-sm-8">
                                    <input id="txt_ourref" type="text" class="form-control" runat="server" placeholder="Our Person Reference" maxlength="50" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label20" runat="server" class="col-sm-3 control-label" title="lbl1">Reference_Name</label>
                                <div class="col-sm-1" id="div3" runat="server">
                                    <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btn_customer_Click" />
                                </div>
                                
                                 <div class="col-sm-8">
                                    <input id="txt_customer" type="text" readonly="readonly" class="form-control" runat="server" placeholder="Customer/Vendor Reference" maxlength="50" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label11" runat="server" class="col-sm-4 control-label" title="lbl1">Reference_Code</label>

                                <div class="col-sm-8">
                                    <input id="txt_custcode" type="text" readonly="readonly" class="form-control" runat="server" placeholder="Reference_Code" maxlength="50" />
                                </div>
                            </div>

                        </div>
                    </div>
                </div>

                <div class="col-md-12">
                    <div>
                        <div class="box-body">
                        </div>
                    </div>
                </div>
                <section class="col-lg-12 connectedSortable" id="AllTabs" runat="server">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">

                                <li><a href="#DescTab1" id="tab1" runat="server" aria-controls="DescTab1" role="tab" data-toggle="tab">Address Details</a></li>
                                <li><a href="#DescTab2" id="tab2" runat="server" aria-controls="DescTab2" role="tab" data-toggle="tab">Other Details</a></li>

                                <%-- <li><a href="#DescTab3" id="tab3" runat="server" aria-controls="DescTab3" role="tab" data-toggle="tab">Bank Details</a></li>
                                <li><a href="#DescTab4" id="tab4" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Control Dates/Others</a></li>
                                <!--<li><a href="#DescTab5" id="tab5" runat="server" aria-controls="DescTab5" role="tab" data-toggle="tab">Ctrl Details</a></li>
                                <li><a href="#DescTab6" id="tab6" runat="server" aria-controls="DescTab6" role="tab" data-toggle="tab">UDF Data</a></li>
                                <li><a href="#DescTab7" id="tab7" runat="server" aria-controls="DescTab7" role="tab" data-toggle="tab">Item Details</a></li>-->
                                <li><a href="#DescTab8" id="tab8" runat="server" aria-controls="DescTab8" role="tab" data-toggle="tab">Excise Registration Details</a></li>
                                 <li><a href="#DescTab9" id="tab9" runat="server" aria-controls="DescTab9" role="tab" data-toggle="tab">Sales Tax/ESI&PF</a></li>--%>
                            </ul>

                            <div class="tab-content">

                                <div role="tabpanel" class="tab-pane active" id="DescTab1">
                                    <div class="lbBody" style="height: 340px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">

                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">

                                                    <div class="form-group">
                                                        <label id="Label18" runat="server" class="col-sm-4 control-label" title="lbl1">Address (Line1)*</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_addr_1" type="text" class="form-control" runat="server" placeholder="Address (Line1)" maxlength="100" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label22" runat="server" class="col-sm-4 control-label" title="lbl1">Address (Line2)</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_addr_2" type="text" class="form-control" runat="server" placeholder="Address (Line2)" maxlength="70" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label9" runat="server" class="col-sm-4 control-label" title="lbl1">Address (Line3)</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_addr_3" type="text" class="form-control" runat="server" placeholder="Address (Line3)" maxlength="70" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label25" runat="server" class="col-sm-4 control-label" title="lbl1">Address(Line4)</label>

                                                        <div class="col-sm-8">
                                                            <input id="txt_addr_4" type="text" class="form-control" runat="server" placeholder="City" maxlength="70" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label12" runat="server" class="col-sm-3 control-label" title="lbl1">State Name*</label>
                                                        <div class="col-sm-1" id="div2" runat="server">
                                                            <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btn_stat_Click" />
                                                        </div>
                                                        <div class="col-sm-8">
                                                            <input id="txt_stat_name" type="text" class="form-control" runat="server" placeholder="State Name" readonly="readonly" maxlength="50" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">

                                                    <div class="form-group">
                                                        <label id="Label7" runat="server" class="col-sm-4 control-label" title="lbl1">Telphone No.*</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_telephonenno" type="text" class="form-control" runat="server" placeholder="Telephone Number" maxlength="38" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label2" runat="server" class="col-sm-4 control-label" title="lbl1">Fax No.</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_faxno" type="text" class="form-control" runat="server" placeholder="fax no." maxlength="38" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label3" runat="server" class="col-sm-4 control-label" title="lbl1">Mobile No.</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_mobile" type="text" class="form-control" runat="server" placeholder="Mobile Number" maxlength="10" />
                                                        </div>
                                                    </div>



                                                    <div class="form-group">
                                                        <label id="Label16" runat="server" class="col-sm-4 control-label" title="lbl1">Email Id*</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_email" type="email" class="form-control" runat="server" placeholder="Email Id" maxlength="50" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label10" runat="server" class="col-sm-3 control-label" title="lbl1">Country Name*</label>
                                                        <div class="col-sm-1" id="div6" runat="server">
                                                            <asp:ImageButton ID="ImageButton5" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btn_count_Click" />
                                                        </div>
                                                        <div class="col-sm-8">
                                                            <input id="txt_count_name" type="text" class="form-control" runat="server" placeholder="Country Name" readonly="readonly" maxlength="50" />
                                                        </div>
                                                    </div>



                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane active" id="DescTab2">
                                    <div class="lbBody" style="height: 340px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">

                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">

                                                    <div class="form-group">
                                                        <label id="Label24" runat="server" class="col-sm-4 control-label" title="lbl1">Sales Tax/TinNo.</label>

                                                        <div class="col-sm-8">
                                                            <input id="txt_sales_tin" type="text" class="form-control" runat="server" placeholder="Sales Tax/ Tin Number" maxlength="20" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label26" runat="server" class="col-sm-4 control-label" title="lbl1">Sales Tax/CSTNo.</label>

                                                        <div class="col-sm-8">
                                                            <input id="txt_sales_cst" type="text" class="form-control" runat="server" placeholder="Sales Tax/CST  Number" maxlength="20" />
                                                        </div>
                                                    </div>



                                                    <div class="form-group">
                                                        <label id="Label8" runat="server" class="col-sm-4 control-label" title="lbl1">Exc_div</label>

                                                        <div class="col-sm-8">
                                                            <input id="txt_exc_dev" type="text" class="form-control" runat="server" placeholder="exc_dev" maxlength="15" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label14" runat="server" class="col-sm-4 control-label" title="lbl1">Exc_Range</label>

                                                        <div class="col-sm-8">
                                                            <input id="txt_exc_range" type="text" class="form-control" maxlength="15" runat="server" placeholder="Exc_Range" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label37" runat="server" class="col-sm-4 control-label" title="lbl1">GST no.</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_gst" type="text" class="form-control" runat="server" placeholder="GST  Number" maxlength="20" />
                                                        </div>
                                                    </div>


                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">

                                                    <div class="form-group">
                                                        <label id="Label31" runat="server" class="col-sm-4 control-label" title="lbl1">Date</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_dt" type="text" class="form-control" runat="server" placeholder="Date for tin number" maxlength="50" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label32" runat="server" class="col-sm-4 control-label" title="lbl1">Date</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_dt1" type="text" class="form-control" runat="server" placeholder="Date for cst number" maxlength="20" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label4" runat="server" class="col-sm-4 control-label" title="lbl1">Extra Frt</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_extrafrt" type="text" class="form-control" runat="server" placeholder="Put 1 or 0" maxlength="7" />
                                                        </div>
                                                        <div class="form-group">
                                                            <label id="Label33" runat="server" class="col-sm-4 control-label" title="lbl1">PAN No.</label>
                                                            <div class="col-sm-8">
                                                                <input id="txt_pan" type="text" class="form-control" runat="server" placeholder="Pan Number" maxlength="10" />
                                                            </div>
                                                        </div>



                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                                <div role="tabpanel" class="tab-pane active" id="DescTab8">
                                    <div class="lbBody" style="height: 340px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">

                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">

                                                    <div class="form-group">
                                                        <label id="Label67" runat="server" class="col-sm-4 control-label" title="lbl1">Ecc Number</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_ecc_num" type="text" class="form-control" runat="server" placeholder="Ecc Number" maxlength="15" />
                                                        </div>
                                                    </div>


                                                    <div class="form-group">
                                                        <label id="Label68" runat="server" class="col-sm-4 control-label" title="lbl1">Exc Regn No</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_exc_regn_no" type="text" class="form-control" runat="server" placeholder="Exc Regn No" maxlength="15" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label69" runat="server" class="col-sm-4 control-label" title="lbl1">PLA No.</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_pla_no" type="text" class="form-control" runat="server" placeholder="PLA No." maxlength="50" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label70" runat="server" class="col-sm-4 control-label" title="lbl1">Serv.Tax No.</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_serv_no" type="text" class="form-control" runat="server" placeholder="Serv.Tax No." maxlength="40" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label71" runat="server" class="col-sm-4 control-label" title="lbl1">Range</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_range" type="text" class="form-control" runat="server" placeholder="Range" maxlength="15" />
                                                        </div>
                                                    </div>


                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">

                                                    <div class="form-group">
                                                        <label id="Label72" runat="server" class="col-sm-4 control-label" title="lbl1">Division</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_divi" type="text" class="form-control" runat="server" placeholder="Division" maxlength="30" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label73" runat="server" class="col-sm-4 control-label" title="lbl1">Address</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_address" type="text" class="form-control" runat="server" placeholder="Address" maxlength="100" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label74" runat="server" class="col-sm-4 control-label" title="lbl1">Commision Rate</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_rate" type="text" class="form-control" runat="server" placeholder="Commision Rate" maxlength="25" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label75" runat="server" class="col-sm-4 control-label" title="lbl1">Notification No.</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_noti" type="text" class="form-control" runat="server" placeholder="Notification No." maxlength="50" />
                                                        </div>
                                                    </div>


                                                </div>
                                            </div>
                                        </div>


                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane active" id="DescTab9">
                                    <div class="lbBody" style="height: 340px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">

                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">

                                                    <div class="form-group">
                                                        <label id="Label76" runat="server" class="col-sm-3 control-label" title="lbl1">Eco Act.Code</label>

                                                        <div class="col-sm-8">
                                                            <input id="txt_act_code" type="text" class="form-control" runat="server" placeholder="Economic Act.Code" maxlength="5" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label77" runat="server" class="col-sm-3 control-label" title="lbl1">PF RegnNo.</label>

                                                        <div class="col-sm-8">
                                                            <input id="txt_pf_no" type="text" class="form-control" runat="server" placeholder="PF RegnNo.(PF Code Provided by the PF department)" maxlength="30" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label78" runat="server" class="col-sm-3 control-label" title="lbl1">Estab.Code</label>

                                                        <div class="col-sm-8">
                                                            <input id="txt_estab_code" type="text" class="form-control" runat="server" placeholder="Estab.Code" maxlength="5" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label79" runat="server" class="col-sm-3 control-label" title="lbl1">ESI Regn No.</label>

                                                        <div class="col-sm-8">
                                                            <input id="txt_esi_regn" type="text" class="form-control" runat="server" placeholder="ESI Regn No.(ESI Code Provided by the ESI Regn No.)" maxlength="5" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label80" runat="server" class="col-sm-3 control-label" title="lbl1">TCS No.</label>

                                                        <div class="col-sm-8">
                                                            <input id="txt_tcs_no" type="text" class="form-control" runat="server" placeholder="TCS No.(On Scrap)" maxlength="5" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label81" runat="server" class="col-sm-3 control-label" title="lbl1">PAN No.</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_pan_no" type="text" class="form-control" runat="server" placeholder="PAN NO." maxlength="10" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label82" runat="server" class="col-sm-3 control-label" title="lbl1">LST/TIN NO.</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_tin_no" type="text" class="form-control" runat="server" placeholder="LST/TIN NO." maxlength="20" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label83" runat="server" class="col-sm-3 control-label" title="lbl1">DT.</label>
                                                        <div class="col-sm-8">
                                                            <input id="Txt_tin_dt" class="form-control" runat="server" placeholder="DT." type="date" />
                                                        </div>
                                                    </div>



                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <label id="Label84" runat="server" class="col-sm-4 control-label" title="lbl1">CST No.</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_cst_no" type="text" class="form-control" runat="server" placeholder="CST No." maxlength="50" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label85" runat="server" class="col-sm-4 control-label" title="lbl1">Date</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_cst_dt" type="date" class="form-control" runat="server" placeholder="CST Date" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label86" runat="server" class="col-sm-4 control-label" title="lbl1">Prefix for P.O.</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_prefix_po" type="text" class="form-control" runat="server" placeholder="Prefix for P.O.(To be Printed before P.O. nUmber in P.O. Printout)" maxlength="80" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label87" runat="server" class="col-sm-4 control-label" title="lbl1">For Invoice</label>
                                                        <div class="col-sm-8">
                                                            <input id="Txt_invoice" type="text" class="form-control" runat="server" placeholder="For Invoice (To be Printed before Invoice nUmber in INV. Printout)" maxlength="20" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label91" runat="server" class="col-sm-3 control-label" title="lbl1">2nd IVlCostCent</label>
                                                        <div class="col-sm-1" id="div7" runat="server">
                                                            <asp:ImageButton ID="ImageButton6" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btn_ivl_Click" />
                                                        </div>
                                                        <div class="col-sm-8">
                                                            <input id="Txt_ivl" type="text" class="form-control" runat="server" placeholder="2nd IVl Cost Cent Code" maxlength="30" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label92" runat="server" class="col-sm-3 control-label" title="lbl1">Bank Account</label>
                                                        <div class="col-sm-1" id="div4" runat="server">
                                                            <asp:ImageButton ID="ImageButton4" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btn_bnkacct_Click" />
                                                        </div>

                                                        <div class="col-sm-8">
                                                            <input id="txt_bank_acc" type="text" class="form-control" runat="server" placeholder="Bank Account" maxlength="40" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label88" runat="server" class="col-sm-4 control-label" title="lbl1">Branch</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_brnch" type="text" class="form-control" runat="server" readonly="readonly" placeholder="Prefix for Branch (To be Printed in A/C Books when consolidated report is printed)" maxlength="50" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label89" runat="server" class="col-sm-4 control-label" title="lbl1">Bank PFChl</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_bankpf" type="text" class="form-control" runat="server" readonly="readonly" placeholder="Bank For PF Chl" maxlength="50" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label90" runat="server" class="col-sm-3 control-label" title="lbl1">CostCent</label>
                                                     <%--   <div class="col-sm-1" id="div6" runat="server">
                                                            <asp:ImageButton ID="ImageButton5" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btn_costcent_Click" />
                                                        </div>--%>
                                                        <div class="col-sm-8">
                                                            <input id="txt_cost_center" type="text" class="form-control" runat="server" placeholder="Cost Center Code for this Unit" maxlength="50" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                                <div role="tabpanel" class="tab-pane active" id="DescTab3">
                                    <div class="lbBody" style="height: 340px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">

                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">

                                                    <div class="form-group">
                                                        <label id="Label34" runat="server" class="col-sm-4 control-label" title="lbl1">Bank Name</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_bank_name" type="text" class="form-control" runat="server" placeholder="Bank Name" maxlength="50" />
                                                        </div>
                                                    </div>

                                                    <%-- <div class="form-group">
                                                    <label id="Label35" runat="server" class="col-sm-4 control-label" title="lbl1">Nature of A/c</label>
                                                    <div class="col-sm-8">
                                                        <input id="txt_ac_nat" type="text" class="form-control" runat="server" placeholder="Nature of A/c" maxlength="10" />
                                                    </div>
                                                </div>--%>

                                                    <div class="form-group">
                                                        <label id="Label36" runat="server" class="col-sm-4 control-label" title="lbl1">Bank Address1</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_bank_addr" type="text" class="form-control" runat="server" placeholder="Bank Address" maxlength="50" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label64" runat="server" class="col-sm-4 control-label" title="lbl1">Bank Address2</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_bank_addr1" type="text" class="form-control" runat="server" placeholder="Bank Address" maxlength="50" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label6" runat="server" class="col-sm-4 control-label" title="lbl1">Bank A/c No.</label>


                                                        <div class="col-sm-8">
                                                            <input id="txt_bank_acno" type="text" class="form-control" runat="server" placeholder="Bank A/c No." maxlength="40" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label28" runat="server" class="col-sm-4 control-label" title="lbl1">IEC Code</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_bank_iec" type="text" class="form-control" runat="server" placeholder="IEC Code" maxlength="30" />
                                                        </div>
                                                    </div>



                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">


                                                    <div class="form-group">
                                                        <label id="Label30" runat="server" class="col-sm-4 control-label" title="lbl1">Bank RTGS/IFC Code</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_bank_rtgs" type="text" class="form-control" runat="server" placeholder="Bank RTGS/IFC Code" maxlength="30" />
                                                        </div>
                                                    </div>


                                                    <div class="form-group">
                                                        <label id="Label5" runat="server" class="col-sm-4 control-label" title="lbl1">LUT/Bond/UTNo</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_lutno" type="text" class="form-control" runat="server" placeholder="Bank LUT/Bond/UTNo" maxlength="4" />
                                                        </div>
                                                    </div>



                                                    <div class="form-group">
                                                        <label id="Label66" runat="server" class="col-sm-4 control-label" title="lbl1">Email Id(Prc)</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_email4" type="text" class="form-control" runat="server" placeholder="Email Id(For Purchase)" maxlength="50" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label29" runat="server" class="col-sm-4 control-label" title="lbl1">Swift Code</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_bank_swift" type="text" class="form-control" runat="server" placeholder="Bank Swift Code" maxlength="30" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label35" runat="server" class="col-sm-4 control-label" title="lbl1">Mfg/LicNo.</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_mfg_no" type="text" class="form-control" runat="server" placeholder="Mfg/LicNo" maxlength="10" />
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>


                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane active" id="DescTab4">
                                    <div class="lbBody" style="height: 340px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">

                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">

                                                    <div class="form-group">
                                                        <label id="Label54" runat="server" class="col-sm-4 control-label" title="lbl1">WIP StartDate</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_wip_srtdate" type="date" class="form-control" runat="server" placeholder="WIP StartDate" maxlength="50" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label55" runat="server" class="col-sm-4 control-label" title="lbl1">LotWise StkDt </label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_cotwise_stkdt" type="text" class="form-control" runat="server" placeholder="CotWise StkDt" maxlength="50" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label56" runat="server" class="col-sm-4 control-label" title="lbl1">ESI RateApplicable</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_esi" type="text" class="form-control" runat="server" placeholder="ESI RateApplicable" maxlength="50" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label57" runat="server" class="col-sm-4 control-label" title="lbl1">Tax Schg(1)/Add1tax</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_tax_add1" type="text" class="form-control" runat="server" placeholder="Tax Schg(1)/Add1tax" maxlength="10" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label58" runat="server" class="col-sm-4 control-label" title="lbl1">Plant Capacity</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_plant_capacity" type="text" class="form-control" runat="server" placeholder="Plant Capacity" maxlength="10" />
                                                        </div>
                                                    </div>


                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">


                                                    <div class="form-group">
                                                        <label id="Label59" runat="server" class="col-sm-4 control-label" title="lbl1">WTM:Purch</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_wtm_purch" type="text" class="form-control" runat="server" placeholder="WTM:Purch" maxlength="10" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label60" runat="server" class="col-sm-4 control-label" title="lbl1">WTM:Cons</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_wtm_cons" type="text" class="form-control" runat="server" placeholder="Wtm:Cons" maxlength="10" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label61" runat="server" class="col-sm-4 control-label" title="lbl1">GST AspIP</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_gsi_aspip" type="text" class="form-control" runat="server" placeholder="GST AspIP" maxlength="10" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label62" runat="server" class="col-sm-4 control-label" title="lbl1">GST EWBUID</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_gst_ewbuild" type="text" class="form-control" runat="server" placeholder="GST EWBUID" maxlength="30" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label63" runat="server" class="col-sm-4 control-label" title="lbl1">GST EWBPWD</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_ewbpwd" type="text" class="form-control" runat="server" placeholder="GST EWBPWD" maxlength="30" />
                                                        </div>
                                                    </div>



                                                </div>
                                            </div>
                                        </div>


                                    </div>
                                </div>


                                <div role="tabpanel" class="tab-pane active" id="DescTab5">
                                    <div class="lbBody" style="height: 340px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">

                                        <div class="col-md-12" id="divCan" runat="server">
                                            <div>
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <label id="Label38" runat="server" class="col-sm-2 control-label" title="lbl1">TDS %</label>
                                                        <div class="col-sm-1">
                                                            <input id="txt_TDS_perc" type="text" class="form-control" runat="server" placeholder="TDS %" maxlength="5" />
                                                        </div>
                                                        <label id="Label39" runat="server" class="col-sm-2 control-label" title="lbl1">TCS %</label>
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
                                                        <label id="Label42" runat="server" class="col-sm-2 control-label" title="lbl1">TDS A/c Code</label>
                                                        <div class="col-sm-1">
                                                            <input id="txt_tds_Ac" type="text" class="form-control" runat="server" placeholder="000000" maxlength="6" />
                                                        </div>
                                                        <label id="Label43" runat="server" class="col-sm-2 control-label" title="lbl1">GST Rating</label>
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
                                                        <label id="Label46" runat="server" class="col-sm-2 control-label" title="lbl1">Std Tolr(J/W)</label>
                                                        <div class="col-sm-1">
                                                            <input id="txt_jw_tolr" type="text" class="form-control" runat="server" placeholder="J/W Tolr %" maxlength="6" />
                                                        </div>
                                                        <label id="Label47" runat="server" class="col-sm-2 control-label" title="lbl1">Delv Days</label>
                                                        <div class="col-sm-1">
                                                            <input id="txt_dlv_days" type="text" class="form-control" runat="server" placeholder="Dlv Days" maxlength="6" />
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
                                                            <input id="txt_hub_stk" type="text" class="form-control" runat="server" placeholder="Y/N" maxlength="1" />
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
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane active" id="DescTab6">
                                    <div class="lbBody" style="height: 340px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
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
                                            <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader" />
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
                                            <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        </asp:GridView>
                                    </div>
                                </div>

                            </div>

                        </div>
                    </div>
                </section>


                <!--  <div class="col-md-12">
                    <div>
                        <div class="box-body">
                            <table>
                                <tr>
                                    <td>
                                        <asp:FileUpload ID="Attch" runat="server" Visible="true" onchange="submitFile()" />

                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAttch" runat="server" MaxLength="100" placeholder="Path Upto 100 Char" ></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label27" runat="server" Text=" Please Link Correct File upto 3MB Size ." ></asp:Label>

                                    </td>

                                </tr>

                            </table>

                            <asp:Button ID="btnAtt" runat="server" Text="Attachment" OnClick="btnAtt_Click" Width="134px" Style="display: none" />

                            <asp:Label ID="lblShow" runat="server"></asp:Label>
                            <asp:Label ID="lblUpload" runat="server" Style="display: none"></asp:Label>

                            <asp:ImageButton ID="btnView1" runat="server" ImageUrl="~/tej-base/images/preview-file.png" OnClick="btnView1_Click" Visible="false" />
                            <asp:ImageButton ID="btnDwnld1" runat="server" ImageUrl="~/tej-base/images/Save.png" OnClick="btnDwnld1_Click" Visible="false" />
                        </div>
                    </div>
                </div>-->


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
    </script>
    <asp:HiddenField ID="TabName" runat="server" />
</asp:Content>


