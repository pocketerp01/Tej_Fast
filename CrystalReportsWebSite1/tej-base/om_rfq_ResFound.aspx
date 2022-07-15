<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="om_rfq_ResFound6" CodeFile="om_rfq_ResFound.aspx.cs" %>

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
                                <label id="Label1" runat="server" class="col-sm-3 control-label">Entry_No</label>
                                <div class="col-sm-1">
                                    <asp:Label ID="lbl1a" runat="server" Text="TC" Style="width: 22px; float: right;" CssClass="col-sm-2 control-label"></asp:Label>
                                </div>
                                <div class="col-sm-3">
                                     <asp:TextBox id="txtvchnum" CssClass="form-control" runat="server" ReadOnly="true"  maxlength="75" />
                                </div>
                                <label id="Label8" runat="server" class="col-sm-1 control-label" >Date</label>
                                <div class="col-sm-4">
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
                                <label id="Label12" runat="server" class="col-sm-3 control-label" >RFQ_No.</label>
                                <div class="col-sm-1" id="div2" runat="server">
                                    <asp:ImageButton ID="btnlbl4" runat="server" ToolTip="Select RFQ" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl4_Click" />
                                </div>
                                <div class="col-sm-3">
                                <asp:TextBox id="txtRFQ" CssClass="form-control" runat="server" ReadOnly="true"  maxlength="75" />
                                </div>
                                <div class="col-sm-5">
                                  <asp:TextBox id="txtRFQDt" CssClass="form-control" runat="server" ReadOnly="true"  maxlength="75" />
                                </div>
                            </div>
                            <div class="form-group" style="display:none">
                                <label id="Label25" runat="server" class="col-sm-3 control-label" >Customer</label>
                                <div class="col-sm-1" id="divacode" runat="server">
                                </div>
                                <div class="col-sm-3">
                                     <asp:TextBox id="txtAcode" CssClass="form-control" runat="server" ReadOnly="true"  maxlength="75" />
                                </div>
                                <div class="col-sm-5">
                                   <asp:TextBox id="txtFstr" CssClass="form-control" runat="server" ReadOnly="true"  maxlength="75" />
                                    <asp:TextBox ID="txtTest" runat="server" Width="100%" Height="30px" ></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label9" runat="server" class="col-sm-3 control-label" >Item_Name</label>
                                <div class="col-sm-1" id="div1" runat="server">
                                </div>
                                <div class="col-sm-3" >
                                     <asp:TextBox id="txtIcode" CssClass="form-control" runat="server" ReadOnly="true"  maxlength="75" />
                                </div>
                                <div class="col-sm-5">
                                    <asp:TextBox id="txtIname" CssClass="form-control" runat="server" ReadOnly="true"  maxlength="75" />
                                </div>
                            </div>

                           <div class="form-group">
                                <label id="Label15" runat="server" class="col-sm-4 control-label" >Drg/Part_No</label>
                                <div class="col-sm-8">
                                    <asp:TextBox id="txtCpartNo" CssClass="form-control" runat="server" ReadOnly="true"  maxlength="75" />
                                </div>
                            </div>
                              <div class="form-group">
                                <label id="Label6" runat="server" class="col-sm-4 control-label" >Raw_Part_No</label>
                                <div class="col-sm-8">
                                    <asp:TextBox id="txtRaw" CssClass="form-control" runat="server"  maxlength="50" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <div class="col-sm-2" id="div4" runat="server">
                                    <label id="Label26" runat="server">SF Code</label>
                                    <asp:ImageButton ID="btnChild" runat="server" ToolTip="Select RFQ" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnChild_Click" />
                                </div>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtChildCode" CssClass="form-control" runat="server" MaxLength="8" ReadOnly="true" Width="100%" />
                                </div>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtChildName" CssClass="form-control" runat="server" ReadOnly="true" Width="100%" />
                                    <asp:TextBox ID="txtParentChild" CssClass="form-control" runat="server" ReadOnly="true" Width="100%" Visible="false" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label11" runat="server" class="col-sm-2 control-label">Mat_Grade</label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="txtMaterial" CssClass="form-control" runat="server" MaxLength="50" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label7" runat="server" class="col-sm-2 control-label">Target_Wt</label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtTarget" CssClass="form-control" runat="server" ReadOnly="true" MaxLength="7" onkeypress="return isDecimalKey(event)" />
                                </div>
                                <label id="Label10" runat="server" class="col-sm-2 control-label">Finish_Wt</label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtFinish" CssClass="form-control" runat="server" MaxLength="7" onkeypress="return isDecimalKey(event)" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label32" runat="server" class="col-sm-2  control-label">Cast_Wt</label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtCast" CssClass="form-control" runat="server" MaxLength="7" onkeypress="return isDecimalKey(event)" onkeyup="cal()" />
                                </div>

                                <label id="Label20" runat="server" class="col-sm-2 control-label">Bunch_Wt</label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtBunch" CssClass="form-control" runat="server" MaxLength="7" onkeypress="return isDecimalKey(event)" ReadOnly="true" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label4" runat="server" class="col-sm-2  control-label">Yield%</label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtYield" CssClass="form-control" runat="server" MaxLength="7" onkeypress="return isDecimalKey(event)" onkeyup="cal()" />
                                </div>
                                <label id="Label3" runat="server" class="col-sm-2 control-label">No.Of_Cavity</label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtCavity" CssClass="form-control" runat="server" MaxLength="7" onkeypress="return isDecimalKey(event)" onkeyup="cal()"/>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <label id="Label13" runat="server" class="col-sm-3 control-label" >No.Of_Core</label>
                                <div class="col-sm-3">
                           <asp:TextBox id="txtCore" CssClass="form-control" runat="server"  maxlength="7" onkeypress="return isDecimalKey(event)" Width="100%"/>
                                </div>
                     <label id="Label2" runat="server" class="col-sm-3 control-label" >Core_Wt</label>
                                <div class="col-sm-3">
                                    <asp:TextBox id="txtCore_Wt" CssClass="form-control" runat="server"  maxlength="7" onkeypress="return isDecimalKey(event)" Width="100%"/>
                                </div>                           
                            </div>
                            <div class="form-group">
                                <label id="Label14" runat="server" class="col-sm-3 control-label" >Core_Type</label>
                                <div class="col-sm-9">
                                    <asp:TextBox id="txtCore_Type" CssClass="form-control" runat="server"  maxlength="50" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label16" runat="server" class="col-sm-3 control-label" >Surface_Treatment</label>
                                <div class="col-sm-9">
                                    <asp:TextBox id="txtSurface" CssClass="form-control" runat="server"  maxlength="100" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label17" runat="server" class="col-sm-3 control-label" >Heat_Treatment</label>
                                <div class="col-sm-9">
                                    <asp:TextBox id="txtHeat" CssClass="form-control" runat="server"  maxlength="45" />
                                </div>
                            </div>

                             <div class="form-group">
                                <label id="Label5" runat="server" class="col-sm-3 control-label" >Sleeve_Cost</label>
                                <div class="col-sm-9">
                                    <asp:TextBox id="txtSleeve" CssClass="form-control" runat="server"  maxlength="7" onkeypress="return isDecimalKey(event)"/>
                                </div></div>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                         <div class="form-group">
                                <label id="Label18" runat="server" class="col-sm-2 control-label" >Lab_Consent</label>
                                <div class="col-sm-10">
                                    <asp:TextBox id="txtLab" style="width: 100%;" CssClass="form-control" runat="server" MaxLength="50" />
                                </div>                                 
                            </div>

                                 <div class="form-group">
                                <label id="Label19" runat="server" class="col-sm-2 control-label" >Chem_Det</label>
                                <div class="col-sm-10">
                                    <asp:TextBox id="txtChemistry" style="width: 100%;" CssClass="form-control" runat="server" MaxLength="50" />
                                </div></div>
                                     
                              <div class="form-group">
                                <label id="Label21" runat="server" class="col-sm-2 control-label" >Extra_Proc</label>
                                <div class="col-sm-10">
                                    <asp:TextBox id="txtExtra" style="width: 100%;" CssClass="form-control" runat="server" MaxLength="50" />
                                </div>                        
                        </div>

                            <div class="form-group" style="display:none">
                                <label id="Label22" runat="server" class="col-sm-2 control-label" >Feasibility</label>
                                <div class="col-sm-10">
                                    <asp:TextBox id="txtFeasiblity" style="width: 100%;" CssClass="form-control" runat="server" MaxLength="50" />
                                </div> 
                    </div>
                            <div class="form-group">
                                <label id="Label23" runat="server" class="col-sm-2 control-label" >Rejection%</label>
                                <div class="col-sm-10">
                                    <asp:TextBox id="txtRejection" style="width: 100%;" CssClass="form-control" runat="server" MaxLength="7" onkeypress="return isDecimalKey(event)"/>
                                </div>
                                </div>
                             <div class="form-group">
                              <label id="Label24" runat="server" class="col-sm-2 control-label" >Tooling_Cost</label>
                                <div class="col-sm-10">
                                    <asp:TextBox id="txtTooling" style="width: 100%;" CssClass="form-control" runat="server" onkeypress="return isDecimalKey(event)" MaxLength="7"/>
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
                                                    <asp:ListItem Text="CONDITIONALLY_APPROVE" Value="CONDITIONALLY_APPROVE"></asp:ListItem>
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
                                        <asp:TemplateField>
                                            <HeaderTemplate>Remarks</HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:TextBox ID="sg1_t5" runat="server" Text='<%#Eval("sg1_t5") %>' Width="100%" MaxLength="50"></asp:TextBox>
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
        </div></section>

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
    function cal() {
        var castwt = 0;
        var yield_per = 0;
        var bunch = 0
        var cavity = 0;
        var cast_cavity = 0;
        castwt = fill_zero(document.getElementById("ContentPlaceHolder1_txtCast").value);
        cavity = fill_zero(document.getElementById("ContentPlaceHolder1_txtCavity").value);
        yield_per = fill_zero(document.getElementById("ContentPlaceHolder1_txtYield").value);
        cast_cavity = (castwt * cavity);
        bunch = (cast_cavity / yield_per) * 100;
        document.getElementById("ContentPlaceHolder1_txtBunch").value = bunch.toFixed(2);
    }
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