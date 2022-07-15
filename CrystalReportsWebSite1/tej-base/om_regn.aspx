<%@ Page Language="C#" AutoEventWireup="true" Inherits="fin_pay_web_om_regn" Async="true" CodeFile="om_regn.aspx.cs" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <%--//--------------%>

    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title></title>
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport" />
    <link rel="shortcut icon" type="image/ico" href="tej-base/images/finsys _small.jpg" />
    <link rel="stylesheet" href="tej-base/bootstrap/css/bootstrap.min.css" />

    <%--<link rel="stylesheet" href="tej-base/ionicons/2.0.1/css/ionicons.min.css" />--%>
    <link rel="stylesheet" href="tej-base/dist/css/AdminLTE.min.css" />

    <script lang="javascript" type="text/javascript">
        function fun() {
            if (event.button == 2) {
                //return false;
            }
        }
    </script>
    <%--<script src="tej-base/Scripts/jquery-ui.min.js" type="text/javascript"></script>--%>

    <link rel="stylesheet" href="../tej-base/bootstrap/css/bootstrap.min.css" />
    <link rel="stylesheet" href="../tej-base/dist/css/AdminLTE.min.css" />
    <link rel="stylesheet" href="../tej-base/dist/css/skins/_all-skins.min.css" />


    
    <script src="../tej-base/Scripts/jquery-1.11.1.min.js" type="text/javascript"></script>
    
    <script src="../tej-base/Scripts/jquery.colorbox.js" type="text/javascript"></script>
    <script src="../tej-base/Scripts/jquery.colorbox-min.js" type="text/javascript"></script>
    <script src="../tej-base/Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../tej-base/Scripts/temp.js" type="text/javascript"></script>

    <link href="../tej-base/Scripts/colorbox.css" type="text/css" rel="Stylesheet" />

    <style type="text/css">
        body {
            padding-top: 2px;
            padding-bottom: 1px;
            position: relative;
        }

            body::before {
                //background: url(tej-base/images/bgImg.jpg) no-repeat center center fixed;
                //background: url(http://localhost:1683/tej-base/images/bgImg.jpg) no-repeat center center fixed;
                background-color: lightblue;
                content: '';
                z-index: -1;
                width: 100%;
                height: 100%;
                position: absolute;
                -webkit-background-size: cover;
                -moz-background-size: cover;
                -o-background-size: cover;
                background-size: cover;
                /*-webkit-filter: blur(1px);
                -moz-filter: blur(1px);
                -o-filter: blur(1px);
                -ms-filter: blur(1px);
                filter: blur(1px);*/
            }

        /* width */
        ::-webkit-scrollbar {
            width: 1px;
        }

        /* Track */
        ::-webkit-scrollbar-track {
            background: #f1f1f1;
        }

        /* Handle */
        ::-webkit-scrollbar-thumb {
            background: #888;
        }

            /* Handle on hover */
            ::-webkit-scrollbar-thumb:hover {
                background: #555;
            }

        /*table {
            font-family: Calibri;
            color: white;
            font-size: 11pt;
            font-style: normal;
            font-weight: bold;
            text-align:;
            background-color: SlateBlue;
            border-collapse: collapse;
            border: 2px solid navy;
        }*/
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <section class="content-header">
                <img src="../tej-base/images/banner.jpg" style="width: 100%" height="200" />

                <table style="width: 100%">
                    <%-- <tr>
                    <td>                        
                        <h1 style="text-align:center; color:red; font-size:40px" >Online Free Seat Booking</h1>
                    </td>  </tr>--%>

                    <tr>
                        <td style="text-align: center">
                            <button type="submit" id="btnnew" class="btn btn-info" style="width: 100px; height: 34px" runat="server" accesskey="N" onserverclick="btnnew_ServerClick"><u>N</u>ew</button>
                            <button type="submit" id="btnedit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="i" onserverclick="btnedit_ServerClick">Ed<u>i</u>t</button>
                            <button type="submit" id="btnsave" class="btn btn-info" style="width: 100px; height: 34px;" runat="server" accesskey="s" onserverclick="btnsave_ServerClick"><u>S</u>ave</button>
                            <button type="submit" id="btnprint" class="btn btn-info" style="width: 100px;" runat="server" accesskey="P" onserverclick="btnprint_ServerClick"><u>P</u>rint</button>
                            <button type="submit" id="btndel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="l" onserverclick="btndel_ServerClick">De<u>l</u>ete</button>
                            <button type="submit" id="btnlist" class="btn btn-info" style="width: 100px;" runat="server" accesskey="t" onserverclick="btnlist_ServerClick">Lis<u>t</u></button>
                            <button type="submit" id="btncancel" class="btn btn-info" style="width: 100px; height: 34px" runat="server" accesskey="c" onserverclick="btncancel_ServerClick"><u>C</u>ancel</button>
                            <button type="submit" id="btnexit" class="btn btn-info" style="width: 100px; height: 34px" runat="server" accesskey="X" onserverclick="btnexit_ServerClick">E<u>x</u>it</button>
                        </td>

                    </tr>
                </table>
            </section>
            <section class="content">
                <%--<div class="container" style="background-color:dodgerblue">
  <h2><b>ProjectID Creation</b></h2>
                </div>--%>
                <div class="row">
                    
                    <div class="col-md-12">
                        <div>
                            <div class="box-body">
                                <div class="form-group">

                               <asp:Label ID="lbl3" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Select No of Delegates</asp:Label>
                                        <div class="col-sm-3">
                              <asp:DropDownList ID="dd_delegate" runat="server" CssClass="form-control" Width="100%" OnSelectedIndexChanged="dd_delegate_SelectedIndexChanged" AutoPostBack="True" ></asp:DropDownList>
                                        </div> </div>                               
                            </div></div></div>

                    <div class="col-md-6" style="display: none">
                        <div>
                            <div class="box-body">

                                <div class="form-group">
                                    <label id="Label1" runat="server" class="col-sm-2 control-label">Regn No.</label>
                                    <div class="col-sm-4">
                                        <input id="txtregn" type="text" class="form-control" runat="server" maxlength="10" style="height: 26px" readonly="true" />
                                    </div>
                                    <label id="Label4" runat="server" class="col-sm-2 control-label">Regn Date</label>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtvchdate" placeholder="Date" runat="server" Width="100%" CssClass="form-control" Style="height: 26px" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>


                    <%--     <table > 
