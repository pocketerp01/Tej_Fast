<%@ Page Title="" Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="om_emp_mas67" CodeFile="om_emp_mas.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../forms/tej-base/Scripts/gridviewScroll.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            // gridviewScroll('#<%=sg1.ClientID%>', gridDiv, 1, 5);
            //calculateSum();
        });
        function gridviewScroll(gridId, gridDiv, headerFreeze, rowFreeze) {
            $(gridId).gridviewScroll({
                width: gridDiv.offsetWidth,
                height: gridDiv.offsetHeight,
                headerrowcount: headerFreeze,
                freezesize: rowFreeze,
                barhovercolor: "#3399FF",
                barcolor: "#3399FF",
                startVertical: $("#<%=hfGridView1SV.ClientID%>").val(),
                startHorizontal: $("#<%=hfGridView1SH.ClientID%>").val(),
                onScrollVertical: function (delta) {
                    $("#<%=hfGridView1SV.ClientID%>").val(delta);
                },
                onScrollHorizontal: function (delta) {
                    $("#<%=hfGridView1SH.ClientID%>").val(delta);
                }
            });
            }

            function fill_zero(val) { if (isNaN(val)) return 0; if (isFinite(val)) return val; }
    </script>
    <script type="text/javascript">
        function openfileDialog() {
            $("#Attch").click();
        }
        function submitFile() {
            $("#<%= btnAtt.ClientID%>").click();
        };
        function isNumber(evt) {
            var theEvent = evt || window.event;
            var key = theEvent.keyCode || theEvent.which;
            var keyCode = key;
            key = String.fromCharCode(key);
            if (key.length == 0) return;
            var regex = /^[0-9.,\b]+$/;
            if (keyCode == 188 || keyCode == 190) {
                return;
            } else {
                if (!regex.test(key)) {
                    theEvent.returnValue = false;
                    if (theEvent.preventDefault) theEvent.preventDefault();
                }
            }
        }
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
                                <label id="Label19" runat="server" class="col-sm-4 control-label">Emp.Code</label>

                                <div class="col-sm-8">
                                    <input id="txt_empcode" type="text" class="form-control" runat="server" maxlength="10" readonly="true" />
                                    <asp:Label ID="lbl1a" runat="server" Text="TC" Style="width: 22px; float: right;" CssClass="col-sm-2 control-label" Visible="false"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label11" runat="server" class="col-sm-4 control-label">Employee Name</label>

                                <div class="col-sm-8">
                                    <input id="txt_empname" type="text" class="form-control" runat="server" maxlength="50" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label2" runat="server" class="col-sm-4 control-label">Father/Husband Name</label>

                                <div class="col-sm-8">
                                    <input id="txt_fhname" type="text" class="form-control" runat="server" maxlength="50" />
                                </div>
                            </div>
                            <div class="form-group" id="HGLO1" runat="server">
                                <label id="lblMother" runat="server" class="col-sm-4 control-label">Mother's Name</label>

                                <div class="col-sm-8">
                                    <input id="txt_Mname" type="text" class="form-control" runat="server" maxlength="50" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">

                            <div class="form-group">
                                <label id="Labelcat" runat="server" class="col-sm-3 control-label">Category</label>
                                <div class="col-sm-3">
                                    <input id="txt_Category" type="text" class="form-control" readonly="readonly" runat="server" maxlength="2" />
                                </div>
                                <div class="col-sm-6">
                                    <input id="txt_CatgName" type="text" class="form-control" readonly="readonly" runat="server" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label13" runat="server" class="col-sm-2 control-label">Designation</label>
                                <div class="col-sm-1" id="div1" runat="server">
                                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btn_sch_Click" />
                                </div>
                                <div class="col-sm-3">
                                    <input id="Txt_descode" type="text" class="form-control" readonly="readonly" runat="server" maxlength="10" />
                                </div>
 
                                <div class="col-sm-6">
                                    <input id="txt_designation" type="text" class="form-control" readonly="readonly" runat="server" maxlength="50" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label17" runat="server" class="col-sm-2 control-label">Department</label>
                                <div class="col-sm-1" id="div20" runat="server">
                                    <asp:ImageButton ID="ImageButton19" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btn_acn_Click" />
                                </div>
                                <div class="col-sm-3">
                                    <input id="txtdepcode" type="text" class="form-control" readonly="readonly" runat="server" maxlength="10" />
                                </div>
                                <div class="col-sm-6">
                                    <input id="txt_departname" type="text" class="form-control" readonly="readonly" runat="server" maxlength="50" />
                                </div>
                            </div>

                            <div class="form-group" runat="server" id="HGLO2">
                                <label id="lblEmergency" runat="server" class="col-sm-2 control-label">Emergency_Contact_No.</label>
                                <div class="col-sm-1" runat="server">
                                </div>
                                <div class="col-sm-3">
                                    <input id="txt_Emergency" type="text" class="form-control" runat="server" maxlength="10" onkeypress="return isDecimalKey(event)" />
                                </div>
                                <label id="lblFixedAmt" runat="server" class="col-sm-2 control-label">Fixed_Amt.</label>
                                <div class="col-sm-4">
                                    <input id="txt_FixedAmt" type="text" class="form-control" runat="server" maxlength="10" onkeypress="return isDecimalKey(event)" />
                                </div>
                            </div>

                        </div>
                    </div>
                </div>

                <section class="col-lg-12 connectedSortable" id="AllTabs" runat="server">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">
                                <li><a href="#DescTab1" id="tab1" runat="server" aria-controls="DescTab1" role="tab" data-toggle="tab">Employee Details</a></li>
                                <li><a href="#DescTab8" id="tab8" runat="server" aria-controls="DescTab8" role="tab" data-toggle="tab">Address Details</a></li>
                                <li><a href="#DescTab2" id="tab2" runat="server" aria-controls="DescTab2" role="tab" data-toggle="tab">PF,ESI Details</a></li>
                                <!--   <li><a href="#DescTab3" id="tab3" runat="server" aria-controls="DescTab3" role="tab" data-toggle="tab">General Details</a></li>-->
                                <li><a href="#DescTab4" id="tab4" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Leave/Salary Details</a></li>
                                <li><a href="#DescTab6" id="tab6" runat="server" aria-controls="DescTab6" role="tab" data-toggle="tab">Earning Details</a></li>
                                <li><a href="#DescTab5" id="tab5" runat="server" aria-controls="DescTab5" role="tab" data-toggle="tab">Deductions Details</a></li>
                                <li><a href="#DescTab7" id="tab7" runat="server" aria-controls="DescTab5" role="tab" data-toggle="tab">Document Details</a></li>
                            </ul>

                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane active" id="DescTab1">
                                    <div class="lbBody" style="height: 310px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <label id="Label3" runat="server" class="col-sm-3 control-label">Official Number</label>
                                                        <div class="col-sm-9">
                                                            <input id="txt_offnumber" type="text" class="form-control" runat="server" maxlength="50" onkeypress="return isNumber(event)" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label74" runat="server" class="col-sm-3 control-label">Gender</label>
                                                        <div class="col-sm-5">
                                                            <%-- <input type="radio" id="rbl_gender" name="gender" value="male" checked="checked" onclick="sel('rbl_gender')" />
                                                            Male
                                                          <input type="radio" id="rbl_gender2" name="gender" value="female" onclick="sel('rbl_gender2')" />
                                                            Female--%>
                                                            <asp:RadioButtonList ID="rdbGender" runat="server" onclick="return GetSelectedItem()" RepeatDirection="Horizontal" Font-Size="Small">
                                                                <asp:ListItem Value="M" Text="Male" Selected="True"> </asp:ListItem>
                                                                <asp:ListItem Value="F" Text="Female"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </div>

                                                        <div class="col-sm-4">
                                                            <input id="txt_gender" name="txt_gender1" type="text" class="form-control" runat="server" maxlength="1" readonly="true" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label75" runat="server" class="col-sm-3 control-label">Date Of Birth</label>
                                                        <div class="col-sm-9">
                                                            <asp:TextBox ID="txt_dob" runat="server" CssClass="form-control" MaxLength="10" Width="100%"></asp:TextBox>
                                                            <asp:CalendarExtender ID="CalendarExtender1" runat="server"
                                                                Enabled="True" TargetControlID="txt_dob"
                                                                Format="dd/MM/yyyy">
                                                            </asp:CalendarExtender>
                                                            <asp:MaskedEditExtender ID="MaskedEditExtender1" runat="server" Mask="99/99/9999"
                                                                MaskType="Date" TargetControlID="txt_dob" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label76" runat="server" class="col-sm-3 control-label">Mobile</label>
                                                        <div class="col-sm-9">
                                                            <input id="txt_mobile" type="text" class="form-control" runat="server" maxlength="30" onkeypress="return isDecimalKey(event)" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label77" runat="server" class="col-sm-3 control-label">Status</label>
                                                        <div class="col-sm-3">
                                                            <input id="txt_status" type="text" class="form-control" runat="server" maxlength="30" readonly="true" />
                                                        </div>
                                                        <label id="Label18" runat="server" class="col-sm-2 control-label">ApprovedBy</label>
                                                        <div class="col-sm-4">
                                                            <input id="txt_App" type="text" class="form-control" runat="server" readonly="true" />
                                                            <input id="txt_Appdt" type="text" class="form-control" runat="server" readonly="true" visible="false" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label78" runat="server" class="col-sm-3 control-label">Working Hour</label>
                                                        <div class="col-sm-9">
                                                            <input id="txt_wrkinghor" type="text" class="form-control" runat="server" maxlength="3" onkeypress="return isDecimalKey(event)" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label36" runat="server" class="col-sm-3 control-label">Pan No.</label>
                                                        <div class="col-sm-9">
                                                            <input id="txt_panno" type="text" class="form-control" runat="server" maxlength="10" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group" id="HGLO3" runat="server">
                                                        <label id="lblList" runat="server" class="col-sm-3 control-label">List</label>
                                                        <div class="col-sm-9">
                                                            <input id="txt_List" type="text" class="form-control" runat="server" maxlength="20" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group" runat="server">
                                                        <label id="lblBranch" runat="server" class="col-sm-2 control-label">Branch</label>
                                                        <div class="col-sm-1">
                                                            <asp:ImageButton ID="imgBranch" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="imgBranch_Click" />
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <input id="txt_Branch" type="text" class="form-control" runat="server" maxlength="50" />
                                                        </div>
                                                        <label id="OTAfter" runat="server" class="col-sm-2 control-label">OT After</label>
                                                        <div class="col-sm-4">
                                                            <input id="txt_OTAfter" type="text" class="form-control" runat="server" maxlength="8" onkeypress="return isDecimalKey(event)" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">

                                                    <div class="form-group">
                                                        <label id="Label79" runat="server" class="col-sm-2 control-label">Section</label>
                                                        <div class="col-sm-1" id="div21" runat="server">
                                                            <asp:ImageButton ID="ImageButton20" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btn_mgr_Click" />
                                                        </div>
                                                        <div class="col-sm-9">
                                                            <input id="txt_section" type="text" class="form-control" runat="server" maxlength="50" readonly="true" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label80" runat="server" class="col-sm-2 control-label">Reports To</label>
                                                        <div class="col-sm-1" id="div22" runat="server">
                                                            <asp:ImageButton ID="ImageButton21" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btn_dist_Click" />
                                                        </div>

                                                        <div class="col-sm-3">
                                                            <input id="txtrepcode" type="text" class="form-control" readonly="readonly" runat="server" maxlength="10" />
                                                        </div>
                                                        <div class="col-sm-6">
                                                            <input id="txt_reportsto" type="text" readonly="readonly" class="form-control" runat="server" maxlength="60" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label89" runat="server" class="col-sm-3 control-label">Blood Group</label>
                                                        <div class="col-sm-9">
                                                            <input id="txt_bloodgrp" type="text" class="form-control" runat="server" maxlength="3" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label90" runat="server" class="col-sm-3 control-label">Adhar No.</label>
                                                        <div class="col-sm-9">
                                                            <input id="txt_adharno" type="text" class="form-control" runat="server" maxlength="12" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label91" runat="server" class="col-sm-3 control-label">Email Id</label>
                                                        <div class="col-sm-9">
                                                            <input id="txt_emailid" type="text" class="form-control" runat="server" maxlength="50" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label5" runat="server" class="col-sm-3 control-label">Last Increment Date</label>
                                                        <div class="col-sm-9">
                                                            <input id="txt_lastincrementdt" type="text" class="form-control" runat="server" maxlength="10" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label28" runat="server" class="col-sm-3 control-label">Confirmation Date</label>
                                                        <div class="col-sm-9">
                                                            <input id="txt_confir" type="text" class="form-control" runat="server" maxlength="10" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label24" runat="server" class="col-sm-3 control-label">Joining Date</label>
                                                        <div class="col-sm-9">
                                                            <asp:TextBox ID="txt_joining" runat="server" CssClass="form-control" MaxLength="10" Width="100%"></asp:TextBox>
                                                            <asp:CalendarExtender ID="txt_joining_CalendarExtender" runat="server"
                                                                Enabled="True" TargetControlID="txt_joining"
                                                                Format="dd/MM/yyyy">
                                                            </asp:CalendarExtender>
                                                            <asp:MaskedEditExtender ID="Maskedit1" runat="server" Mask="99/99/9999"
                                                                MaskType="Date" TargetControlID="txt_joining" />
                                                        </div>
                                                    </div>                                                    

                                                    <div class="form-group" id="HGLO5" runat="server">
                                                        <label id="lblPlant" runat="server" class="col-sm-3 control-label">Plant</label>
                                                        <div class="col-sm-9">
                                                            <input id="txt_Plant" type="text" class="form-control" runat="server" maxlength="50" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group" id="HGLO6" runat="server">
                                                        <label id="Label23" runat="server" class="col-sm-3 control-label">2.5 Days(Y/N)</label>
                                                        <div class="col-sm-3">
                                                            <input id="txt_Days" type="text" class="form-control" runat="server" maxlength="1" />
                                                        </div>
                                                        <label id="Label22" runat="server" class="col-sm-3 control-label">Bonus(Y/N)</label>
                                                        <div class="col-sm-3">
                                                            <input id="txt_Bonus" type="text" class="form-control" runat="server" maxlength="1" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane active" id="DescTab8">
                                    <div class="lbBody" style="height: 310px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <label id="Label64" runat="server" class="col-sm-4 control-label">Address 1</label>

                                                        <div class="col-sm-8">
                                                            <input id="txt_add1" type="text" class="form-control" runat="server" maxlength="30" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label65" runat="server" class="col-sm-4 control-label">Address 2</label>

                                                        <div class="col-sm-8">
                                                            <input id="txt_add2" type="text" class="form-control" runat="server" maxlength="30" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label66" runat="server" class="col-sm-4 control-label">City</label>

                                                        <div class="col-sm-8">
                                                            <input id="txt_city" type="text" class="form-control" runat="server" maxlength="30" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label67" runat="server" class="col-sm-3 control-label">State</label>
                                                        <div class="col-sm-1" id="div13" runat="server">
                                                            <asp:ImageButton ID="ImageButton12" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btn_stat_Click" />
                                                        </div>
                                                        <div class="col-sm-8">
                                                            <input id="txt_state" type="text" class="form-control" runat="server" readonly="readonly" maxlength="30" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label68" runat="server" class="col-sm-3 control-label">Country</label>
                                                        <div class="col-sm-1" id="div14" runat="server">
                                                            <asp:ImageButton ID="ImageButton13" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btn_ctry_Click" />
                                                        </div>
                                                        <div class="col-sm-8">
                                                            <input id="txt_country" type="text" class="form-control" runat="server" readonly="readonly" maxlength="30" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label82" runat="server" class="col-sm-4 control-label">Tele</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_tele" type="text" class="form-control" runat="server" maxlength="40" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label83" runat="server" class="col-sm-4 control-label">PinCode</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_pincode" type="text" class="form-control" runat="server" maxlength="20" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Labelchk1" runat="server" class="col-sm-3 control-label">Same Permanent Add</label>
                                                        <div class="col-sm-2" runat="server">
                                                            <asp:ImageButton ID="btnPermanentsame" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnPermanentsame_Click" />
                                                        </div>
                                                        <label id="Label4" runat="server" class="col-sm-3 control-label">Same Present Add</label>
                                                        <div class="col-sm-2" runat="server">
                                                            <asp:ImageButton ID="btnPresentsame" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnPresentsame_Click" />
                                                        </div>
                                                    </div>


                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <label id="Label69" runat="server" class="col-sm-4 control-label">Address (Line 1)</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_add3" type="text" class="form-control" runat="server" maxlength="50" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label70" runat="server" class="col-sm-4 control-label">Address (Line 2)</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_add4" type="text" class="form-control" runat="server" maxlength="50" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label1" runat="server" class="col-sm-4 control-label">City</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_city1" type="text" class="form-control" runat="server" maxlength="50" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label71" runat="server" class="col-sm-3 control-label">State</label>
                                                        <div class="col-sm-1" id="div3" runat="server">
                                                            <asp:ImageButton ID="btnState1" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnState1_Click" />
                                                        </div>
                                                        <div class="col-sm-8">
                                                            <input id="txt_state1" type="text" class="form-control" runat="server" maxlength="50" readonly="true" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label72" runat="server" class="col-sm-3 control-label">Country</label>
                                                        <div class="col-sm-1" id="div4" runat="server">
                                                            <asp:ImageButton ID="btnCountry1" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnCountry1_Click" />
                                                        </div>
                                                        <div class="col-sm-8">
                                                            <input id="txt_country1" type="text" class="form-control" runat="server" maxlength="40" readonly="true" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label73" runat="server" class="col-sm-4 control-label">Tele</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_tele1" type="text" class="form-control" runat="server" maxlength="40" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label81" runat="server" class="col-sm-4 control-label">Pin Code</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_pincode1" type="text" class="form-control" runat="server" maxlength="20" />
                                                        </div>
                                                    </div>


                                                    <div class="form-group">
                                                        <label id="Label88" runat="server" class="col-sm-3 control-label">Mode of Payment</label>

                                                        <div class="col-sm-1" id="div2" runat="server">
                                                            <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btn_mode_Click" />
                                                        </div>
                                                        <div class="col-sm-8">
                                                            <input id="txt_mop" type="text" class="form-control" readonly="readonly" runat="server" maxlength="1" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane active" id="DescTab2">
                                    <div class="lbBody" style="height: 310px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">

                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">



                                                    <div class="form-group">
                                                        <label id="Label26" runat="server" class="col-sm-4 control-label">Probation Mths</label>

                                                        <div class="col-sm-8">
                                                            <input id="txt_probationmths" type="text" class="form-control" runat="server" maxlength="2" onkeypress="return isDecimalKey(event)" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Labelbnk" runat="server" class="col-sm-4 control-label">Bank </label>

                                                        <div class="col-sm-8">
                                                            <input id="txt_bankname" type="text" class="form-control" runat="server" maxlength="15" />
                                                        </div>
                                                    </div>


                                                    <div class="form-group">
                                                        <label id="Label20" runat="server" class="col-sm-4 control-label">Bankers A/cNo</label>

                                                        <div class="col-sm-8">
                                                            <input id="txt_bankers" type="text" class="form-control" runat="server" maxlength="15" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label8" runat="server" class="col-sm-4 control-label">Cut PF (Y/N)</label>
                                                        <div class="col-sm-3">
                                                            <input id="txt_cutpf" type="text" class="form-control" runat="server" maxlength="1" />
                                                        </div>
                                                        <label id="Label12" runat="server" class="col-sm-3 control-label">Apply_15000_PF_Limit</label>
                                                        <div class="col-sm-2">
                                                            <input id="txt_PFLimit" type="text" class="form-control" runat="server" maxlength="1" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label14" runat="server" class="col-sm-4 control-label">PF No.</label>

                                                        <div class="col-sm-8">
                                                            <input id="txt_pfno" type="text" class="form-control" runat="server" maxlength="20" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label85" runat="server" class="col-sm-4 control-label">Leaving Date</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_leavingdate" type="text" class="form-control" runat="server" maxlength="10" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label86" runat="server" class="col-sm-4 control-label">Card No.</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_cardno" type="text" class="form-control" runat="server" maxlength="10" />
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">

                                                    <div class="form-group">
                                                        <label id="Label31" runat="server" class="col-sm-3 control-label">Cut VPF (Y/N)</label>
                                                        <div class="col-sm-3">
                                                            <input id="txt_cutvpf" type="text" class="form-control" runat="server" maxlength="1" />
                                                        </div>
                                                        <label id="Label10" runat="server" class="col-sm-3 control-label">Cut WF (Y/N)</label>
                                                        <div class="col-sm-3">
                                                            <input id="txt_cutwf" type="text" class="form-control" runat="server" maxlength="1" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label32" runat="server" class="col-sm-3 control-label">Cut ESI (Y/N)</label>
                                                        <div class="col-sm-9">
                                                            <input id="txt_cutesi" type="text" class="form-control" runat="server" maxlength="1" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label33" runat="server" class="col-sm-3 control-label">ESI NO. </label>
                                                        <div class="col-sm-9">
                                                            <input id="txt_esi" type="text" class="form-control" runat="server" maxlength="20" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label7" runat="server" class="col-sm-2 control-label">ESI Disp</label>
                                                        <div class="col-sm-1" id="div23" runat="server">
                                                            <asp:ImageButton ID="ImageButton22" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btn_conti_Click" />
                                                        </div>

                                                        <div class="col-sm-9">
                                                            <input id="txt_esidisp" type="text" class="form-control" runat="server" readonly="readonly" maxlength="1" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label15" runat="server" class="col-sm-2 control-label">Scale</label>
                                                        <div class="col-sm-1" id="div24" runat="server">
                                                            <asp:ImageButton ID="ImageButton23" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btn_zone_Click" />
                                                        </div>
                                                        <div class="col-sm-9">
                                                            <input id="txt_scale" type="text" class="form-control" runat="server" maxlength="30" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label84" runat="server" class="col-sm-3 control-label">Reason</label>
                                                        <div class="col-sm-9">
                                                            <input id="txt_reason" type="text" class="form-control" runat="server" maxlength="15" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label87" runat="server" class="col-sm-3 control-label">Old Code</label>
                                                        <div class="col-sm-9">
                                                            <input id="txt_oldcode" type="text" class="form-control" runat="server" maxlength="10" />
                                                        </div>
                                                    </div>


                                                    <div class="form-group">
                                                        <label id="Label16" runat="server" class="col-sm-2 control-label">M/C</label>
                                                        <div class="col-sm-1" id="div26" runat="server">
                                                            <asp:ImageButton ID="ImageButton25" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btn_m_c_Click" />
                                                        </div>

                                                        <div class="col-sm-9">
                                                            <input id="txt_m_c" type="text" class="form-control" runat="server" maxlength="30" />
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane active" id="DescTab3">
                                    <div class="lbBody" style="height: 310px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">

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

                                <div role="tabpanel" class="tab-pane active" id="DescTab4">
                                    <div class="lbBody" style="height: 310px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">

                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">

                                                    <div class="form-group">
                                                        <label id="Label54" runat="server" class="col-sm-4 control-label">Earned Leaves</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_earned" type="text" class="form-control" runat="server" maxlength="3" onkeypress="return isDecimalKey(event)" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label55" runat="server" class="col-sm-4 control-label">Casual Leaves</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_casual" type="text" class="form-control" runat="server" maxlength="3" onkeypress="return isDecimalKey(event)" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label56" runat="server" class="col-sm-4 control-label">Sick Leaves</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_sick" type="text" class="form-control" runat="server" maxlength="3" onkeypress="return isDecimalKey(event)" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label57" runat="server" class="col-sm-4 control-label">C/off Op.Bal</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_coff" type="text" class="form-control" runat="server" maxlength="8" onkeypress="return isDecimalKey(event)" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label58" runat="server" class="col-sm-4 control-label">Maternity Leave Days</label>
                                                        <div class="col-sm-2">
                                                            <input id="txt_maternal" type="text" class="form-control" runat="server" maxlength="3" width="100%" />
                                                        </div>
                                                        <label id="Label9" runat="server" class="col-sm-4 control-label">Relation (For WF Report)</label>
                                                        <div class="col-sm-2">
                                                            <input id="txt_Relation" type="text" class="form-control" runat="server" maxlength="20" width="100%" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label30" runat="server" class="col-sm-3 control-label">Emp Type</label>
                                                        <div class="col-sm-1" id="div27" runat="server">
                                                            <asp:ImageButton ID="ImageButton26" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btn_emptype_Click" />
                                                        </div>
                                                        <div class="col-sm-8">
                                                            <input id="txt_emptype" type="text" readonly="readonly" class="form-control" runat="server" maxlength="30" />
                                                        </div>
                                                    </div>


                                                    <div class="form-group">
                                                        <label id="Label6" runat="server" class="col-sm-4 control-label">Experience</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_exp" type="text" class="form-control" runat="server" maxlength="5" onkeypress="return isDecimalKey(event)" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label29" runat="server" class="col-sm-3 control-label">Marriage Date</label>

                                                        <div class="col-sm-1">
                                                            <%--<input id="chk" type="checkbox" runat="server" onclick="document.getElementById('txt_marriagedt').enabled = this.checked;" />--%>
                                                            <asp:CheckBox ID="chk" runat="server" OnCheckedChanged="chk_CheckedChanged" AutoPostBack="true" />
                                                        </div>
                                                        <div class="col-sm-8">
                                                            <%--<input id="txt_marriagedt" type="text" class="form-control"  runat="server" placeholder="Marriage Date" maxlength="10" readonly="true"/>--%>
                                                            <asp:TextBox ID="txt_marriagedt" CssClass="form-control" runat="server" MaxLength="10" ReadOnly="true"></asp:TextBox>
                                                            <%-- <asp:CalendarExtender ID="CalendarExtender2" runat="server"
                                                Enabled="True" TargetControlID="txt_marriagedt"
                                                Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="MaskedEditExtender2" runat="server" Mask="99/99/9999"
                                                MaskType="Date" TargetControlID="txt_marriagedt" />--%>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Labelerp" runat="server" class="col-sm-4 control-label">ERP Code</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_erp" type="text" class="form-control" runat="server" maxlength="10" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">


                                                    <div class="form-group">
                                                        <label id="Label59" runat="server" class="col-sm-3 control-label">Accounts Category</label>
                                                        <div class="col-sm-1" id="div28" runat="server">
                                                            <asp:ImageButton ID="ImageButton27" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btn_accounts_Click" />
                                                        </div>
                                                        <div class="col-sm-8">
                                                            <input id="txt_accounts" type="text" readonly="readonly" class="form-control" runat="server" maxlength="30" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label_1" runat="server" class="col-sm-3 control-label">Accounts Code(ERP)</label>
                                                        <div class="col-sm-1" id="div_2" runat="server">
                                                            <asp:ImageButton ID="ImageButton_2" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btn_accounts_code_Click" />
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <input id="txt_accountscode" type="text" readonly="readonly" class="form-control" runat="server" maxlength="30" />
                                                        </div>
                                                        <div class="col-sm-6">
                                                            <input id="txt_accounterp" type="text" readonly="readonly" class="form-control" runat="server" maxlength="30" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label60" runat="server" class="col-sm-4 control-label">IFSC Code</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_ifsccode" type="text" class="form-control" runat="server" maxlength="20" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label61" runat="server" class="col-sm-4 control-label">Joining Salary</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_joiningsal" type="text" class="form-control" runat="server" maxlength="8" onkeypress="return isDecimalKey(event)" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label62" runat="server" class="col-sm-4 control-label">UAN No.</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_uanno" type="text" class="form-control" runat="server" maxlength="12" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="Label63" runat="server" class="col-sm-2 control-label">Curr CTC</label>
                                                        <div class="col-sm-2" runat="server">
                                                            <asp:Button ID="btnCTC" runat="server" Text="Calc CTC" OnClick="btnCTC_Click" />
                                                        </div>
                                                        <div class="col-sm-8">
                                                            <input id="txt_currctc" type="text" class="form-control" runat="server" readonly="true" maxlength="30" onkeypress="return isDecimalKey(event)" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label34" runat="server" class="col-sm-4 control-label">Employee Qualification</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_quali" type="text" class="form-control" runat="server" maxlength="50" />
                                                        </div>
                                                    </div>


                                                    <div class="form-group">
                                                        <label id="Label35" runat="server" class="col-sm-3 control-label">Skills</label>
                                                        <div class="col-sm-1" id="div25" runat="server">
                                                            <asp:ImageButton ID="ImageButton24" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btn_skill_Click" />
                                                        </div>
                                                        <div class="col-sm-8">
                                                            <input id="txt_skills" type="text" class="form-control" runat="server" readonly="readonly" maxlength="10" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Labelcgild" runat="server" class="col-sm-4 control-label">Children If any</label>
                                                        <div class="col-sm-8">
                                                            <input id="txt_child" type="text" class="form-control" runat="server" maxlength="8" onkeypress="return isDecimalKey(event)" />
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane active" id="DescTab5">
                                    <div class="lbBody" style="height: 310px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <fin:CoolGridView ID="sg3" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="100%" Font-Size="13px"
                                            AutoGenerateColumns="False" OnRowDataBound="sg3_RowDataBound"
                                            OnRowCommand="sg3_RowCommand">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t1" runat="server" Text='<%#Eval("sg3_t1") %>' Width="100%" MaxLength="6" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t2" runat="server" Text='<%#Eval("sg3_t2") %>' Width="100%" MaxLength="6" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t3" runat="server" Text='<%#Eval("sg3_t3") %>' Width="100%" MaxLength="6" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t4" runat="server" Text='<%#Eval("sg3_t4") %>' Width="100%" MaxLength="6" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t5" runat="server" Text='<%#Eval("sg3_t5") %>' Width="100%" MaxLength="6" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t6" runat="server" Text='<%#Eval("sg3_t6") %>' Width="100%" MaxLength="6" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t7" runat="server" Text='<%#Eval("sg3_t7") %>' Width="100%" MaxLength="6" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t8" runat="server" Text='<%#Eval("sg3_t8") %>' Width="100%" MaxLength="6" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate></HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t9" runat="server" Text='<%#Eval("sg3_t9") %>' Width="100%" MaxLength="6" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t10" runat="server" Text='<%#Eval("sg3_t10") %>' Width="100%" MaxLength="6" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t11" runat="server" Text='<%#Eval("sg3_t11") %>' Width="100%" MaxLength="6" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate></HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t12" runat="server" Text='<%#Eval("sg3_t12") %>' Width="100%" MaxLength="6" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t13" runat="server" Text='<%#Eval("sg3_t13") %>' Width="100%" MaxLength="6" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t14" runat="server" Text='<%#Eval("sg3_t14") %>' Width="100%" MaxLength="6" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t15" runat="server" Text='<%#Eval("sg3_t15") %>' Width="100%" MaxLength="6" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t16" runat="server" Text='<%#Eval("sg3_t16") %>' Width="100%" MaxLength="6" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t17" runat="server" Text='<%#Eval("sg3_t17") %>' Width="100%" MaxLength="6" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t18" runat="server" Text='<%#Eval("sg3_t18") %>' Width="100%" MaxLength="6" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t19" runat="server" Text='<%#Eval("sg3_t19") %>' Width="100%" MaxLength="6" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t20" runat="server" Text='<%#Eval("sg3_t20") %>' Width="100%" MaxLength="6" onkeypress="return isDecimalKey(event)"></asp:TextBox>
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

                                <div role="tabpanel" class="tab-pane active" id="DescTab6">
                                    <div class="lbBody" style="height: 310px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <fin:CoolGridView ID="sg4" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="100%" Font-Size="13px"
                                            AutoGenerateColumns="False" OnRowDataBound="sg4_RowDataBound"
                                            OnRowCommand="sg4_RowCommand">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg4_t1" runat="server" Text='<%#Eval("sg4_t1") %>' Width="100%" MaxLength="6" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>

                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg4_t2" runat="server" Text='<%#Eval("sg4_t2") %>' Width="100%" MaxLength="6" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg4_t3" runat="server" Text='<%#Eval("sg4_t3") %>' Width="100%" MaxLength="6" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg4_t4" runat="server" Text='<%#Eval("sg4_t4") %>' Width="100%" MaxLength="6" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg4_t5" runat="server" Text='<%#Eval("sg4_t5") %>' Width="100%" MaxLength="6" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg4_t6" runat="server" Text='<%#Eval("sg4_t6") %>' Width="100%" MaxLength="6" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg4_t7" runat="server" Text='<%#Eval("sg4_t7") %>' Width="100%" MaxLength="6" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg4_t8" runat="server" Text='<%#Eval("sg4_t8") %>' Width="100%" MaxLength="6" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate></HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg4_t9" runat="server" Text='<%#Eval("sg4_t9") %>' Width="100%" MaxLength="6" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg4_t10" runat="server" Text='<%#Eval("sg4_t10") %>' Width="100%" MaxLength="6" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg4_t11" runat="server" Text='<%#Eval("sg4_t11") %>' Width="100%" MaxLength="6" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate></HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg4_t12" runat="server" Text='<%#Eval("sg4_t12") %>' Width="100%" MaxLength="6" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg4_t13" runat="server" Text='<%#Eval("sg4_t13") %>' Width="100%" MaxLength="6" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg4_t14" runat="server" Text='<%#Eval("sg4_t14") %>' Width="100%" MaxLength="6" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg4_t15" runat="server" Text='<%#Eval("sg4_t15") %>' Width="100%" MaxLength="6" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg4_t16" runat="server" Text='<%#Eval("sg4_t16") %>' Width="100%" MaxLength="6" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg4_t17" runat="server" Text='<%#Eval("sg4_t17") %>' Width="100%" MaxLength="6" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg4_t18" runat="server" Text='<%#Eval("sg4_t18") %>' Width="100%" MaxLength="6" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg4_t19" runat="server" Text='<%#Eval("sg4_t19") %>' Width="100%" MaxLength="6" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg4_t20" runat="server" Text='<%#Eval("sg4_t20") %>' Width="100%" MaxLength="6" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg4_t21" runat="server" Text='<%#Eval("sg4_t21") %>' Width="100%" MaxLength="6" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg4_t22" runat="server" Text='<%#Eval("sg4_t22") %>' Width="100%" MaxLength="6" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg4_t23" runat="server" Text='<%#Eval("sg4_t23") %>' Width="100%" MaxLength="6" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg4_t24" runat="server" Text='<%#Eval("sg4_t24") %>' Width="100%" MaxLength="6" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg4_t25" runat="server" Text='<%#Eval("sg4_t25") %>' Width="100%" MaxLength="6" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg4_t26" runat="server" Text='<%#Eval("sg4_t26") %>' Width="100%" MaxLength="6" onkeypress="return isDecimalKey(event)"></asp:TextBox>
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

                                <div role="tabpanel" class="tab-pane active" id="DescTab7">
                                    <div class="lbBody" id="gridDiv" style="height: 310px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <fin:CoolGridView ID="sg1" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="100%" Font-Size="13px"
                                            AutoGenerateColumns="False" OnRowDataBound="sg1_RowDataBound"
                                            OnRowCommand="sg1_RowCommand">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:BoundField DataField="sg1_h1" HeaderText="sg1_h1" />
                                                <asp:BoundField DataField="sg1_h2" HeaderText="sg1_h2" />
                                                <asp:BoundField DataField="sg1_h3" HeaderText="sg1_h3" />
                                                <asp:BoundField DataField="sg1_h4" HeaderText="sg1_h4" />
                                                <asp:BoundField DataField="sg1_h5" HeaderText="sg1_h5" />
                                                <asp:BoundField DataField="sg1_h6" HeaderText="sg1_h6" />
                                                <asp:BoundField DataField="sg1_h7" HeaderText="sg1_h7" />
                                                <asp:BoundField DataField="sg1_h8" HeaderText="sg1_h8" />
                                                <asp:BoundField DataField="sg1_h9" HeaderText="sg1_h9" />
                                                <asp:BoundField DataField="sg1_h10" HeaderText="sg1_h10" />
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Add</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btnadd" runat="server" CommandName="SG1_ROW_ADD" ImageAlign="Middle" ImageUrl="../tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Document" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Del</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btnrmv" runat="server" CommandName="SG1_RMV" ImageUrl="../tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Document" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="sg1_srno" HeaderText="SrNo." />
                                                <asp:BoundField DataField="sg1_f1" HeaderText="sg1_f1" Visible="false" />
                                                <asp:BoundField DataField="sg1_f2" HeaderText="sg1_f2" Visible="false" />
                                                <asp:BoundField DataField="sg1_f3" HeaderText="sg1_f3" Visible="false" />
                                                <asp:BoundField DataField="sg1_f4" HeaderText="sg1_f4" Visible="false" />
                                                <asp:BoundField DataField="sg1_f5" HeaderText="sg1_f5" Visible="false" />
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Document</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t1" runat="server" Text='<%#Eval("sg1_t1") %>' Width="100%" MaxLength="30"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Doc.No.</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t2" runat="server" Text='<%#Eval("sg1_t2") %>' Width="100%" MaxLength="30"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Issue_Date</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t3" runat="server" Text='<%#Eval("sg1_t3") %>' Width="100%" MaxLength="10"></asp:TextBox>
                                                        <asp:CalendarExtender ID="sg1_t3_CalendarExtender" runat="server"
                                                            Enabled="True" TargetControlID="sg1_t3" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                        <asp:MaskedEditExtender ID="Maskedit1" runat="server" Mask="99/99/9999" MaskType="Date" TargetControlID="sg1_t3" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Expiry_Date</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t4" runat="server" Text='<%#Eval("sg1_t4") %>' Width="100%" MaxLength="10"></asp:TextBox>
                                                        <asp:CalendarExtender ID="sg1_t4_CalendarExtender" runat="server"
                                                            Enabled="True" TargetControlID="sg1_t4" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                        <asp:MaskedEditExtender ID="Maskedit2" runat="server" Mask="99/99/9999" MaskType="Date" TargetControlID="sg1_t4" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Iss_From</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t5" runat="server" Text='<%#Eval("sg1_t5") %>' Width="100%" MaxLength="50"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Remarks</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t6" runat="server" Text='<%#Eval("sg1_t6") %>' Width="100%" MaxLength="100"></asp:TextBox>
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
                                                <asp:BoundField DataField="sg1_t9" HeaderText="File Name" />
                                                <asp:BoundField DataField="sg1_t10" HeaderText="File Path" />
                                                <asp:TemplateField>
                                                    <HeaderTemplate>File Upload</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:FileUpload ID="FileUpload1" runat="server" EnableViewState="true" onChange="FileUploadCall(this)" ToolTip="Do not Use Special Characters for File Name" />
                                                        <asp:Button ID="btnUpload" runat="server" CommandName="SG1_UPLD" Text="OK" OnClick="btnUpload_Click" Style="display: none" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>sg1_t12</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t12" runat="server" Text='<%#Eval("sg1_t12") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>sg1_t13</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t13" runat="server" Text='<%#Eval("sg1_t13") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>sg1_t14</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t14" runat="server" Text='<%#Eval("sg1_t14") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>sg1_t15</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t15" runat="server" Text='<%#Eval("sg1_t15") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>sg1_t16</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t16" runat="server" Text='<%#Eval("sg1_t16") %>' Width="100%"></asp:TextBox>
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

                <div class="col-md-12">
                    <div>
                        <div class="box-body">
                            <table>
                                <tr>
                                    <td>
                                        <asp:FileUpload ID="Attch" runat="server" Visible="true" onchange="submitFile()" />

                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAttch" runat="server" MaxLength="100" placeholder="Path Upto 100 Char" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label27" runat="server" Text=" Please Link Correct File upto 3MB Size ."></asp:Label>

                                    </td>

                                </tr>

                            </table>

                            <asp:Button ID="btnAtt" runat="server" Text="Attachment" OnClick="btnAtt_Click" Width="134px" Style="display: none" />

                            <asp:Label ID="lblShow" runat="server"></asp:Label>
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
            var tabName = $("[id*=TabName]").val() != "" ? $("[id*=TabName]").val() : "DescTab1";
            $('#Tabs a[href="#' + tabName + '"]').tab('show');
            $("#Tabs a").click(function () {
                $("[id*=TabName]").val($(this).attr("href").replace("#", ""));
            });
        });
    </script>


    <script>
        function sel(n1) {
           <%-- //alert('hfrehflrfh');
            debugger;
            //var x = document.getElementById(n1).value;            
            //var x1 = document.getElementById("rbl_gender2").value;
            // alert(x);
            if (x.innerText == "Male") {
                document.getElementById('<%=txt_gender.ClientID%>').value = "M";
            }
            else if (x.innerText == "Female") {
                document.getElementById('<%=txt_gender.ClientID%>').value = "F";
                document.getElementByI
            }--%>
        }

        function GetSelectedItem() {
            var rb = document.getElementById("<%=rdbGender.ClientID%>");
            var radio = rb.getElementsByTagName("input");
            var label = rb.getElementsByTagName("label");
            for (var i = 0; i < radio.length; i++) {
                if (radio[i].checked) {
                    document.getElementById('<%=txt_gender.ClientID%>').value = radio[i].value;
                    break;
                }
            }
        }

        function marrieddate(n1) {
            var x = document.getElementById(n1).value;
            if (x.checked == "true") {
                document.getElementById('txt_marriagedt').readonly = false;
            }
        }

        function FileUploadCall(fileUpload) {
            if (fileUpload.value != '') {
                var a = $(fileUpload).next("[id*='btnUpload']");
                a.click();
            }
        }

    </script>
    <asp:HiddenField ID="TabName" runat="server" />
</asp:Content>
