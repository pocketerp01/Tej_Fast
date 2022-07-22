<%@ Page Language="C#" AutoEventWireup="true" Inherits="Login" Async="true" CodeFile="Login.aspx.cs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">   
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title></title>
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport" />
    <link rel="shortcut icon" type="image/ico" href="tej-base/images/finsys _small.jpg" />

    <link rel="stylesheet" href="tej-base/bootstrap/css/bootstrap.min.css" />
    <link rel="stylesheet" href="tej-base/dist/css/AdminLTE.min.css" />

    <script src="tej-base/Scripts/jquery-1.11.1.min.js" type="text/javascript"></script>

    <script src="tej-base/Scripts/jquery.colorbox-min.js" type="text/javascript"></script>
    <link type="text/css" rel="Stylesheet" href="tej-base/Scripts/colorbox.css" />
    <script src="tej-base/Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <script src="tej-base/Scripts/temp.js" type="text/javascript"></script>
    <script lang="javaScript" type="text/javascript" src="crystalreportviewers13/js/crviewer/crv.js"></script>

    <style type="text/css">
        body {
            padding-top: 2px;
            padding-bottom: 1px;
            position: relative;
            font-family:-webkit-pictograph!important;
        }
            body::before {
                background: url(bg-image/bbg1.jpg) center center no-repeat fixed;
                content: '';
                z-index: -1;
                width: 100%;
                height: 120%;
                position: absolute;
                -webkit-background-size: cover;
                -moz-background-size: cover;
                -o-background-size: cover;
                background-size: cover;
                -webkit-filter: blur(0);
                -moz-filter: blur(0);
                -o-filter: blur(0);
                -ms-filter: blur(0);
                filter: blur(0);

            }

        ::-webkit-scrollbar {
            width: 1px;
        }

        ::-webkit-scrollbar-track {
            background: #f1f1f1;
        }

        ::-webkit-scrollbar-thumb {
            background: #888;
        }

            ::-webkit-scrollbar-thumb:hover {
                background: #555;
            }
             input[type=text],input[type=password], select, input[type=date], date, textarea {
            /*border-right: 1px solid;*/
            border-bottom: 1px solid;
            border-top: 0px;
            border-left: 0px;
            border-right: 0px;
            padding-left: 5px;
            border-radius: 0px;
            outline: none;
            border-bottom-color: #0094ff;
            font-family:-webkit-pictograph!important;
            /*box-shadow: 0 0 5px ##249563;*/
        }
    </style>