<tr>
<td>Comp Code</td>
<td><asp:TextBox id="txt_cocode1" class="form-control" runat="server" Height="25px" Width="300px" type="text" name="Comp_Code" maxlength="4" OnTextChanged="txt_cocode1_TextChanged" AutoPostBack="true" />
(max 4 characters A-Z)
</td>
</tr>
                    <tr>
                        <td>Comp Name</td>
                        <td>
                            <input id="txt_coname1" type="text" class="form-control" runat="server" maxlength="100" readonly="true" style="width:300px; height:25px;"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        _Name</td>
                        <td>
                            <input id="txt_Vname1" type="text" class="form-control" runat="server" maxlength="50" style="width:300px; height:25px;" />
                        </td>
                    </tr>
                    <tr>
                        <td>Designation</td>
                        <td>
                            <input id="txt_desg1" type="text" class="form-control" runat="server" maxlength="50" style="width:300px; height:25px;"/>
                        </td>
                    </tr>
                    <tr>
                        <td>Department</td>
                        <td>
                            <input id="txt_deptt1" type="text" class="form-control" runat="server" maxlength="50" style="width:300px; height:25px;"/>
                        </td>
                    </tr>
                    <tr>
                        <td>Email_Id</td>
                        <td>
                            <input id="txt_email1" type="text" class="form-control" runat="server" maxlength="50" style="width:300px; height:25px;"/>
                        </td>
                    </tr>
                    <tr>
                        <td>Mobile_No</td>
                        <td>
                            <input id="txt_mob1" type="text" class="form-control" runat="server" maxlength="10" style="width:300px; height:25px;"/>
                        </td>
                    </tr>                   

                    </table>      --%>


                    <div class="col-md-6" runat="server" id="fstdiv">
                        <div>
                            <div class="box-body">
                                <h3 style="text-align: left; color: red;">First Delegate</h3>

                                <div class="form-group">
                                    <label id="Label19" runat="server" class="col-sm-2 control-label">Comp Code</label>
                                    <div class="col-sm-10">
                                        <asp:TextBox ID="txt_cocode1" type="text" CssClass="uppercase" class="form-control" Width="100%" Height="34px" runat="server" MaxLength="4" OnTextChanged="txt_cocode1_TextChanged" AutoPostBack="true" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label id="Label11" runat="server" class="col-sm-2 control-label">Comp Name</label>
                                    <div class="col-sm-10">
                                        <input id="txt_coname1" type="text" class="form-control" runat="server" maxlength="100" style="width: 100%; height: 34px;" readonly="true" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label id="Label13" runat="server" class="col-sm-2 control-label">Visitor_Name</label>
                                    <div class="col-sm-10">
                                        <input id="txt_Vname1" type="text" class="form-control" runat="server" maxlength="50" style="width: 100%; height: 34px;" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label id="Label2" runat="server" class="col-sm-2 control-label">Designation</label>
                                    <div class="col-sm-10">
                                        <input id="txt_desg1" type="text" class="form-control" runat="server" maxlength="50" style="width: 100%; height: 34px;" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label id="lblDepartment" runat="server" class="col-sm-2 control-label">Department</label>

                                    <div class="col-sm-10">
                                        <input id="txt_deptt1" type="text" class="form-control" runat="server" maxlength="50" style="width: 100%; height: 34px;" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label id="Label3" runat="server" class="col-sm-2 control-label">Email_Id</label>

                                    <div class="col-sm-10">
                                        <input id="txt_email1" type="text" class="form-control" runat="server" maxlength="50" style="width: 100%; height: 34px;" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label id="Label5" runat="server" class="col-sm-2 control-label">Mobile_No</label>

                                    <div class="col-sm-10">
                                        <input id="txt_mob1" type="text" class="form-control" runat="server" maxlength="10" style="width: 100%; height: 34px;" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>


                    <%--    <table> 
