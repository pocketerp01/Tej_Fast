<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="om_mld_mast" CodeFile="om_mld_mast.aspx.cs" %>

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
                                <label id="Label1" runat="server" class="col-sm-2 control-label">Entry_No</label>
                                <div class="col-sm-1">
                                    <asp:Label ID="lbl1a" runat="server" Text="TC" Style="width: 22px; float: right;" CssClass="col-sm-2 control-label"></asp:Label>
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
                            <div class="form-group">
                                <label id="Label9" runat="server" class="col-sm-2 control-label" title="lbl1">Mould_Code</label>
                                <div class="col-sm-1" id="div1" runat="server">
                                    <asp:ImageButton ID="btnmldcode" runat="server" ToolTip="Select Mould" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnmldcode_Click" />
                                </div>
                                <div class="col-sm-3">
                                    <input id="txtmldcode" type="text" class="form-control" readonly="readonly" runat="server"  />
                                </div>
                                <div class="col-sm-6">
                                    <input id="txtmldname" type="text" class="form-control" readonly="readonly" runat="server"  />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label25" runat="server" class="col-sm-2 control-label" title="lbl1">Customer_Name</label>
                                <div class="col-sm-1" id="divacode" runat="server">
                                    <asp:ImageButton ID="btnacode" runat="server" ToolTip="Select Party" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnacode_click" Visible="False" />
                                </div>
                                <div class="col-sm-3">
                                    <input id="txtacode" type="text" class="form-control" runat="server" readonly="readonly"  />
                                </div>
                                <div class="col-sm-6">
                                    <input id="txtcustname" type="text" class="form-control" runat="server" readonly="readonly"  />
                                </div>
                            </div>

                           <div class="form-group">
                                <label id="Label15" runat="server" class="col-sm-2 control-label" title="lbl1">Product_Name</label>
                                <div class="col-sm-1" id="div2" runat="server">
                                    <asp:ImageButton ID="ImageButton1" runat="server" ToolTip="Select Party" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnprod_click" Visible="False" />
                                </div>
                                <div class="col-sm-3">
                                    <input id="txtprcode" type="text" class="form-control" runat="server" readonly="readonly"  />
                                </div>
                                <div class="col-sm-6">
                                    <input id="txtprname" type="text" class="form-control" runat="server" readonly="readonly"  />
                                </div>
                            </div>

                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <label id="Label7" runat="server" class="col-sm-3 control-label" title="lbl1">Part No</label>
                                <div class="col-sm-4">
                                    <input id="txtpart_no" type="text" class="form-control" runat="server"  maxlength="75" />
                                </div>
                                <label id="Label10" runat="server" class="col-sm-2 control-label" title="lbl1">Part Name</label>
                                <div class="col-sm-3">
                                    <input id="txtpart_name" type="text" class="form-control" runat="server"  maxlength="100" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label3" runat="server" class="col-sm-3 control-label" title="lbl1">Model_Name</label>
                                <div class="col-sm-4">
                                    <input id="txtmodel" type="text" class="form-control" runat="server"  maxlength="70" />
                                </div>

                                <label id="Label12" runat="server" class="col-sm-2 control-label" title="lbl1">Commision_Dt</label>
                                <div class="col-sm-3">
                                    <input id="txtcmsn_dt" type="date" class="form-control" runat="server"  style="font-size: small" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label2" runat="server" class="col-sm-3 control-label" title="lbl1">Mould ID</label>
                                <div class="col-sm-4">
                                    <input id="txtmld_id" readonly="true"  type="text" class="form-control" runat="server"  maxlength="60" />
                                </div>
                                <label id="Label6" runat="server" class="col-sm-2 control-label" title="lbl1">Mould_Size</label>
                                <div class="col-sm-3">
                                    <input id="txtmld_size" type="text" style="width: 100%;" class="form-control" runat="server" placeholder="L*B*H" maxlength="25" />
                                </div>
                            </div>
                            <div class="form-group">
                              <label id="Label32" runat="server" class="col-sm-3 control-label" title="lbl1">Material</label>
                                <div class="col-sm-4">
                                    <input id="txtmat"  class="form-control" runat="server"  maxlength="100" />
                                 </div>

                                  <label id="Label20" runat="server" class="col-sm-2 control-label" title="lbl1">First_HM_Count</label>
                                <div class="col-sm-3">
                                    <input id="txtfirst_hm_count"  class="form-control" runat="server" onkeypress="return isDecimalKey(event)" maxlength="10" />
                                 </div>
                              </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <label id="Label5" runat="server" class="col-sm-3 control-label" title="lbl1">No_of_Cavities</label>
                                <div class="col-sm-3">
                                    <input id="txtn_cavit" onkeypress="return isDecimalKey(event)" style="width: 100%;" class="form-control" runat="server"  maxlength="10"  readonly="true"/>
                                </div>
                                <label id="Label27" runat="server" class="col-sm-3 control-label" title="lbl1">PM_Freq_Shots</label>
                                <div class="col-sm-3">
                                    <input id="txt_pm_freq_shots" onkeypress="return isDecimalKey(event)" style="width: 100%;" class="form-control" runat="server"  maxlength="10" readonly="true"/>
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label26" runat="server" class="col-sm-3 control-label" title="lbl1">Tool_Life</label>
                                <div class="col-sm-3">
                                    <input id="txt_tool_lyf" onkeypress="return isDecimalKey(event)" style="width: 100%;" class="form-control" runat="server" onkeyup="caltotshot()" maxlength="20" readonly="true"/>
                                </div>
                                <label id="Label29" runat="server" class="col-sm-3 control-label" title="lbl1">HM_Freq_Shots</label>
                                <div class="col-sm-3">
                                    <input id="txt_hm_freq_shots" onkeypress="return isDecimalKey(event)" class="form-control" runat="server"  maxlength="10" readonly="true"/>
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label16" runat="server" class="col-sm-3 control-label" title="lbl1">Cycle_Time</label>
                                <div class="col-sm-3">
                                    <input id="txt_cycle_tm" onkeypress="return isDecimalKey(event)" class="form-control" runat="server"  maxlength="10" />
                                </div>
                          <label id="Label13" runat="server" class="col-sm-3 control-label" title="lbl1">Clamping_Tonng</label>
                                <div class="col-sm-3">
                                    <input id="txt_tonnage" onkeypress="return isDecimalKey(event)" class="form-control" runat="server"  maxlength="10" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <label id="Label22" runat="server" class="col-sm-3 control-label" title="Shot_till_Acquire ">Shots_At_Acquisition</label>
                                <div class="col-sm-4">
                                    <input id="txt_shots_acq" onkeypress="return isDecimalKey(event)" class="form-control" onkeyup="caltotshot()" runat="server"   maxlength="10"/>
                                </div>
                                  <label id="Label31" runat="server" class="col-sm-2 control-label" >Balance_Shot</label>
                                <div class="col-sm-3">
                                    <input id="txtblnc" title="total shots- Our_shots" onkeypress="return isDecimalKey(event)" class="form-control" onkeyup="caltotshot()" runat="server" readonly="true" placeholder="Balance Shot" maxlength="10" />
                                </div>

                            </div>
                            <div class="form-group">                              
                                <label id="Label30" runat="server" class="col-sm-3 control-label" title="lbl1">CO_Total_Shots</label>
                                <div class="col-sm-4">
                                    <input id="txt_tot_shot" onkeypress="return isDecimalKey(event)" class="form-control" onkeyup="caltotshot()" runat="server"  maxlength="10" />
                                </div>
                               
                            </div>
                            <div class="form-group">
                                 <label id="Label21" runat="server" class="col-sm-2 control-label" title="lbl1">Tot_Shot_Till_Dt</label>
                                <div class="col-sm-3">
                                    <input id="txt_shot"  class="form-control" runat="server" onkeyup="caltotshot()"  readonly="true" />
                                </div>
                                <div class="col-sm-3 ">
                                    <label id="Label19" runat="server" title="lbl1">Mould_Name_ID</label>
                                </div>
                                <div class="col-sm-9">
                                    <input id="txt_mould_name_id"  class="form-control" runat="server" maxlength="100" />
                                </div>
                            </div>

                            <div class="form-group">

                                 <label id="Label4" runat="server" class="col-sm-2 control-label" visible="false" title="lbl1">HM_Checkup_Frequency</label>
                                <div class="col-sm-3">
                                    <input id="txt_hm_chk_freq" onkeypress="return isDecimalKey(event)" visible="false" class="form-control" runat="server"  maxlength="10" />
                                </div>
                                 </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <label id="Label18" runat="server" class="col-sm-3 control-label" title="lbl1">PM_Freq_Mths</label>
                                <div class="col-sm-3">
                                    <input id="txt_pm_freq_mth" onkeypress="return isDecimalKey(event)" class="form-control" runat="server" maxlength="10" />
                                </div>
                                 <label id="Label14" runat="server" class="col-sm-3 control-label" title="lbl1">PM_Alert_Shots_Mail</label>
                                <div class="col-sm-3">
                                    <input id="txt_pm_alert" onkeypress="return isDecimalKey(event)" class="form-control" runat="server"  maxlength="10" />
                                </div>
                           </div>                            
                            <div class="form-group">
                                <label id="Label17" runat="server" class="col-sm-3 control-label" title="lbl1">Last_PM_Date</label>
                                <div class="col-sm-3">
                                    <input id="txt_last_pm_dt" type="date" style="font-size:12px" class="form-control" runat="server" />
                                </div>
                                <label id="Label24" runat="server" class="col-sm-3 control-label" title="lbl1">Op_PM_Count</label>
                                <div class="col-sm-3">
                                    <input id="txt_op_pm_count" onkeypress="return isDecimalKey(event)" style="width: 100%;" class="form-control" runat="server" maxlength="15" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label33" runat="server" class="col-sm-3 control-label" title="lbl1">Mould_Dispose</label>
                                <div class="col-sm-3">
                                    <input id="txtDispose"  class="form-control" runat="server" maxlength="1" readonly="true" />
                                </div>
                                 <label id="Label35" runat="server" class="col-sm-3 control-label" title="lbl1">Dispose_Dt</label>
                               <div class="col-sm-3">
                                    <input id="txtDisposeDt" style="font-size:12px" class="form-control" runat="server" type="date" readonly="true"/>
                                </div>
                                </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <label id="Label28" runat="server" class="col-sm-3 control-label" title="lbl1">HM_Freq_Mths</label>
                                <div class="col-sm-3">
                                    <input id="txt_hm_freq_mth" onkeypress="return isDecimalKey(event)" class="form-control" runat="server" maxlength="10" />
                                </div>
                                 <label id="Label34" runat="server" class="col-sm-3 control-label" title="lbl1">HM_Alert_Shots_Mail</label>
                                <div class="col-sm-3">
                                    <input id="txt_hm_alert" onkeypress="return isDecimalKey(event)" style="width: 100%;" class="form-control" runat="server"  maxlength="10"/>
                                </div>
                           </div>
                            <div class="form-group">
                                <label id="Label11" runat="server" class="col-sm-3 control-label" title="lbl1">Last_HM_Date</label>
                                <div class="col-sm-3">
                                    <input id="txt_last_hm_dt" style="font-size:12px" class="form-control" runat="server" type="date" max="9999-99-99" />
                                </div>
                                <label id="Label23" runat="server" class="col-sm-3 control-label" title="lbl1">Op_HM_Count</label>
                                <div class="col-sm-3">
                                    <input id="txt_op_hm_count" onkeypress="return isDecimalKey(event)" style="width: 100%;" class="form-control" runat="server" maxlength="15" />
                                </div>
                            </div>
                             <div class="form-group">
                                  <label id="Label36" runat="server" class="col-sm-3 control-label" title="lbl1">Dispose_By</label>
                                <div class="col-sm-9">
                                    <input id="txtDisposeBy" style="width: 100%;" class="form-control" runat="server" maxlength="15" readonly="true"/>
                                </div>
                                 </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-12" id="div3" runat="server">
                    <div>
                        <div class="box-body">
                            <asp:Label ID="lbltxtrmk" runat="server" Text="Remarks" Font-Bold="true" CssClass="col-sm-2 control-label" ></asp:Label>
                            <asp:TextBox ID="txtrmk" runat="server" Width="99%" MaxLength="300" placeholder="Remarks upto 300 Char" ></asp:TextBox>
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
        var value1 = fill_zero(document.getElementById("ContentPlaceHolder1_txt_shots_acq").value),
        value2 = fill_zero(document.getElementById("ContentPlaceHolder1_txt_tot_shot").value),
        value3 = fill_zero(document.getElementById("ContentPlaceHolder1_txt_tool_lyf").value);

        var valuetot = fill_zero(Number(value1) + Number(value2));
        document.getElementById("ContentPlaceHolder1_txt_shot").value = valuetot;
        var valuebal = fill_zero(Number(value3) - Number(valuetot));
        document.getElementById("ContentPlaceHolder1_txtblnc").value = valuebal;

    }

    function fill_zero(val) { if (isNaN(val)) return 0; if (isFinite(val)) return val; }
    </script>

    <asp:HiddenField ID="TabName" runat="server" />
</asp:Content>
