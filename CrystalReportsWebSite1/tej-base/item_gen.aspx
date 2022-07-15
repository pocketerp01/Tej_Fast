<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="item_gen" CodeFile="item_gen.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../tej-base/Scripts/gridviewScroll.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            itemNameGen();
            cinameItem();
        });
        function cinameItem() {
            if (document.getElementById("ContentPlaceHolder1_txt_ciname").value == "" || document.getElementById("ContentPlaceHolder1_txt_ciname").value == "-")
                document.getElementById("ContentPlaceHolder1_txt_ciname").value = document.getElementById("ContentPlaceHolder1_txt_iname").value;
        }
        function itemNameGen() {
            var IndType = $('input[id$=hfIndType]').val();
            if (IndType == "05" || IndType == "06" || IndType == "12" || IndType == "13") {
                var subGname = $('input[id$=txt_subgrp]').val();
                var itmWidth = $('input[id$=toprate1]').val();
                var itmLength = $('input[id$=toprate2]').val();
                var itmGSM = $('input[id$=toprate3]').val();
                var itmMillName = $('input[id$=t_Mill]').val();
                var othSpec = $('input[id$=t_oth3]').val();
                var subGCode = "";
                

                if (subGname.length > 2) {
                    subGCode = subGname.split(':')[0];
                    subGname = subGname.split(':')[1];
                }
                var newItemName = subGname + ' ' + padL(parseFloat(itmWidth).toFixed(3), 3) + ' X ' + padL(parseFloat(itmLength).toFixed(3), 3) + ' X ' + itmGSM + ' GSM ' + othSpec;
                if (subGCode.substr(0, 2) == "02") newItemName = subGname + ' ' + padL(parseFloat(itmWidth).toFixed(3), 3) + ' X ' + padL(parseFloat(itmLength).toFixed(3), 3) + ' X ' + itmGSM + ' GSM ' + othSpec;
                if (subGCode.substr(0, 2) == "07") newItemName = subGname + ' ' + padL(parseFloat(itmWidth).toFixed(3), 3) + ' X ' + itmGSM + ' GSM ' + itmMillName + ' ' + othSpec;


                if (subGCode.substr(0, 2) == "02") {
                    var atxt_wt_grs = ((((itmWidth*1) * (itmLength*1) * (itmGSM*1)) / 10000)/1000);
                    
                    document.getElementById("ContentPlaceHolder1_txt_wt_grs").value = atxt_wt_grs;
                }

                if (subGCode.substr(0, 2) == "02" || subGCode.substr(0, 2) == "07") {
                    document.getElementById("ContentPlaceHolder1_txt_iname").value = newItemName;
                    if (document.getElementById("ContentPlaceHolder1_txt_ciname").value.length > 2)
                        document.getElementById("ContentPlaceHolder1_txt_ciname").value = newItemName;
                }
            }
        }
        function padL(fieldName, len) {
            var s = fieldName;
            var c = "000";
            if (fieldName.includes('.')) {
                s = fieldName.split('.')[0];
                c = fieldName.split('.')[1];
            }
            while (s.length < len) s = '0' + s;
            return (s + "." + c);
        }
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
                   
                    <td style="text-align: left">
                        <button type="submit" id="btnnew" class="btn btn-info" style="width: 100px;" runat="server" accesskey="N" onserverclick="btnnew_ServerClick"><u>N</u>ew</button>
                        <button type="submit" id="btnedit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="i" onserverclick="btnedit_ServerClick">Ed<u>i</u>t</button>
                        <button type="submit" id="btnsave" class="btn btn-info" style="width: 100px;" runat="server" accesskey="s" onserverclick="btnsave_ServerClick"><u>S</u>ave</button>
                        <button type="submit" id="btnprint" class="btn btn-info" style="width: 100px;" runat="server" accesskey="P" onserverclick="btnprint_ServerClick"><u>P</u>rint</button>
                        <button type="submit" id="btndel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="l" onserverclick="btndel_ServerClick">De<u>l</u>ete</button>
                        <button type="submit" id="btnlist" class="btn btn-info" style="width: 100px;" runat="server" accesskey="t" onserverclick="btnlist_ServerClick">Lis<u>t</u></button>
                        <button type="submit" id="btncancel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="c" onserverclick="btncancel_ServerClick"><u>C</u>ancel</button>
                       <button type="submit" id="btnAtch" class="btn btn-info" style="width: 100px;" runat="server" onserverclick="btnAtch_ServerClick">Attachment</button>
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
                                <label id="Label19" runat="server" class="col-sm-3 control-label" title="lbl1">Main Group*</label>
                                <div class="col-sm-1" id="div1" runat="server">
                                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btn_mg_Click" />
                                </div>
                                <div class="col-sm-8">
                                    <input id="txt_mangrp" type="text" class="form-control" runat="server" placeholder="Item Main Group" readonly="readonly" maxlength="50" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label11" runat="server" class="col-sm-3 control-label" title="lbl1">Sub Group*</label>
                                <div class="col-sm-1" id="div3" runat="server">
                                    <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btn_sg_Click" />
                                </div>
                                <div class="col-sm-8">
                                    <input id="txt_subgrp" type="text" class="form-control" runat="server" placeholder="Item Sub Group" readonly="readonly" maxlength="50" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label2" runat="server" class="col-sm-3 control-label" title="lbl1">HSN CODE*</label>
                                <div class="col-sm-1" id="div9" runat="server">
                                    <asp:ImageButton ID="ImageButton9" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btn_hsc_Click" />
                                </div>
                                <div class="col-sm-8">
                                    <input id="txt_hscode" type="text" class="form-control" runat="server" placeholder="HS CODE" readonly="readonly" maxlength="40" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label49" runat="server" class="col-sm-3 control-label" title="lbl1">Show Only In </label>
                                <div class="col-sm-1" id="div19" runat="server">
                                    <asp:ImageButton ID="ImageButton_br" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btn_br_wise_Click" />
                                </div>
                                <div class="col-sm-8">
                                    <input id="txt_showin" type="text" class="form-control" runat="server" placeholder="Show Only in Selected Branch" readonly="readonly" maxlength="40" />
                                </div>
                            </div>


                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">

                            <div class="form-group">
                                <label id="Label3" runat="server" class="col-sm-4 control-label" title="lbl1">ERP Item Code</label>
                                <div class="col-sm-8">
                                    <input id="txt_erp_code" type="text" class="form-control" runat="server" readonly="readonly" placeholder="ERP_CODE" maxlength="30" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label13" runat="server" class="col-sm-4 control-label" title="lbl1">Part Number</label>
                                <div class="col-sm-8">
                                    <input id="txt_partno" type="text" class="form-control" runat="server" placeholder="Part Number" maxlength="250" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label17" runat="server" class="col-sm-4 control-label" title="lbl1">Drawing No.</label>
                                <div class="col-sm-8">
                                    <input id="txt_drgno" type="text" class="form-control" runat="server" placeholder="Drawing No." maxlength="250" />
                                </div>
                            </div>


                            <div class="form-group">
                                <label id="Label32" runat="server" class="col-sm-4 control-label" title="lbl1">Opening Balance</label>
                                <div class="col-sm-8">
                                    <input id="txt_balop" class="form-control" runat="server" placeholder="Opening Balance" maxlength="10" onkeypress="return isDecimalKey(event)" />
                                </div>
                            </div>


                        </div>
                    </div>
                </div>



                <div class="col-md-12">
                    <div>
                        <div class="box-body">

                            <div class="form-group">
                                <label id="Label68" runat="server" class="col-sm-2 control-label" title="lbl1">Item Name*</label>
                                <div class="col-sm-10">
                                    <input id="txt_iname" type="text" class="form-control" onkeyup="max_length(this,250)" runat="server" placeholder="Name of Item being Opened" maxlength="250" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label1" runat="server" class="col-sm-2 control-label" title="lbl1">Item_Name(As Per Customer/Vendor)</label>
                                <div class="col-sm-10">
                                    <input id="txt_ciname" type="text" class="form-control" runat="server" onkeyup="max_length(this,250)" placeholder="Customer/Vendor Specified Item Name" onkeydown="cinameItem();" maxlength="250" />
                                </div>
                            </div>

                        </div>
                    </div>
                </div>



