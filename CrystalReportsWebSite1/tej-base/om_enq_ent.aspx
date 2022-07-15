<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="om_enq_ent" CodeFile="om_enq_ent.aspx.cs" %>

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
                                    <input id="txtvchnum" type="text" class="form-control" runat="server" placeholder="Entry_No" readonly="readonly" />
                                </div>
                                <label id="Label8" runat="server" class="col-sm-1 control-label">Date</label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtvchdate" placeholder="Date" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtvchdate_CalendarExtender" runat="server"
                                        Enabled="True" TargetControlID="txtvchdate"
                                        Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="Maskedit1" runat="server" Mask="99/99/9999"
                                        MaskType="Date" TargetControlID="txtvchdate" />
                                </div>
                                <%--    <div class="col-sm-2">
                                    <asp:TextBox ID="txtvtime" runat="server" ReadOnly="true"
                                        placeholder="" Width="100%" CssClass="form-control"></asp:TextBox>
                                </div>--%>
                            </div>

                            <div class="form-group">
                                <label id="Label25" runat="server" class="col-sm-3 control-label">Customer</label>
                                <div class="col-sm-1" id="divacode" runat="server">
                                    <asp:ImageButton ID="btnacode" runat="server" ToolTip="Select Customer" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnacode_Click" />
                                </div>
                                <div class="col-sm-3">
                                    <input id="txtacode" type="text" class="form-control" runat="server" readonly="readonly" />
                                </div>
                                <div class="col-sm-5">
                                    <input id="txtsuppname" type="text" class="form-control" runat="server" readonly="readonly" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label9" runat="server" class="col-sm-3 control-label">Item_Name</label>
                                <div class="col-sm-1" id="div1" runat="server">
                                    <asp:ImageButton ID="btnitem" runat="server" ToolTip="Select Item" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnitem_Click" />
                                </div>
                                <div class="col-sm-3">
                                    <input id="txticode" type="text" class="form-control" runat="server" readonly="readonly" />
                                   <input id="txtTotChild_RF" type="text" class="form-control" runat="server" readonly="readonly" visible="false" />
                                    <input id="txtTotChild_MC" type="text" class="form-control" runat="server" readonly="readonly" visible="false" />
                                </div>
                                <div class="col-sm-5">
                                    <input id="txtitmname" type="text" class="form-control" readonly="readonly" runat="server" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label15" runat="server" class="col-sm-4 control-label">Drg/Part_No</label>
                                <div class="col-sm-8">
                                    <input id="txtupl_sup" type="text" class="form-control" runat="server" readonly="true" />
                                </div>
                            </div>

                            <div class="form-group" id="prior" runat="server">
                                <label id="lblPriority" runat="server" class="col-sm-4 control-label">Priority</label>
                                <div class="col-sm-8">
                                    <input id="txtPriority" type="text" class="form-control" runat="server" maxlength="50" />
                                </div>
                            </div>
                             <div class="form-group" id="Div2" runat="server">
                             <label id="Label29" runat="server" class="col-sm-4 control-label">Payment_Terms</label>
                                <div class="col-sm-8">
                                    <input id="txtpymtterm" type="text" readonly="true" class="form-control" runat="server" maxlength="20" />
                                </div></div>
                        </div>
                    </div>
                </div>

                <div class="col-md-6" id="ecntextbox" runat="server">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <label id="Label12" runat="server" class="col-sm-3 control-label">Target_dt_for_Implement</label>
                                <div class="col-sm-3">
                                    <%--  <input id="txtTrgtDt" type="date" class="form-control" runat="server" />--%>
                                    <asp:TextBox ID="txtTrgtDt" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtTrgtDt_CalendarExtender" runat="server"
                                        Enabled="True" TargetControlID="txtTrgtDt"
                                        Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="MaskedEditExtender1" runat="server" Mask="99/99/9999"
                                        MaskType="Date" TargetControlID="txtTrgtDt" />
                                </div>
                                <label id="Label13" runat="server" class="col-sm-3 control-label">Current_Price</label>

                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtPrice" type="text" CssClass="form-control" runat="server" MaxLength="8" ReadOnly="true"/>
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label14" runat="server" class="col-sm-3  control-label">Target_Casting_Wt</label>
                                <div class="col-sm-3">
                                    <input id="txtTrgtWt" class="form-control" runat="server" onkeypress="return isDecimalKey(event)" maxlength="10" />
                                </div>

                                <label id="Label16" runat="server" class="col-sm-3 control-label">Lead_Time</label>
                                <div class="col-sm-3">
                                    <input id="txtLdtime" class="form-control" type="time" runat="server" maxlength="10" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label17" runat="server" class="col-sm-3  control-label">Existing_Component_Wt</label>
                                <div class="col-sm-3">
                                    <input id="txtCompWt" class="form-control" runat="server" onkeypress="return isDecimalKey(event)" maxlength="9" readonly="true"/>
                                </div>

                                <label id="Label18" runat="server" class="col-sm-3 control-label">Existing_Fdy_Tool_Cost</label>
                                <div class="col-sm-3">
                                    <input id="txtFdyToolCost" class="form-control" runat="server" onkeypress="return isDecimalKey(event)" maxlength="11" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label22" runat="server" class="col-sm-3  control-label">Existing_Mch_Tool_Cost</label>
                                <div class="col-sm-3">
                                    <input id="txtMchToolcost" class="form-control" runat="server" onkeypress="return isDecimalKey(event)" maxlength="8" />
                                </div>

                                <label id="Label23" runat="server" class="col-sm-3 control-label">New_Casting_Price</label>
                                <div class="col-sm-3">
                                    <input id="TxtCastPrice" class="form-control" runat="server" onkeypress="return isDecimalKey(event)" maxlength="11" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label24" runat="server" class="col-sm-3 control-label">New_Fdy_Tooling_Cost</label>
                                <div class="col-sm-3">
                                    <input id="txtToolcost" type="text" class="form-control" onkeypress="return isDecimalKey(event)" runat="server" maxlength="10" />
                                </div>
                              <label id="Label30" runat="server" class="col-sm-3 control-label">Drawing_Rev_No.</label>
                                <div class="col-sm-3">
                                    <input id="txtDrawingRev" type="text" class="form-control" onkeypress="return isDecimalKey(event)" runat="server" maxlength="9" />
                                </div>
                                <div class="col-sm-1" style="display: none;">
                                    <asp:TextBox ID="txtTest" runat="server" Width="100%" Height="30px"></asp:TextBox>
                                     <asp:TextBox ID="txtMC_Flag" runat="server" Width="100%" Height="30px"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                              <label id="Label27" runat="server" class="col-sm-3 control-label">New_Shop_Machining_Prc</label>
                                <div class="col-sm-9">
                                    <input id="txtShopMchPrice" type="text" class="form-control" onkeypress="return isDecimalKey(event)" runat="server" maxlength="9" />
                                </div></div>
                        </div>
                    </div>
                </div>

                <div class="col-md-6" id="enqentbox" runat="server">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <label id="Label6" runat="server" class="col-sm-3 control-label">Volume_Per_Year</label>

                                <div class="col-sm-3">
                                    <input id="txtvolpyr" type="text" class="form-control" onkeypress="return isDecimalKey(event)" runat="server" maxlength="7" />
                                </div>
                                <label id="Label11" runat="server" class="col-sm-3 control-label">SOP</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtsopdate" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtsopdate_CalendarExtender" runat="server"
                                        Enabled="True" TargetControlID="txtsopdate"
                                        Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="Maskedit3" runat="server" Mask="99/99/9999"
                                        MaskType="Date" TargetControlID="txtsopdate" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label7" runat="server" class="col-sm-3 control-label">Target_Price</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txttarget" type="text" CssClass="form-control" runat="server" onkeypress="return isDecimalKey(event)" MaxLength="7" />
                                </div>
                                <label id="Label10" runat="server" class="col-sm-3 control-label">Target_Casting_Weight</label>
                                <%--<label id="Label10" runat="server" class="col-sm-2 control-label" >Part Name</label>--%>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txttrgtwgt" type="text" CssClass="form-control" runat="server" onkeypress="return isDecimalKey(event)" MaxLength="7" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label32" runat="server" class="col-sm-3  control-label">Lead_Time_Development</label>
                                <div class="col-sm-3">
                                    <input id="txt_lead_time" class="form-control" runat="server" maxlength="25" type="time" />
                                </div>

                                <label id="Label20" runat="server" class="col-sm-3 control-label">As_cast/Fully_Finish</label>
                                <div class="col-sm-3">
                                    <input id="txt_cast" class="form-control" runat="server" maxlength="30" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label4" runat="server" class="col-sm-3  control-label">Annual_Business</label>
                                <div class="col-sm-9">
                                    <input id="txt_ann_bus" class="form-control" runat="server" onkeypress="return isDecimalKey(event)" maxlength="12" />
                                </div>
                            </div>

                        </div>
                    </div>
                </div>

                <div class="col-md-6" id="enqentbox2" runat="server">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <label id="Label3" runat="server" class="col-sm-4 control-label">Delivery_Location</label>
                                <div class="col-sm-8">
                                    <input id="txtlocn" type="text" class="form-control" runat="server" readonly="readonly" maxlength="70" />
                                </div>
                                <%--<label id="Label12" runat="server" class="col-sm-2 control-label" >Commision_Dt</label>
                                <div class="col-sm-3">
                                    <input id="txtcmsn_dt" type="date" class="form-control" runat="server"  style="font-size: small" />
                                </div>--%>
                            </div>
                            <div class="form-group">
                                <label id="Label2" runat="server" class="col-sm-4 control-label">Payment_Terms</label>
                                <div class="col-sm-8">
                                    <input id="txt_paymt_term" type="text" class="form-control" runat="server" readonly="readonly" maxlength="60" />
                                </div>
                                <%--<label id="Label6" runat="server" class="col-sm-2 control-label" >Mould_Size</label>
                                <div class="col-sm-3">
                                    <input id="txtmld_sz" type="text" style="width: 100%;" class="form-control" runat="server" placeholder="L*B*H" maxlength="25" />
                                </div>--%>
                            </div>



                        </div>
                    </div>
                </div>

                <div class="col-md-6" id="enqentbox3" runat="server">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <label id="Label5" runat="server" class="col-sm-3 control-label">Priority</label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txt_req_sent" Style="width: 100%;" MaxLength="20" CssClass="form-control" runat="server" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label26" runat="server" class="col-sm-3 control-label">Other</label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txt_req_rtrn" Style="width: 100%;" MaxLength="250" CssClass="form-control" runat="server" />
                                </div>
                            </div>
                            <div class="form-group" style="display: none;">
                                <label id="Label21" runat="server" class="col-sm-4 control-label">Edt_By</label>
                                <div class="col-sm-8">
                                    <input id="txt_edtby" class="form-control" runat="server" readonly="true" />
                                </div>
                                <div class="col-sm-4 ">
                                    <label id="Label19" runat="server">Edt_dt</label>
                                </div>
                                <div class="col-sm-8">
                                    <input id="txt_edt_dt" class="form-control" runat="server" maxlength="100" />
                                </div>
                            </div>


                        </div>
                    </div>
                </div>

                <div class="col-md-12" id="div3" runat="server">
                    <div>
                        <div class="box-body">
                            <asp:Label ID="lbltxtrmk" runat="server" Text="Remarks" Font-Bold="true" CssClass="col-sm-2 control-label"></asp:Label>
                            <asp:TextBox ID="txtrmk" runat="server" Width="99%" MaxLength="300" placeholder="Remarks upto 300 Char"></asp:TextBox>
                        </div>
                        <div class="box-body" id="ecnrmk" runat="server">
                            <asp:Label ID="Label28" runat="server" Text="M/C Remarks" Font-Bold="true" CssClass="col-sm-2 control-label"></asp:Label>
                            <asp:TextBox ID="txtrmk2" runat="server" Width="99%" MaxLength="300" placeholder="Remarks upto 300 Char"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-12" style="display:none">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <asp:FileUpload ID="Attch" CssClass="col-sm-1" runat="server" Visible="true" onchange="submitFile()"></asp:FileUpload><%--</td>--%>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtAttch" runat="server" Width="110%" ReadOnly="true" MaxLength="100" placeholder="File Name 100 Char"></asp:TextBox>
                                </div>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtAttchPath" runat="server" Width="101%" ReadOnly="true" MaxLength="100" placeholder="Path Upto 100 Char"></asp:TextBox><%--</td>--%>
                                </div>
                            </div>
                            <asp:Button ID="btnAtt" runat="server" Text="Attachment" OnClick="btnAtt_Click" Width="134px" Style="display: none" />
                            <asp:Label ID="lblShow" runat="server" Visible="false"></asp:Label>
                            <asp:Label ID="lblUpload" runat="server" Style="display: none"></asp:Label>

                            <asp:ImageButton ID="btnView1" ToolTip="View File" runat="server" ImageUrl="~/tej-base/images/preview-file.png" OnClick="btnView1_Click" Visible="false" />
                            <asp:ImageButton ID="btnDwnld1" ToolTip="Download File" runat="server" ImageUrl="~/tej-base/images/Save.png" OnClick="btnDwnld1_Click" Visible="false" />
                        </div>
                    </div>
                </div>

                <div class="col-md-12">
                    <div>
                        <div class="box-body">
                            <div class="lbBody" id="gridDiv" style="color: White; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                <fin:CoolGridView ID="sg1" runat="server" ForeColor="#333333"
                                    Style="background-color: #FFFFFF; color: White;" Width="100%" Height="150px" Font-Size="13px"
                                    AutoGenerateColumns="false" OnRowDataBound="sg1_RowDataBound"
                                    OnRowCommand="sg1_RowCommand">
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>Del</HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="sg1_btnrmv" runat="server" CommandName="SG1_RMV" ImageUrl="../tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Attachment" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>Download</HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="sg1_btndown" runat="server" CommandName="SG1_DWN" ImageUrl="~/tej-base/images/Save.png" Width="20px" ImageAlign="Middle" ToolTip="Download Attachment" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>View</HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="sg1_btnview" runat="server" CommandName="SG1_VIEW" ImageUrl="~/tej-base/images/preview-file.png" Width="20px" ImageAlign="Middle" ToolTip="View Attachment" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>Drawing_Type</HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:TextBox ID="sg1_t1" runat="server" Text='<%#Eval("sg1_t1") %>' Width="100%" MaxLength="50" ReadOnly="true"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>Yes/No</HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DropDownList ID="sg1_t2" runat="server" Width="100%">
                                                    <asp:ListItem Text="PLEASE SELECT" Value="PLEASE SELECT"></asp:ListItem>
                                                    <asp:ListItem Text="YES" Value="YES"></asp:ListItem>
                                                    <asp:ListItem Text="NO" Value="NO"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:HiddenField ID="cmd1" Value='<%#Eval("sg1_t2") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="sg1_t3" HeaderText="File Name" />
                                        <asp:BoundField DataField="sg1_t4" HeaderText="File Path" />
                                        <asp:TemplateField>
                                            <HeaderTemplate>FileUpload</HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:FileUpload ID="FileUpload1" runat="server" EnableViewState="true" onChange="FileUploadCall(this)" ToolTip="Do not Use Special Characters for File Name"/>
                                                <asp:Button ID="btnUpload" runat="server" CommandName="SG1_UPLD" Text="OK" OnClick="btnUpload_Click" Style="display: none" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EditRowStyle BackColor="#999999" />
                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader2" />
                                    <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager2" />
                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem2" />
                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                </fin:CoolGridView>
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

    <script>
        function FileUploadCall(fileUpload) {
            if (fileUpload.value != '') {
                var a = $(fileUpload).next("[id*='btnUpload']");
                a.click();
            }
        }

        function fill_zero(val) { if (isNaN(val)) return 0; if (isFinite(val)) return val; }
    </script>

    <asp:HiddenField ID="TabName" runat="server" />
</asp:Content>