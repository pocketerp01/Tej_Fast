<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="om_ac_club" Title="Tejaxo" CodeFile="om_ac_club.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="content-wrapper">
        <section class="content-header">
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large">Account Clubbing</asp:Label></td>
                    <td style="text-align: right">
                        <button type="submit" id="btn_new" class="btn btn-info" style="width: 80px;" runat="server" accesskey="C" onserverclick="btn_new_ServerClick" visible="false">New</button>
                        <button type="submit" id="btn_cancel" class="btn btn-info" style="width: 80px;" runat="server" accesskey="C" onserverclick="btn_cancel_ServerClick" visible="false">Cancel</button>
                        <button type="submit" id="btn_ext" class="btn btn-info" style="width: 80px;" runat="server" accesskey="E" onserverclick="btn_ext_Click"><u>E</u>xit</button>

                    </td>

                </tr>
            </table>
        </section>

        <section class="content">
            <div class="row">
                <section class="col-md-2"></section>

                <section class="col-md-8 connectedSortable">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">

                                <li><a href="#DescTab" id="A1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab">Account Group Change</a></li>
                                <li><a href="#DescTab2" id="A2" runat="server" aria-controls="DescTab2" role="tab" data-toggle="tab">Clubbing of accounts</a></li>
                                <li><a href="#DescTab3" id="A3" runat="server" aria-controls="DescTab2" role="tab" data-toggle="tab">Clubbing of accounts Manually</a></li>
                            </ul>
                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane active" id="DescTab">
                                    <div class="lbBody" id="gridDiv" runat="server" style="height: 300px; overflow: hidden; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <div class="col-md-12">
                                            <div>
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <label id="Label9" runat="server" class="col-sm-12 control-label" title="lbl1" style="text-align: center"><b>Transfer From One Group to Another</b></label>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label6" runat="server" class="col-sm-2 control-label" title="lbl1">A/C Name</label>
                                                        <div class="col-sm-1">
                                                            <asp:ImageButton ID="btnacode" runat="server" ImageUrl="~/tej-base/css/images/bdsearch5.png" OnClick="btnacode_Click" Style="width: 22px; float: right;" />
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtacode" ReadOnly="true" class="form-control" runat="server" placeholder="Code" MaxLength="4" Style="height: 28px;"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-6">
                                                            <asp:TextBox ID="txtaname" ReadOnly="true" class="form-control" runat="server" placeholder="Name" Style="height: 28px; width: 300px"></asp:TextBox>

                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label1" runat="server" class="col-sm-12 control-label" title="lbl1" style="text-align: center"><b>(Select the Name of the account for which you want to change the A/c Group or Schedule)</b></label>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label2" runat="server" class="col-sm-2 control-label" title="lbl1">A/c Group</label>
                                                        <div class="col-sm-1">
                                                            <asp:ImageButton ID="btnagrp" runat="server" ImageUrl="~/tej-base/css/images/bdsearch5.png" OnClick="btnagrp_Click" Style="width: 22px; float: right;" />
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtacgp" ReadOnly="true" class="form-control" runat="server" placeholder="Code" MaxLength="4" Style="height: 28px;"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-6">
                                                            <asp:TextBox ID="txtacname" ReadOnly="true" class="form-control" runat="server" placeholder="Name" Style="height: 28px; width: 300px"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label3" runat="server" class="col-sm-12 control-label" title="lbl1" style="text-align: center"><b>(Select the Destination Group to which you want to transfer the above account)</b></label>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label4" runat="server" class="col-sm-2 control-label" title="lbl1">Schedule Code</label>
                                                        <div class="col-sm-1">
                                                            <asp:ImageButton ID="btnscode" runat="server" ImageUrl="~/tej-base/css/images/bdsearch5.png" OnClick="btnscode_Click" Style="width: 22px; float: right;" />
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtscode" ReadOnly="true" class="form-control" runat="server" placeholder="Code" MaxLength="4" Style="height: 28px;"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-6">
                                                            <asp:TextBox ID="txtsname" ReadOnly="true" class="form-control" runat="server" placeholder="Name" Style="height: 28px; width: 300px"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="col-sm-4">
                                                    </div>
                                                    <div class="col-sm-2">
                                                        <br />
                                                        <button type="submit" id="btn_club" class="btn btn-info" style="width: 180px;" runat="server" accesskey="C" onserverclick="btn_club_Click">Change Account Group</button>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane" id="DescTab2">
                                    <div class="lbBody" id="gridDiv1" style="height: 300px; overflow: hidden; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <div class="col-md-12">
                                            <div>
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <label id="Label10" runat="server" class="col-sm-12 control-label" title="lbl1" style="text-align: center">
                                                            </label>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label5" runat="server" class="col-sm-3 control-label" title="lbl1">A/c Name(to be removed)</label>
                                                        <div class="col-sm-1">
                                                            <asp:ImageButton ID="btnacode1" runat="server" ImageUrl="~/tej-base/css/images/bdsearch5.png" OnClick="btnacode1_Click" Style="width: 22px; float: right;" />
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtacode1" type="text" ReadOnly="true" class="form-control" runat="server" placeholder="Code" MaxLength="4" Style="height: 28px;"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-5">
                                                            <asp:TextBox ID="txtaname1" type="text" ReadOnly="true" class="form-control" runat="server" placeholder="Name" MaxLength="4" Style="height: 28px; width: 300px"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label7" runat="server" class="col-sm-3 control-label" title="lbl1">A/c Name(to continue)</label>
                                                        <div class="col-sm-1">
                                                            <asp:ImageButton ID="btnacode2" runat="server" ImageUrl="~/tej-base/css/images/bdsearch5.png" OnClick="btnacode2_Click" Style="width: 22px; float: right;" />
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtacode2" type="text" ReadOnly="true" class="form-control" runat="server" placeholder="Code" MaxLength="4" Style="height: 28px;"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-5">
                                                            <asp:TextBox ID="txtaname2" type="text" ReadOnly="true" class="form-control" runat="server" placeholder="Name" MaxLength="4" Style="height: 28px; width: 300px"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label8" runat="server" class="col-sm-12 control-label" title="lbl1" style="text-align: center"></label>
                                                    </div>
                                                    <div class="form-group" style="display: none">
                                                        <label id="Label11" runat="server" class="col-sm-3 control-label" title="lbl1">Fixed A/c Code</label>
                                                        <div class="col-sm-1">
                                                            &nbsp;
                                                        </div>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtfixcode" type="text" class="form-control" runat="server" placeholder="Code" MaxLength="4" Style="height: 28px; width: 150px"></asp:TextBox>
                                                        </div>
                                                    </div>


                                                    <div class="form-group">
                                                        <div class="col-sm-6">
                                                            &nbsp;
                                                        </div>
                                                        <div class="col-sm-7">
                                                            <br />
                                                            <button type="submit" id="btn_similar" class="btn btn-info" style="width: 180px;" runat="server" accesskey="I" onserverclick="btn_similar_ServerClick">Club Above Accounts</button>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                  <div role="tabpanel" class="tab-pane" id="DescTab3">
                                    <div class="lbBody" id="gridDiv3" style="height: 300px; overflow: hidden; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <div class="col-md-12">
                                            <div>
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <label id="Label12" runat="server" class="col-sm-12 control-label" title="lbl1" style="text-align: center">
                                                            </label>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label13" runat="server" class="col-sm-3 control-label" title="lbl1">A/c Name(to be removed)</label>
                                                        <div class="col-sm-1">
                                                            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/tej-base/css/images/bdsearch5.png" OnClick="ImageButton1_Click" Style="width: 22px; float: right;" />
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="TextBox1" type="text" ReadOnly="true" class="form-control" runat="server" placeholder="Code" MaxLength="4" Style="height: 28px;"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-5">
                                                            <asp:TextBox ID="TextBox2" type="text" ReadOnly="true" class="form-control" runat="server" placeholder="Name" MaxLength="4" Style="height: 28px; width: 300px"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label14" runat="server" class="col-sm-4 control-label" title="lbl1">A/c Name(to continue)</label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="TextBox3" type="text" class="form-control" runat="server" placeholder="Code" MaxLength="8" Style="height: 28px;"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <div class="col-sm-6">
                                                            &nbsp;
                                                        </div>
                                                        <div class="col-sm-7">
                                                            <br />
                                                            <button type="submit" id="Button1" class="btn btn-info" style="width: 180px;" runat="server" accesskey="I" onserverclick="Button1_ServerClick">Club Above Accounts</button>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>
            </div>
        </section>

    </div>

    <asp:Button ID="btnhideF" runat="server" Text="!" OnClick="btnhideF_Click" Style="display: none;" />
    <asp:Button ID="btnhideF_s" runat="server" Text="!" OnClick="btnhideF_s_Click" Style="display: none;" />
    <asp:HiddenField ID="hf2" runat="server" />
    <asp:Button ID="btnOKTarget" runat="server" Text="!" OnClick="btnOKTarget_Click" Style="display: none;" />
    <asp:HiddenField ID="hfname" runat="server" />
    <asp:HiddenField ID="hfmode" runat="server" />


    <script type="text/javascript">
        $(function () {
            var tabName = $("[id*=TabName]").val() != "" ? $("[id*=TabName]").val() : "DescTab2";
            $('#Tabs a[href="#' + tabName + '"]').tab('show');
            $("#Tabs a").click(function () {
                $("[id*=TabName]").val($(this).attr("href").replace("#", ""));
            });
        });
    </script>
    <asp:HiddenField ID="TabName" runat="server" />
</asp:Content>
