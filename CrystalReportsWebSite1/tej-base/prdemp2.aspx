<%@ Page Language="C#" AutoEventWireup="true" Inherits="prdemp2" CodeFile="prdemp2.aspx.cs" %>
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
    <script type="text/javascript">
        $(function () {
            $("#txtfromdt").focus();
            $("#txtfromdt").select();
        });
    </script>
    <script src="Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery.colorbox.js" type="text/javascript"></script>
    <link href="Styles/colorbox.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/temp.js" type="text/javascript"></script>
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
<body style="background-color:#f2f4fa;">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    <div style="margin-top:35px;">
    <img src="images/header.png" width="475px" height="40px" style="position:absolute; " alt=""/>
    <span style="position:fixed; padding:10px 0 0 175px; font-family: Arial, Helvetica, sans-serif; font-size: large; color: #333333;">
    Select Period</span>
   <span style="position:absolute; z-index:102; top:85px; left:10px; font-size:medium;" class="font_css">Date From</span>
    <asp:TextBox ID="txtfromdt" runat="server" MaxLength="12" Width="130px" style="position:absolute; z-index:102; top:85px; left:95px;" TabIndex="1" ></asp:TextBox>
        <cc1:MaskedEditExtender ID="MaskedEditExtender1" runat="server" Mask="99/99/9999" 
                            MaskType="Date" TargetControlID="txtfromdt" />
    <asp:RadioButtonList ID="RadioButtonList1" runat="server" CssClass="font_css" style="position:absolute; z-index:102; top:77px; left:235px;" TabIndex="3"
                AutoPostBack="True" BackColor="#BDEDFF" 
            onselectedindexchanged="RadioButtonList1_SelectedIndexChanged">
                <asp:ListItem>Y.T.D.(Year To Date)</asp:ListItem>
                <asp:ListItem>M.T.D.(Month To Date)</asp:ListItem>
                <asp:ListItem>Previous Month</asp:ListItem>
                <asp:ListItem>Next Month</asp:ListItem>
                <asp:ListItem>Yesterday</asp:ListItem>
                <asp:ListItem>Today</asp:ListItem>
            </asp:RadioButtonList>
    <asp:RadioButtonList ID="RadioButtonList2" runat="server" CssClass="font_css" style="position:absolute; z-index:102; top:77px; left:378px; height:140px;" 
                AutoPostBack="True" BackColor="#BDEDFF" 
            onselectedindexchanged="RadioButtonList2_SelectedIndexChanged">
                <asp:ListItem>Current Month</asp:ListItem>
                <asp:ListItem>First Qtr</asp:ListItem>
                <asp:ListItem>Second Qtr</asp:ListItem>
                <asp:ListItem>Third Qtr</asp:ListItem>
                <asp:ListItem>Fourth Qtr</asp:ListItem>
            </asp:RadioButtonList>
   <span style="position:absolute; z-index:102; top:115px; left:10px; font-size:medium" class="font_css">Date To</span>
    <asp:TextBox ID="txttodt" runat="server" MaxLength="12" Width="130px" style="position:absolute; z-index:102; top:115px; left:95px;" TabIndex="2" ></asp:TextBox>
            <cc1:MaskedEditExtender ID="Maskedit1" runat="server" Mask="99/99/9999" 
                            MaskType="Date" TargetControlID="txttodt" />
    </div>
    <button id="btnsubmit" onserverclick="btnsubmit_ServerClick" runat="server" class="btnyes" accesskey="S" style="width:92px; height:30px; position:absolute; top:185px;"><u>S</u>ubmit</button>
    <button id="btnexit" onserverclick="btnexit_ServerClick" runat="server" class="btnno" accesskey="x" style="width:92px; height:30px; position:absolute; top:185px; left:102px;">E<u>x</u>it</button>
    <asp:Label ID="lblerr" runat="server" CssClass="font_css" style="position:absolute; top:150px; color:Red; left:15px;"></asp:Label>
    </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
