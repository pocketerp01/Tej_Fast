<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="vmrec" Title="Tejaxo" CodeFile="vmrec.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">

        $(document).ready(function () {
            $('#btncp1').click(function (e) {
                $("#canvas").hide();
                $("#video").show();
                StartCam();
            });
        });

        function StartCam() {
            var canvas = $("#canvas"),
            context = canvas[0].getContext("2d"),
            video = $("#video")[0],
            videoObj = { "video": true },
            errHandler = function (error) {
                console.log("Video capture error: ", error.code);
            };

            if (navigator.mediaDevices && navigator.mediaDevices.getUserMedia) {
                navigator.mediaDevices.getUserMedia({ video: true }).then(function (stream) {
                    video.srcObject = stream;
                    video.play();
                }, errHandler);
            }
            else if (navigator.getUserMedia) {
                navigator.getUserMedia({ video: true }, function (stream) {
                    video.srcObject = stream;
                    video.play();
                }, errHandler);
            }
            else if (navigator.webkitGetUserMedia) { // WebKit-prefixed
                navigator.webkitGetUserMedia({ video: true }, function (stream) {
                    video.srcObject = window.webkitURL.createObjectURL(stream);
                    video.play();
                }, errHandler);
            }
            else if (navigator.mozGetUserMedia) { // Firefox-prefixed
                navigator.mozGetUserMedia(videoObj, function (stream) {
                    video.src = window.URL.createObjectURL(stream);
                    video.play();
                }, errHandler);
            }

            $("#btncp2").click(function (e) {
                e.preventDefault();
                $("#canvas").hide();
                $("#video").hide();
                context.drawImage(video, 0, 0, 187, 140);
                setTimeout(function () {
                    var canvas = $("#canvas");
                    var imgStr = canvas[0].toDataURL('image/png');
                    imgStr = imgStr.replace('data:image/png;base64,', '');
                    $('#ContentPlaceHolder1_hfImage').val(imgStr);
                    $('#ContentPlaceHolder1_empImage').attr('src', 'data:image/png;base64,' + imgStr);
                    $('#ContentPlaceHolder1_imgData').val(imgStr);
                }, 100);
            });
        }
    </script>


    <style type="text/css">
        .style1 {
            width: 416px;
        }

        .style2 {
            height: 23px;
        }

        .style4 {
            height: 23px;
            width: 1198px;
        }

        .style6 {
            height: 23px;
            width: 1294px;
        }
    </style>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper">
        <section class="content-header">
            <table style="width: 100%">
                <tr>
                    <td style="float: left;">
                        <button type="submit" id="btnnew" accesskey="N" class="btn btn-info" style="width: 100px;" runat="server" onserverclick="btnnew_Click"><u>N</u>ew</button>
                        <button type="submit" id="btnedit" accesskey="i" class="btn btn-info" style="width: 100px;" runat="server" onserverclick="btnedit_Click">Ed<u>i</u>t</button>
                        <button type="submit" id="btnsave" accesskey="S" class="btn btn-info" style="width: 100px;" runat="server" onserverclick="btnsave_Click"><u>S</u>ave</button>
                        <button type="submit" id="btnprint" accesskey="P" class="btn btn-info" style="width: 100px;" runat="server" onserverclick="btnprint_Click"><u>P</u>rint</button>
                        <button type="submit" id="btntag" accesskey="T" class="btn btn-info" style="width: 100px;" runat="server" onserverclick="btntag_ServerClick"><u>T</u>ag</button>
                        <button type="submit" id="btndelete" accesskey="l" class="btn btn-info" style="width: 100px;" runat="server" onserverclick="btndelete_Click">De<u>l</u>ete</button>
                        <button type="submit" id="btnlist" accesskey="t" class="btn btn-info" style="width: 100px;" runat="server" onserverclick="btnlist_Click">Lis<u>t</u></button>
                        <button type="submit" id="btnexit" class="btn btn-info" style="width: 100px;" runat="server" onserverclick="btnexit_Click">E<u>x</u>it</button>
                    </td>
                    <td>
                        <asp:Label ID="lblheader" runat="server" Text="Visitor Movement Record" Font-Bold="True" Font-Size="X-Large"></asp:Label></td>
                   
                </tr>
            </table>
        </section>

        <section class="content">
            <div class="row">
                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <label id="tdentryno" runat="server" class="col-sm-4 control-label">Entry No.</label>
                                <div class="col-sm-1">
                                    <asp:TextBox ID="txtdocno" runat="server" placeholder="Entry No." Width="53px" CssClass="form-control" Height="28px" autocomplete="Off"></asp:TextBox>
                                </div>
                                <label class="col-sm-1 control-label">Date</label>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtdate" runat="server" placeholder="Date" Width="100%" CssClass="form-control" Height="28px"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtdate_CalendarExtender" runat="server" Enabled="True" TargetControlID="txtdate" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="MaskedEdit2" runat="server" Mask="99/99/9999" MaskType="Date" TargetControlID="txtdate" />
                                </div>
                                <asp:Label ID="Label44" runat="server" Text="lbl3" CssClass="col-sm-1 control-label" Height="30px" Font-Bold="True">Direct</asp:Label>
                                <div class="col-sm-3">
                                    <asp:RadioButtonList ID="rd_done" runat="server" RepeatDirection="Horizontal" Height="30px" BackColor="#FFC107" OnSelectedIndexChanged="rd_done_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Text="&nbsp;&nbsp;&nbsp;Y &nbsp;&nbsp;&nbsp;" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="&nbsp;&nbsp;&nbsp;N &nbsp;&nbsp;&nbsp;" Value="1"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                            </div>
                            <div runat="server" id="r1">
                                <div class="form-group">
                                    <div class="col-sm-4"></div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtrid" runat="server" ReadOnly="true" Width="100px" Height="30px" CssClass="form-control" autocomplete="Off"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2"></div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtrdt" runat="server" ReadOnly="true" Width="100%" Height="30px" CssClass="form-control" autocomplete="Off"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="tdvisit" runat="server" class="col-sm-3 control-label">Visitors Name</label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnvisit" runat="server" Height="23px" Width="26px" ToolTip="Select Visitors Name"
                                        ImageAlign="left" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;"
                                        OnClick="btndept_Click" />
                                </div>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtvname" runat="server" Width="100%" placeholder="" ReadOnly="true" Height="28px" autocomplete="Off"></asp:TextBox>
                                </div>
                            </div>


                            <div class="form-group">
                                <label id="company" runat="server" class="col-sm-4 control-label">Company</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtcomp" runat="server" Width="100%" placeholder="" ReadOnly="true" Height="28px" autocomplete="Off"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Location" runat="server" class="col-sm-4 control-label">Address</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtloc" runat="server" Width="100%" placeholder="" ReadOnly="true" Height="28px" autocomplete="Off"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Purpose" runat="server" class="col-sm-4 control-label">Purpose</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtpurpose" runat="server" Width="100%" placeholder="" ReadOnly="true" Height="28px" autocomplete="Off"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label1" runat="server" class="col-sm-3 control-label">Allotted_Time(in Min)</label>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtAlotMin" runat="server" Width="100%" placeholder="" Height="28px" MaxLength="5" autocomplete="Off"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body" style="min-height: 169px;">
                            <div class="form-group">
                                <div style="text-align: center;">
                                    <table>
                                        <tr>
                                            <td>
                                                <video id="video" height="100"></video>
                                                <%--120--%>
                                                <canvas id="canvas" height="100"></canvas>
                                                <%--120--%>
                                            </td>
                                            <td>
                                                <asp:Image ID="empImage" runat="server" Height="120px" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div style="text-align: center;">
                                <input id="btncp1" type="button" class="searchbtn" value="Open Camera" title="Open Camera" style="width: 150px" />
                                <input id="btncp2" type="button" class="searchbtn" value="Capture Image" title="Capture Image" style="width: 150px" />
                                <asp:HiddenField ID="imgData" runat="server" />
                                <input id="hfImage" type="hidden" value="" runat="server" />
                            </div>


                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <label id="Department" runat="server" class="col-sm-4 control-label">Department</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtdept" runat="server" Width="100%" placeholder="" ReadOnly="true" Height="28px" autocomplete="Off"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Designation" runat="server" class="col-sm-4 control-label">Designation</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtdesig" runat="server" Width="100%" placeholder="" ReadOnly="true" Height="28px" autocomplete="Off"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="lastvsistedon" runat="server" class="col-sm-4 control-label">Last_Visited_On</label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtvdate" runat="server" Width="100%" placeholder="" ReadOnly="true" Height="28px" autocomplete="Off"></asp:TextBox>
                                </div>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtvtype" runat="server" Width="100%" ReadOnly="true" Height="28px" autocomplete="Off"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Mobile" runat="server" class="col-sm-4 control-label">Mobile No.</label>
                                <%--<div class="col-sm-1">
                                    <span style="color: red">*</span>
                                </div>--%>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtmob" runat="server" Width="100%" placeholder="" Height="28px" MaxLength="20" autocomplete="Off"></asp:TextBox>
                                </div>
                                <%--   <label id="Visitortype" runat="server" class="col-sm-2 control-label">Visitor_Type</label>--%>
                                <div class="col-sm-3" style="display: none;">
                                    <asp:DropDownList ID="ddvtype" runat="server" TabIndex="11" Width="100%" Height="32px" autocomplete="Off">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group" id="divOtp" runat="server">
                                <label id="lblOtp" runat="server" class="col-sm-1 control-label">Otp</label>
                                <div class="col-sm-3">
                                    <span style="color: red" id="redC" runat="server">*</span>
                                </div>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtotp" runat="server" Width="100%" Height="28px" OnTextChanged="txtotp_TextChanged" AutoPostBack="true"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Requestby" runat="server" class="col-sm-4 control-label">Request By</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtapp" runat="server" Width="100%" placeholder="" ReadOnly="true" Height="28px" autocomplete="Off"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="carrylap" runat="server" class="col-sm-4 control-label">Carry Laptop?</label>
                                <div class="col-sm-2">
                                    <asp:DropDownList ID="ddcarry" runat="server" TabIndex="11" Width="100%" Height="28px">
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group">
                                    <label id="idproof" runat="server" class="col-sm-4 control-label">ID Proof</label>
                                    <div class="col-sm-2">
                                        <asp:DropDownList ID="ddid" runat="server" TabIndex="11" Width="100%" Height="28px" autocomplete="Off"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="LaptopMfg" runat="server" class="col-sm-4 control-label">Laptop Mfg.</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtmfg" runat="server" Width="100%" placeholder="" Height="28px" autocomplete="Off"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Serialno" runat="server" class="col-sm-4 control-label">Serial No.</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtsrno" runat="server" Width="100%" placeholder="" Height="28px" autocomplete="Off"></asp:TextBox>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <label id="Name" runat="server" class="col-sm-4 control-label">ID.Type</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtiname" runat="server" Width="100%" placeholder="" Height="28px" autocomplete="Off"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="value" runat="server" class="col-sm-4 control-label">ID.No.</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtivalue" runat="server" Width="100%" placeholder="" Height="28px" autocomplete="Off"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Inform" runat="server" class="col-sm-3 control-label">To_Meet</label>
                                <%--<div class="col-sm-2">
                                    <span style="color: red">*</span>
                                </div>--%>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnemp" runat="server" Height="23px" Width="26px" ImageAlign="left" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" ToolTip="Select Inform" OnClick="btndept_Click" />
                                </div>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtempid" runat="server" Width="100%" placeholder="" ReadOnly="true" Height="28px" autocomplete="Off"></asp:TextBox>
                                </div>
                            </div>


                            <div class="form-group">
                                <label id="Name1" runat="server" class="col-sm-4 control-label">Name</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtname" runat="server" Width="100%" placeholder="" ReadOnly="true" Height="28px" autocomplete="Off"></asp:TextBox>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>

                        <div class="box-body">
                            <div class="form-group">
                                <label id="Departmnt" runat="server" class="col-sm-4 control-label">Department</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtedept" runat="server" Width="100%" placeholder="" Height="28px" autocomplete="Off"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Desg" runat="server" class="col-sm-4 control-label">Designation</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtedesing" runat="server" Width="100%" Placeholder="" autocomplete="Off" Height="28px"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="modifiedby" runat="server" class="col-sm-4 control-label">Mod_By/Mod_Dt</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtedit" runat="server" Width="100%" placeholder="" autocomplete="Off" ReadOnly="true" Height="28px"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Preparedby" runat="server" class="col-sm-4 control-label">Prep_By/Prep_Dt</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtpre" runat="server" Width="100%" placeholder="" autocomplete="Off" ReadOnly="true" Height="28px"></asp:TextBox>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>

                <div class="col-md-12">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <label id="Remarks" runat="server" class="col-sm-2 control-label">Remarks</label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="txtrmk" runat="server"
                                        placeholder="Remarks upto 300 characters"
                                        Width="100%" TextMode="MultiLine" MaxLength="300"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="TimeIn" runat="server" class="col-sm-2 control-label">Time In</label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txttimein" runat="server" Width="100%" placeholder="" ReadOnly="true" autocomplete="Off" Height="28px"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="TimeOut" runat="server" class="col-sm-2 control-label">Time Out</label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txttimeout" runat="server" Width="100%" placeholder="" ReadOnly="true" autocomplete="Off" Height="28px"></asp:TextBox>
                                </div>
                            </div>

                        </div>
                    </div>

                </div>

            </div>
        </section>

        <asp:HiddenField ID="hf1" runat="server" />
        <asp:HiddenField ID="hf2" runat="server" />
        <asp:HiddenField ID="hf3" runat="server" />
        <asp:HiddenField ID="hf4" runat="server" />
        <asp:HiddenField ID="hfbtnmode" runat="server" />
        <asp:HiddenField ID="hfedmode" runat="server" />
        <asp:HiddenField ID="hffielddt" runat="server" />
        <asp:HiddenField ID="doc_nf" runat="server" />
        <asp:HiddenField ID="doc_df" runat="server" />
        <input type="button" id="btnhideF" runat="server" onserverclick="btnhideF_Click" style="display: none" />
        <input type="button" id="btnhideF_s" runat="server" onserverclick="btnhideF_s_Click" style="display: none" />

        <asp:Button ID="btnOKTarget" runat="server" Text="!" OnClick="btnOKTarget_Click" Style="display: none;" />
        <asp:Button ID="btnCancelTarget" runat="server" Text="!" OnClick="btnCancelTarget_Click" Style="display: none;" />
    </div>
</asp:Content>
