<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="om_ECN" CodeFile="om_ECN.aspx.cs" %>

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
                                <label id="Label1" runat="server" class="col-sm-3 control-label">RFQ_No</label>
                                <div class="col-sm-1">
                                    <asp:Label ID="lbl1a" runat="server" Text="TC" Style="width: 22px; float: right;" CssClass="col-sm-2 control-label"></asp:Label>
                                </div>
                                <div class="col-sm-3">
                                    <input id="txtvchnum" type="text" class="form-control" runat="server"  readonly="readonly" />
                                </div>
                                <label id="Label8" runat="server" class="col-sm-2 control-label" title="lbl1">Inquiry_Date</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtvchdate"  runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                            <asp:CalendarExtender ID="txtvchdate_CalendarExtender" runat="server"
                                                Enabled="True" TargetControlID="txtvchdate"
                                                Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="Maskedit1" runat="server" Mask="99/99/9999"
                                                MaskType="Date" TargetControlID="txtvchdate" />
                                </div>                        
                            </div>         
                            
                            <div class="form-group">
                                <label id="Label25" runat="server" class="col-sm-3 control-label" title="lbl1">Customer</label>   
                                 <div class="col-sm-1" id="divacode" runat="server">
                                    <asp:ImageButton ID="btnrfq" runat="server" ToolTip="Select Customer" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnrfq_Click" />
                                </div>                          
                                <div class="col-sm-3">
                                    <input id="txtacode" type="text" class="form-control"  runat="server" readonly="readonly"  />
                                </div>
                                <div class="col-sm-5">
                                    <input id="txtsuppname" type="text" class="form-control" runat="server"  readonly="readonly"  />
                                </div>
                            </div>


                            <div class="form-group">
                                <label id="Label9" runat="server" class="col-sm-3 control-label" title="lbl1">Item</label>
                            <div class="col-sm-1" id="div1" runat="server">
                                    <asp:ImageButton ID="btnitem" runat="server" ToolTip="Select Item" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnitem_Click1" />
                                </div> 
                                <div class="col-sm-3" >
                                    <input id="txticode" type="text" class="form-control"  runat="server" readonly="readonly"  />
                                </div>
                                <div class="col-sm-5">
                                    <input id="txtDragNo" type="text" class="form-control"   readonly="readonly"  runat="server"  />
                                </div>
                            </div>

                           <div class="form-group">
                                <label id="Label15" runat="server" class="col-sm-4 control-label" title="lbl1">Item Name</label>
                          
                                <div class="col-sm-8">
                                    <input id="txtitmname" readonly="readonly" type="text"  class="form-control" runat="server" />
                                </div>
                            </div>

                               <div class="form-group">
                                <label id="Label6" runat="server" class="col-sm-4 control-label">Priority</label>                                                                  
                                <div class="col-sm-3">
                                    <input id="txtPriority" type="text" class="form-control" runat="server" maxlength="50"  />
                                </div>
                                <label id="Label5" runat="server" class="col-sm-2 control-label" title="lbl1">Payment_Terms</label>
                                <div class="col-sm-3">
                                    <input id="txtpymtterm" type="text" readonly="true"  class="form-control" runat="server"  maxlength="20" />
                                </div>
                            </div>
                            <div class="form-group" style="display:none;">
                                <label id="Label11" runat="server" class="col-sm-4 control-label" title="lbl1">Drawing</label>
                                <div class="col-sm-3">
                                      <input id="txtDrawing" type="text" class="form-control" maxlength="50" runat="server"  />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <label id="Label7" runat="server" class="col-sm-3 control-label" title="lbl1">Target_dt_for_Implement</label>
                                <div class="col-sm-3">
                          <%--  <input id="txtTrgtDt" type="date" class="form-control" runat="server" />--%>
                                    <asp:TextBox ID="txtTrgtDt" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>                                     
                                     <asp:CalendarExtender ID="txtTrgtDt_CalendarExtender" runat="server"
                                                Enabled="True" TargetControlID="txtTrgtDt"
                                                Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="Maskedit3" runat="server" Mask="99/99/9999"
                                                MaskType="Date" TargetControlID="txtTrgtDt" />
                                </div>
                                <label id="Label10" runat="server" class="col-sm-3 control-label" title="lbl1">Current_Price</label>

                                <div class="col-sm-3" >                                     
                                    <asp:TextBox id="txtPrice" type="text" CssClass="form-control" runat="server"  maxlength="8" />
                                </div>
                            </div>
                            
                            <div class="form-group">
                              <label id="Label32" runat="server" class="col-sm-3  control-label" title="lbl1">Target_Casting_Wt</label>
                                <div class="col-sm-3">
                                    <input id="txtTrgtWt" class="form-control" runat="server"  onkeypress="return isDecimalKey(event)" maxlength="10" />
                                 </div>

                                  <label id="Label20" runat="server" class="col-sm-3 control-label" title="lbl1">Lead_Time</label>
                                <div class="col-sm-3">
                                    <input id="txtLdtime"  class="form-control" type="time" runat="server"  maxlength="10" />
                                 </div>
                              </div>
                            <div class="form-group">
                              <label id="Label14" runat="server" class="col-sm-3  control-label" title="lbl1">Existing_Component_Wt</label>
                                <div class="col-sm-3">
                                    <input id="txtCompWt" class="form-control" runat="server" onkeypress="return isDecimalKey(event)" maxlength="9" />
                                 </div>

                                  <label id="Label16" runat="server" class="col-sm-3 control-label" title="lbl1">Existing_Fdy_Tool_Cost</label>
                                <div class="col-sm-3">
                                    <input id="txtFdyToolCost"  class="form-control" runat="server" onkeypress="return isDecimalKey(event)" maxlength="11" />
                                 </div>
                              </div>
                            <div class="form-group">
                              <label id="Label4" runat="server" class="col-sm-3  control-label" title="lbl1">Existing_Mch_Tool_Cost</label>
                                <div class="col-sm-3">
                                    <input id="txtMchToolcost" class="form-control" runat="server" onkeypress="return isDecimalKey(event)" maxlength="8" />
                                 </div>

                                  <label id="Label17" runat="server" class="col-sm-3 control-label" title="lbl1">New_Casting_Price</label>
                                <div class="col-sm-3">
                                    <input id="TxtCastPrice"  class="form-control"  runat="server" onkeypress="return isDecimalKey(event)" maxlength="11" />
                                 </div>
                              </div>

                            <div class="form-group">
                                 <label id="Label3" runat="server" class="col-sm-3 control-label" title="lbl1">New_Fdy_Tooling_Cost</label>
                                <div class="col-sm-3">
                                    <input id="txtToolcost" type="text"  class="form-control" onkeypress="return isDecimalKey(event)" runat="server"  maxlength="10" />
                                </div>
                                <label id="Label2" runat="server" class="col-sm-3 control-label" title="lbl1">New_Shop_Machining_Prc</label>
                                <div class="col-sm-3">
                                    <input id="txtShopMchPrice" type="text" class="form-control"  onkeypress="return isDecimalKey(event)" runat="server"  maxlength="9" />
                                </div>
                            </div>                                                       
                        </div>
                    </div>
                </div>           

                <div class="col-md-12" id="div3" runat="server">
                    <div>
                        <div class="box-body">
                            <asp:Label ID="lbltxtrmk" runat="server" Text="Remarks" Font-Bold="true" CssClass="col-sm-2 control-label" ></asp:Label>
                            <%--<asp:TextBox ID="txtrmk" runat="server" Width="99%" MaxLength="300" placeholder="Remarks upto 300 Char" ></asp:TextBox>--%>
                            <input id="txtRmk" runat="server" type="text" class="form-control" maxlength="300" />
                        </div>
                    </div>
                </div>

                <div class="col-md-12" >
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <asp:FileUpload ID="Attch" CssClass="col-sm-1" runat="server" Visible="true" onchange="submitFile()"></asp:FileUpload><%--</td>--%>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtAttch" runat="server" Width="110%" ReadOnly="true" MaxLength="100" placeholder="File Name 100 Char"></asp:TextBox>
                                </div>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtAttchPath" runat="server" Width="101%" ReadOnly="true" MaxLength="250" placeholder="Path Upto 250 Char"></asp:TextBox><%--</td>--%>
                                </div>
                            </div>
                            <asp:Button ID="btnAtt" runat="server" Text="Attachment" OnClick="btnAtt_Click" Width="134px" Style="display: none" />
                            <asp:Label ID="lblShow" runat="server" Visible="false"></asp:Label>
                            <asp:Label ID="lblUpload" runat="server" Style="display: none"></asp:Label>

                            <asp:ImageButton ID="btnView1" runat="server" ImageUrl="~/tej-base/images/preview-file.png" OnClick="btnView1_Click" Visible="false" />
                            <asp:ImageButton ID="btnDwnld1" runat="server" ImageUrl="~/tej-base/images/Save.png" OnClick="btnDwnld1_Click" Visible="false" />


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
