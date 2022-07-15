<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="frmUmst8" CodeFile="frmUmst.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function validatePassword() {
            var p = document.getElementById('ContentPlaceHolder1_txtPwd').value,
                errors = [];
            if (p.length < 8) {
                errors.push("Your password must be at least 8 characters");
            }
            if (p.search(/[a-zA-Z]/) < 0) {
                errors.push("Your password must contain at least one letter.");
            }
            if (p.search(/\d/) < 0) {
                errors.push("Your password must contain at least one digit.");
            }
            if (p.search(/[@#$%^&+=]/) < 0) {
                errors.push("Your password must contain at least one Special Char.");
            }
            if (errors.length > 0) {
                alert(errors.join("\n"));
                document.getElementById('ContentPlaceHolder1_pwd1').value = "WRONG";
                document.getElementById('ContentPlaceHolder1_txtPwd').style.borderColor = "Red";
                return false;
            }
            document.getElementById('ContentPlaceHolder1_pwd1').value = "";
            document.getElementById('ContentPlaceHolder1_txtPwd').style.borderColor = "";
            return true;
        }
        function openfileDialog() {
            $("#Attch").click();
        }
        function submitFile() {
            $("#<%= btnAtt.ClientID%>").click();
        };
    </script>
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
                                <label id="lbl1" runat="server" class="col-sm-4 control-label" title="lbl1">Code</label>
                                <div class="col-sm-8">
                                    <input id="txtUserIDNo" type="text" style="height: 28px" class="form-control" runat="server" placeholder="UserId No." readonly="true" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="lbl2" runat="server" class="col-sm-4 control-label" title="lbl1">UserID</label>
                                <div class="col-sm-8">
                                    <input id="txtUserID" type="text" style="height: 28px;" class="form-control" runat="server" placeholder="User ID"  onblur="checkTextValIsValid(this)"/>
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="lbl3" runat="server" class="col-sm-4 control-label" title="lbl1">User Full Name</label>
                                <div class="col-sm-8">
                                    <input id="txtUserName" type="text" style="height: 28px;" class="form-control" runat="server" placeholder="User Full Name"  onblur="checkTextValIsValid(this)"/>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <label id="lbl4" runat="server" class="col-sm-3 control-label" title="lbl1">Department</label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnDept" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnDept_Click" />
                                </div>
                                <div class="col-sm-8">
                                    <input id="txtDeptCode" type="text" style="height: 28px; display: none;" runat="server" />
                                    <input id="txtDept" type="text" style="height: 28px;" class="form-control" runat="server" placeholder="Department"  onblur="checkTextValIsValid(this)"/>
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="lbl8" runat="server" visible="true" class="col-sm-3 control-label" title="lbl1">Multi Plant Access</label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnMplant" Visible="true" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnMplant_Click" />
                                </div>
                                <div class="col-sm-8">
                                    <input id="txtMultiPlant" type="text" style="height: 28px;" class="form-control" runat="server" placeholder="Multi Plant Access"  onblur="checkTextValIsValid(this)"/>
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="lbl6" runat="server" class="col-sm-4 control-label" title="lbl1">Rights/Role</label>
                                <div class="col-sm-8">
                                    <select id="dd2" runat="server" class="form-control">
                                        <option value="0">Owner/Mgmt</option>
                                        <option value="1">Administrator</option>
                                        <option value="2">Deptt Head</option>
                                        <option value="2.5">View Only</option>
                                        <option value="3">Operator</option>
                                        <option value="4">Secured User</option>
                                    </select>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div>
                        <div class="box-body">

                            <div class="form-group">
                                <label id="Label3" runat="server" class="col-sm-4 control-label" title="lbl1">Email ID</label>
                                <div class="col-sm-8">
                                    <input id="txtEmail" type="email" style="height: 28px;" class="form-control" runat="server" placeholder="Email ID"  onblur="checkTextValIsValid(this)"/>
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="lbl9" runat="server" class="col-sm-4 control-label" title="lbl1">Mobile</label>
                                <div class="col-sm-8">
                                    <input id="txtMobile" type="tel" style="height: 28px" class="form-control" runat="server" placeholder="Mobile No" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label12" runat="server" class="col-sm-3 control-label" title="lbl1">Deactivate_User</label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnDeact_Click" />
                                </div>
                                <div class="col-sm-8">
                                    <input id="txt_deac_by" type="text" style="height: 28px" class="form-control" runat="server" readonly="true" placeholder="Deactivate User" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label14" runat="server" class="col-sm-3 control-label" title="lbl1">WIP_Access</label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnWIP_Click" />
                                </div>
                                <div class="col-sm-8">
                                    <input id="txt_wip" type="text" style="height: 28px" class="form-control" runat="server" placeholder="WIP Access " />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label16" runat="server" class="col-sm-4 control-label" title="lbl1">Sessions</label>
                                <div class="col-sm-8">
                                    <input id="txt_sess" type="number" class="form-control" runat="server" placeholder="concurrent Sessions that user can open " maxlength="4" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label19" runat="server" class="col-sm-4 control-label" title="lbl1">Web_ERP</label>
                                <div class="col-sm-8">
                                    <input id="txt_grid" type="text" style="height: 28px" readonly="true" class="form-control" runat="server" placeholder="Web_ERP " />
                                </div>

                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <label id="Label1" runat="server" class="col-sm-4 control-label" title="lbl1">Password</label>
                                <div class="col-sm-8">
                                    <input id="txtPwd" type="text" style="height: 28px;" class="form-control" runat="server" placeholder="Password" onblur="validatePassword()" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label2" runat="server" class="col-sm-4 control-label" title="lbl1">Confirm Password</label>
                                <div class="col-sm-8">
                                    <input id="txtCpwd" type="text" style="height: 28px;" class="form-control" runat="server" placeholder="Confirm Password"  onblur="checkTextValIsValid(this)"/>
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label13" runat="server" class="col-sm-4 control-label" title="lbl1">Deactivation_Date</label>
                                <div class="col-sm-8">
                                    <input id="txt_deac_dt" type="text" style="height: 28px" class="form-control" runat="server" readonly="true" placeholder="Deactivation Date" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label15" runat="server" class="col-sm-3 control-label" title="lbl1">Factory Section</label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnfsec_Click" />
                                </div>
                                <div class="col-sm-8">
                                    <input id="txt_fsec" type="text" style="height: 28px" class="form-control" runat="server" placeholder="Factory Section " />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label17" runat="server" class="col-sm-4 control-label" title="lbl1">Password_Change_Dt</label>
                                <div class="col-sm-8">
                                    <input id="txt_pwdchg" type="text" style="height: 28px" readonly="true" class="form-control" runat="server" placeholder="Last Password Changed Date " />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label18" runat="server" class="col-sm-4 control-label" title="lbl1">Desktop Alerts</label>
                                <div class="col-sm-8">
                                    <input id="Txt_desk" type="text" style="height: 28px" readonly="true" class="form-control" runat="server" placeholder="Enable Desktop Alerts " />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-12" id="divCan" runat="server">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <label id="Label4" runat="server" readonly="true" class="col-sm-2 control-label" title="lbl1">User Can Add Data</label>
                                <div class="col-sm-1">
                                    <input id="txtCanAdd" type="text" style="height: 28px" readonly="true" class="form-control" runat="server" placeholder="Y/N" maxlength="1" />
                                </div>
                                <label id="Label5" runat="server" readonly="true" class="col-sm-2 control-label" title="lbl1">User Can Edit Data</label>
                                <div class="col-sm-1">
                                    <input id="txtCanEdit" type="text" style="height: 28px" readonly="true" class="form-control" runat="server" placeholder="Y/N" maxlength="1" />
                                </div>
                                <label id="Label6" runat="server" readonly="true" class="col-sm-2 control-label" title="lbl1">User Can Del Data</label>
                                <div class="col-sm-1">
                                    <input id="txtCanDel" type="text" style="height: 28px" readonly="true" class="form-control" runat="server" placeholder="Y/N" maxlength="1" />
                                </div>
                                <label id="Label7" runat="server" readonly="true" class="col-sm-2 control-label" title="lbl1">User Can View Prev Year</label>
                                <div class="col-sm-1">
                                    <input id="txtCanVW" type="text" style="height: 28px" readonly="true" class="form-control" runat="server" placeholder="Y/N" maxlength="1" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label8" runat="server" readonly="true" class="col-sm-2 control-label" title="lbl1">Enter Master File</label>
                                <div class="col-sm-1">
                                    <input id="txtMstFile" type="text" style="height: 28px" readonly="true" class="form-control" runat="server" placeholder="Y/N" maxlength="1" />
                                </div>
                                <label id="Label9" runat="server" readonly="true" class="col-sm-2 control-label" title="lbl1">Can Approve P.R</label>
                                <div class="col-sm-1">
                                    <input id="txtCanAppr" type="text" style="height: 28px" readonly="true" class="form-control" runat="server" placeholder="Y/N" maxlength="1" />
                                </div>
                                <label id="Label10" runat="server" readonly="true" class="col-sm-2 control-label" title="lbl1">User View Cons.Reps</label>
                                <div class="col-sm-1">
                                    <input id="txtCanViewCons" type="text" style="height: 28px" readonly="true" class="form-control" runat="server" placeholder="Y/N" maxlength="1" />
                                </div>
                                <label id="Label20" runat="server" readonly="true" class="col-sm-1 control-label" title="lbl1">Upload_Pic</label>
                                <div class="col-sm-2">
                                    <asp:FileUpload ID="Attch" runat="server" Visible="true" onchange="submitFile()" />
                                    <asp:Button ID="btnAtt" runat="server" Text="Attachment" OnClick="btnAtt_Click" Width="134px" Style="display: none" />

                                    <asp:Label ID="lblUpload" runat="server"></asp:Label>

                                    <asp:Button ID="btnView1" runat="server" CssClass="btn-success" Text="View" OnClick="btnView1_Click" Visible="false" />
                                    <asp:ImageButton ID="btnDwnld1" runat="server" ImageUrl="~/tej-base/images/Save.png" OnClick="btnDwnld1_Click" Visible="false" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-12" id="div1" runat="server">
                        <div>
                            <div class="box-body">
                                <div class="col-md-12">
                                    <label id="Label11" runat="server" style="background-color: aquamarine; font-size: medium; text-align: center; height: 40px" readonly="true" class="col-md-12 control-label" title="lbl1">Detailed User rights are to be provided through user rights form with module wise and form level wise new, edit & delete rights.Hence, the above mentioned options of can add, edit & delete are not to be used.</label>
                                </div>

                            </div>
                        </div>
                    </div>


                    <section class="col-lg-12 connectedSortable" style="display: none">
                        <div class="panel panel-default">
                            <div id="Tabs" role="tabpanel">
                                <ul class="nav nav-tabs" role="tablist">
                                    <li><a href="#DescTab" id="tab1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab">Form Details</a></li>
                                    <li><a href="#DescTab2" id="tab2" runat="server" aria-controls="DescTab2" role="tab" data-toggle="tab">Comm.Terms</a></li>
                                    <li><a href="#DescTab3" id="tab3" runat="server" aria-controls="DescTab3" role="tab" data-toggle="tab">Addl.Terms</a></li>
                                    <li><a href="#DescTab4" id="tab4" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Delv.Sch</a></li>
                                    <li><a href="#DescTab5" id="tab5" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Inv.Dtl</a></li>
                                </ul>

                                <div class="tab-content">
                                    <div role="tabpanel" class="tab-pane active" id="DescTab">
                                        <div class="lbBody" style="height: 200px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                            <div class="col-md-6">
                                                <div>
                                                    <div class="box-body">
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-6">
                                                <div>
                                                    <div class="box-body">
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                    <div role="tabpanel" class="tab-pane active" id="DescTab2">
                                        <div class="lbBody" style="height: 200px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                            <div class="lbBody" style="height: 200px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                                <div class="col-md-6">
                                                    <div>
                                                        <div class="box-body">
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="col-md-6">
                                                    <div>
                                                        <div class="box-body">
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                    <div role="tabpanel" class="tab-pane active" id="DescTab3">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </section>
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
    <input id="pwd1" runat="server" style="display: none" />
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