<%--                <div class="col-md-12">
                    <div>
                        <div class="box-body">

                            <div class="form-group">
                                <label id="Label1" runat="server" class="col-sm-2 control-label" title="lbl1">Item_Name(As Per Customer/Vendor)</label>
                                <div class="col-sm-10">
                                    <input id="txt_ciname" type="text" class="form-control" runat="server" onkeyup="max_length(this,120)" placeholder="Customer/Vendor Specified Item Name" onkeydown="cinameItem();" maxlength="120" />
                                </div>
                            </div>

                        </div>
                    </div>
                </div>--%>



<%--                <div class="col-md-12">
                    <div>
                        <div class="box-body">
                            <asp:Label ID="Label1" runat="server" Text="Item_Name(Specified_by_Customer/Vendor)" CssClass="col-sm-2 control-label"></asp:Label>
                            <asp:TextBox ID="txt_ciname" runat="server" Width="99%" onkeyup="max_length(this,120)" placeholder="Customer Specified Item Name" onkeydown="cinameItem();"></asp:TextBox>
                        </div>
                    </div>
                </div>--%>


                <section class="col-lg-12 connectedSortable" id="AllTabs" runat="server">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">

                                <li><a href="#DescTab1" id="tab1" runat="server" aria-controls="DescTab1" role="tab" data-toggle="tab">Units,Packing</a></li>
                                <li><a href="#DescTab2" id="tab2" runat="server" aria-controls="DescTab2" role="tab" data-toggle="tab">Class,Location</a></li>
                                <li><a href="#DescTab3" id="tab3" runat="server" aria-controls="DescTab3" role="tab" data-toggle="tab">Weights,Levels</a></li>
                                <li><a href="#DescTab4" id="tab4" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Industry Specific Data</a></li>
                                <li><a href="#DescTab5" id="tab5" runat="server" aria-controls="DescTab5" role="tab" data-toggle="tab">Dimensions,Activation</a></li>
                                <li><a href="#DescTab6" id="tab6" runat="server" aria-controls="DescTab6" visible="false" role="tab" data-toggle="tab">UDF Data</a></li>
                            </ul>

                            <div class="tab-content">

                                <div role="tabpanel" class="tab-pane active" id="DescTab1">
                                    <div class="lbBody" style="height: 175px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">


                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">


                                                    <div class="form-group">
                                                        <label id="Label25" runat="server" class="col-sm-3 control-label" title="lbl1">Unit(For_Stock)*</label>
                                                        <div class="col-sm-1" id="div8" runat="server">
                                                            <asp:ImageButton ID="ImageButton8" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btn_puom_Click" />
                                                        </div>
                                                        <div class="col-sm-8">
                                                            <input id="txt_pri_unit" type="text" class="form-control" runat="server" placeholder="Unit for Stock" readonly="readonly" maxlength="6" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label12" runat="server" class="col-sm-3 control-label" title="lbl1">Unit(Secondary)</label>
                                                        <div class="col-sm-1" id="div2" runat="server">
                                                            <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btn_suom_Click" />
                                                        </div>
                                                        <div class="col-sm-8">
                                                            <input id="txt_sec_unit" type="text" class="form-control" runat="server" placeholder="Secondary Unit" readonly="readonly" maxlength="6" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label21" runat="server" class="col-sm-3 control-label" title="lbl1">Critical_Item</label>
                                                        <div class="col-sm-1" id="div7" runat="server">
                                                            <asp:ImageButton ID="ImageButton7" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btn_crit_Click" />
                                                        </div>
                                                        <div class="col-sm-8">
                                                            <input id="txt_crit_itm" type="text" class="form-control" runat="server" placeholder="Critical(Y/N)" maxlength="1" readonly="readonly" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label4" runat="server" class="col-sm-3 control-label" title="lbl1">Cust_Vend_Code</label>
                                                        <div class="col-sm-1" id="div10" runat="server">
                                                            <asp:ImageButton ID="ImageButton10" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btn_crit_Click" />
                                                        </div>
                                                        <div class="col-sm-8">
                                                            <input id="txt_cust_vend" type="text" class="form-control" runat="server" readonly="readonly" placeholder="Primary Customer/Vendor Code" maxlength="10" />
                                                        </div>
                                                    </div>


                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">

                                                    <div class="form-group">
                                                        <label id="Label18" runat="server" class="col-sm-4 control-label" title="lbl1">Standard Rate</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_irate" type="text" class="form-control" runat="server" placeholder="Standard Rate" maxlength="10" onkeypress="return isDecimalKey(event)" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label22" runat="server" class="col-sm-4 control-label" title="lbl1">Standard Packing*</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_pack" type="text" class="form-control" runat="server" placeholder="Standard Packing" maxlength="10" onkeypress="return isDecimalKey(event)" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label50" runat="server" class="col-sm-3 control-label" title="lbl1">Made_In_Which_Plant</label>
                                                        <div class="col-sm-1" id="div20" runat="server">
                                                            <asp:ImageButton ID="ImageButton19" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btn_madein_click" />
                                                        </div>
                                                        <div class="col-sm-8">
                                                            <input id="txt_madein" type="text" class="form-control" runat="server" placeholder="Manufactured In Our Which Branch?" maxlength="10" readonly="readonly" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label5" runat="server" class="col-sm-4 control-label" title="lbl1">Cust_Vend_Name</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_deacDt_xx" type="text" class="form-control" runat="server" readonly="readonly" placeholder="Primary Customer/Vendor Name" maxlength="45" />
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>


                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane active" id="DescTab2">
                                    <div class="lbBody" style="height: 175px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">

                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">

                                                    <div class="form-group">
                                                        <label id="Label24" runat="server" class="col-sm-3 control-label" title="lbl1">A/B/C Class</label>
                                                        <div class="col-sm-1" id="div5" runat="server">
                                                            <asp:ImageButton ID="ImageButton5" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btn_abc_Click" />
                                                        </div>
                                                        <div class="col-sm-8">
                                                            <input id="txt_abc" type="text" class="form-control" runat="server" placeholder="A/B/C Classification" readonly="readonly" maxlength="5" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label26" runat="server" class="col-sm-3 control-label" title="lbl1">BIN/Locn</label>
                                                        <div class="col-sm-1" id="div6" runat="server">
                                                            <asp:ImageButton ID="ImageButton6" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btn_locn_Click" />
                                                        </div>
                                                        <div class="col-sm-8">
                                                            <input id="txt_locn" type="text" class="form-control" runat="server" placeholder="Bin/Location" readonly="readonly" maxlength="30" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label20" runat="server" class="col-sm-3 control-label" title="lbl1">Category</label>
                                                        <div class="col-sm-1" id="div4" runat="server">
                                                            <asp:ImageButton ID="ImageButton4" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btn_catg_Click" />
                                                        </div>
                                                        <div class="col-sm-8">
                                                            <input id="txt_icat" type="text" class="form-control" runat="server" placeholder="Dom/IMP/N/a" readonly="readonly" maxlength="5" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label7" runat="server" class="col-sm-3 control-label" title="lbl1">Request_By</label>
                                                        <div class="col-sm-1" id="div17" runat="server">
                                                            <asp:ImageButton ID="ImageButton17" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btn_catg_Click" />
                                                        </div>
                                                        <div class="col-sm-8">
                                                            <input id="txt_req_by" type="text" class="form-control" runat="server" placeholder="Item Opened For" readonly="readonly" maxlength="5" />
                                                        </div>
                                                    </div>



                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">

                                                    <div class="form-group">
                                                        <label id="Label31" runat="server" class="col-sm-4 control-label" title="lbl1">Brand/Maker</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_brand" type="text" class="form-control" runat="server" placeholder="Brand/Maker" maxlength="100" />
                                                        </div>
                                                    </div>


                                                    <div class="form-group">
                                                        <label id="Label33" runat="server" class="col-sm-4 control-label" title="lbl1">J/w Qty Control</label>
                                                        <div class="col-sm-8">
                                                            <select id="txt_jw_ctrl" runat="server" class="form-control">
                                                                <option value="Y">Y</option>
                                                                <option value="N">N</option>
                                                            </select>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label6" runat="server" class="col-sm-4 control-label" title="lbl1">Service_Item</label>
                                                        <div class="col-sm-8">
                                                            <select id="txt_stk_ctrl" runat="server" class="form-control">
                                                                <option value="Y">N</option>
                                                                <option value="N">Y</option>
                                                            </select>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label23" runat="server" class="col-sm-4 control-label" title="lbl1">Shelf Life Days</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_shelf" type="text" class="form-control" runat="server" placeholder="Shelf Life Days" maxlength="10" onkeypress="return isDecimalKey(event)" />
                                                        </div>
                                                    </div>




                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane active" id="DescTab3">
                                    <div class="lbBody" style="height: 175px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">

                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">

                                                    <div class="form-group">
                                                        <label id="Label34" runat="server" class="col-sm-4 control-label" title="lbl1">Gross Wt.</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_wt_grs" type="text" class="form-control" runat="server" placeholder="Gross Wt." maxlength="10" onkeypress="return isDecimalKey(event)" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label35" runat="server" class="col-sm-4 control-label" title="lbl1">Net Wt.</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_wt_net" type="text" class="form-control" runat="server" placeholder="Net Wt." maxlength="10" onkeypress="return isDecimalKey(event)" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label36" runat="server" class="col-sm-4 control-label" title="lbl1">Last_MRR_Rate</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_iqd" type="text" class="form-control" runat="server" placeholder="Last Rate" maxlength="10" onkeypress="return isDecimalKey(event)" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label37" runat="server" class="col-sm-4 control-label" title="lbl1">Lead Time</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_leadt" type="text" class="form-control" runat="server" placeholder="Lead Time" maxlength="10" onkeypress="return isDecimalKey(event)" />
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">

                                                    <div class="form-group">
                                                        <label id="Label28" runat="server" class="col-sm-4 control-label" title="lbl1">Min Level</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_min" type="text" class="form-control" runat="server" placeholder="Min Level" maxlength="10" onkeypress="return isDecimalKey(event)" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label29" runat="server" class="col-sm-4 control-label" title="lbl1">Max Level</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_max" type="text" class="form-control" runat="server" placeholder="Max Level" maxlength="10" onkeypress="return isDecimalKey(event)" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label30" runat="server" class="col-sm-4 control-label" title="lbl1">Re-Order Level</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_rol" type="text" class="form-control" runat="server" placeholder="Re-Order Level" maxlength="10" onkeypress="return isDecimalKey(event)" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label38" runat="server" class="col-sm-4 control-label" title="lbl1">Slow_Moving_days</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_slow" type="text" class="form-control" runat="server" placeholder="Consider Slow After X Days" maxlength="10" onkeypress="return isDecimalKey(event)" />
                                                        </div>
                                                    </div>


                                                </div>
                                            </div>
                                        </div>


                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane active" id="DescTab4">
                                    <div class="lbBody" style="height: 175px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">

                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">

                                                    <div class="form-group">
                                                        <label id="Label8" runat="server" class="col-sm-4 control-label" title="lbl1">Width_(cm)</label>
                                                        <div class="col-sm-8">
                                                            <input id="toprate1" type="text" class="form-control" runat="server" placeholder="Width" maxlength="10" onkeyup="itemNameGen();" onkeypress="return isDecimalKey(event)" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label9" runat="server" class="col-sm-4 control-label" title="lbl1">Length_(cm)</label>
                                                        <div class="col-sm-8">
                                                            <input id="toprate2" type="text" class="form-control" runat="server" placeholder="Length" maxlength="10" onkeyup="itemNameGen();" onkeypress="return isDecimalKey(event)" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label10" runat="server" class="col-sm-4 control-label" title="lbl1">GSM</label>
                                                        <div class="col-sm-8">
                                                            <input id="toprate3" type="text" class="form-control" runat="server" placeholder="GSM" maxlength="10" onkeyup="itemNameGen();" onkeypress="return isDecimalKey(event)" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label15" runat="server" class="col-sm-4 control-label" title="lbl1">B.F.</label>
                                                        <div class="col-sm-8">
                                                            <input id="t_BF" type="text" class="form-control" runat="server" placeholder="B.F." maxlength="10" onkeypress="return isDecimalKey(event)" />
                                                        </div>
                                                    </div>



                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">

                                                    <div class="form-group">
                                                        <label id="Label16" runat="server" class="col-sm-3 control-label" title="lbl1">Mill_Name</label>
                                                        <div class="col-sm-1" id="div11" runat="server">
                                                            <asp:ImageButton ID="ImageButton11" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btn_mill_Click" />
                                                        </div>
                                                        <div class="col-sm-8">
                                                            <input id="t_Mill" type="text" class="form-control" runat="server" placeholder="Mill Name" maxlength="10" readonly="readonly" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label40" runat="server" class="col-sm-4 control-label" title="lbl1">Other Specs</label>
                                                        <div class="col-sm-8">
                                                            <input id="t_oth3" type="text" class="form-control" runat="server" placeholder="Other Specs(1)" maxlength="10" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label14" runat="server" class="col-sm-4 control-label" title="lbl1">Density</label>
                                                        <div class="col-sm-8">
                                                            <input id="t_oth1" type="text" class="form-control" runat="server" placeholder="Density" maxlength="10" onkeyup="itemNameGen();" onkeypress="return isDecimalKey(event)" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label39" runat="server" class="col-sm-4 control-label" title="lbl1">Micron</label>
                                                        <div class="col-sm-8">
                                                            <input id="t_oth2" type="text" class="form-control" runat="server" placeholder="Micron" maxlength="10" onkeypress="return isDecimalKey(event)" />
                                                        </div>
                                                    </div>



                                                </div>
                                            </div>
                                        </div>


                                    </div>
                                </div>