</head>
<body oncontextmenu="return fun(); " class="hold-transition login-page">
    <form id="Form1" runat="server" style="color: transparent">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" EnableCdn="true">
        </asp:ScriptManager>
        <div class="login-box" style="box-shadow: 0 20px 80px 0 rgba(151, 158, 161, 0.70), 0 6px 20px 0 rgba(0, 0, 0, 0.5);">
            <div class="login-box-body">
                <div class="login-logo">
                    <img id="imglogo" class="logo_finsys" runat="server" alt="Tejaxo" src="tej-base/images/f_logo.jpg" style="max-width:300px"/>
                </div>
                <div class="form-group has-feedback">
                    <input type="text" placeholder="Company Code" value="" class="form-control" maxlength="8"
                        name="compcode" runat="server" id="txtcompcode" autocomplete="off" />
                </div>
                <div class="form-group has-feedback">
                    <input type="text" placeholder="Year" value="" class="form-control" maxlength="4" name="year" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false"
                        id="txtyear" runat="server" />
                </div>
                <div class="form-group has-feedback">
                    <input type="text" placeholder="User Name" value="" class="form-control" name="username" accesskey="U"
                        id="txtusername" runat="server" maxlength="12" autocomplete="off" />

                </div>
                <div class="form-group has-feedback">
                    <input type="password" placeholder="Password" class="form-control" id="txtPassword"
                        name="pw" autocomplete="off" runat="server" onkeypress="if (event.keyCode == 13) {$('#btnlogin').click();}" maxlength="13" />

                </div>
                <hr />
                <div class="row">
                    <div class="col-xs-2"></div>
                    <div class="col-xs-8">
                        
                        <button class="btn btn-primary btn-block btn-flat" style="background-color:#0E7192;color:white" id="btnLogin" runat="server" name="Login" onserverclick="btnLogin_ServerClick" accesskey="L">
                            <u>L</u>og In
                        </button>
                    </div>
                    <div class="col-xs-2"></div>
                </div>
                <div class="progress" id="divpbig" style="display: none">
                    <div id="divppp" class="progress-bar progress-bar-success progress-bar-striped active" role="progressbar"
                        aria-valuenow="40" aria-valuemin="0" aria-valuemax="100">
                    </div>
                </div>
                <div id="changeText" style="font-size: medium; text-align: center; display: none;"></div>
                <br />

                <div style="text-align: center">
                    <a id="lnkfpass" runat="server" style="cursor: pointer;" onserverclick="lnkfpass_ServerClick" accesskey="o">F<u>o</u>rgot password</a>
                    &nbsp;|&nbsp;
                    <a id="lnkchng" runat="server" style="cursor: pointer;" onserverclick="lnkchng_ServerClick" accesskey="C"><u>C</u>hange Your Password</a>
                    <hr />
                    <u>Tejaxo ERP © 2017-2021 </u>&nbsp;|&nbsp;
                    <br />
                    <b>Build Date: </b>
                    <asp:Label ID="lbldttime" runat="server"></asp:Label>
                </div>

                <input id="dd" type="button" onclick="changeText()" title="aa" style="display: none" />
            </div>
        </div>
        <asp:Button ID="btnhideF" runat="server" OnClick="btnhideF_Click" Text="!" Style="display: none;" />
        <asp:Button ID="btnhideFS" runat="server" OnClick="btnhideFS_Click" Text="!" Style="display: none;" />
        <asp:HiddenField ID="hf_unqid" runat="server" />

        <asp:HiddenField ID="hfWindowSize" runat="server" />
        <script type="text/javascript">
            $(document).ready(function () {
                $("input").not($(":button")).keypress(function (e) { if (13 == e.keyCode && (iname = $(this).val(), "Submit" !== iname)) { var t = $(this).parents("form:eq(0),body").find("button,input,textarea,select"), n = t.index(this); return n > -1 && n + 1 < t.length && (e.preventDefault(), t.eq(n + 1).focus(), t.eq(n + 1).select(), e.keyCode = 9), !1 } });
                $("#progressbar").progressbar({ value: 0 });

                $("#hfWindowSize").val($(window).width());
            });
        </script>

        <%-- crystal  --%>
        <div id="div1" style="display: none" runat="server">
            <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server"
                AutoDataBind="true" OnUnload="CrystalReportViewer1_Unload" HasCrystalLogo="False"
                Height="50px" Width="350px" Style="margin-left: 30px;" EnableDrillDown="false" />
        </div>

        <script lang="javascript" type="text/javascript">
            function fun() { if (2 == event.button) return !1 }

            function preventBack() { window.history.forward(); }
            setTimeout("preventBack()", 0);
            window.onunload = function () { null };

            /*
                Function to show message on login screen, added on 09/04/2020 -- VV
                changed on 10/04/2020 -- vv
                */
            var procounter = 10;
            function changeText() {
                
                $("#changeText").show();
                $("#divpbig").show();
                var myvar = setInterval(updateProg, 100);
                function updateProg() {
                    
                    document.getElementById("divppp").innerHTML = procounter + "%";
                    document.getElementById("divppp").style.cssText = "width : " + procounter + "%";
                    procounter++;
                    if (procounter > 50) {
                        clearInterval(myvar);
                        myvar = setInterval(updateProg, 500);
                    }
                    if (procounter > 70) {
                        clearInterval(myvar);
                        myvar = setInterval(updateProg, 700);
                    }
                    if (procounter > 90) {
                        clearInterval(myvar);
                        myvar = setInterval(updateProg, 900);
                    }
                    if (procounter > 95) {
                        clearInterval(myvar);
                        myvar = setInterval(updateProg, 1100);
                    }
                    if (procounter > 98) {
                        clearInterval(myvar);
                    }
                }

                var dot = ".";
                var texttos = ["Checking the Credentials", "Checking the Roles and Rights", "Checking and Creating the Icons", "Making up the theme", "Setting the tools", "Adjusting Menu's"];
                var counter = 0;
                document.getElementById("changeText").innerHTML = "Welcome to ERP...";
                window.setInterval(function () {
                    
                    document.getElementById("changeText").innerHTML = texttos[counter];
                    counter++;
                    if (counter >= texttos.length) {
                        counter = 0;
                    }
                }, 4000);
            }
        </script>
    </form>
</body>
</html>
