<%@ Page Title="" Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="om_caliper_flute" CodeFile="om_caliper_flute.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
 

    <script src="../tej-base/Scripts/gridviewScroll.min.js" type="text/javascript"></script>
    
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
                    <td style="text-align:right">
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
                                <div  class="col-sm-3 control-label">
                                <label id="Label19" runat="server"  title="lbl1">Voucher_No</label>                                
                                 <asp:Label ID="lbl1a" runat="server" Text="TC" Style="width: 22px;"></asp:Label>
                                </div>
                                <div class="col-sm-3">
                                    <input id="txt_code" type="text" class="form-control" runat="server" readonly="readonly" placeholder="Voucher_No"   />
                                </div>
                                 <label id="Label1" runat="server" class="col-sm-3 control-label" title="lbl1">Voucher_Date</label>
                               
                                <div class="col-sm-3">
                                     <asp:TextBox ID="txtvchdate" placeholder="Date" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtvchdate_CalendarExtender" runat="server"
                                                Enabled="True" TargetControlID="txtvchdate"
                                                Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="Maskedit1" runat="server" Mask="99/99/9999"
                                                MaskType="Date" TargetControlID="txtvchdate" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label11" runat="server" class="col-sm-3 control-label" title="lbl1">Flute</label>
                                
                                <div class="col-sm-9">
                                    <input id="txt_param1" type="text" class="form-control" runat="server"  maxlength="20" />
                                </div>
                            </div>

                             <div class="form-group">
                                <label id="Label12" runat="server" class="col-sm-3 control-label" title="lbl1">Index1</label>
                                
                                <div class="col-sm-9">
                                    <input id="txt_param2" type="text" class="form-control" runat="server"  maxlength="20" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label14" runat="server" class="col-sm-3 control-label" title="lbl1">FEFCO code</label>
                                
                                <div class="col-sm-9">
                                    <input id="txt_param3" type="text" class="form-control" runat="server" maxlength="100" />
                                </div>
                            </div>
                            
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">

                             <div class="form-group">
                                <label id="Label13" runat="server" class="col-sm-3 control-label" title="lbl1">Caliper</label>                               
                                <div class="col-sm-9">
                                    <input id="txt_param4" type="text"   class="form-control" runat="server" maxlength="50" />
                                </div>
                            </div>

                              <div class="form-group">
                             <label id="Label15" runat="server" class="col-sm-3 control-label" title="lbl1">Remark</label>
                               <div class="col-sm-9">
                                <input id="txt_param5" type="text" class="form-control" runat="server"  maxlength="20" />
                                 </div>
                                  </div>
 
                             <div class="form-group">
                                <label id="Label16" runat="server" class="col-sm-3 control-label" title="lbl1" >Index2</label>                                
                                <div class="col-sm-9">
                                    <input id="txt_param6" type="text" class="form-control" runat="server" onkeypress="return isDecimalKey(event)" maxlength="6" />
                                </div>
                                 </div>
                                         
                              <div class="form-group">
                                <label id="Label17" runat="server" class="col-sm-3 control-label" title="lbl1">Area</label>                                
                                <div class="col-sm-9">
                                    <input id="txt_param7" type="text" class="form-control" runat="server"  maxlength="100" />
                                </div>
                            </div>
                        </div>
                </div>
           </div>     
                                               
                <div  runat="server" id="tab_upload">
            <%--<section class="content">
            <div class="row">--%>
                <div class="col-md-12">
                    <div>
                        <div class="box-body">
                          
                                        <asp:FileUpload ID="Attch" runat="server" Visible="true" onchange="submitFile()" Enabled="false" />
                                 
                                        <asp:TextBox ID="txtAttch" runat="server" MaxLength="100" placeholder="Path Upto 100 Char" ></asp:TextBox>                                                                          
                                        <asp:TextBox ID="txtAttchPath" Width="350px" runat="server" MaxLength="250" placeholder="Path Upto 250 Char" ></asp:TextBox>
                                         <asp:Label ID="Label27" runat="server" Text=" Please Link Correct File upto 3MB Size ." ></asp:Label>

                         <asp:Button ID="btnAtt" runat="server" Text="Attachment"  OnClick="btnAtt_Click" Width="134px" />
                          <asp:Label ID="lblShow" runat="server"></asp:Label>
                            <asp:Label ID="lblUpload" runat="server" ></asp:Label>
                            <asp:ImageButton ID="btnView1" ToolTip="View Image" runat="server" ImageUrl="~/tej-base/images/preview-file.png" OnClick="btnView1_Click"  />
                            <asp:ImageButton ID="btnDwnld1" ToolTip="Save Image" runat="server" ImageUrl="~/tej-base/images/Save.png" OnClick="btnDwnld1_Click"  />
                            </div>
                        </div>
                    </div>

           <%-- </div></section>--%>
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
   <%-- <script type="text/javascript">
        $(function () {
            var tabName = $("[id*=TabName]").val() != "" ? $("[id*=TabName]").val() : "DescTab1";
            $('#Tabs a[href="#' + tabName + '"]').tab('show');
            $("#Tabs a").click(function () {
                $("[id*=TabName]").val($(this).attr("href").replace("#", ""));
            });
        });
    </script>--%>
    <asp:HiddenField ID="TabName" runat="server" />
</asp:Content>


