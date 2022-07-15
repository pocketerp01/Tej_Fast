<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="om_mould_disp" CodeFile="om_mould_disp.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../tej-base/Scripts/gridviewScroll.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {

        });
        function fill_zero(val) { if (isNaN(val)) return 0; if (isFinite(val)) return val; }
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
                                <label id="Label1" runat="server" class="col-sm-2 control-label">Entry_No</label>
                                <div class="col-sm-1">
                                    <asp:Label ID="lbl1a" runat="server" Text="TC" Style="width: 22px;float:right;" CssClass="col-sm-2 control-label"></asp:Label>
                                </div>
                                <div class="col-sm-3">
                                    <input id="txtvchnum" type="text" class="form-control" runat="server" placeholder="Entry_No" readonly="readonly" />
                                </div>
                                <label id="Label8" runat="server" class="col-sm-1 control-label" title="lbl1">Date</label>
                                <div class="col-sm-5">
                                    <asp:TextBox ID="txtvchdate" placeholder="Date" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                            <asp:CalendarExtender ID="txtvchdate_CalendarExtender" runat="server"
                                                Enabled="True" TargetControlID="txtvchdate"
                                                Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="Maskedit1" runat="server" Mask="99/99/9999"
                                                MaskType="Date" TargetControlID="txtvchdate" />
                                </div>                                
                            </div>
                            
                            <div class="form-group" runat="server" visible="false">
                                <label id="Label25" runat="server" class="col-sm-3 control-label" title="lbl1">Raw_Matl_Supplier</label>
                                <div class="col-sm-1" id="divacode" runat="server">
                                    
                                </div>
                                <div class="col-sm-3" style="display:none;">
                                    <input id="txtacode" type="text" class="form-control" runat="server" readonly="readonly"  />
                                </div>
                                <div class="col-sm-8">
                                    <input id="txtsuppname" type="text" class="form-control" runat="server" readonly="readonly"  />
                                </div>
                            </div>

                           <div class="form-group" runat="server" visible="false">
                                <label id="Label15" runat="server" class="col-sm-4 control-label" title="lbl1">Upload_Supervision_With</label>
                               <%-- <div class="col-sm-1" id="div2" runat="server">
                                    <asp:ImageButton ID="ImageButton1" runat="server" ToolTip="Select Party" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnprod_click" Visible="False" />
                                </div>
                                <div class="col-sm-3">
                                    <input id="txtprcode" type="text" class="form-control" runat="server" readonly="readonly"  />
                                </div>--%>
                                <div class="col-sm-8">
                                    <input id="txtupl_sup" type="text" class="form-control" runat="server" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label9" runat="server" class="col-sm-2 control-label" title="lbl1">Mould</label>
                                <div class="col-sm-1" id="div1" runat="server">
                                    <asp:ImageButton ID="btnitem" runat="server" ToolTip="Select Raw Material" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnitem_Click" />
                                </div>
                                <div class="col-sm-3">
                                    <input id="txticode" type="text" class="form-control" runat="server" readonly="readonly"  />                                    
                                </div>
                                <div class="col-sm-1"></div>
                                <div class="col-sm-5">
                                    <input id="txtitmname" type="text" class="form-control" readonly="readonly" runat="server"  />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group" runat="server" visible="false">
                                <label id="Label6" runat="server" class="col-sm-3 control-label">Mrr_No</label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnmrr" runat="server" ToolTip="Select MRR" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnmrr_click" />
                                </div>
                                <div class="col-sm-3">
                                    <input id="txtmrrno" type="text" class="form-control" runat="server" placeholder="MRR_No" readonly="readonly" />
                                </div>
                                <label id="Label11" runat="server" class="col-sm-2 control-label" title="lbl1">MRR_Date</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtmrrdate" placeholder="Date" runat="server" ReadOnly="true" Width="100%" CssClass="form-control"></asp:TextBox>
                                            <%--<asp:CalendarExtender ID="txtmrrdate_CalendarExtender" runat="server"
                                                Enabled="True" TargetControlID="txtmrrdate"
                                                Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="MaskedEditExtender1" runat="server" Mask="99/99/9999"
                                                MaskType="Date" TargetControlID="txtmrrdate" />--%>
                                </div>
                            </div>
                            <div class="form-group" runat="server" visible="false">
                                <label id="Label7" runat="server" class="col-sm-4 control-label" title="lbl1">Bill_No</label>
                                <div class="col-sm-8">
                                    <asp:TextBox id="txtbill_no" type="text" CssClass="form-control" runat="server" ReadOnly="true"  maxlength="75" />
                                </div>
                                <%--<label id="Label10" runat="server" class="col-sm-2 control-label" title="lbl1">Part Name</label>--%>
                                <div class="col-sm-3" style="display:none;">
                                    <asp:TextBox id="txtbill_date" type="text" CssClass="form-control" runat="server"  maxlength="100" />
                                </div>
                            </div>
                            
                            <div class="form-group">
                              <label id="Label32" runat="server" class="col-sm-4  control-label" title="lbl1">Reason_For_Deactivation</label>
                                <div class="col-sm-8">
                                    <input id="txt_qty_rcv"  class="form-control" runat="server" maxlength="150" />
                                 </div>

                                  <%--<label id="Label20" runat="server" class="col-sm-2 control-label" title="lbl1">First_HM_Count</label>
                                <div class="col-sm-3">
                                    <input id="txtfirst_hm_count"  class="form-control" runat="server" onkeypress="return isDecimalKey(event)" maxlength="10" />
                                 </div>--%>
                              </div>
                            <div class="form-group">
                                <label id="Label4" runat="server" class="col-sm-4  control-label" title="lbl1">Done_by</label>
                                 <div class="col-sm-8">
                                    <input id="txt_sample_tak"  class="form-control" runat="server"  maxlength="30" />
                                 </div>
                            </div>
                            
                        </div>
                    </div>
                </div>

                <div class="col-md-6" runat="server" visible="false">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <label id="Label3" runat="server" class="col-sm-4 control-label" title="lbl1">Location_In_Store</label>
                                <div class="col-sm-8">
                                    <input id="txtlocn" type="text" class="form-control" runat="server"  maxlength="70" />
                                </div>

                                <%--<label id="Label12" runat="server" class="col-sm-2 control-label" title="lbl1">Commision_Dt</label>
                                <div class="col-sm-3">
                                    <input id="txtcmsn_dt" type="date" class="form-control" runat="server"  style="font-size: small" />
                                </div>--%>
                            </div>
                            <div class="form-group">
                                <label id="Label2" runat="server" class="col-sm-4 control-label" title="lbl1">No_Of_Pack_&_Pack_Size</label>
                                <div class="col-sm-8">
                                    <input id="txt_pck_size"  type="text" class="form-control" runat="server"  maxlength="60" />
                                </div>
                                <%--<label id="Label6" runat="server" class="col-sm-2 control-label" title="lbl1">Mould_Size</label>
                                <div class="col-sm-3">
                                    <input id="txtmld_sz" type="text" style="width: 100%;" class="form-control" runat="server" placeholder="L*B*H" maxlength="25" />
                                </div>--%>
                            </div>

                            
                           
                        </div>
                    </div>
                </div>

                <div class="col-md-6" runat="server" visible="false">
                    <div>
                        <div class="box-body">
                                                <div class="form-group">
                                <label id="Label5" runat="server" class="col-sm-4 control-label" title="lbl1">Approval_Req_Sent_To_QC_On</label>
                                <div class="col-sm-8">
                                    <asp:TextBox id="txt_req_sent" style="width: 100%;" CssClass="form-control" runat="server" />
                                    <asp:CalendarExtender ID="txt_req_sent_CalendarExtender" runat="server"
                                                Enabled="True" TargetControlID="txt_req_sent"
                                                Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="MaskedEditExtender1" runat="server" Mask="99/99/9999"
                                                MaskType="Date" TargetControlID="txt_req_sent" />

                                </div>                               
                            </div>
                            <div class="form-group">
                                <label id="Label26" runat="server" class="col-sm-4 control-label" title="lbl1">Approved_Req_Retrn_By_QC_On</label>
                                <div class="col-sm-8">
                                    <asp:TextBox id="txt_req_rtrn"  style="width: 100%;" CssClass="form-control" runat="server"/>
                                    <asp:CalendarExtender ID="txt_req_rtrn_CalendarExtender" runat="server"
                                                Enabled="True" TargetControlID="txt_req_rtrn"
                                                Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="MaskedEditExtender2" runat="server" Mask="99/99/9999"
                                                MaskType="Date" TargetControlID="txt_req_rtrn" />
                                </div>
                               
                            </div>        
                            <div class="form-group" style="display:none;">
                                 <label id="Label21" runat="server" class="col-sm-4 control-label" title="lbl1">Edt_By</label>
                                <div class="col-sm-8">
                                    <input id="txt_edtby"  class="form-control" runat="server" onkeyup="caltotshot()"  readonly="true" />
                                </div>
                                <div class="col-sm-4 ">
                                    <label id="Label19" runat="server" title="lbl1">Edt_dt</label>
                                </div>
                                <div class="col-sm-8">
                                    <input id="txt_edt_dt"  class="form-control" runat="server" maxlength="100" />
                                </div>
                            </div>

                           
                        </div>
                    </div>
                </div>

                <div class="col-md-12" id="div3" runat="server" visible="false">
                    <div>
                        <div class="box-body">
                            <asp:Label ID="lbltxtrmk" runat="server" Text="Remarks" Font-Bold="true" CssClass="col-sm-2 control-label" ></asp:Label>
                            <asp:TextBox ID="txtrmk" runat="server" Width="99%" MaxLength="300" placeholder="Remarks upto 300 Char" ></asp:TextBox>
                        </div>
                    </div>
                </div>

                   <section class="col-lg-12 connectedSortable" id="AllTabs" runat="server">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">
                                <li><a href="#DescTab" id="tab1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab">List</a></li>
                            </ul>

                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane active" id="DescTab">
                                    <div class="lbBody" style="height: 400px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <asp:GridView ID="sg1" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="100%" Font-Size="13px">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
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

<script>
    function caltotshot() {
        //var value1 = fill_zero(document.getElementById("ContentPlaceHolder1_txt_prev_cnt").value),
        //value2 = fill_zero(document.getElementById("ContentPlaceHolder1_txt_tot_shot").value),
        //value3 = fill_zero(document.getElementById("ContentPlaceHolder1_txt_tool_lyf").value);

        //var valuetot = fill_zero(Number(value1) + Number(value2));
        //document.getElementById("ContentPlaceHolder1_txt_shot").value = valuetot;
        //var valuebal = fill_zero(Number(value3) - Number(valuetot));
        //document.getElementById("ContentPlaceHolder1_txtblnc").value = valuebal;

    }

    function fill_zero(val) { if (isNaN(val)) return 0; if (isFinite(val)) return val; }
    </script>

    <asp:HiddenField ID="TabName" runat="server" />
</asp:Content>