<tr>
<td>Comp Code</td>
<td><input id="txt_cocode2" class="form-control" runat="server" type="text" name="First_Name" maxlength="10" style="width:300px; height:25px;"/>
(max 4 characters A-Z)
</td>
</tr>
                    <tr>
                        <td>Comp Name</td>
                        <td>
                            <input id="txt_coname2" type="text" class="form-control" runat="server" maxlength="100" readonly="true" style="width:300px; height:25px;"/>
                        </td>
                    </tr>
                    <tr>
                        <td>Visitor_Name</td>
                        <td>
                            <input id="txt_Vname2" type="text" class="form-control" runat="server" maxlength="50" style="width:300px; height:25px;" />
                        </td>
                    </tr>
                    <tr>
                        <td>Designation</td>
                        <td>
                            <input id="txt_desg2" type="text" class="form-control" runat="server" maxlength="50" style="width:300px; height:25px;" />
                        </td>
                    </tr>
                    <tr>
                        <td>Department</td>
                        <td>
                            <input id="txt_deptt2" type="text" class="form-control" runat="server" maxlength="50" style="width:300px; height:25px;"/>
                        </td>
                    </tr>
                    <tr>
                        <td>Email_Id</td>
                        <td>
                            <input id="txt_email2" type="text" class="form-control" runat="server" maxlength="50" style="width:300px; height:25px;"/>
                        </td>
                    </tr>
                    <tr>
                        <td>Mobile_No</td>
                        <td>
                            <input id="txt_mob2" type="text" class="form-control" runat="server" style="width:300px; height:25px;" maxlength="10"/>
                        </td>
                    </tr>
                    </table>--%>

                    <div class="col-md-6" runat="server" id="scnddiv">
                        <div>
                            <div class="box-body">
                                <h3 style="text-align: left; color: red;">Second Delegate</h3>


                                <div class="form-group">

                                    <label id="Label10" runat="server" class="col-sm-2 control-label">Comp Code</label>
                                    <div class="col-sm-10">
                                        <input id="txt_cocode2" type="text" readonly="true" class="form-control" runat="server" maxlength="10" style="width: 100%; height: 34px;" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label id="Label12" runat="server" class="col-sm-2 control-label">Comp Name</label>
                                    <div class="col-sm-10">
                                        <input id="txt_coname2" type="text" class="form-control" runat="server" maxlength="100" readonly="true" style="width: 100%; height: 34px;" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label id="Label14" runat="server" class="col-sm-2 control-label">Visitor_Name</label>
                                    <div class="col-sm-10">
                                        <input id="txt_Vname2" type="text" class="form-control" runat="server" maxlength="50" style="width: 100%; height: 34px;" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label id="Label8" runat="server" class="col-sm-2 control-label">Designation</label>
                                    <div class="col-sm-10">
                                        <input id="txt_desg2" type="text" class="form-control" runat="server" maxlength="50" style="width: 100%; height: 34px;" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label id="Label9" runat="server" class="col-sm-2 control-label">Department</label>

                                    <div class="col-sm-10">
                                        <input id="txt_deptt2" type="text" class="form-control" runat="server" maxlength="50" style="width: 100%; height: 34px;" />
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label id="Label6" runat="server" class="col-sm-2 control-label">Email_Id</label>

                                    <div class="col-sm-10">
                                        <input id="txt_email2" type="text" class="form-control" runat="server" maxlength="50" style="width: 100%; height: 34px;" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label id="Label7" runat="server" class="col-sm-2 control-label">Mobile_No</label>

                                    <div class="col-sm-10">
                                        <input id="txt_mob2" type="text" class="form-control" runat="server" maxlength="10" style="width: 100%; height: 34px;" />
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>

                    <div class="col-md-12">
                        <div>
                            <div class="box-body">
                                <div class="form-group">
                                    <p style="font: bold;">Free entry for only 2 Delegates. For Paid Delegates, Please click <a href="http://213.136.94.9/GST/" target="_blank">Payments.pocketdriver.in</a> link for registration through payment (Rs 750/- + 18% GST). On spot Registration Charges (Rs.1500/- + 18% GST), For more details,please click <a href="https://pocketdriver.in/finsys-webtel-2019-october-4th-conclave/" target="_blank">Finsys Webtel 2019 October 4th Conclave</a> </p>
                                    <label id="Label15" runat="server" class="col-sm-12 control-label"></label>
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
        <asp:HiddenField ID="doc_nf" runat="server" />
        <asp:HiddenField ID="doc_df" runat="server" />
        <asp:HiddenField ID="doc_vty" runat="server" />
        <asp:HiddenField ID="doc_addl" runat="server" />
        <asp:HiddenField ID="edmode" runat="server" />
        <asp:HiddenField ID="hf1" runat="server" />
        <asp:HiddenField ID="hfGridView1SV" runat="server" />
        <asp:HiddenField ID="hfGridView1SH" runat="server" />
        <script src="tej-base/Scripts/jquery.colorbox-min.js" type="text/javascript"></script>
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
                //alert('hfrehflrfh');
                debugger;
                var x = document.getElementById(n1).value;
                //var x1 = document.getElementById("rbl_gender2").value;
                // alert(x);

            }

            function marrieddate(n1) {
                var x = document.getElementById(n1).value;
                if (x.checked == "true") {
                    document.getElementById('txt_marriagedt').readonly = false;
                }
            }

        </script>
        <asp:HiddenField ID="TabName" runat="server" />
        </div>
    </form>
</body>
</html>