<%--                                <div role="tabpanel" class="tab-pane active" id="DescTab5">
                                    <div class="lbBody" style="height: 175px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl10" runat="server" Text="lbl10" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl10" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl10_Click" /></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl10" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl11" runat="server" Text="lbl11" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl11" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl11_Click" /></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl11" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl12" runat="server" Text="lbl12" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl12" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl12_Click" /></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl12" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl13" runat="server" Text="lbl13" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl13" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl13_Click" /></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl13" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl14" runat="server" Text="lbl14" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl14" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl14_Click" /></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl14" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl15" runat="server" Text="lbl15" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl15" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl15_Click" /></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl15" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl16" runat="server" Text="lbl16" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl16" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl16_Click" /></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl16" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl17" runat="server" Text="lbl17" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl17" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl17_Click" /></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl17" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl18" runat="server" Text="lbl18" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl18" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl18_Click" /></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl18" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl19" runat="server" Text="lbl19" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl19" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl19_Click" /></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl19" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>
--%>


                                <div role="tabpanel" class="tab-pane active" id="DescTab5">
                                    <div class="lbBody" style="height: 175px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">


                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">


                                                    <div class="form-group">
                                                        <label id="Label41" runat="server" class="col-sm-3 control-label" title="lbl1">Item_Type</label>
                                                        <div class="col-sm-1" id="div12" runat="server">
                                                            <asp:ImageButton ID="ImageButton12" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btn_dim1_Click" />
                                                        </div>
                                                        <div class="col-sm-8">
                                                            <input id="titem_dim1" type="text" class="form-control" runat="server" placeholder="Broad Identification (Dimension 1)" readonly="readonly" maxlength="30" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label42" runat="server" class="col-sm-3 control-label" title="lbl1">Item_Application</label>
                                                        <div class="col-sm-1" id="div13" runat="server">
                                                            <asp:ImageButton ID="ImageButton13" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btn_dim2_Click" />
                                                        </div>
                                                        <div class="col-sm-8">
                                                            <input id="titem_dim2" type="text" class="form-control" runat="server" placeholder="Application (Dimension 2)" readonly="readonly" maxlength="30" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label43" runat="server" class="col-sm-3 control-label" title="lbl1">Item_Class</label>
                                                        <div class="col-sm-1" id="div14" runat="server">
                                                            <asp:ImageButton ID="ImageButton14" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btn_dim3_Click" />
                                                        </div>
                                                        <div class="col-sm-8">
                                                            <input id="titem_dim3" type="text" class="form-control" runat="server" placeholder="Item Class (Dimension 3)" readonly="readonly" maxlength="30" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label44" runat="server" class="col-sm-3 control-label" title="lbl1">Item_Sub_Class</label>
                                                        <div class="col-sm-1" id="div15" runat="server">
                                                            <asp:ImageButton ID="ImageButton15" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btn_dim4_Click" />
                                                        </div>
                                                        <div class="col-sm-8">
                                                            <input id="titem_dim4" type="text" class="form-control" runat="server" placeholder="Item Sub Class (Dimension 4)" readonly="readonly" maxlength="30" />
                                                        </div>
                                                    </div>


                                                </div>
                                            </div>
                                        </div>


                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">


                                                    <div class="form-group">
                                                        <label id="Label45" runat="server" class="col-sm-3 control-label" title="lbl1">Approved_By</label>
                                                        <div class="col-sm-1" id="div16" runat="server">
                                                            <asp:ImageButton ID="ImageButton16" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btn_puom_Click" />
                                                        </div>
                                                        <div class="col-sm-8">
                                                            <input id="txt_appby" type="text" class="form-control" runat="server" placeholder="Approved By" readonly="readonly" maxlength="15" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label46" runat="server" class="col-sm-4 control-label" title="lbl1">Approved_On</label>

                                                        <div class="col-sm-8">
                                                            <input id="txt_appdt" type="text" class="form-control" runat="server" placeholder="Approved On" readonly="readonly" maxlength="10" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label47" runat="server" class="col-sm-3 control-label" title="lbl1">Deactivated_By</label>
                                                        <div class="col-sm-1" id="div18" runat="server">
                                                            <asp:ImageButton ID="ImageButton18" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btn_crit_Click" />
                                                        </div>
                                                        <div class="col-sm-8">
                                                            <input id="txt_deacby" type="text" class="form-control" runat="server" placeholder="Deactivated By" maxlength="15" readonly="readonly" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label48" runat="server" class="col-sm-4 control-label" title="lbl1">Deactivated_On</label>

                                                        <div class="col-sm-8">
                                                            <input id="txt_deacDt" type="text" class="form-control" runat="server" readonly="readonly" placeholder="Deactivated_On" maxlength="10" />
                                                        </div>
                                                    </div>


                                                </div>
                                            </div>
                                        </div>




                                    </div>
                                </div>







                                <div role="tabpanel" class="tab-pane active" id="DescTab6">
                                    <div class="lbBody" style="height: 175px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
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
                                        <asp:FileUpload ID="Attch" runat="server" Visible="true" onchange="submitFile()" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAttch" runat="server" MaxLength="100" placeholder="Path Upto 100 Char"></asp:TextBox>
                                    </td>
                                    <td>
                                    <asp:Label ID="lblUpload" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnView1" runat="server" CssClass="btn-success" Text="View" OnClick="btnView1_Click" Visible="false" />
                                    </td>
                                    <td>
                                    <asp:ImageButton ID="btnDwnld1" runat="server" ImageUrl="~/tej-base/images/Save.png" OnClick="btnDwnld1_Click" Visible="false" />
                                    </td>
                                    <td>
                                        <asp:Label ID="Label27" runat="server" Text="&nbsp &nbsp  Image Link (Please Link Correct File upto 3MB Size) &nbsp &nbsp"></asp:Label>
                                    </td>

                                </tr>

                            </table>

                            <asp:Button ID="btnAtt" runat="server" Text="Attachment" OnClick="btnAtt_Click" Width="134px" Style="display: none" />

                            <asp:Label ID="lblShow" runat="server"></asp:Label>

                            
                            
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
    <asp:HiddenField ID="hfIndType" runat="server" />
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
