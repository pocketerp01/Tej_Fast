<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="om_mrr_edi" CodeFile="om_mrr_edi.aspx.cs" %>

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
        function closePopup() { parent.$.colorbox.close() }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content" style="background-color: #ecf0f5">
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
                                <asp:Label ID="lbl1" runat="server" Text="EntryNo" CssClass="col-sm-2 control-label"></asp:Label>
                                <asp:Label ID="lbl1a" runat="server" Text="TC" Style="width: 22px; float: right;" CssClass="col-sm-2 control-label"></asp:Label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtvchnum" runat="server" Width="80px" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtvchdate" placeholder="Date" runat="server" Width="100px"></asp:TextBox>
                                </div>

                            </div>
                            <br />
                            <asp:FileUpload ID="FileUpload1" runat="server" Width="250px" onchange="submitFile()" />
                            <asp:Button ID="btnupload" runat="server" Text="Upload" class="myButton" Style="display: none" OnClick="btnupload_Click" />

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
                </div>


                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <label id="Label2" runat="server" class="col-sm-12 control-label" title="lbl1">1. Click on New Button, and click on choose file button to upload the file.</label>
                                <label id="Label3" runat="server" class="col-sm-12 control-label" title="lbl1">2. File must of (.xls) or (.csv) type, check and save.</label>
                            </div>
                            <div class="form-group" style="display: none">
                                <label id="Label9" runat="server" class="col-sm-3 control-label" title="lbl1">Customer</label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnAcode" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnAcode_Click" />
                                </div>
                                <div class="col-sm-2">
                                    <input id="txtacode" type="text" readonly="true" class="form-control" runat="server" placeholder="Code" maxlength="4" style="height: 28px;" />
                                </div>
                                <div class="col-sm-6">
                                    <input id="txtAname" type="text" readonly="true" class="form-control" runat="server" placeholder="Name" maxlength="4" style="height: 28px;" />
                                </div>
                            </div>
                            <div class="form-group" style="display: none">
                                <label id="Label1" runat="server" class="col-sm-3 control-label" title="lbl1">Ledger</label>
                                <div class="col-sm-1" id="div2" runat="server">
                                    <asp:ImageButton ID="btnRcode" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnRcode_Click" />
                                </div>
                                <div class="col-sm-2">
                                    <input id="txtRcode" type="text" readonly="true" class="form-control" runat="server" placeholder="Code" maxlength="4" style="height: 28px;" />
                                </div>
                                <div class="col-sm-6">
                                    <input id="Text2" type="text" readonly="true" class="form-control" runat="server" placeholder="Name" maxlength="4" style="height: 28px;" />
                                </div>
                            </div>
                            <div class="form-group" style="display: none">
                                <label id="Label6" runat="server" class="col-sm-3 control-label" title="lbl1">D/N C/N Reason</label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnDNCN" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnDNCN_Click" />
                                </div>
                                <div class="col-sm-2">
                                    <input id="txtDnCnCode" type="text" readonly="true" class="form-control" runat="server" placeholder="Code" maxlength="4" style="height: 28px;" />
                                </div>
                                <div class="col-sm-6">
                                    <input id="txtDnCnName" type="text" readonly="true" class="form-control" runat="server" placeholder="Name" maxlength="4" style="height: 28px;" />
                                </div>
                            </div>
                            <div class="form-group" style="display: none">
                                <label id="Label8" runat="server" class="col-sm-3 control-label" title="lbl1">GST Class</label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnGstClass" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnGstClass_Click" />
                                </div>
                                <div class="col-sm-2">
                                    <input id="txtGstClassCode" type="text" readonly="true" class="form-control" runat="server" placeholder="Code" maxlength="4" style="height: 28px;" />
                                </div>
                                <div class="col-sm-6">
                                    <input id="txtGstClassName" type="text" readonly="true" class="form-control" runat="server" placeholder="Name" maxlength="4" style="height: 28px;" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <section class="col-lg-12">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">
                                <li><a href="#DescTab" id="tab1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab">Form Details</a></li>
                                <li>
                                    <asp:Button ID="btnTempl" runat="server" OnClick="btnTempl_Click" CssClass="bg-green btn-foursquare" Style="margin-left: 30px;" Text="Download Template" />
                                </li>

                            </ul>
                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane active" id="DescTab">
                                    <div class="lbBody" style="box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <%--testing--%>
                                        <fin:CoolGridView ID="sg1" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="100%" Height="400px" Font-Size="13px"
                                            AutoGenerateColumns="true">
                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader2" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager2" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem2" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        </fin:CoolGridView>

                                        <div id="datadiv" style="overflow: scroll; display: none; width: auto; height: 400px;" runat="server" class="handsontable"
                                            data-originalstyle="width: auto; height: 400px; overflow: scroll">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>

                <div class="col-md-12" id="divRmk" runat="server">
                    <div>
                        <div class="box-body">
                            <asp:TextBox ID="txtrmk" runat="server" Width="99%" TextMode="MultiLine" onkeyup="max_length(this,200)" placeholder="Remarks"></asp:TextBox>
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
    <asp:HiddenField ID="TabName" runat="server" />
</asp:Content>
