<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="om_multi_item_upt" CodeFile="om_multi_item_upt.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">    
    <script type="text/javascript" src="../tej-base/Scripts/jquery.handsontable.full.js"></script>
    <link rel="Stylesheet" type="text/css" href="../tej-base/Styles/jquery.handsontable.full.css" />
    <script type="text/javascript">
        var size = 2;
        var id = 0;
        function submitFile() {
            $("#<%= btnupload.ClientID%>").click();
            ProgressBar();
        };
        function ProgressBar() {
            if (document.getElementById('<%=FileUpload1.ClientID %>').value != "") {
                document.getElementById("ContentPlaceHolder1_divProgress").style.display = "block";
                document.getElementById("ContentPlaceHolder1_divUpload").style.display = "block";
                id = setInterval("progress()", 20);
                return true;
            }
            else {
                alert("Select a file to upload");
                return false;
            }
        }
        function progress() {
            size = size + 1;
            if (size > 199) {
                clearTimeout(id);
            }
            document.getElementById("ContentPlaceHolder1_divProgress").style.width = size + "pt";
            document.getElementById("<%=lblPercentage.ClientID %>").
                firstChild.data = parseInt(size / 2) + "%";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper">
        <section class="content-header">
            <table style="width: 100%">
                <tr>
                    <td style="text-align:left">
                        <button type="submit" id="btnnew" class="btn btn-info" style="width: 100px;" runat="server" accesskey="N" onserverclick="btnnew_ServerClick"><u>N</u>ew</button>
                        <button type="submit" id="btnedit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="i" onserverclick="btnedit_ServerClick">ed<u>i</u>t</button>
                        <button type="submit" id="btnsave" class="btn btn-info" style="width: 100px;" runat="server" accesskey="s" onserverclick="btnsave_ServerClick"><u>S</u>ave</button>
                        <button type="submit" id="btnupdate" class="btn btn-info" style="width: 100px;" runat="server" accesskey="s" onserverclick="btnupdate_ServerClick"><u>Up</u>date</button>
                        <button type="submit" id="btnprint" class="btn btn-info" style="width: 100px;" runat="server" accesskey="P" onserverclick="btnprint_ServerClick"><u>P</u>rint</button>
                        <button type="submit" id="btndel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="l" onserverclick="btndel_ServerClick">De<u>l</u>ete</button>
                        <button type="submit" id="btnlist" class="btn btn-info" style="width: 100px;" runat="server" accesskey="t" onserverclick="btnlist_ServerClick">Lis<u>t</u></button>
                        <button type="submit" id="btncancel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="c" onserverclick="btncancel_ServerClick"><u>C</u>ancel</button>
                        <button type="submit" id="btnvalidate" class="btn btn-info" style="width: 100px;" runat="server" accesskey="X" onserverclick="btnvalidate_ServerClick">Va<u>li</u>date</button>
                         <button type="submit" id="btnexport" class="btn btn-info" style="width: 100px;" runat="server" accesskey="m" onserverclick="btnexport_ServerClick">Te<u>m</u>plate</button>
                        <button type="submit" id="btnexit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="X" onserverclick="btnexit_ServerClick">E<u>x</u>it</button>
                    </td>
                    <td><asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label></td>

                </tr>
            </table>
        </section>

        <section class="content">
            <div class="row">

                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                               <asp:Label ID="lbl1" runat="server" Text="Please download format, enter desired data w/o changing column name/ position. Choose file, validate then update." CssClass="col-sm-12 control-label"></asp:Label>
                                <asp:Label ID="lbl1a" runat="server" Text="TC" Style="width: 22px;display:none;" CssClass="col-sm-1 control-label"></asp:Label>
                                                              
                                <div class="col-sm-4">
                                   <asp:TextBox ID="txtvchdate" style="display:none;" placeholder="Date" class="form-control" runat="server" Width="100px" ></asp:TextBox>
                                    <asp:CalendarExtender ID="txtvchdate_CalendarExtender"  runat="server"
                                        Enabled="True" TargetControlID="txtvchdate"
                                        Format="dd/MM/yyyy"></asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="Maskedit1" runat="server" Mask="99/99/9999"
                                        MaskType="Date" TargetControlID="txtvchdate" />
                                </div>
                                  <div class="col-sm-4" style="visibility:hidden;">
                                       <asp:TextBox ID="TextBox1" runat="server" Width="80px" ReadOnly="true"></asp:TextBox>
                                   </div>   
                                
                            </div>
                            <div class="form-group">    
                               
                            <asp:FileUpload ID="FileUpload1" runat="server" Width="250px" onchange="submitFile()" Enabled="false"  />
                            <asp:Button ID="btnupload" runat="server" Text="Upload" class="myButton" Style="display: none" OnClick="btnupload_Click" />
                               
                    <div class="col-sm-4">
                            <div id="divUpload" style="display: none" runat="server">
                                <div id="Div1" style="width: 200pt; height: 15px; border: solid 1pt gray" runat="server">
                                    <div id="divProgress" runat="server" style="width: 1pt; height: 15px; background-color: #1797c0; display: none">
                                    </div>
                                </div>
                               
                                <div style="width: 200pt; text-align: center;">
                                    <asp:Label ID="lblPercentage" runat="server" Text="Label"></asp:Label>
                                     </div>
                                 </div>
     </div>
                            </div>
                                       

                               <div class="form-group">
                                  
                                <label id="Label3" runat="server" class="col-sm-3 control-label" title="lbl1">Choose Fields</label>
                                <div class="col-sm-1" id="div4" runat="server">
                                    <asp:ImageButton ID="ImageButton_upt" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnupdatefields_Click" />
                                </div>
                                <div class="col-sm-8">
                                    <input id="txtuptcol" type="text"  readonly="true" class="form-control" runat="server" placeholder="Update Fields" maxlength="40" />
                                </div>
                            
                                    </div>
                                </div>
                        </div>
                    </div>
              

                   <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <label id="Label9" runat="server" class="col-sm-3 control-label" title="lbl1">Max/Min/ROL</label>
                                <div class="col-sm-1" id="divCocd" runat="server">
                                    <asp:ImageButton ID="btnAcode" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnbranchmax_Click" />
                                </div>
                                <div class="col-sm-8">
                                    <input id="txtmax" type="text"  readonly="true" class="form-control" runat="server" placeholder="Select BranchWise Or Head Office" maxlength="40" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label1" runat="server" class="col-sm-3 control-label" title="lbl1" >Irate Value</label>
                                <div class="col-sm-1" id="div2" runat="server">
                                    <asp:ImageButton ID="btnRcode"  runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnbranchirate_Click" />
                                </div>
                             
                                <div class="col-sm-8">
                                    <input id="txtirate" type="text"  readonly="true"  class="form-control" runat="server" placeholder="Select BranchWise Or Head Office" maxlength="40" />
                                </div>
                            </div>

                              <div class="form-group">
                                <label id="Label2" runat="server" class="col-sm-3 control-label" title="lbl1" >Binno Value</label>
                                <div class="col-sm-1" id="div3" runat="server">
                                    <asp:ImageButton ID="ImageButton1"  runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnbranchbinno_Click" />
                                </div>
                               
                                <div class="col-sm-8">
                                    <input id="txtbinno" type="text"  readonly="true" class="form-control" runat="server" placeholder="Select BranchWise Or Head Office" maxlength="40" />
                                </div>
                                 <asp:Label ID="Labelh" runat="server" Text="TC" Style="width: 22px; visibility:hidden;" CssClass="col-sm-1 control-label"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>

                <section class="col-lg-12 connectedSortable">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">
                                <li><a href="#DescTab" id="tab1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab">Form Details</a></li>
                            </ul>
                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane active" id="DescTab">
                                    <div class="lbBody" style="box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <%--testing--%>
                                        <div id="datadiv" style="overflow: scroll; width: auto; height: 280px;" runat="server" class="handsontable"
                                            data-originalstyle="width: auto; height:280px; overflow: scroll">
                                        </div>
                                    </div>

                               <asp:ImageButton ID="btnexptoexl" runat="server" ImageUrl="~/tej-base/images/excel_icon.png"
                            ToolTip="Export to Excel" Width="161px" Height="65px" OnClick="btnexptoexl_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </section>
                <div class="col-md-12">
                    <div>
                        <div class="box-body">
                            <asp:TextBox ID="txtrmk" runat="server" Width="99%" TextMode="MultiLine" placeholder="Remarks" Visible="false"></asp:TextBox>
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
    <asp:HiddenField ID="hfCNote" runat="server" />

    <script type="text/javascript">
        $(function () {
            var tabName = $("[id*=TabName]").val() != "" ? $("[id*=TabName]").val() : "DescTab";
            $('#Tabs a[href="#' + tabName + '"]').tab('show');
            $("#Tabs a").click(function () {
                $("[id*=TabName]").val($(this).attr("href").replace("#", ""));
            });
        });
    </script>
    <asp:HiddenField ID="TabName" runat="server" />
</asp:Content>
