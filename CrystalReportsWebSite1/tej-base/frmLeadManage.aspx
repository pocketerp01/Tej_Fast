<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="frmLeadManage" CodeFile="frmLeadManage.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper">
        <section class="content-header">
            <table style="width: 100%">
                <tr>
                    <td><asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label></td>
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
                                <label id="lbl1" runat="server" class="col-sm-2 control-label" title="lbl1">UserID</label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtvchnum" runat="server" ReadOnly="true" class="form-control" ></asp:TextBox>
                                </div>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtvchdate" placeholder="Date" runat="server" ReadOnly="true" class="form-control" ></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="lbl4" runat="server" class="col-sm-2 control-label" title="lbl4">Project</label>
                                <div class="col-sm-1">
                                    <asp:imagebutton id="btnProj" runat="server" imageurl="../tej-base/css/images/bdsearch5.png" style="width: 22px; float: right;" OnClick="btnProj_Click" />
                                </div>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtlbl4" runat="server" class="form-control" ></asp:TextBox>
                                </div>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtlbl4a" runat="server" class="form-control" ></asp:TextBox>
                                </div>
                                <asp:TextBox ID="txtProj" runat="server" style="display:none" class="form-control" ></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label id="lbl7" runat="server" class="col-sm-2 control-label" title="lbl7">Hrs</label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtlbl7" runat="server" class="form-control" ></asp:TextBox>
                                </div>
                                <label id="lbl7a" runat="server" class="col-sm-2 control-label" title="lbl7">Date</label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtlbl7a" runat="server" class="form-control" ></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label1" runat="server" class="col-sm-2 control-label" title="lbl7">Remarks</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtRMk" runat="server"   MaxLength="150" TextMode="MultiLine" class="form-control" ></asp:TextBox>
                                </div>
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
    <asp:HiddenField ID="TabName" runat="server" />
</asp:Content>
