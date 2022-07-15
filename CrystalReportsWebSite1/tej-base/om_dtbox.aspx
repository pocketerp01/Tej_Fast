<%@ Page Language="C#" AutoEventWireup="true" Inherits="om_dtbox" CodeFile="om_dtbox.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Finsys</title>
    <link href="Styles/fin.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function closePopup1() {
            $('#ContentPlaceHolder1_btnhideF', window.parent.document).trigger('click');
            parent.$.colorbox.close();
        }
        function closePopup2() {
            $('#ContentPlaceHolder1_btnhideF_s', window.parent.document).trigger('click');
            parent.$.colorbox.close();
        }
        function onlyclose() {
            parent.$.colorbox.close();
        }
    </script>
    <script src="../tej-base/Scripts/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../tej-base/Scripts/jquery.colorbox.js" type="text/javascript"></script>
    <link href="../tej-base/Styles/colorbox.css" rel="stylesheet" type="text/css" />
    <link href="../tej-base/Styles/vip_vrm.css" rel="stylesheet" type="text/css" />
    <script src="../tej-base/Scripts/temp.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("input").not($(":button")).keypress(function (evt) {
                if (evt.keyCode == 13) {
                    iname = $(this).val();
                    if (iname !== 'Submit') {
                        var fields = $(this).parents('form:eq(0),body').find('button,input,textarea,select');
                        var index = fields.index(this);
                        if (index > -1 && (index + 1) < fields.length) {
                            fields.eq(index + 1).focus();
                            fields.eq(index + 1).select();
                        }
                        return false;
                    }
                }
            });
        });
    </script>
</head>
<body style="background-color: #f2f4fa;">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div style="margin-top: 35px;">
                    <%--<span style="position:fixed; padding:10px 0 0 155px; font-family: Arial, Helvetica, sans-serif; font-size: large; color: #333333;">Choose Date</span>--%>
                    <span style="position: absolute; z-index: 102; top: 55px; left: 10px; font-size: medium;" class="font_css">Date</span>
                    <asp:TextBox ID="txtfromdt" runat="server" MaxLength="12" Width="130px" Style="position: absolute; z-index: 102; top: 55px; left: 70px;" TabIndex="1" TextMode="Date"></asp:TextBox>
                </div>
                <button id="btnsubmit" onserverclick="btnsubmit_ServerClick" runat="server" class="btnyes" accesskey="S" style="width: 100px; height: 30px; position: absolute; top: 115px;"><u>S</u>ubmit</button>
                <button id="btnexit" onserverclick="btnexit_ServerClick" runat="server" class="btnno" accesskey="x" style="width: 100px; height: 30px; position: absolute; top: 115px; left: 112px;">E<u>x</u>it</button>

                <asp:Label ID="lblerr" runat="server" CssClass="font_css" Style="position: absolute; top: 145px; color: Red; left: 15px;"></asp:Label>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
