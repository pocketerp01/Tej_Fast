<%@ Page Title="" Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="om_csbs_est" CodeFile="om_csbs_est.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">


    <script src="../tej-base/Scripts/gridviewScroll.min.js" type="text/javascript"></script>



    <script type="text/javascript">
        $(document).ready(function () {

        });


        function fill_zero(val) { if (isNaN(val)) return 0; if (isFinite(val)) return val; }
    </script>
    <script type="text/javascript">
        function openfileDialog() {
            $("#Attch").click();
        }
        <%--function submitFile() {
            $("#<%= btnAtt.ClientID%>").click();
       };--%>
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
                        <button type="submit" id="btncancel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="c" onserverclick="btncancel_ServerClick"><u>C</u>ancel</button>
                        <button type="submit" id="btnexit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="X" onserverclick="btnexit_ServerClick">E<u>x</u>it</button>
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
                                <label id="Label19" runat="server" class="col-md-3 control-label" title="lbl1">EntryNo.</label>

                                <div class="col-md-3">
                                    <input id="txtVchnum" type="text" class="form-control" style="height: 30px" runat="server" placeholder="Entry No" maxlength="6" />
                                </div>

                                <label id="Label11" runat="server" class="col-md-3 control-label" title="lbl1">Entry_Dt</label>

                                <div class="col-md-3">
                                    <input id="txtVchdate" type="text" class="form-control" style="height: 30px" runat="server" placeholder="Entry Date" maxlength="10" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label13" runat="server" class="col-md-3 control-label" title="lbl1">Customer Name</label>

                                <div class="col-md-9">
                                    <input id="txtCustomer" type="text" class="form-control" style="height: 30px" runat="server" placeholder="Customer Name" maxlength="50" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label17" runat="server" class="col-md-3 control-label" title="lbl1">Item Name</label>

                                <div class="col-md-9">
                                    <input id="txtItem" type="text" class="form-control" runat="server" placeholder="Item Name" maxlength="50" />
                                </div>
                            </div>

                        </div>
                    </div>
                </div>


                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <asp:ImageButton ID="btn_img" AlternateText="Select Box Type to see the image" CssClass="col-md-6" Height="98px" runat="server" OnClick="btn_img_Click" />
                            <label id="Label39" runat="server" class="col-md-6 control-label" style="text-align: justify; border: 2px solid #1290b7;" title="lbl1">
                                1) The grey textboxes are autocalculated. Please enter values in white textboxes.
                               <br />
                                2) Enter desirable Box dimensions.<br />
                                3) Press calculate and get the required compression strength and paper grade based on GSM & BF selected.
                            </label>
                        </div>
                    </div>
                </div>

                <div class="col-md-12">
                    <div>
                        <div class="box-body">

                            <div class="col-md-4">
                                <div class="form-group">
                                    <label id="Label48" runat="server" class="col-md-12 control-label" title="lbl1" style="background-color: #a6e2f5; align-content: center; text-align: center;">Determining Required BCT</label>
                                </div>
                                <div class="form-group">
                                    <label id="Label1" runat="server" class="col-md-6 control-label" title="lbl1">Gross_Wt_of_Packed Box</label>

                                    <div class="col-md-3">
                                        <input id="txt_gross" type="text" class="form-control" style="height: 30px; text-align: right;" runat="server" maxlength="9" />
                                    </div>
                                    <div class="col-md-3">
                                        <input id="txt_gross_unit" type="text" class="form-control" style="height: 30px" readonly="readonly" runat="server" maxlength="9" value="kg" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label id="Label2" runat="server" class="col-md-6 control-label" title="lbl1">Stack Height</label>



                                    <div class="col-md-3">
                                        <input id="txt_stckhgt" type="text" style="height: 30px; text-align: right;" class="form-control" runat="server" maxlength="9" />
                                    </div>
                                    <div class="col-md-3">
                                        <input id="txt_stckhgt_unit" type="text" style="height: 30px;" readonly="readonly" class="form-control" runat="server" value="mm" maxlength="9" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label id="Label7" runat="server" class="col-md-6 control-label" title="lbl1">No. of Boxes stacked</label>
                                    <div class="col-md-3">
                                        <input id="txt_box_stckd" type="text" style="height: 30px; text-align: right;" readonly="readonly" class="form-control" runat="server" maxlength="9" />
                                    </div>
                                    <div class="col-md-3">
                                        <input id="txt_box_stckd_unit" type="text" style="height: 30px;" value="Nos" readonly="readonly" class="form-control" runat="server" maxlength="9" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label id="Label8" runat="server" class="col-md-6 control-label" title="lbl1">Load_On_bottom</label>
                                    <div class="col-md-3">
                                        <input id="txt_load" type="text" style="height: 30px; text-align: right;" readonly="readonly" class="form-control" runat="server" maxlength="9" />
                                    </div>
                                    <div class="col-md-3">
                                        <input id="txt_load_unit" type="text" style="height: 30px;" readonly="readonly" value="kg" class="form-control" runat="server" maxlength="9" />
                                    </div>
                                </div>


                                <div class="form-group">
                                    <label id="Label9" runat="server" class="col-md-12 control-label" title="lbl1" style="background-color: #a6e2f5; align-content: center; text-align: center;">Environment Factors</label>
                                </div>

                                <div class="form-group">
                                    <label id="Label10" runat="server" class="col-md-6 control-label" title="lbl1">a.Storage Time(Days)</label>
                                    <div class="col-md-3">
                                        <input id="txt_storagea" type="text" style="height: 30px; text-align: right;" class="form-control" runat="server" maxlength="9" />
                                    </div>
                                    <div class="col-md-3">
                                        <input id="txt_strorageb" type="text" style="height: 30px; text-align: right;" readonly="readonly" class="form-control" runat="server" maxlength="9" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label id="Label12" runat="server" class="col-md-6 control-label" title="lbl1">b.Humidity(%)</label>



                                    <div class="col-md-3">
                                        <input id="txt_humida" type="text" style="height: 30px; text-align: right;" class="form-control" runat="server" maxlength="9" />
                                    </div>
                                    <div class="col-md-3">
                                        <input id="txt_humidb" type="text" style="height: 30px; text-align: right;" readonly="readonly" class="form-control" runat="server" maxlength="9" />
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label id="Label14" runat="server" class="col-md-12 control-label" title="lbl1" style="background-color: #a6e2f5; align-content: center; text-align: center;">Pallet Patterns</label>
                                </div>

                                <div class="form-group">
                                    <label id="Label15" runat="server" class="col-md-6 control-label" title="lbl1">Columnar,Aligned ?</label>



                                    <div class="col-md-3">
                                        <select id="ddcolumna" runat="server" style="height: 30px; font-size: small;" class="form-control">
                                            <option value="Y">Yes</option>
                                            <option value="N" selected="selected">No</option>
                                        </select>
                                    </div>
                                    <div class="col-md-3">
                                        <input id="txt_columna" style="height: 30px; text-align: right;" type="text" readonly="readonly" class="form-control" runat="server" maxlength="9" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label id="Label16" runat="server" class="col-md-6 control-label" title="lbl1">Columnar Mis-aligned ?</label>



                                    <div class="col-md-3">
                                        <select id="ddcolumnb" runat="server" style="height: 30px; font-size: small" class="form-control">
                                            <option value="Y">Yes</option>
                                            <option value="N" selected="selected">No</option>
                                        </select>
                                    </div>
                                    <div class="col-md-3">
                                        <input id="txt_columnb" type="text" style="height: 30px; text-align: right;" readonly="readonly" class="form-control" runat="server" maxlength="9" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label id="Label18" runat="server" class="col-md-6 control-label" title="lbl1">Inter-locked ?</label>



                                    <div class="col-md-3">
                                        <select id="ddinterlock" runat="server" style="height: 30px; font-size: small" class="form-control">
                                            <option value="Y">Yes</option>
                                            <option value="N" selected="selected">No</option>
                                        </select>
                                    </div>
                                    <div class="col-md-3">
                                        <input id="txt_interlock" style="height: 30px; text-align: right;" type="text" readonly="readonly" class="form-control" runat="server" maxlength="9" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label id="Label20" runat="server" class="col-md-6 control-label" title="lbl1">Overhang ?</label>



                                    <div class="col-md-3">
                                        <select id="ddoverhang" runat="server" style="height: 30px; font-size: small" class="form-control">
                                            <option value="Y">Yes</option>
                                            <option value="N" selected="selected">No</option>
                                        </select>

                                    </div>
                                    <div class="col-md-3">
                                        <input id="txt_overhng" type="text" style="height: 30px; text-align: right;" readonly="readonly" class="form-control" runat="server" maxlength="9" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label id="Label21" runat="server" class="col-md-6 control-label" title="lbl1">Deckboard Gap ?</label>



                                    <div class="col-md-3">

                                        <select id="dddeckboard" runat="server" style="height: 30px; font-size: small" class="form-control">
                                            <option value="Y">Yes</option>
                                            <option value="N" selected="selected">No</option>
                                        </select>


                                    </div>
                                    <div class="col-md-3">
                                        <input id="txtdeckboard" type="text" style="height: 30px; text-align: right;" readonly="readonly" class="form-control" runat="server" maxlength="9" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label id="Label22" runat="server" class="col-md-6 control-label" title="lbl1">Excessive Handling ?</label>


                                    <div class="col-md-3">

                                        <select id="ddexchnd" runat="server" style="height: 30px; font-size: small" class="form-control">
                                            <option value="Y" selected="selected">Yes</option>
                                            <option value="N">No</option>
                                        </select>


                                    </div>
                                    <div class="col-md-3">
                                        <input id="txtexchand" type="text" style="height: 30px; text-align: right;" readonly="readonly" class="form-control" runat="server" maxlength="9" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label id="Label23" runat="server" class="col-md-6 control-label" title="lbl1">Total_Envirn_Factor ?</label>



                                    <div class="col-md-3">
                                        <input id="txt_total_envir_faca" type="text" style="height: 30px; text-align: right;" readonly="readonly" visible="false" class="form-control" runat="server" maxlength="9" />
                                    </div>
                                    <div class="col-md-3">
                                        <input id="txt_total_envir_facb" type="text" style="height: 30px; text-align: right;" readonly="readonly" class="form-control" runat="server" maxlength="9" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label id="Label24" runat="server" class="col-md-6 control-label" title="lbl1" style="font-weight: 600">Required_Safety_Factor ?</label>



                                    <div class="col-md-3">
                                        <input id="txt_require_safetya" type="text" style="height: 30px; text-align: right; background-color: yellow; font-size: 15px;" readonly="readonly" visible="false" class="form-control" runat="server" maxlength="9" />
                                    </div>
                                    <div class="col-md-3">
                                        <input id="txt_require_safetyb" type="text" style="height: 30px; text-align: right; background-color: yellow; font-size: 15px;" readonly="readonly" class="form-control" runat="server" maxlength="9" />
                                    </div>
                                    <div class="form-group">
                                        <label id="Label25" runat="server" class="col-md-6 control-label" title="lbl1" style="font-weight: 600">Required_BCT_in(kgs) ?</label>
                                        <div class="col-md-3">
                                            <input id="txt_require_bcta" type="text" style="height: 30px; text-align: right;" readonly="readonly" visible="false" class="form-control" runat="server" maxlength="9" />
                                        </div>
                                        <div class="col-md-3">
                                            <input id="txt_require_bctb" type="text" style="height: 30px; text-align: right; background-color: yellow; font-size: 15px;" readonly="readonly" class="form-control" runat="server" maxlength="9" />
                                        </div>
                                    </div>
                                </div>
                            </div>


                            <!-- 2nd-->

                            <div class="col-md-4">

                                <div class="form-group">
                                    <div class="col-md-12">
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label id="Label47" runat="server" class="col-md-12 control-label" title="lbl1" style="background-color: #a6e2f5; align-content: center; text-align: center;">Specification of Carton</label>

                                    <div class="form-group">
                                        <label id="Label3" runat="server" class="col-md-3 control-label" title="lbl1">Box Type (FEFCO Code)</label>
                                        <div class="col-md-9">
                                            <select id="ddbox_fef" runat="server" style="height: 30px; font-size: small" class="form-control" onchange="changetextbox(this.value);">
                                            </select>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label id="Label36" runat="server" class="col-md-12 control-label" title="lbl1" style="background-color: #a6e2f5; align-content: center; text-align: center;">BOX SPECIFICATION</label>
                                    </div>

                                    <div class="form-group">
                                        <label id="Label4" runat="server" class="col-md-9 control-label" title="lbl1">Length &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; (mm)</label>



                                        <div class="col-md-3">
                                            <input id="txt_len" type="text" class="form-control" style="height: 30px; text-align: right;" runat="server" maxlength="9" />
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label id="Label5" runat="server" class="col-md-9 control-label" title="lbl1">Width &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; (mm)</label>

                                        <div class="col-md-3">
                                            <input id="txt_wid" type="text" class="form-control" style="height: 30px; text-align: right;" runat="server" maxlength="9" />
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label id="Label6" runat="server" class="col-md-9 control-label" title="lbl1">Height &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; (mm)</label>


                                        <div class="col-md-3">
                                            <input id="txt_hgt" type="text" class="form-control" style="height: 30px; text-align: right;" runat="server" maxlength="9" />
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label id="Label26" runat="server" class="col-md-9 control-label" title="lbl1">No.of Ply's &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; (3 or 5)</label>


                                        <div class="col-md-3">
                                            <select id="ddno_piles" runat="server" class="form-control" onchange="changetextbox(this.value);">
                                                <option value="3">3</option>
                                                <option value="5">5</option>
                                            </select>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label id="Label27" runat="server" class="col-md-9 control-label" title="lbl1">Flute Profile</label>


                                        <div class="col-md-3">
                                            <select id="ddflutep" runat="server" class="form-control">
                                            </select>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label id="Label28" runat="server" class="col-md-4 control-label" title="lbl1">Manufacturing Process</label>
                                        <label id="Label49" style="font-size: 10px" class="col-md-5 control-label">Manual(M) / Automatic (A)</label>

                                        <div class="col-md-3">
                                            <select id="ddManf_Procs" runat="server" class="form-control">
                                                <option value="A">A</option>
                                                <option value="M">M</option>
                                            </select>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label id="Label29" runat="server" class="col-md-9 control-label" title="lbl1">Board Calliper in mm</label>


                                        <div class="col-md-3">
                                            <input id="txt_board_callipr" type="text" class="form-control" style="height: 30px; text-align: right; background-color: yellow" runat="server" readonly="readonly" maxlength="9" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label id="Label30" runat="server" class="col-md-9 control-label" title="lbl1">Area of Board</label>


                                        <div class="col-md-3">
                                            <input id="txt_area" type="text" class="form-control" style="height: 30px; text-align: right; background-color: yellow" runat="server" readonly="readonly" maxlength="9" />
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label id="Label37" runat="server" class="col-md-12 control-label" title="lbl1" style="background-color: #a6e2f5; align-content: center;"></label>
                                    </div>


                                    <div class="form-group">
                                        <label id="Label31" runat="server" class="col-md-9 control-label" title="lbl1">Required ECT of Board</label>


                                        <div class="col-md-3">
                                            <input id="txt_req_ect" type="text" class="form-control" style="height: 30px; text-align: right; background-color: yellow" runat="server" readonly="readonly" maxlength="9" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label id="Label32" runat="server" class="col-md-9 control-label" title="lbl1">Required ∑ RCT Board</label>


                                        <div class="col-md-3">
                                            <input id="txt_req_rct" type="text" class="form-control" style="height: 30px; text-align: right; background-color: yellow" runat="server" readonly="readonly" maxlength="9" />
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label id="Label38" runat="server" class="col-md-12 control-label" title="lbl1" style="background-color: #a6e2f5; align-content: center; text-align: center;">RCT contribution of each ply</label>



                                    </div>
                                    <div class="form-group">
                                        <label id="Label33" runat="server" class="col-md-9 control-label" title="lbl1">Top Ply(%)</label>


                                        <div class="col-md-3">
                                            <input id="txt_topply" type="text" class="form-control" style="height: 30px; text-align: right;" runat="server" maxlength="9" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label id="Label34" runat="server" class="col-md-9 control-label" title="lbl1">Liner ply(%)</label>


                                        <div class="col-md-3">
                                            <input id="txt_linerply" type="text" class="form-control" style="height: 30px; text-align: right;" runat="server" maxlength="9" />
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label id="Label35" runat="server" class="col-md-9 control-label" title="lbl1">Flute Ply(%)</label>


                                        <div class="col-md-3">
                                            <input id="txt_fluteply" type="text" class="form-control" style="height: 30px; text-align: right;" runat="server" maxlength="9" />
                                        </div>

                                    </div>



                                </div>
                            </div>

                            <!-- 3rd-->
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label id="Label56" runat="server" class="col-md-3 control-label" title="lbl1" style="height: 70px; background-color: #a6e2f5; align-content: center;">PLY</label>
                                    <label id="Label57" runat="server" class="col-md-2 control-label" title="lbl1" style="height: 70px; background-color: #a6e2f5; text-align: center;">Reqd RCT of Paper</label>
                                    <label id="Label58" runat="server" class="col-md-2 control-label" title="lbl1" style="height: 70px; background-color: #a6e2f5; text-align: center;">Select BF of paper</label>
                                    <label id="Label59" runat="server" class="col-md-3 control-label" title="lbl1" style="height: 70px; background-color: #a6e2f5; text-align: center;">Calculated GSM of Paper Based on Selected Bf</label>
                                    <label id="Label60" runat="server" class="col-md-2 control-label" title="lbl1" style="height: 70px; background-color: #a6e2f5; text-align: center;">Select GSM </label>


                                </div>

                                <div class="form-group">
                                    <label id="Label61" runat="server" class="col-md-3 control-label" title="lbl1">Top Liner</label>

                                    <div class="col-md-2">
                                        <input id="txt_toplinera" type="text" class="form-control" style="height: 30px" runat="server" readonly="readonly" maxlength="9" />
                                    </div>
                                    <div class="col-md-2">
                                        <select id="ddtoplinerb" runat="server" class="form-control">
                                        </select>
                                    </div>
                                    <div class="col-md-3">
                                        <input id="txttoplinerc" type="text" class="form-control" style="height: 30px" runat="server" readonly="readonly" maxlength="9" />
                                    </div>
                                    <div class="col-md-2">
                                        <select id="ddtoplinerd" runat="server" class="form-control">
                                            <option value="0">0</option>
                                            <option value="100" selected="selected">100</option>
                                            <option value="110">110</option>
                                            <option value="120">120</option>
                                            <option value="140">140</option>
                                            <option value="150">150</option>
                                            <option value="170">170</option>
                                            <option value="180">180</option>
                                            <option value="200">200</option>
                                            <option value="225">225</option>
                                            <option value="250">250</option>
                                            <option value="275">275</option>
                                        </select>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label id="Label40" runat="server" class="col-md-3 control-label" title="lbl1">Flute 1</label>

                                    <div class="col-md-2">
                                        <input id="txtflute1a" type="text" class="form-control" style="height: 30px" runat="server" readonly="readonly" maxlength="9" />
                                    </div>
                                    <div class="col-md-2">
                                        <select id="ddflute1b" runat="server" class="form-control">
                                        </select>
                                    </div>
                                    <div class="col-md-3">
                                        <input id="txtflute1c" type="text" class="form-control" style="height: 30px" runat="server" readonly="readonly" maxlength="9" />
                                    </div>
                                    <div class="col-md-2">
                                        <select id="ddfluted" runat="server" class="form-control">
                                            <option value="0">0</option>
                                            <option value="100" selected="selected">100</option>
                                            <option value="110">110</option>
                                            <option value="120">120</option>
                                            <option value="140">140</option>
                                            <option value="150">150</option>
                                            <option value="170">170</option>
                                            <option value="180">180</option>
                                            <option value="200">200</option>
                                            <option value="225">225</option>
                                            <option value="250">250</option>
                                            <option value="275">275</option>
                                        </select>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label id="Label41" runat="server" class="col-md-3 control-label" title="lbl1">Middle Liner</label>

                                    <div class="col-md-2">
                                        <input id="txtmidlinera" type="text" class="form-control" style="height: 30px" runat="server" readonly="readonly" maxlength="9" />
                                    </div>
                                    <div class="col-md-2">
                                        <select id="ddmidlinerb" runat="server" class="form-control">
                                        </select>
                                    </div>
                                    <div class="col-md-3">
                                        <input id="txtmidlinerc" type="text" class="form-control" style="height: 30px" runat="server" readonly="readonly" maxlength="9" />
                                    </div>
                                    <div class="col-md-2">
                                        <select id="ddmidlinerd" runat="server" class="form-control">
                                            <option value="0">0</option>
                                            <option value="100" selected="selected">100</option>
                                            <option value="110">110</option>
                                            <option value="120">120</option>
                                            <option value="140">140</option>
                                            <option value="150">150</option>
                                            <option value="170">170</option>
                                            <option value="180">180</option>
                                            <option value="200">200</option>
                                            <option value="225">225</option>
                                            <option value="250">250</option>
                                            <option value="275">275</option>
                                        </select>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label id="Label42" runat="server" class="col-md-3 control-label" title="lbl1">Flute 2</label>

                                    <div class="col-md-2">
                                        <input id="txtflute2a" type="text" class="form-control" style="height: 30px" runat="server" readonly="readonly" maxlength="9" />
                                    </div>
                                    <div class="col-md-2">
                                        <select id="ddflute2b" runat="server" class="form-control">
                                        </select>
                                    </div>
                                    <div class="col-md-3">
                                        <input id="txtflute2c" type="text" class="form-control" style="height: 30px" readonly="readonly" runat="server" maxlength="9" />
                                    </div>
                                    <div class="col-md-2">
                                        <select id="ddflute2d" runat="server" class="form-control">
                                            <option value="0">0</option>
                                            <option value="100" selected="selected">100</option>
                                            <option value="110">110</option>
                                            <option value="120">120</option>
                                            <option value="140">140</option>
                                            <option value="150">150</option>
                                            <option value="170">170</option>
                                            <option value="180">180</option>
                                            <option value="200">200</option>
                                            <option value="225">225</option>
                                            <option value="250">250</option>
                                            <option value="275">275</option>
                                        </select>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label id="Label43" runat="server" class="col-md-3 control-label" title="lbl1">Inner Liner</label>

                                    <div class="col-md-2">
                                        <input id="txtinlinera" type="text" class="form-control" style="height: 30px" runat="server" readonly="readonly" maxlength="9" />
                                    </div>
                                    <div class="col-md-2">
                                        <select id="ddinlinerb" runat="server" class="form-control">
                                        </select>
                                    </div>
                                    <div class="col-md-3">
                                        <input id="txtinlinerc" type="text" readonly="readonly" class="form-control" runat="server" maxlength="9" />
                                    </div>
                                    <div class="col-md-2">
                                        <select id="ddinlinerd" runat="server" class="form-control">
                                            <option value="0">0</option>
                                            <option value="100" selected="selected">100</option>
                                            <option value="110">110</option>
                                            <option value="120">120</option>
                                            <option value="140">140</option>
                                            <option value="150">150</option>
                                            <option value="170">170</option>
                                            <option value="180">180</option>
                                            <option value="200">200</option>
                                            <option value="225">225</option>
                                            <option value="250">250</option>
                                            <option value="275">275</option>
                                        </select>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label id="Label44" runat="server" class="col-md-3 control-label" title="lbl1">Depth factor</label>

                                    <div class="col-md-2">
                                        <input id="txtdepfaca" type="text" class="form-control" style="height: 30px" runat="server" readonly="readonly" maxlength="9" />
                                    </div>
                                    <div class="col-md-2">
                                        <input id="txtdepfacb" type="text" class="form-control" runat="server" style="background-color: grey; height: 30px" readonly="readonly" maxlength="9" />
                                    </div>
                                    <div class="col-md-3">
                                        <input id="txtdepfacc" type="text" class="form-control" style="height: 30px" runat="server" value="L/W Factor" maxlength="9" />
                                    </div>
                                    <div class="col-md-2">
                                        <input id="txtdepfacd" type="text" class="form-control" style="height: 30px" runat="server" readonly="readonly" maxlength="9" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label id="Label45" runat="server" class="col-md-10 control-label" title="lbl1">Total Board GSM</label>


                                    <div class="col-md-2">
                                        <input id="txttotalgsm" type="text" class="form-control" style="height: 30px; text-align: right; background-color: yellow" runat="server" readonly="readonly" maxlength="9" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label id="Label46" runat="server" class="col-md-10 control-label" title="lbl1">Total Weight of  Carton in grams</label>


                                    <div class="col-md-2">
                                        <input id="txttotalwght" type="text" class="form-control" style="height: 30px; text-align: right; background-color: yellow" runat="server" readonly="readonly" maxlength="9" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label id="Label55" runat="server" class="col-md-10 control-label" title="lbl1">Total Board BS in Kg/Sq.cm</label>


                                    <div class="col-md-2">
                                        <input id="txttotalboardbs" type="text" class="form-control" style="height: 30px; text-align: right; background-color: yellow" readonly="readonly" runat="server" maxlength="9" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label id="Label62" runat="server" class="col-md-10 control-label" title="lbl1">Total CS in kgs of Carton</label>


                                    <div class="col-md-2">
                                        <input id="txttotalcs" type="text" class="form-control" style="height: 30px; text-align: right; background-color: yellow" runat="server" readonly="readonly" maxlength="9" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label id="Label63" runat="server" class="col-md-10 control-label" title="lbl1">∑ RCT of board based on GSM & BF selected</label>


                                    <div class="col-md-2">
                                        <input id="txttotalrct" type="text" class="form-control" style="background-color: yellow; text-align: right; font-size: medium; font: bold; height: 30px" runat="server" readonly="readonly" maxlength="9" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label id="Label64" runat="server" class="col-md-10 control-label" title="lbl1">ECT of board based on GSM & BF selected</label>


                                    <div class="col-md-2">
                                        <input id="txttotalect" type="text" class="form-control" style="background-color: yellow; text-align: right; font-size: medium; font: bold; height: 30px" runat="server" readonly="readonly" maxlength="9" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label id="Label65" runat="server" class="col-md-10 control-label" title="lbl1">Difference between Reqd.ECT and Selected ECT</label>


                                    <div class="col-md-2">
                                        <input id="txttotaldiff" type="text" class="form-control" style="background-color: yellow; text-align: right; font-size: medium; font: bold; height: 30px" runat="server" readonly="readonly" maxlength="9" />
                                    </div>
                                </div>
                                <div class="form-group" style="text-align: center">
                                    <div class="col-md-12">
                                        <asp:Button ID="btncal" runat="server" CssClass="btn" Style="background-color: #ecf0f5; border: groove" OnClick="btncal_Click" Text="Click to Calculate" Font-Bold="true" ToolTip="Pease click the calculate button after entering all values to calculate  Box-Cost." />
                                    </div>
                                </div>

                            </div>

                        </div>
                    </div>

                </div>

                <div class="col-md-12">
                    <div>
                        <div class="box-body">
                            <p><b>RCT ECT BS CS</b></p>
                            <h2>Corrugation Terms</h2>
                            <p>Customers of Corrugated boxes usually specify the CS and BS they want</p>
                            <p>They also sometimes specify the RCT and ECT</p>
                            <p>What are they about ? </p>
                            <ol>
                                <li>Ring Crush Test (RCT) to evaluate the vertical rigidity of Paper</li>
                                <li>Edge Crush Test (ECT) to evaluate the vertical load carrying capacity or rigidity in the direction of the flutes.</li>
                                <li>Better RCT &amp; Better ECT = Better Carton</li>
                            </ol>

                            <p>
                                CS = Compression Strength ... Higher CS means that the Box can withstand higher weight of the boxes on the top of each other.
                                BS = Bursting Strength ... Higher BS means Box will not Burst under a particular High Pressure. Again Higher BS is safer for the protection of the Contents inside.
                                At the same time, more CS and more BS means , more money, More cost
                                So, here comes the intellegence... to tell you what is right and what is not so correct... based on logics and science and experience.
                            </p>

                            Learn about them at<br />
                            <p><a target="_blank" href="https://pocketdriver.in/corrugated-boxes-erp-software-for-rct-ect-bs-cs/">1. Corrugated Boxes ERP Software for RCT ECT BS CS</a></p>
                            <p><a target="_blank" href="https://www.prestogroup.com/blog/bursting-strength-of-corrugated-boxes/">2. https://www.prestogroup.com/blog/bursting-strength-of-corrugated-boxes/</a></p>
                            <p><a target="_blank" href="https://www.prestogroup.com/products-new/compressive-testing-machine/">3. https://www.prestogroup.com/products-new/compressive-testing-machine/</a></p>
                            <p><a target="_blank" href="https://en.wikipedia.org/wiki/Corrugated_box_design#Estimating_compression">4. https://en.wikipedia.org/wiki/Corrugated_box_design#Estimating_compression</a></p>
                            <p><a target="_blank" href="https://www.westpak.com/page/resources/calculator/bct-calculator">5. https://www.westpak.com/page/resources/calculator/bct-calculator</a></p>

                            <p>Tejaxo ERP CS-BS Module helps you in Exact estimation of Compression strength- Burst Factor etc which are critical factors for the F/G . Based on best industry experts scientific formulations, Takes into account technical specifications attributed for Raw Material selection, Finished product handling specifications, Required BF , Required R C T , Ply ,Flute, CS ,Caliper ,Area etc</p>
                            <h3>The Finsys software calculates =</h3>
                            <ul>
                                <li>Total Board GSM ,</li>
                                <li>Total Weight of Carton in Grams</li>
                                <li>Total Board BS in Kg/sq.cm</li>
                                <li>Total CS in Kgs of carton</li>
                                <li>&sum; RCT of board based on GSM &amp; BF selected</li>
                                <li>ECT of board based on GSM &amp; BF selected.</li>
                                <li>Difference between Reqd. ECT and selected ECT</li>
                            </ul>
                            <h3>Benefit</h3>
                            <p>You enter &ldquo;Specs&rdquo; and &ldquo;desired information&rdquo;</p>
                            <p>You get</p>
                            <ul>
                                <li>The alternate paper that can be used of same or slightly different specifications.</li>
                                <li>The effect of various combinations can be seen instantly&hellip;..</li>
                                <li>Thereby helping in quick decision making, best cost and best ECT-BCT combination</li>
                            </ul>
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
            var tabName = $("[id*=TabName]").val() != "" ? $("[id*=TabName]").val() : "DescTab1";
            $('#Tabs a[href="#' + tabName + '"]').tab('show');
            $("#Tabs a").click(function () {
                $("[id*=TabName]").val($(this).attr("href").replace("#", ""));
            });
        });
    </script>
    <asp:HiddenField ID="TabName" runat="server" />
</asp:Content>


