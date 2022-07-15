<%--<%@ Page Language="C#" MasterPageFile="~/finmast.master" AutoEventWireup="true" CodeFile="upd_sdr.aspx.cs"
    <%--<%--Inherits="upd_sdr" Title="Tejaxo" %>--%>

<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="om_upd_sdr" Title="Tejaxo" CodeFile="om_upd_sdr.aspx.cs" %>

<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" language="javascript">
        function ShowMsg() {
          <%--  if(document.getElementById("<%=txtstatus.ClientID%>").value=="" || document.getElementById("<%=txtstatus.ClientID%>").value=="-")
            {
                <%--jAlert('Please Select Status.','FINSYS Warning');
                document.getElementById("<%=txtstatus.ClientID%>").focus();
                return false;--%>
            //}
            __doPostBack('ctl00$ContentPlaceHolder1$btnsave', '');
        }
        function ShowMsg1() {
          <%--  if(document.getElementById("<%=txtstatus.ClientID%>").value=="" || document.getElementById("<%=txtstatus.ClientID%>").value=="-")
            {
                <%--jAlert('Please Select Status.','FINSYS Warning');
                document.getElementById("<%=txtstatus.ClientID%>").focus();
                return false;--%>
            //}
            __doPostBack('ctl00$ContentPlaceHolder1$btnsubmit', '');
        }

        function openfileDialog() {
            $("#Attch").click();
        }
        function submitFile() {
            $("#<%= btnAtt.ClientID%>").click();
        };

        $(function () {
            var tabName = $("[id*=TabName]").val() != "" ? $("[id*=TabName]").val() : "DescTab";
            $('#Tabs a[href="#' + tabName + '"]').tab('show');
            $("#Tabs a").click(function () {
                $("[id*=TabName]").val($(this).attr("href").replace("#", ""));
            });
        });
    </script>

</asp:Content>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper">
        <section class="content-header">
            <table style="width: 100%">
                <%-- <table align="center" style="border-color: #000080; width: 98%; background-color: #8FD1F1;">--%>

                <tr>
                    <td>
                        <asp:Button ID="cmd_btn" runat="server" Visible="false" Text="SDR PAGE1" OnClick="cmd_btn_Click" />
                    </td>
                    <td>
                        <asp:Button ID="cmd_btn2" runat="server" Visible="false" Text="SDR PAGE2" OnClick="cmd_btn2_Click" />


                        <asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label>
                    </td>
                    <td style="text-align: right">
                        <button type="submit" id="btnnew" class="btn btn-info" style="width: 100px;" runat="server" accesskey="N" onserverclick="btnnew_ServerClick"><u>N</u>ew</button>
                        <button type="submit" id="btnedit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="i" onserverclick="btnedit_ServerClick">Ed<u>i</u>t</button>
                        <button type="submit" id="btnsave" class="btn btn-info" style="width: 100px;" runat="server" accesskey="s" onserverclick="btnsave_ServerClick"><u>S</u>ave</button>
                        <button type="submit" id="btnprint" class="btn btn-info" style="width: 100px;" runat="server" accesskey="P" onserverclick="btnprint_ServerClick"><u>P</u>rint</button>
                        <button type="submit" id="btndel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="l" onserverclick="btndel_ServerClick">De<u>l</u>ete</button>
                        <button type="submit" id="btnlist" class="btn btn-info" style="width: 100px;" runat="server" accesskey="t" onserverclick="btnlist_ServerClick">Lis<u>t</u></button>
                        <button type="submit" id="btnsubmit" class="btn btn-info" onclick="return ShowMsg1();" onserverclick="btnsubmit_ServerClick" accesskey="b" visible="false" style="width: 100px;" runat="server">Su<u>b</u>mit</button>
                        <button type="submit" id="btncancel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="c" onserverclick="btncancel_ServerClick"><u>C</u>ancel</button>
                        <button type="submit" id="btnexit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="X" onserverclick="btnexit_ServerClick">E<u>x</u>it</button>
                     
                        <%--<button type="submit" id="Button1" class="btn btn-info" style="width: 100px;" runat="server" accesskey="N" onserverclick="btnnew_ServerClick"><u>N</u>ew</button>
                        <button type="submit" id="Button2" class="btn btn-info" style="width: 100px;" runat="server" accesskey="i" onserverclick="btnedit_ServerClick">Ed<u>i</u>t</button>
                        <button type="submit" id="Button3" class="btn btn-info" style="width: 100px;" runat="server" accesskey="s" onserverclick="btnsave_ServerClick"><u>S</u>ave</button>
                        <button type="submit" id="Button4" class="btn btn-info" style="width: 100px;" runat="server" accesskey="P" onserverclick="btnprint_ServerClick"><u>P</u>rint</button>
                        <button type="submit" id="Button5" class="btn btn-info" style="width: 100px;" runat="server" accesskey="l" onserverclick="btndel_ServerClick">De<u>l</u>ete</button>
                        <button type="submit" id="Button6" class="btn btn-info" style="width: 100px;" runat="server" accesskey="t" onserverclick="btnlist_ServerClick">Lis<u>t</u></button>
                        <button type="submit" id="btngtag" class="btn btn-info" style="width: 100px;" runat="server" accesskey="g" onserverclick="btnSticker_ServerClick">Gate Ta<u>g</u></button>
                        <button type="submit" id="Button7" class="btn btn-info" style="width: 100px;" runat="server" accesskey="c" onserverclick="btncancel_ServerClick"><u>C</u>ancel</button>
                        <button type="submit" id="Button8" class="btn btn-info" style="width: 100px;" runat="server" accesskey="X" onserverclick="btnexit_ServerClick">E<u>x</u>it</button>--%>
                    </td>
                </tr>
            </table>
        </section>

        <%--  <tr>
            <td align="center" colspan="7">
                <button id="btnnew" onserverclick="btnnew_ServerClick" accesskey="N" style="width: 65px;" runat="server"><u>N</u>ew</button>
                <button id="btnedit" onserverclick="btnedit_ServerClick" accesskey="i" style="width: 65px;" runat="server">Ed<u>i</u>t</button>
                <button id="btnsave" onclick="return ShowMsg();" onserverclick="btnsave_ServerClick" accesskey="S" style="width: 65px;" runat="server"> <u>S</u>ave</button>
                <button id="btndel" onserverclick="btndel_ServerClick" accesskey="l" style="width: 65px;" runat="server"> De<u>l</u>ete</button>
                <button id="btnprint" onserverclick="btnprint_ServerClick" accesskey="P" style="width: 65px;" runat="server"> <u>P</u>rint</button>
                <button id="btnsubmit" onclick="return ShowMsg1();" onserverclick="btnsubmit_ServerClick" accesskey="b" style="width: 65px;" runat="server">Su<u>b</u>mit</button>
                <button id="btnlist" onserverclick="btnlist_ServerClick" accesskey="t" style="width: 65px;"-  runat="server">Lis<u>t</u></button>
                <asp:Button ID="btnexit" runat="server" OnClick="btnexit_Click" Text="Exit" Width="65px" />
                <asp:HiddenField ID="lbledmode" runat="server" />
                <asp:HiddenField ID="lblname" runat="server" />
                <asp:HiddenField ID="HFOLDDT" runat="server" />
                <asp:HiddenField ID="HFOPT" runat="server" />
                <asp:HiddenField ID="hffield" runat="server" />
                <asp:HiddenField ID="hf2" runat="server" />
            </td>
        </tr>--%>

        <section class="content">
            <div class="row">
                  <section class="col-lg-12 connectedSortable">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">
                                <li><a href="#DescTab" id="tab1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab">SDR Page 1</a></li>
                                <li><a href="#DescTab1" id="tab2" runat="server" aria-controls="DescTab1" role="tab" data-toggle="tab">SDR Page 2</a></li>
                            </ul>
                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane active" id="DescTab">
                                    <div class="lbBody" style="height: 500px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                       <%-- <div class="col-md-12">
                                            <div>
                                                <div class="box-body">
                                                    <div class="form-group">

                                                        <asp:Label ID="lbl1" runat="server" Height="10px" Font-Size="18px"  CssClass="col-sm-12 control-label"> <b>SDR &nbsp; Page 1 : Shade / Special Product Devlopment Request</b></asp:Label>

                                                        <div class="col-sm-2" style="display: none;">
                                                            <asp:Label ID="lblhead" runat="server" Font-Bold="True" ForeColor="#CC0000" Text="lbl"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-2" style="display: none;">
                                                            <asp:Label ID="lblbrh" runat="server" Text="BRH.SDR No."></asp:Label>
                                                        </div>
                                                        <div class="col-sm-2" style="display: none;">
                                                            <asp:Label ID="lblbrno" runat="server" Text="F/1006"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-2" style="display: none;">
                                                            Development&nbsp;<asp:RadioButtonList ID="rbdeve" runat="server" RepeatDirection="Horizontal"
                                                                RepeatLayout="Flow" Width="120px">
                                                                <asp:ListItem Selected="True" Text="New" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="Existing" Value="1"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>--%>

                                        <div class="col-md-12">
                                            <div>
                                                <div class="box-body">
                                                      <div class="form-group">
                                                        <asp:Label ID="lbl1" runat="server" Font-Size="18px" BackColor="LightGray"  CssClass="col-sm-12 control-label" style="text-align:center;"> <b>SDR &nbsp; Page 1 : Shade / Special Product Devlopment Request</b></asp:Label>
                                                            </div>

                                                     <div class="form-group">
                                                        <div class="col-sm-12"">
                                                            <asp:Label ID="lblhead" runat="server" Font-Bold="True" Font-Size="16px" style="text-align:center;" ForeColor="#CC0000" Text="lbl"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-2" style="display: none;">
                                                            <asp:Label ID="lblbrh" runat="server" Text="BRH.SDR No."></asp:Label>
                                                        </div>
                                                        <div class="col-sm-2" style="display: none;">
                                                            <asp:Label ID="lblbrno" runat="server" Text="F/1006"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-2" style="display: none;">
                                                            Development&nbsp;<asp:RadioButtonList ID="rbdeve" runat="server" RepeatDirection="Horizontal"
                                                                RepeatLayout="Flow" Width="120px">
                                                                <asp:ListItem Selected="True" Text="New" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="Existing" Value="1"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </div>
                                                    </div>
                                                    </div>
                                                </div>
                                            </div>

                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">                                                     

                                                    <div class="form-group">
                                                        <asp:Label ID="lbl4" runat="server" Text="lbl4" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Format No.</asp:Label>
                                                         <div class="col-sm-1">
                                                    <asp:Label ID="lbl1a" runat="server" Text="ES"  Style="width: 22px; float: right;" CssClass="col-sm-1 control-label"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtFormat" runat="server" CssClass="form-control" MaxLength="6" Width="100%" TabIndex="1" ReadOnly="True"></asp:TextBox>
                                                        </div>

                                                        <asp:Label ID="Label2" runat="server" Text="lbl4" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Revision No.</asp:Label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtRev" runat="server" CssClass="form-control" MaxLength="6" Width="100%" TabIndex="2" ReadOnly="True"></asp:TextBox>
                                                        </div>
                                                        </div>

                                                       <div class="form-group">
                                                        <asp:Label ID="Label3" runat="server" Text="lbl4" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True"> Effective Date:</asp:Label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtEffDate" runat="server" CssClass="form-control" MaxLength="20" Width="100%" TabIndex="3" ReadOnly="True"></asp:TextBox>
                                                        </div>
                                                             <asp:Label ID="Label4" runat="server" Text="lbl4" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True"> SDR No.</asp:Label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtsdrno" runat="server" CssClass="form-control" ReadOnly="True" Width="100%" TabIndex="4"></asp:TextBox>
                                                        </div>

                                                    </div>

                                                    <div class="form-group">                                                      
                                                        <asp:Label ID="Label5" runat="server" Text="lbl4" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True"> Date</asp:Label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtdate1" runat="server" CssClass="form-control" ToolTip="Enter Date in dd/mm/yyyy Format" Width="100%" Rows="5"></asp:TextBox>
                                                            <asp:CalendarExtender ID="CE5" runat="server" Enabled="True" TargetControlID="txtdate1" Format="dd/MM/yyyy"></asp:CalendarExtender>
                                                            <asp:MaskedEditExtender ID="MEE5" runat="server" Mask="99/99/9999" MaskType="Date" TargetControlID="txtdate1" />
                                                            <%-- <asp:MaskedEditExtender ID="MEE5" runat="server" Mask="99/99/9999" MaskType="Date" TargetControlID="txtdate1" />
                                    <asp:CalendarExtender ID="CE5" runat="server" Enabled="True" Format="dd/MM/yyyy" PopupButtonID="ImgDate5" TargetControlID="txtdate1">
                                    </asp:CalendarExtender>
                                    <asp:ImageButton ID="ImgDate5" runat="server" ImageUrl="~/images/btn_calendar.gif" />--%>
                                                        </div>                                                 
                                                         <asp:Label ID="Label7" runat="server" Text="lbl4" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Branch:-</asp:Label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtbranch" runat="server" CssClass="form-control" ReadOnly="True" Width="100%" TabIndex="8"></asp:TextBox>
                                                        </div>
                                                        </div>

                                                     <div class="form-group"> 
                                                         <asp:Label ID="Label6" runat="server" Text="lbl4" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Userid</asp:Label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtUserid" CssClass="form-control" runat="server" MaxLength="6" ReadOnly="True" Width="100%" TabIndex="6"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-6">
                                                            <asp:TextBox ID="txtUserName" CssClass="form-control" runat="server" ReadOnly="True" Width="100%" TabIndex="7"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">                                                     
                                                        <asp:Label ID="Lblenqno" Visible="false" runat="server" Text="lbl4" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Enquiry No.</asp:Label>
                                                        <div class="col-sm-2" style="display: none;">
                                                            <asp:TextBox ID="txtenqno" runat="server" CssClass="form-control" ReadOnly="True" Width="70px" TabIndex="9"></asp:TextBox>
                                                        </div>

                                                        <asp:Label ID="Lblenqdt" Visible="false" runat="server" Text="lbl4" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True"> Date: </asp:Label>
                                                        <div class="col-sm-2" style="display: none;">
                                                            <asp:TextBox ID="txtdate" runat="server" CssClass="form-control" ReadOnly="True" Width="80px" TabIndex="10"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    </div>
                                                </div>
                                            </div>

                                          <div class="col-md-6">
                                            <div>
                                                <div class="box-body">

                                                    <div class="form-group">
                                                        <asp:Label ID="Label10" runat="server" Text="lbl4" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Customer_Name:</asp:Label>
                                                        <div class="col-sm-9">
                                                            <asp:TextBox ID="txtcust" runat="server" CssClass="form-control" MaxLength="70" Width="100%" TabIndex="11"></asp:TextBox>
                                                        </div>
                                                        </div>

                                                         <div class="form-group">
                                                        <asp:Label ID="Label11" runat="server" Text="lbl4" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Contact Person:</asp:Label>
                                                        <div class="col-sm-9">
                                                            <asp:TextBox ID="txtcontact" CssClass="form-control" runat="server" MaxLength="50" Width="100%" TabIndex="12"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <asp:Label ID="Label12" runat="server" Text="lbl4" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Address</asp:Label>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtaddr1" runat="server" CssClass="form-control" Width="100%" MaxLength="50" TabIndex="13"></asp:TextBox>
                                                        </div>
                                                            <asp:Label ID="Label13" runat="server" Text="lbl4" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True"> Tel:</asp:Label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txttel" runat="server" ONKEYPRESS="return isNumberKey(event)" CssClass="form-control" Width="100%" MaxLength="20" TabIndex="14"></asp:TextBox>
                                                        </div>  
                                                        </div>

                                                          <div class="form-group">                                                                                                    
                                                        <asp:Label ID="Label8" runat="server" Text="lbl4" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True"> E-Mail:</asp:Label>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtemail" runat="server" CssClass="form-control" Width="100%" MaxLength="50" TabIndex="15"></asp:TextBox>
                                                        </div>
                                                         <asp:Label ID="Label9" runat="server" Text="lbl4" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">City</asp:Label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtaddr2" runat="server" CssClass="form-control" Width="100%" MaxLength="50" TabIndex="16"></asp:TextBox>
                                                        </div>
                                                    </div>    
                                                    
                                                          <div class="form-group" style="display: none;">                                                    
                                                        <asp:Label ID="Label14" runat="server" Text="lbl4" Visible="false" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Fax:</asp:Label>
                                                        <div class="col-sm-2" style="display: none;">
                                                            <asp:TextBox ID="txtfax" runat="server" Height="18px" Width="150px" ONKEYPRESS="return isNumberKey(event)"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group" style="display: none;">
                                                        <asp:Label ID="Label15" runat="server" Text="lbl4" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">State</asp:Label>
                                                        <div class="col-sm-10">
                                                            <asp:TextBox ID="txtaddr3" runat="server" CssClass="form-control" Width="300px" MaxLength="100"></asp:TextBox>
                                                        </div>
                                                    </div>                                       

                                                      </div>  
                                            </div> 
                                         </div>

                                             <div class="col-md-6">
                                            <div>
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <asp:Label ID="Label16" runat="server" Text="lbl4" CssClass="col-sm-12 control-label" Font-Size="14px" Font-Bold="True" Font-Underline="true">PRODUCT DETAILS:</asp:Label>
                                                    </div>

                                                    <div class="form-group">
                                                        <asp:Label ID="Label17" runat="server" Text="lbl4" CssClass="col-sm-7 control-label" Font-Size="14px" Font-Bold="True">Nature_of_Business<span style="font-size:x-small;">(OEM/JOB Coater/Dealer Network)</span></asp:Label>
                                                        <div class="col-sm-5">
                                                            <asp:TextBox ID="txtnature" runat="server" CssClass="form-control" Width="100%" MaxLength="50" TabIndex="17"></asp:TextBox>
                                                        </div>
                                                        </div>

                                                         <div class="form-group">
                                                        <asp:Label ID="Label18" runat="server" Text="lbl4" CssClass="col-sm-7 control-label" Font-Size="14px" Font-Bold="True">Justification_of_Development <span style="font-size:x-small;">(New/Existing)</span></asp:Label>
                                                        <div class="col-sm-5">
                                                            <asp:TextBox ID="txtjusti" runat="server" CssClass="form-control" Width="100%" MaxLength="50" TabIndex="18"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <asp:Label ID="Label19" runat="server" Text="lbl4" CssClass="col-sm-7 control-label" Font-Size="14px" Font-Bold="True">Customer Specification</asp:Label>
                                                        <div class="col-sm-5">
                                                            <asp:RadioButtonList ID="rbspeci" runat="server" CssClass="form-control" RepeatDirection="Horizontal" Height="30px" BackColor="#FFC107" RepeatLayout="Flow" OnSelectedIndexChanged="rbspeci_SelectedIndexChanged" AutoPostBack="true" Width="100%" TabIndex="19">
                                                                <asp:ListItem Text="Not Attached" Value="1" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Text="Attached" Value="0"></asp:ListItem>
                                                            </asp:RadioButtonList>

                                                            <%--  <asp:RadioButtonList ID="rd_done" runat="server" RepeatDirection="Horizontal"  Height="30px"  BackColor="#FFC107" OnSelectedIndexChanged="rd_done_SelectedIndexChanged" AutoPostBack="true">                                    
                                                <asp:ListItem Text="&nbsp;&nbsp;&nbsp;Y &nbsp;&nbsp;&nbsp;" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="&nbsp;&nbsp;&nbsp;N &nbsp;&nbsp;&nbsp;" Value="1"></asp:ListItem>                                    
                                            </asp:RadioButtonList>--%>
                                                        </div>


                                                        <asp:Label ID="Label20" runat="server" Text="lbl4" Visible="false" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Specification</asp:Label>
                                                        <div class="col-sm-9" style="display: none;">
                                                            <asp:RadioButtonList ID="rbcust" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" Height="30px" BackColor="#FFC107" Enabled="False" TabIndex="11">
                                                                <asp:ListItem Text="Available" Value="0" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Text="Not Available" Value="1"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </div>
                                                    </div>

                                                  <%--  <div class="form-group">
                                                         <table>
                                <tr id="attch1" runat="server">
                                    <td>
                                        <asp:FileUpload ID="Attch" runat="server" Visible="true" onchange="submitFile()" /></td>
                                    <td>
                                        <asp:TextBox ID="txtAttch" runat="server" MaxLength="100" placeholder="Path Upto 100 Char" ></asp:TextBox></td>
                                </tr>
                            </table>
                            <asp:Button ID="btnAtt" runat="server" Text="Attachment" OnClick="btnAtt_Click" Width="134px" Style="display: none" />

                            <asp:Label ID="lblShow" runat="server"></asp:Label>
                            <asp:Label ID="lblUpload" runat="server" ></asp:Label>

                            <asp:ImageButton ID="btnView1" runat="server" ImageUrl="~/tej-base/images/preview-file.png" OnClick="btnView1_Click" Visible="false" />
                            <asp:ImageButton ID="btnDown" runat="server" ImageUrl="~/tej-base/images/Save.png" OnClick="btnDown_Click" Visible="false" />
                                                    </div>--%>

                  <%--                                  <div class="form-group" >
                                                        <div class="col-sm-3" id="attch1" runat="server">
                                                            <asp:FileUpload ID="Attch" runat="server" onchange="submitFile()" />
                                                        </div>
                                                           <div class="col-sm-1" id="txtatch" runat="server">
                                                          <asp:TextBox ID="txtAttch" runat="server" MaxLength="100" placeholder="Path Upto 100 Char" ></asp:TextBox>
                                                               </div>
                                                        <div class="col-sm-2">
                                                            <asp:ImageButton ID="btnView1" runat="server" ImageUrl="~/tej-base/images/preview-file.png" OnClick="btnView1_Click" Visible="false" />
                                                            <asp:Button ID="btnAtt" runat="server" Text="Upload Attachment" Visible="false"  OnClick="btnAtt_Click" Width="134px" />
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:Label ID="lblShow" runat="server"></asp:Label>
                                                            <asp:Label ID="lblUpload" runat="server" style="display:none" ></asp:Label>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:Button ID="btnDown" runat="server" Text="Download" OnClick="btnDown_Click" Visible="false" />
                                                        </div>
                                                    </div>--%>


                                                    <div class="form-group" style="display: none;" id="txtatthment1" runat="server">
                                                        <div class="col-sm-12">
                                                            <asp:TextBox ID="txtAllAttachments" runat="server" CssClass="form-control" ReadOnly="true" Width="638px"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                     <div class="form-group">
                                                      <asp:Label ID="lblgloss" runat="server" Text="lbl4" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">GLoss 20*/60*</asp:Label>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtgloss" runat="server" CssClass="form-control" TabIndex="24" Width="100%" MaxLength="50"></asp:TextBox>
                                                        </div>
                                                          <asp:Label ID="lblPre" runat="server" Text="lbl4" CssClass="col-sm-4 control-label" Font-Size="14px" Font-Bold="True">Pre-Treatment_Type</asp:Label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtpre" runat="server" CssClass="form-control" Width="100%" MaxLength="50" TabIndex="28"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    </div>
                                                </div>
                                                 </div>


                                                    <div class="col-md-6">
                                            <div>
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <asp:Label ID="Label21" runat="server" Text="lbl4" CssClass="col-sm-12 control-label" Font-Size="14px" Font-Bold="True" Font-Underline="true">GENERAL REQUIREMENTS :</asp:Label>
                                                    </div>

                                                    <div class="form-group">
                                                        <asp:Label ID="lblProduct" runat="server" Text="lbl4" CssClass="col-sm-7 control-label" Font-Size="14px" Font-Bold="True">Product Typeh<span style="font-size:x-small;">(ST/AD/TSA/2KPU/2U/1K Epoxy/Thinner/Other)</span></asp:Label>
                                                        <div class="col-sm-5">
                                                            <asp:TextBox ID="txtproduct" runat="server" CssClass="form-control" Width="100%" MaxLength="50" TabIndex="20"></asp:TextBox>
                                                        </div>
                                                        </div>

                                                    <div class="form-group">
                                                        <asp:Label ID="lblShade" runat="server" Text="lbl4" CssClass="col-sm-7 control-label" Font-Size="14px" Font-Bold="True">Shade<span style="font-size:x-small;">(Solid/Metallic/Candy/Other)</span></asp:Label>
                                                        <div class="col-sm-5">
                                                            <asp:TextBox ID="txtshade" runat="server" CssClass="form-control" Width="100%" MaxLength="50" TabIndex="21"></asp:TextBox>
                                                        </div>
                                                         </div>

                                                    <div class="form-group">
                                                        <asp:Label ID="Label22" runat="server" Text="lbl4" CssClass="col-sm-7 control-label" Font-Size="14px" Font-Bold="True">Finish<span style="font-size:x-small;">(Glossy/Semi-Glossy/Satin/Matt/Other)</span></asp:Label>
                                                        <div class="col-sm-5">
                                                            <asp:TextBox ID="txtfinish" runat="server" CssClass="form-control" Width="100%" MaxLength="30" TabIndex="22"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group" style="display: none;">
                                                        <asp:Label ID="lblsolid" runat="server" Text="lbl4" CssClass="col-sm-11 control-label" Font-Size="14px" Font-Bold="True">Solid/Metallic/Candy/Other</asp:Label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtsolid" runat="server" CssClass="form-control" TabIndex="23" Width="350px" MaxLength="50"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group"  style="display: none;">
                                                      
                                                        <asp:Label ID="lblsalt" runat="server" Visible="false" Text="lbl4" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Salt Spray Resistance</asp:Label>
                                                        <div class="col-sm-1" style="display: none;">
                                                            <asp:TextBox ID="txtsalt" runat="server" CssClass="form-control" TabIndex="25" Width="100px" MaxLength="50"></asp:TextBox>
                                                        </div>

                                                        <asp:Label ID="lbltest" runat="server" Text="lbl4" Visible="false" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Accelerated Weathering Test</asp:Label>
                                                        <div class="col-sm-1" style="display: none;">
                                                            <asp:TextBox ID="txttest" runat="server" CssClass="form-control" TabIndex="26" Width="150px" MaxLength="50"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <asp:Label ID="Label23" runat="server" Text="lbl4" CssClass="col-sm-7 control-label" Font-Size="14px" Font-Bold="True"> Substrate Type <span style="font-size:x-small;">(MS/GI/SUS/AL/AL.DC/IDC/ABS/PP/Others)</span></asp:Label>
                                                        <div class="col-sm-5">
                                                            <asp:TextBox ID="txtsubs" runat="server" CssClass="form-control" Width="100%" MaxLength="30" TabIndex="27"></asp:TextBox>
                                                        </div>
                                                        </div>

                                                    </div>
                                                </div>
                                                        </div>


                                          <div class="col-md-12" id="img" runat="server">
                                            <div>
                                                <div class="box-body">

                                      <div class="form-group">
                                                         <table>
                                <tr id="attch1" runat="server">
                                    <td>
                                        <asp:FileUpload ID="Attch" runat="server" Visible="true" onchange="submitFile()" /></td>
                                    <td>
                                        <asp:TextBox ID="txtAttch" runat="server" MaxLength="100" placeholder="Path Upto 100 Char" ></asp:TextBox></td>
                                </tr>
                            </table>
                            <asp:Button ID="btnAtt" runat="server" Text="Attachment" OnClick="btnAtt_Click" Width="134px" Style="display: none" />

                            <asp:Label ID="lblShow" runat="server"></asp:Label>
                            <asp:Label ID="lblUpload" runat="server" ></asp:Label>

                            <asp:ImageButton ID="btnView1" runat="server" ImageUrl="~/tej-base/images/preview-file.png" OnClick="btnView1_Click" Visible="false" />
                            <asp:ImageButton ID="btnDown" runat="server" ImageUrl="~/tej-base/images/Save.png" OnClick="btnDown_Click" Visible="false" />
                                                    </div>
                                                    </div>
                                                </div>
                                              </div>
                                                     <div class="col-md-6">
                                            <div>
                                                <div class="box-body">
                                                           <div class="form-group">
                                                       <%-- <asp:Label ID="lblPre" runat="server" Text="lbl4" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Pre-Treatment Type</asp:Label>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtpre" runat="server" CssClass="form-control" Width="100px" MaxLength="50" TabIndex="28"></asp:TextBox>
                                                        </div>--%>
                                                        <asp:Label ID="lblMethod" runat="server" Text="lbl4" CssClass="col-sm-7 control-label" Font-Size="14px" Font-Bold="True">Application Method <span style="font-size:x-small;">(Dip/C-Spray/E-Spray/Auto-Spray/Other)</span></asp:Label>
                                                        <div class="col-sm-5">
                                                            <asp:TextBox ID="txtmethod" runat="server" CssClass="form-control" Width="100%" MaxLength="200" TabIndex="29"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <asp:Label ID="lblthinner" runat="server" Text="lbl4" CssClass="col-sm-7 control-label" Font-Size="14px" Font-Bold="True">Application Viscosity</asp:Label>
                                                        <div class="col-sm-5">
                                                            <asp:TextBox ID="txtthinner" runat="server" CssClass="form-control" Width="100%" MaxLength="50" TabIndex="30"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <asp:Label ID="lblIntake" runat="server" Text="lbl4" CssClass="col-sm-7 control-label" Font-Size="14px" Font-Bold="True">Thinner Intake Online</asp:Label>
                                                        <div class="col-sm-5">
                                                            <asp:TextBox ID="txtIntake" runat="server" CssClass="form-control" Width="100%" MaxLength="50" TabIndex="31"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <asp:Label ID="lblsystem" runat="server" Text="lbl4" CssClass="col-sm-7 control-label" Font-Size="14px" Font-Bold="True">App System Flash off Time B/w Primer and Paint</asp:Label>
                                                        <div class="col-sm-5">
                                                            <asp:TextBox ID="txtsystem" runat="server" CssClass="form-control" Width="100%" MaxLength="50" TabIndex="32"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <asp:Label ID="lblBanking" runat="server" Text="lbl4" CssClass="col-sm-7 control-label" Font-Size="14px" Font-Bold="True">Banking Schedule (Temp and Time) - E.M.T </asp:Label>
                                                        <div class="col-sm-5">
                                                            <asp:TextBox ID="txtBanking" runat="server" CssClass="form-control" Width="100%" MaxLength="50" TabIndex="33"></asp:TextBox>
                                                        </div>
                                                    </div>
                         
                                                    </div>
                                                </div>
                                            </div>

                                                       <div class="col-md-6">
                                            <div>
                                                <div class="box-body">

                                                     <div class="form-group">
                                                        <asp:Label ID="lblSpecific" runat="server" Text="lbl4" CssClass="col-sm-8 control-label" Font-Size="14px" Font-Bold="True">Specific Liquid Properties <span style="font-size:x-small;">(Supply Viscosity,NVM,ER)</span></asp:Label>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtSpecific" runat="server" CssClass="form-control" Width="100%" MaxLength="50" TabIndex="34"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <asp:Label ID="lbldft" runat="server" Text="lbl4" CssClass="col-sm-8 control-label" Font-Size="14px" Font-Bold="True">DFT STD Specified by Cust<span style="font-size:x-small;">(For Composites System, Each Coat)</span></asp:Label>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtdft" runat="server" CssClass="form-control" Width="100%" MaxLength="50" TabIndex="35"></asp:TextBox>
                                                        </div>
                                                        </div>

                                                         <div class="form-group">
                                                        <asp:Label ID="lblsst" runat="server" Text="lbl4" CssClass="col-sm-8 control-label" Font-Size="14px" Font-Bold="True">SST Required pls Specify HRS</asp:Label>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtsst" runat="server" CssClass="form-control" Width="100%" MaxLength="50" TabIndex="36"></asp:TextBox>
                                                        </div>
                                                             </div>

                                                              <div class="form-group" style="display:none;">
                                                        <asp:Label ID="lblhrs" runat="server" Text="lbl4" Visible="false" CssClass="col-sm-9 control-label" Font-Size="14px" Font-Bold="True">Solid Contain</asp:Label>
                                                        <div class="col-sm-3" style="display: none;">
                                                            <asp:TextBox ID="txthrs" runat="server" CssClass="form-control" Width="100%" TabIndex="37"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <asp:Label ID="lblAccelerated" runat="server" Text="lbl4" CssClass="col-sm-8 control-label" Font-Size="14px" Font-Bold="True">Accelerated Weathering Test Required. Pls Specify HRS</asp:Label>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtAccelerated" runat="server" CssClass="form-control" Width="100%" MaxLength="50" TabIndex="38"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <asp:Label ID="lblAny" runat="server" Text="lbl4" CssClass="col-sm-8 control-label" Font-Size="14px" Font-Bold="True">Any Other Potential Test Relevant to Development </asp:Label>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtAny" runat="server" CssClass="form-control" Width="100%" MaxLength="50" TabIndex="39"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group" style="display: none;">
                                                        <asp:Label ID="lblliquid" runat="server" Text="lbl4" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Liquid Properties</asp:Label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtliquid" runat="server" CssClass="form-control" Width="100px" MaxLength="50"></asp:TextBox>
                                                        </div>
                                                        <asp:Label ID="lbldry" runat="server" Text="lbl4" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Dry Film Properties(Short Term Tests &amp; Long Term Tests)</asp:Label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtdry" runat="server" CssClass="form-control" Width="150px" MaxLength="40"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    </div>
                                                </div>
                                             </div>

                                                      <div class="col-md-6">
                                            <div>
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <asp:Label ID="Label24" runat="server" Text="lbl4" CssClass="col-sm-12 control-label" Font-Size="14px" Font-Bold="True" Font-Underline="true"> BUSINESS POTENTIAL:</asp:Label>
                                                        <div class="col-sm-4" style="display:none;">
                                                            <asp:TextBox ID="txtbusin" runat="server" CssClass="form-control" ONKEYPRESS="return isNumberKey(event)" TabIndex="41" Width="100%" MaxLength="50" Visible="false"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <asp:Label ID="Label25" runat="server" Text="lbl4" CssClass="col-sm-12 control-label" Font-Size="14px" Font-Bold="True">Total Business Potential of Customer</asp:Label>
                                                    </div>

                                                    <div class="form-group">
                                                        <asp:Label ID="Label26" runat="server" Text="lbl4" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Volume</asp:Label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtVolume" runat="server" CssClass="form-control" Width="100px" MaxLength="50" TabIndex="42"></asp:TextBox>
                                                        </div>
                                                        <asp:Label ID="Label27" runat="server" Text="lbl4" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True"> Value</asp:Label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtValue" runat="server" CssClass="form-control" Width="150px" MaxLength="50" TabIndex="43"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <asp:Label ID="Label28" runat="server" Text="lbl4" CssClass="col-sm-12 control-label" Font-Size="14px" Font-Bold="True">Business Expected (Volume);</asp:Label>
                                                    </div>


                                                    <div class="form-group">
                                                        <asp:Label ID="Label29" runat="server" Text="lbl4" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Immediate</asp:Label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtimmid" runat="server" CssClass="form-control" Width="100px" MaxLength="50" TabIndex="44"></asp:TextBox>
                                                        </div>
                                                        <asp:Label ID="Label30" runat="server" Text="lbl4" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Long Term</asp:Label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtlong" runat="server" CssClass="form-control" Width="150px" MaxLength="20" TabIndex="45"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <asp:Label ID="Label1" runat="server" Text="lbl4" CssClass="col-sm-12 control-label" Font-Size="14px" Font-Bold="True">Future Business Volume With Other Customer</asp:Label>
                                                    </div>

                                                    <div class="form-group">
                                                        <asp:Label ID="Label31" runat="server" Text="lbl4" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Volume</asp:Label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtFutureVol" runat="server" CssClass="form-control" Width="100px" MaxLength="50" TabIndex="46"></asp:TextBox>
                                                        </div>
                                                        <asp:Label ID="Label32" runat="server" Text="lbl4" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Value</asp:Label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtFutureVal" runat="server" CssClass="form-control" Width="150px" MaxLength="50" TabIndex="47"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    </div>
                                                </div>
                                                          </div>

                                                 <div class="col-md-6">
                                            <div>
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <asp:Label ID="Label33" runat="server" Text="lbl4" CssClass="col-sm-12 control-label" Font-Size="14px" Font-Bold="True">Present Supplier(s),Their Share &amp; Prices:</asp:Label>
                                                    </div>

                                                    <div class="form-group">
                                                        <asp:Label ID="Label34" runat="server" Text="lbl4" CssClass="col-sm-1 control-label" Font-Size="14px" Font-Bold="True"> i)</asp:Label>
                                                        <div class="col-sm-5">
                                                            <asp:TextBox ID="txt1" runat="server" CssClass="form-control" Width="100%" TabIndex="48" MaxLength="100"></asp:TextBox>
                                                        </div>
                                                        <asp:Label ID="Label35" runat="server" Text="lbl4" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Basic Price</asp:Label>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="Textpr1" runat="server" CssClass="form-control" MaxLength="50" ONKEYPRESS="return isNumberKey(event)" TabIndex="49" Width="100%"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <asp:Label ID="Label36" runat="server" Text="lbl4" CssClass="col-sm-1 control-label" Font-Size="14px" Font-Bold="True"> ii)</asp:Label>
                                                        <div class="col-sm-5">
                                                            <asp:TextBox ID="txt11" runat="server" CssClass="form-control" Width="100%" MaxLength="200" TabIndex="50"></asp:TextBox>
                                                        </div>
                                                        <asp:Label ID="Label37" runat="server" Text="lbl4" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Basic Price</asp:Label>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="Textpr2" runat="server" CssClass="form-control" MaxLength="50" ONKEYPRESS="return isNumberKey(event)" TabIndex="51" Width="100%"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <asp:Label ID="lblBasicPrice" runat="server" Text="lbl4" CssClass="col-sm-8 control-label" Font-Size="14px" Font-Bold="True">Basic Price at which Business Can be Obtained :</asp:Label>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtBasicPrice" runat="server" CssClass="form-control" Width="100%" MaxLength="50" TabIndex="52"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <asp:Label ID="lblPymt" runat="server" Text="lbl4" CssClass="col-sm-8 control-label" Font-Size="14px" Font-Bold="True">Payment Terms of Customer</asp:Label>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtPymt" runat="server" CssClass="form-control" Width="100%" MaxLength="50" TabIndex="53"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <asp:Label ID="Label38" runat="server" Text="lbl4" CssClass="col-sm-4 control-label" Font-Size="14px" Font-Bold="True">Competitors_Prod_Sample</asp:Label>

                                                        <div class="col-sm-4">
                                                            <asp:RadioButtonList ID="rbcust0" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow"
                                                                TabIndex="55">
                                                                <asp:ListItem Selected="True" Text="Available" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="Not Available" Value="1"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </div>
                                                        <div class="col-sm-3" style="display: none;">
                                                            <asp:TextBox ID="txtpsample" runat="server" CssClass="form-control" MaxLength="50" TabIndex="54" Width="150px"></asp:TextBox>
                                                        </div>
                                                        <asp:Label ID="Label39" runat="server" Text="lbl4" CssClass="col-sm-1 control-label" Font-Size="14px" Font-Bold="True">Qty</asp:Label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtQty" runat="server" CssClass="form-control" MaxLength="56" ONKEYPRESS="return isNumberKey(event)" TabIndex="56" Width="100%"></asp:TextBox>
                                                        </div>
                                                        </div>                                                        

                                                    <div class="form-group" style="display: none;">
                                                        <asp:Label ID="lblbasic" runat="server" Text="lbl4" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Price At Which Business can be Obtained(Per Kg/Ltr.)</asp:Label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtbasic" runat="server" CssClass="form-control" Width="150px"></asp:TextBox>
                                                        </div>
                                                        <asp:Label ID="lblCust3" runat="server" Text="lbl4" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Basic Price/Selling Price</asp:Label>
                                                        <div class="col-sm-3">
                                                            <asp:RadioButtonList ID="rbcust3" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                                <asp:ListItem Selected="True" Text="Basic " Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="Selling " Value="1"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </div>
                                                    </div>

                                                    </div>
                                                </div>
                                                     </div>

                                                      <div class="col-md-6">
                                            <div>
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <asp:Label ID="Label41" runat="server" Text="lbl4" CssClass="col-sm-12 control-label" Font-Size="14px" Font-Bold="True">Shade/Finish Referance</asp:Label>
                                                    </div>

                                                    <div class="form-group">
                                                        <asp:Label ID="Label42" runat="server" Text="lbl4" CssClass="col-sm-7 control-label" Font-Size="14px" Font-Bold="True">Standard Shade Panel Given & Signed by Cust:</asp:Label>
                                                        <div class="col-sm-5">
                                                            <asp:RadioButtonList ID="rbcust1" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" Width="100%"   Enabled="False" TabIndex="57">
                                                                <asp:ListItem Text="Available" Value="0" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Text="Not Available" Value="1"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <asp:Label ID="Label43" runat="server" Text="lbl4" CssClass="col-sm-7 control-label" Font-Size="14px" Font-Bold="True">Time Frame for Development:</asp:Label>
                                                        <div class="col-sm-5">
                                                            <asp:TextBox ID="txttime" runat="server" CssClass="form-control" Width="100%" MaxLength="15" TabIndex="58"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <asp:Label ID="lblSampleQty" runat="server" Text="lbl4" CssClass="col-sm-7 control-label" Font-Size="14px" Font-Bold="True">Sample Qty Req. For Trail <span style="font-size:x-small;">(Standard 1- Ltr.)for Extra Qty of Sample Pls Specify</span></asp:Label>
                                                        <div class="col-sm-5">
                                                            <asp:TextBox ID="txtSampleQty" runat="server" CssClass="form-control" Width="100%" MaxLength="50" TabIndex="59"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <asp:Label ID="Label44" runat="server" Text="lbl4" CssClass="col-sm-7 control-label" Font-Size="14px" Font-Bold="True">Suggest Final Name of the Product</asp:Label>
                                                        <div class="col-sm-5">
                                                            <asp:TextBox ID="txtSuggest" runat="server" CssClass="form-control" Width="100%" MaxLength="50" TabIndex="60"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    </div>
                                                </div>
                                              </div>

                                                     <div class="col-md-6">
                                            <div>
                                                <div class="box-body">

                                                    <div class="form-group">
                                                        <asp:Label ID="Label45" runat="server" Text="lbl4" CssClass="col-sm-7 control-label" Font-Size="14px" Font-Bold="True"> Additional Information(If Any):</asp:Label>
                                                        <div class="col-sm-5">
                                                            <asp:TextBox ID="txtaddition" runat="server" CssClass="form-control" Width="100%" MaxLength="100" TabIndex="61"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <asp:Label ID="Label46" runat="server" Text="lbl4" CssClass="col-sm-7 control-label" Font-Size="14px" Font-Bold="True">Requested By BDM Name</asp:Label>
                                                        <div class="col-sm-5">
                                                            <asp:TextBox ID="txtreques" runat="server" CssClass="form-control" Width="100%" MaxLength="50" TabIndex="62"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group" style="display: none">
                                                        <div class="col-sm-6">
                                                            <asp:TextBox ID="txtprodname" runat="server" CssClass="form-control" Width="100%" MaxLength="50" TabIndex="63"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-6">
                                                            <asp:Button ID="btnstaus" runat="server" CssClass="form-control" OnClick="btnstaus_Click" Style="float: right;" Text="!" Width="17px" Visible="false" />
                                                        </div>
                                                        <div class="col-sm-6">
                                                            <asp:TextBox ID="txtstatus" runat="server" CssClass="form-control" Width="100%" MaxLength="50" TabIndex="64"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <asp:Label ID="Label47" runat="server" Text="lbl4" CssClass="col-sm-7 control-label" Font-Size="14px" Font-Bold="True" >Approved By <span style="font-size:x-small;">(Marketing Head Name)</span></asp:Label>
                                                        <div class="col-sm-5">
                                                            <asp:TextBox ID="txtrecomm" runat="server" CssClass="form-control" MaxLength="25" TabIndex="65" Width="100%"></asp:TextBox>
                                                        </div>
                                                        </div>

                                                           <div class="form-group">
                                                        <asp:Label ID="Label48" runat="server" Text="lbl4" CssClass="col-sm-7 control-label" Font-Size="14px" Font-Bold="True" > Remarks for Refusal</asp:Label>
                                                        <div class="col-sm-5">
                                                            <asp:TextBox ID="txtRefusal1" runat="server" CssClass="form-control" Width="100%" MaxLength="50" TabIndex="66"></asp:TextBox>
                                                        </div>
                                                    </div>


                                                </div>

                                            </div>
                                        </div>
                                    </div>                                
                                </div>

                                <div role="tabpanel" class="tab-pane active" id="DescTab1">
                                    <div class="lbBody" style="height: 500px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <div class="col-md-12">
                                            <div>
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <asp:Label ID="Label40" runat="server" Text="lbl4" BackColor="LightGray" CssClass="col-sm-12 control-label" Font-Size="18px" Font-Bold="True" style="text-align:center;"> SDR Page 2 : Feasibility Assessment</asp:Label>
                                                    </div>
                                                    </div>
                                                </div>
                                            </div>


                                              <div class="col-md-6">
                                            <div>
                                                <div class="box-body">

                                                    <div class="form-group">
                                                        <asp:Label ID="Label49" runat="server" Text="lbl4" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Doc No.</asp:Label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtFeasDoc" runat="server" CssClass="form-control" MaxLength="6" Width="100%" TabIndex="1" ReadOnly="True"></asp:TextBox>
                                                        </div>
                                                        <asp:Label ID="Label50" runat="server" Text="lbl4" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Revision No.</asp:Label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtFeasRev" runat="server" CssClass="form-control" MaxLength="6" Width="100%" TabIndex="2" ReadOnly="True"></asp:TextBox>
                                                        </div>
                                                        </div>

                                                           <div class="form-group">
                                                        <asp:Label ID="Label52" runat="server" Text="lbl4" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Draft No.</asp:Label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtDraft" runat="server" CssClass="form-control" ReadOnly="True" Width="100%" TabIndex="4"></asp:TextBox>
                                                        </div>
                                                        <asp:Label ID="Label53" runat="server" Text="lbl4" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True"> Draft Date</asp:Label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtDraftDt" runat="server" CssClass="form-control" ToolTip="Enter Date in dd/mm/yyyy Format" Width="100%" MaxLength="5"></asp:TextBox>                                                           
                                                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtDraftDt" Format="dd/MM/yyyy">
                                                            </asp:CalendarExtender>
                                                            <asp:MaskedEditExtender ID="MaskedEditExtender1" runat="server" Mask="99/99/9999" MaskType="Date" TargetControlID="txtDraftDt" />
                                                        </div>
                                                    </div>

                                                    </div>
                                                </div>
                                                  </div>


                                                    <div class="col-md-6">
                                            <div>
                                                <div class="box-body">

                                                      <div class="form-group">
                                                        <asp:Label ID="Label51" runat="server" Text="lbl4" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True"> W.E.F</asp:Label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtFeasDate" runat="server" CssClass="form-control" MaxLength="20" Width="100%" TabIndex="3" ReadOnly="True"></asp:TextBox>
                                                        </div>
                                                            <asp:Label ID="Label54" runat="server" Text="lbl4" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Development No.</asp:Label>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtSDR2" runat="server" ReadOnly="true" Width="100%" CssClass="form-control" TabIndex="6"></asp:TextBox>
                                                        </div>
                                                    </div>

  
                                                    <div class="form-group">                                                      
                                                        <asp:Label ID="Label55" runat="server" Text="lbl4" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True"> Date</asp:Label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtSDR2Date" runat="server" ReadOnly="true" CssClass="form-control" Width="100%" MaxLength="7"></asp:TextBox>
                                                        </div>
                                                   
                                                        <asp:Label ID="Label56" runat="server" Text="lbl4" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True"> Customer</asp:Label>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtCust2" runat="server" ReadOnly="true" Width="100%" CssClass="form-control" Rows="8"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group" style="display: none;">
                                                        <asp:Label ID="Label57" runat="server" Text="lbl4" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Is the design a new concept to the company?</asp:Label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtprod" runat="server" CssClass="form-control" Width="480px" MaxLength="60" TabIndex="9"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group" style="display: none;">
                                                        <asp:Label ID="Label58" runat="server" Text="lbl4" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True"> Are new product used in this product?</asp:Label>

                                                        <div class="col-sm-3">
                                                            <asp:RadioButtonList ID="rbminor" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                                <asp:ListItem Selected="True" Text="Yes" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="No" Value="1"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </div>
                                                        <asp:Label ID="Label59" runat="server" Text="lbl4" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True" Font-Underline="true"> Are new types of tooling needed for producing this product ?</asp:Label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtprodcode" runat="server" CssClass="form-control" Width="100px" MaxLength="50"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group" style="display: none;">
                                                        <asp:Label ID="Label60" runat="server" Text="lbl4" CssClass="col-sm-12 control-label" Font-Size="14px" Font-Bold="True"> New Product</asp:Label>
                                                    </div>

                                                    <div class="form-group" style="display: none;">
                                                        <asp:Label ID="Label61" runat="server" Text="lbl4" CssClass="col-sm-6 control-label" Font-Size="14px" Font-Bold="True">Are all special charcteristics identified in the specification?</asp:Label>
                                                        <div class="col-sm-6">
                                                            <asp:TextBox ID="txtdpperiod" runat="server" CssClass="form-control" Width="480px" MaxLength="100"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group" style="display: none;">
                                                        <asp:Label ID="Label62" runat="server" Text="lbl4" CssClass="col-sm-6 control-label" Font-Size="14px" Font-Bold="True">Are any standards/test specification required from customer ? Following are some standards: production Validation,Continous Confermance,Reliability testing,Material testing, Process Standards,Performance Standards,Appearance Standards,Lab Procedures,Goverment Standards</asp:Label>
                                                        <div class="col-sm-6">
                                                            <asp:TextBox ID="txtdpcost" runat="server" CssClass="form-control" Width="480px" MaxLength="80"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group" style="display: none;">
                                                        <asp:Label ID="Label63" runat="server" Text="lbl4" CssClass="col-sm-6 control-label" Font-Size="14px" Font-Bold="True">Are additional resources (both people &amp; equipments) needed for this design or additional training required ?</asp:Label>
                                                        <div class="col-sm-6">
                                                            <asp:TextBox ID="txtsample" runat="server" CssClass="form-control" Width="480px" MaxLength="80"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group" style="display: none;">
                                                        <asp:Label ID="Label64" runat="server" Text="lbl4" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Do we need to develop special test equipments? Do we require customer approval for these equipments ?</asp:Label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtRegular" runat="server" Height="18px" Width="480px" MaxLength="50"></asp:TextBox>
                                                        </div>
                                                        <asp:Label ID="Label65" runat="server" Text="lbl4" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Are design changes pending from customer at the customer end ?</asp:Label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtCostSheet" runat="server" CssClass="form-control" Width="480px" MaxLength="50"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group" style="display: none;">
                                                        <asp:Label ID="Label66" runat="server" Text="lbl4" CssClass="col-sm-6 control-label" Font-Size="14px" Font-Bold="True"> Is first sample needed ?</asp:Label>
                                                        <div class="col-sm-6">
                                                            <asp:TextBox ID="txtSampleSize" runat="server" CssClass="form-control" Width="480px" MaxLength="50"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group" style="display: none;">
                                                        <asp:Label ID="Label67" runat="server" Text="lbl4" CssClass="col-sm-12 control-label" Font-Size="14px" Font-Bold="True">Additional Resources Needed</asp:Label>
                                                    </div>

                                                    <div class="form-group" style="display: none;">
                                                        <asp:Label ID="Label68" runat="server" Text="lbl4" CssClass="col-sm-6 control-label" Font-Size="14px" Font-Bold="True">Any other specific requirements ?</asp:Label>
                                                        <div class="col-sm-6">
                                                            <asp:TextBox ID="txtfordp" runat="server" CssClass="form-control" Width="480px" MaxLength="80"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group" style="display: none;">
                                                        <asp:Label ID="Label69" runat="server" Text="lbl4" CssClass="col-sm-6 control-label" Font-Size="14px" Font-Bold="True">Have we faced quality /warrenty problems in similar product ?</asp:Label>
                                                        <div class="col-sm-6">
                                                            <asp:TextBox ID="txtforreg" runat="server" CssClass="form-control" vWidth="480px" MaxLength="15"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group" style="display: none;">
                                                        <asp:Label ID="Label70" runat="server" Text="lbl4" CssClass="col-sm-6 control-label" Font-Size="14px" Font-Bold="True">Is the product sensitive to customer`s plant process or assembly ?</asp:Label>
                                                        <div class="col-sm-6">
                                                            <asp:TextBox ID="txtfortest" runat="server" CssClass="form-control" Width="480px" MaxLength="15"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group" style="display: none;">
                                                        <asp:Label ID="Label71" runat="server" Text="lbl4" CssClass="col-sm-6 control-label" Font-Size="14px" Font-Bold="True">Are unique in process &amp; final inspection required ? Can we develop these?</asp:Label>
                                                        <div class="col-sm-6">
                                                            <asp:TextBox ID="txtEstimated" runat="server" CssClass="form-control" Width="480px" MaxLength="50"></asp:TextBox>
                                                        </div>
                                                    </div>


                                                    <div class="form-group" style="display: none;">
                                                        <asp:Label ID="Label72" runat="server" Text="lbl4" CssClass="col-sm-6 control-label" Font-Size="14px" Font-Bold="True">Will any new or existing processes or capacity enhancement required to reach current customer requirement ?</asp:Label>
                                                        <div class="col-sm-6">
                                                            <asp:TextBox ID="txtremark" runat="server" CssClass="form-control" Width="480px" MaxLength="100"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    </div>
                                                </div>
                                                        </div>

                                                     <div class="col-md-12">
                                            <div>
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <asp:GridView ID="sg1" runat="server" Width="100%" AutoGenerateColumns="False" OnRowCommand="sg1_RowCommand" OnRowDataBound="sg1_RowDataBound" Style="font-size: smaller;" Height="200px">
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        A
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="btnadd" runat="server" CommandName="Add" ImageAlign="Middle"
                                                                            ImageUrl="~/images/Btn_addn.png" Width="20px" ToolTip="Insert Item" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="11px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        D
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="btnrmv" runat="server" CommandName="Rmv" ImageUrl="~/images/Btn_remn.png"
                                                                            Width="20px" ImageAlign="Middle" ToolTip="Remove Item" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="11px" />
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="srno" HeaderText="Srno" ReadOnly="True">
                                                                    <HeaderStyle Width="100px" />
                                                                    <ItemStyle Width="100px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Code" HeaderText="Code" ReadOnly="True">
                                                                    <HeaderStyle Width="100px" />
                                                                    <ItemStyle Width="100px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="name" HeaderText="Name" ReadOnly="True">
                                                                    <HeaderStyle Width="3000px" />
                                                                    <ItemStyle Width="3000px" />
                                                                </asp:BoundField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        Yes / No
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtYes" runat="server" Text='<%#Eval("yes") %>' oncontextmenu="return false;" Style="text-align: left" MaxLength="6" Width="200px"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        Remarks
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtRemarks" runat="server" Text='<%#Eval("Remarks") %>' oncontextmenu="return false;" Style="text-align: left" MaxLength="50" Width="500px"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <HeaderStyle BackColor="#1797c0" ForeColor="White" Height="20px" CssClass="GridviewScrollHeader"
                                                                Font-Bold="True" />
                                                        </asp:GridView>
                                                    </div>
                                                    </div>
                                                </div>
                                                         </div>

                                                     <div class="col-md-12">
                                            <div>
                                                <div class="box-body">

                                                    <div class="form-group" style="display: none;">
                                                        <asp:Label ID="Label73" runat="server" Text="lbl4" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True" Font-Underline="true">Head R&amp;D</asp:Label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txthead" runat="server" CssClass="form-control" Width="100px" MaxLength="15"></asp:TextBox>
                                                        </div>
                                                        <asp:Label ID="Label74" runat="server" Text="lbl4" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True" Font-Underline="true">Sig/Date</asp:Label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtsign" runat="server" CssClass="form-control" ToolTip="Enter Date in dd/mm/yyyy Format" Width="70px"></asp:TextBox>
                                                            <asp:CalendarExtender ID="CE1" runat="server" Enabled="True" TargetControlID="txtsign" Format="dd/MM/yyyy">
                                                            </asp:CalendarExtender>
                                                            <asp:MaskedEditExtender ID="MEE1" runat="server" Mask="99/99/9999" MaskType="Date" TargetControlID="txtsign" />
                                                            <%-- <asp:CalendarExtender ID="CE1" runat="server" Enabled="True" Format="dd/MM/yyyy" PopupButtonID="ImgDate1" TargetControlID="txtsign">
                                    </asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="MEE1" runat="server" Mask="99/99/9999" MaskType="Date" TargetControlID="txtsign" />
                                    <asp:ImageButton ID="ImgDate1" runat="server" ImageUrl="~/images/btn_calendar.gif" />--%>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <asp:Label ID="Label75" runat="server" Text="lbl4" CssClass="col-sm-12 control-label" Font-Size="14px" Font-Bold="True">Conclusions</asp:Label>
                                                    </div>
                                                    </div>
                                                </div>
                                                         </div>


                                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <asp:Label ID="Label76" runat="server" Text="lbl4" CssClass="col-sm-12 control-label" Font-Size="14px" Font-Bold="True"> 1. CFT Members:</asp:Label>
                                                    </div>

                                                    <div class="form-group">
                                                        <asp:Label ID="Label90" runat="server" CssClass="col-sm-6 control-label" Font-Size="14px" Font-Bold="True"></asp:Label>
                                                        <div class="col-sm-6">
                                                            <asp:TextBox ID="txti" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <asp:Label ID="Label92" runat="server" CssClass="col-sm-6 control-label" Font-Size="14px" Font-Bold="True"></asp:Label>
                                                        <div class="col-sm-6">
                                                            <asp:TextBox ID="txtii" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                                                        </div>
                                                      <%--  <asp:Label ID="Label93" runat="server" CssClass="col-sm-4 control-label" Font-Size="14px" Font-Bold="True"></asp:Label>--%>


                                                        <asp:Label ID="Label77" runat="server" Text="lbl4" CssClass="col-sm-12 control-label" Visible="false" Font-Size="14px" Font-Bold="True"> Approval of Additional Capital Cost</asp:Label>
                                                        <div class="col-sm-3" style="display: none;">
                                                            <asp:TextBox ID="txtAppAdd" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    </div>
                                                </div>
                                                         </div>

                                                       <div class="col-md-6">
                                            <div>
                                                <div class="box-body">

                                                    <div class="form-group">
                                                        <asp:Label ID="Label78" runat="server" Text="lbl4" CssClass="col-sm-12 control-label" Font-Size="14px" Font-Bold="True">1. Project Feasible:</asp:Label>
                                                    </div>

                                                    <div class="form-group">
                                                        <asp:Label ID="Label85" runat="server" CssClass="col-sm-6 control-label" Font-Size="14px" Font-Bold="True"></asp:Label>
                                                        <div class="col-sm-6">
                                                            <asp:TextBox ID="txtiii" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                                                        </div>

                                                        <asp:Label ID="Label79" runat="server" Text="lbl4" CssClass="col-sm-2 control-label" Visible="false" Font-Size="14px" Font-Bold="True">M.D.Signature Date</asp:Label>
                                                        <div class="col-sm-3" style="display: none;">
                                                            <asp:TextBox ID="txtmddate" runat="server" CssClass="form-control" Width="100%" MaxLength="20"></asp:TextBox>
                                                            <asp:CalendarExtender ID="txtvchdate_CalendarExtender" runat="server" Enabled="True" TargetControlID="txtmddate" Format="dd/MM/yyyy"></asp:CalendarExtender>
                                                            <asp:MaskedEditExtender ID="Maskedit1" runat="server" Mask="99/99/9999" MaskType="Date" TargetControlID="txtmddate" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <asp:Label ID="Label87" runat="server" CssClass="col-sm-6 control-label" Font-Size="14px" Font-Bold="True"></asp:Label>
                                                        <div class="col-sm-6">
                                                            <asp:TextBox ID="txtiiii" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    </div>
                                                </div>
                                                           </div>


                                                      <div class="col-md-6">
                                            <div>
                                                <div class="box-body">

                                                    <div class="form-group">
                                                        <asp:Label ID="Label80" runat="server" Text="lbl4" CssClass="col-sm-6 control-label" Font-Size="14px" Font-Bold="True">Condition for Acceptance /Rejection</asp:Label>
                                                        <div class="col-sm-6">
                                                            <asp:TextBox ID="txtfeedback" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                                                        </div>

                                                    </div>

                                                    <div class="form-group">
                                                        <asp:Label ID="Label81" runat="server" Text="lbl4" CssClass="col-sm-6 control-label" Font-Size="14px" Font-Bold="True">3 Costing / Quotation Required</asp:Label>
                                                        <div class="col-sm-6">
                                                            <asp:TextBox ID="txtclosed" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <asp:Label ID="Label86" runat="server" CssClass="col-sm-6 control-label" Font-Size="14px" Font-Bold="True"></asp:Label>
                                                        <div class="col-sm-6">
                                                            <asp:TextBox ID="txtclosed2" runat="server" CssClass="form-control" Width="100%" MaxLength="15"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    </div>
                                                </div>
                                                    </div>


                                                       <div class="col-md-6">
                                            <div>
                                                <div class="box-body">

                                                    <div class="form-group">
                                                        <asp:Label ID="Label82" runat="server" Text="lbl4" CssClass="col-sm-6 control-label" Font-Size="14px" Font-Bold="True">Approval From R&amp;D Head Date &amp; Time</asp:Label>
                                                        <div class="col-sm-6">
                                                            <asp:TextBox ID="txtApproval" runat="server" CssClass="form-control" Width="100%" MaxLength="50" TabIndex="67"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <asp:Label ID="Label83" runat="server" Text="lbl4" CssClass="col-sm-6 control-label" Font-Size="14px" Font-Bold="True"> Remarks for Refusal</asp:Label>
                                                        <div class="col-sm-6">
                                                            <asp:TextBox ID="txtRefusal2" runat="server" CssClass="form-control" MaxLength="50" TabIndex="68" Width="100%"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group" style="display: none;">
                                                        <asp:Label ID="Label84" runat="server" Text="lbl4" CssClass="col-sm-6 control-label" Font-Size="14px" Font-Bold="True">Head Marketing SIgn/Date</asp:Label>
                                                        <div class="col-sm-6">
                                                            <asp:TextBox ID="txthmdate" runat="server" Height="18px" Width="100px" ToolTip="Enter Date in dd/mm/yyyy Format" MaxLength="20"></asp:TextBox>
                                                            <asp:CalendarExtender ID="CE2" runat="server" Enabled="True" Format="dd/MM/yyyy" PopupButtonID="ImgDate2" TargetControlID="txthmdate"></asp:CalendarExtender>
                                                            <asp:MaskedEditExtender ID="MEE2" runat="server" Mask="99/99/9999" MaskType="Date" TargetControlID="txthmdate" />                                                          
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

    <table>
        <tr>
            <td>
                <asp:MultiView ID="MultiView1" runat="server">
                    <asp:View ID="View1" runat="server">
                        <table align="center" style="border-color: #000080; width: 98%; height: 333px; font-family: Arial; font-size: small;"
                            frame="box">
                            <%-- <tr>
                                <td align="center" colspan="10" style="background-image: url('images/icons_bg.gif');
                                    font-weight: bold; font-size: medium;">
                                    SDR &nbsp; Page 1 : Shade / Special Product Devlopment Request
                                </td>
                            </tr>--%>
                            <%--  <tr>
                                <td colspan="4">
                                    <asp:Label ID="lblhead" runat="server" Font-Bold="True" ForeColor="#CC0000" Text="lbl"></asp:Label>
                                </td>
                                <td colspan="3" style="display:none">
                                    <asp:Label ID="lblbrh" runat="server" Text="BRH.SDR No."></asp:Label>
                                </td>
                                <td colspan="2" style="display:none">
                                    <asp:Label ID="lblbrno" runat="server" Text="F/1006"></asp:Label>
                                </td>
                                <td style="display: none">
                                    Development&nbsp;<asp:RadioButtonList ID="rbdeve" runat="server" RepeatDirection="Horizontal"
                                        RepeatLayout="Flow" Width="120px">
                                        <asp:ListItem Selected="True" Text="New" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Existing" Value="1"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>--%>
                            <%--     <tr>
                                <td>
                                    Format No.
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtFormat" runat="server" Height="18px" MaxLength="6" Width="100px"
                                        TabIndex="1" ReadOnly="True"></asp:TextBox>
                                </td>
                                <td>
                                    Revision No.
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtRev" runat="server" Height="18px" MaxLength="6" Width="70px"
                                        TabIndex="2" ReadOnly="True"></asp:TextBox>
                                </td>
                                <td colspan="2">
                                    Effective Date:-&nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEffDate" runat="server" Height="18px" MaxLength="20" Width="80px"
                                        TabIndex="3" ReadOnly="True"></asp:TextBox>
                                    &nbsp;
                                </td>
                            </tr>--%>

                            <%--  <tr>
                                <td>
                                    <asp:Label ID="lblSdr" runat="server" Text="SDR No."></asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtsdrno" runat="server" Height="18px" ReadOnly="True" Width="100px" TabIndex="4"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="lblSdrDate" runat="server" Text="Date"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtdate1" runat="server" Height="18px" ToolTip="Enter Date in dd/mm/yyyy Format"
                                        Width="70px" Rows="5"></asp:TextBox>
                                    <asp:MaskedEditExtender ID="MEE5" runat="server" Mask="99/99/9999" MaskType="Date"
                                        TargetControlID="txtdate1" />
                                    <asp:CalendarExtender ID="CE5" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                        PopupButtonID="ImgDate5" TargetControlID="txtdate1">
                                    </asp:CalendarExtender>
                                    <asp:ImageButton ID="ImgDate5" runat="server" ImageUrl="~/images/btn_calendar.gif" />
                                </td>
                                 <td colspan="2">Userid</td>
                                 <td>
                                     <asp:TextBox ID="txtUserid" runat="server" MaxLength="6" ReadOnly="True" Width="70px" TabIndex="6"></asp:TextBox>
                                     <asp:TextBox ID="txtUserName" runat="server" ReadOnly="True" Width="200px" TabIndex="7"></asp:TextBox>
                                 </td>
                                 <td>
                                     &nbsp;</td>
                                 <td>&nbsp;</td>
                            </tr>--%>
                            <%--         <tr>
                                <td>
                                    Branch:-
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtbranch" runat="server" Height="18px" ReadOnly="True" Width="100px" TabIndex="8"></asp:TextBox>
                                </td>
                                <td style="display:none">
                                    Enquiry No.
                                </td>
                                <td colspan="3" style="display:none">
                                    <asp:TextBox ID="txtenqno" runat="server" Height="18px" ReadOnly="True" Width="70px" TabIndex="9"></asp:TextBox>
                                </td>
                                <td colspan="2" style="display:none">
                                    Date:-&nbsp;
                                </td>
                                <td style="display:none">
                                    <asp:TextBox ID="txtdate" runat="server" Height="18px" ReadOnly="True" Width="80px" TabIndex="10"></asp:TextBox>
                                    &nbsp;
                                </td>
                            </tr>--%>
                            <%--         <tr>
                                <td bgcolor="#F1F1ED">
                                    Address
                                </td>
                                <td bgcolor="#F1F1ED" colspan="6">
                                    <asp:TextBox ID="txtaddr1" runat="server" Height="18px" Width="300px" MaxLength="50"
                                        TabIndex="13"></asp:TextBox>
                                </td>
                                <td bgcolor="#F1F1ED" colspan="2">
                                    Tel:
                                </td>
                                <td bgcolor="#F1F1ED">
                                    <asp:TextBox ID="txttel" runat="server" Height="18px" ONKEYPRESS="return isNumberKey(event)"
                                        Width="150px" MaxLength="20" TabIndex="14"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <%--       <tr>
                                <td bgcolor="#F1F1ED">
                                    E-Mail:
                                </td>
                                <td bgcolor="#F1F1ED">
                                    <asp:TextBox ID="txtemail" runat="server" Height="18px" Width="300px" MaxLength="50"
                                        TabIndex="15"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <%-- <tr>
                                <td bgcolor="#F1F1ED">
                                    City
                                </td>
                                <td colspan="6" bgcolor="#F1F1ED">
                                    <asp:TextBox ID="txtaddr2" runat="server" Height="18px" Width="300px" MaxLength="50" TabIndex="16"></asp:TextBox>
                                </td>
                                <td bgcolor="#F1F1ED" colspan="2" style="display:none">
                                    Fax:&nbsp;
                                </td>
                                <td bgcolor="#F1F1ED" style="display:none">
                                    <asp:TextBox ID="txtfax" runat="server" Height="18px" Width="150px" ONKEYPRESS="return isNumberKey(event)"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <%--       <tr style="display: none">
                                <td bgcolor="#F1F1ED">
                                    State
                                </td>
                                <td colspan="6" bgcolor="#F1F1ED">
                                    <asp:TextBox ID="txtaddr3" runat="server" Height="18px" Width="300px" MaxLength="100"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <%--<tr>
                                <td colspan="10" style="font-weight: bold; text-decoration: underline;">
                                    PRODUCT DETAILS:
                                </td>
                            </tr>--%>
                            <%--          <tr>
                                <td bgcolor="#F1F1ED">
                                    Nature of Business (OEM/JOB Coater/Dealer Network)
                                </td>
                                <td colspan="6" bgcolor="#F1F1ED">
                                    <asp:TextBox ID="txtnature" runat="server" Height="18px" Width="300px" MaxLength="50"
                                        TabIndex="17"></asp:TextBox>
                                </td>
                                <td bgcolor="#F1F1ED" colspan="2">
                                    Justification of Development (New/Existing)
                                </td>
                                <td bgcolor="#F1F1ED">
                                    <asp:TextBox ID="txtjusti" runat="server" Height="18px" Width="150px" MaxLength="50"
                                        TabIndex="18"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <%--  <tr>
                                <td bgcolor="#F1F1ED">
                                    Customer Specification
                                </td>
                                <td colspan="6" bgcolor="#F1F1ED">
                                    <asp:RadioButtonList ID="rbspeci" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" OnSelectedIndexChanged="rbspeci_SelectedIndexChanged" AutoPostBack="true"
                                        Width="170px" TabIndex="19">
                                      <asp:ListItem Text="Not Attached" Value="1"  Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Attached" Value="0" ></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td bgcolor="#F1F1ED" colspan="2" style="display: none">
                                    Specification
                                </td>
                                <td bgcolor="#F1F1ED" style="display: none">
                                    <asp:RadioButtonList ID="rbcust" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow"
                                        Enabled="False" TabIndex="11">
                                        <asp:ListItem Text="Available" Value="0" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Not Available" Value="1"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>--%>
                            <%--   <tr>
                                <td> <asp:FileUpload ID="Attch" runat="server" Visible="false" onchange="submitFile()"/></td>
                                <td><asp:Button ID="btnAtt" runat="server" text="Upload Attachment" Style="display:none"  OnClick="btnAtt_Click" Width="134px"/></td>
                                <td><asp:Label ID="lblShow" runat="server" ></asp:Label> <asp:Label ID="lblUpload" runat="server" style="display:none"></asp:Label></td>
                                <td><asp:Button ID="btnDown" runat="server" Text="Download" OnClick="btnDown_Click" Visible="false" /></td>
                            </tr>
                            <tr>
                                <td colspan="3"><asp:TextBox ID="txtAllAttachments" runat="server" ReadOnly="true" Visible="False" Width="638px" ></asp:TextBox></td>
                            </tr>--%>
                            <%--<tr>
                                <td colspan="10" style="font-weight: bold; text-decoration: underline;">
                                GENERAL REQUIREMENTS :
                                    </td>
                            </tr>--%>
                            <%--     <tr>
                                <td bgcolor="#F1F1ED">
                                    <asp:Label ID="lblProduct" runat="server" Text="Product Type (ST /AD/TSA/2KPU/2U/1K Epoxy  /Thinner/ Other)"></asp:Label>
                                </td>
                                <td bgcolor="#F1F1ED" colspan="2">
                                    <asp:TextBox ID="txtproduct" runat="server" Height="18px" Width="100px" MaxLength="50"
                                        TabIndex="20"></asp:TextBox>
                                </td>
                                <td bgcolor="#F1F1ED">
                                    <asp:Label ID="lblShade" runat="server" Text="Shade (Solid/ Metallic/Candy/ Other)"></asp:Label>
                                </td>
                                <td bgcolor="#F1F1ED" colspan="3">
                                    <asp:TextBox ID="txtshade" runat="server" Height="18px" Width="100px" MaxLength="50"
                                        TabIndex="21"></asp:TextBox>
                                </td>
                                <td bgcolor="#F1F1ED" colspan="2">
                                    Finish (Glossy/ Semi-Glossy/ Satin/ Matt/ Other)
                                </td>
                                <td bgcolor="#F1F1ED">
                                    <asp:TextBox ID="txtfinish" runat="server" Height="18px" Width="150px" MaxLength="30"
                                        TabIndex="22"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <%--      <tr>
                                <td bgcolor="#F1F1ED" colspan="3" style="display: none">
                                    <asp:Label ID="lblsolid" runat="server" Text="Solid/Metallic/Candy/Other"></asp:Label>
                                </td>
                                <td bgcolor="#F1F1ED" colspan="7" style="display: none">
                                    <asp:TextBox ID="txtsolid" runat="server" Height="18px" TabIndex="23" Width="350px"
                                        MaxLength="50"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <%--       <tr>
                                <td bgcolor="#F1F1ED">
                                    <asp:Label ID="lblgloss" runat="server" Text="GLoss 20*/60*"></asp:Label>
                                </td>
                                <td bgcolor="#F1F1ED" colspan="2">
                                    <asp:TextBox ID="txtgloss" runat="server" Height="18px" TabIndex="24" Width="100px"
                                        MaxLength="50"></asp:TextBox>
                                </td>
                                <td bgcolor="#F1F1ED" style="display: none">
                                    <asp:Label ID="lblsalt" runat="server" Text="Salt Spray Resistance"></asp:Label>
                                </td>
                                <td bgcolor="#F1F1ED" colspan="3" style="display: none">
                                    <asp:TextBox ID="txtsalt" runat="server" Height="18px" TabIndex="25" Width="100px" MaxLength="50"></asp:TextBox>
                                </td>
                                <td bgcolor="#F1F1ED" colspan="2" style="display: none">
                                    <asp:Label ID="lbltest" runat="server" Text="Accelerated Weathering Test"></asp:Label>
                                </td>
                                <td bgcolor="#F1F1ED" style="display: none">
                                    <asp:TextBox ID="txttest" runat="server" Height="18px" TabIndex="26" Width="150px" MaxLength="50"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <%--  <tr>
                                <td bgcolor="#F1F1ED">
                                    Substrate Type (MS/GI/SUS/AL/AL.DC/IDC/ABS/PP/Others)
                                </td>
                                <td bgcolor="#F1F1ED" colspan="2">
                                    <asp:TextBox ID="txtsubs" runat="server" Height="18px" Width="100px" MaxLength="30"
                                        TabIndex="27"></asp:TextBox>
                                </td>
                                <td bgcolor="#F1F1ED">
                                    <asp:Label ID="lblPre" runat="server" Text="Pre-Treatment Type"></asp:Label>
                                </td>
                                <td colspan="3" bgcolor="#F1F1ED">
                                    <asp:TextBox ID="txtpre" runat="server" Height="18px" Width="100px" MaxLength="50"
                                        TabIndex="28"></asp:TextBox>
                                </td>
                                <td bgcolor="#F1F1ED" colspan="2">
                                    <asp:Label ID="lblMethod" runat="server" Text="Application Method (Dip/C-Spray/E-Spray/Auto-Spray/Other)"></asp:Label>
                                </td>
                                <td bgcolor="#F1F1ED">
                                    <asp:TextBox ID="txtmethod" runat="server" Height="18px" Width="150px" MaxLength="200"
                                        TabIndex="29"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <%-- <tr>
                                <td bgcolor="#F1F1ED" colspan="7">
                                    <asp:Label ID="lblthinner" runat="server" Text="Application Viscosity"></asp:Label>
                                </td>
                                <td bgcolor="#F1F1ED" colspan="3">
                                    <asp:TextBox ID="txtthinner" runat="server" Height="18px" Width="309px" MaxLength="50"
                                        TabIndex="30"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <%-- <tr>
                                <td bgcolor="#F1F1ED" colspan="7">
                                    <asp:Label ID="lblIntake" runat="server" Text="Thinner Intake Online"></asp:Label>
                                </td>
                                <td bgcolor="#F1F1ED" colspan="3">
                                    <asp:TextBox ID="txtIntake" runat="server" Height="18px" Width="309px" MaxLength="50"
                                        TabIndex="31"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <%--   <tr>
                                <td bgcolor="#F1F1ED" colspan="7">
                                    <asp:Label ID="lblsystem" runat="server" Text="Application System Flash off Time Between Primer and Paint"></asp:Label>
                                </td>
                                <td bgcolor="#F1F1ED" colspan="3">
                                    <asp:TextBox ID="txtsystem" runat="server" Height="18px" Width="309px" MaxLength="50"
                                        TabIndex="32"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <%--    <tr>
                                <td bgcolor="#F1F1ED" colspan="7">
                                    <asp:Label ID="lblBanking" runat="server" Text="Banking Schedule (Temp and Time) - E.M.T "></asp:Label>
                                </td>
                                <td bgcolor="#F1F1ED" colspan="3">
                                    <asp:TextBox ID="txtBanking" runat="server" Height="18px" Width="309px" MaxLength="50"
                                        TabIndex="33"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <%--  <tr>
                                <td bgcolor="#F1F1ED" colspan="7">
                                    <asp:Label ID="lblSpecific" runat="server" Text="Specific Liquid Properties (Supply Viscosity,NVM,ER) "></asp:Label>
                                </td>
                                <td bgcolor="#F1F1ED" colspan="3">
                                    <asp:TextBox ID="txtSpecific" runat="server" Height="18px" Width="309px" MaxLength="50"
                                        TabIndex="34"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <%--  <tr>
                                <td bgcolor="#F1F1ED">
                                    <asp:Label ID="lbldft" runat="server" Text="DFT STD Specified by Customer(For Composites System, Each Coat)"></asp:Label>
                                </td>
                                <td bgcolor="#F1F1ED" colspan="2">
                                    <asp:TextBox ID="txtdft" runat="server" Height="18px" Width="100px" MaxLength="50"
                                        TabIndex="35"></asp:TextBox>
                                </td>
                                <td bgcolor="#F1F1ED">
                                    <asp:Label ID="lblsst" runat="server" Text="SST Required pls Specify HRS "></asp:Label>
                                </td>
                                <td bgcolor="#F1F1ED" colspan="3">
                                    <asp:TextBox ID="txtsst" runat="server" Height="18px" Width="100px" MaxLength="50"
                                        TabIndex="36"></asp:TextBox>
                                </td>
                                <td bgcolor="#F1F1ED" colspan="2" style="display: none">
                                    <asp:Label ID="lblhrs" runat="server" Text="Solid Contain"></asp:Label>
                                </td>
                                <td bgcolor="#F1F1ED" style="display: none">
                                    <asp:TextBox ID="txthrs" runat="server" Height="18px" Width="150px" TabIndex="37"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <%--    <tr>
                                <td bgcolor="#F1F1ED" colspan="7">
                                    <asp:Label ID="lblAccelerated" runat="server" Text="Accelerated Weathering Test Required. Pls Specify HRS"></asp:Label>
                                </td>
                                <td bgcolor="#F1F1ED" colspan="3">
                                    <asp:TextBox ID="txtAccelerated" runat="server" Height="18px" Width="309px" MaxLength="50"
                                        TabIndex="38"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <%--   <tr>
                                <td bgcolor="#F1F1ED" colspan="7">
                                    <asp:Label ID="lblAny" runat="server" Text="Any Other Potential Test Relevant to Development "></asp:Label>
                                </td>
                                <td bgcolor="#F1F1ED" colspan="3">
                                    <asp:TextBox ID="txtAny" runat="server" Height="18px" Width="309px" MaxLength="50"
                                        TabIndex="39"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <%--    <tr style="display: none">
                                <td bgcolor="#F1F1ED">
                                    <asp:Label ID="lblliquid" runat="server" Text="Liquid Properties"></asp:Label>
                                </td>
                                <td bgcolor="#F1F1ED" colspan="2">
                                    <asp:TextBox ID="txtliquid" runat="server" Height="18px" Width="100px" MaxLength="50"></asp:TextBox>
                                </td>
                                <td bgcolor="#F1F1ED" colspan="6">
                                    <asp:Label ID="lbldry" runat="server" Text="Dry Film Properties(Short Term Tests &amp; Long Term Tests)"></asp:Label>
                                </td>
                                <td bgcolor="#F1F1ED">
                                    <asp:TextBox ID="txtdry" runat="server" Height="18px" Width="150px" MaxLength="40"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <%-- <tr>
                                <td colspan="3" style="font-weight: bold; text-decoration: underline">
                                    BUSINESS POTENTIAL:
                                </td>
                                <td colspan="7">
                                    <asp:TextBox ID="txtbusin" runat="server" Height="18px" ONKEYPRESS="return isNumberKey(event)"
                                        TabIndex="41" Width="350px" MaxLength="50"  Visible="false"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <%-- <tr>
                                <td colspan="3">
                                    Total Business Potential of Customer
                                </td>
                                <td colspan="7">
                                    &nbsp;
                                </td>
                            </tr>--%>
                            <%-- <tr>
                                <td bgcolor="#F1F1ED">
                                    Volume
                                </td>
                                <td bgcolor="#F1F1ED" colspan="2">
                                    <asp:TextBox ID="txtVolume" runat="server" Height="18px" Width="100px" MaxLength="50"
                                        TabIndex="42"></asp:TextBox>
                                </td>
                                <td bgcolor="#F1F1ED" colspan="6">
                                    Value
                                </td>
                                <td bgcolor="#F1F1ED">
                                    <asp:TextBox ID="txtValue" runat="server" Height="18px" Width="150px" MaxLength="50"
                                        TabIndex="43"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <%--  <tr>
                                <td colspan="3">
                                    Business Expected (Volume);
                                </td>
                                <td colspan="7">
                                    &nbsp;
                                </td>
                            </tr>--%>
                            <%--<tr>
                                <td bgcolor="#F1F1ED">
                                    Immediate
                                </td>
                                <td bgcolor="#F1F1ED" colspan="2">
                                    <asp:TextBox ID="txtimmid" runat="server" Height="18px" Width="100px" MaxLength="50"
                                        TabIndex="44"></asp:TextBox>
                                </td>
                                <td bgcolor="#F1F1ED" colspan="6">
                                    Long Term
                                </td>
                                <td bgcolor="#F1F1ED">
                                    <asp:TextBox ID="txtlong" runat="server" Height="18px" Width="150px" MaxLength="20"
                                        TabIndex="45"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <%--  <tr>
                                <td bgcolor="#F1F1ED" colspan="7">
                                    <asp:Label ID="Label1" runat="server" Text="Future Business Volume With Other Customer"></asp:Label>
                                </td>
                            </tr>--%>
                            <%--<tr>
                                <td bgcolor="#F1F1ED">
                                    Volume
                                </td>
                                <td bgcolor="#F1F1ED" colspan="2">
                                    <asp:TextBox ID="txtFutureVol" runat="server" Height="18px" Width="100px" MaxLength="50"
                                        TabIndex="46"></asp:TextBox>
                                </td>
                                <td bgcolor="#F1F1ED" colspan="6">
                                    Value
                                </td>
                                <td bgcolor="#F1F1ED">
                                    <asp:TextBox ID="txtFutureVal" runat="server" Height="18px" Width="150px" MaxLength="50"
                                        TabIndex="47"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <%--    <tr>
                                <td bgcolor="#F1F1ED" colspan="10">
                                    Present Supplier(s),Their Share &amp; Prices:
                                </td>
                            </tr>--%>
                            <%--  <tr>
                                <td colspan="6" valign="top">
                                    i)&nbsp;
                                    <asp:TextBox ID="txt1" runat="server" Height="18px" Width="320px" TabIndex="48" MaxLength="100"></asp:TextBox>
                                </td>
                                <td colspan="3">
                                    Basic Price</td>
                                <td>
                                    <asp:TextBox ID="Textpr1" runat="server" Height="18px" MaxLength="50" ONKEYPRESS="return isNumberKey(event)"
                                        TabIndex="49" Width="150px"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <%--<tr>
                                <td colspan="6" valign="top">
                                    ii)
                                    <asp:TextBox ID="txt11" runat="server" Height="18px" Width="320px" MaxLength="200"
                                        TabIndex="50"></asp:TextBox>
                                </td>
                                <td colspan="3">
                                    Basic Price</td>
                                <td>
                                    <asp:TextBox ID="Textpr2" runat="server" Height="18px" MaxLength="50" ONKEYPRESS="return isNumberKey(event)"
                                        TabIndex="51" Width="150px"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <%-- <tr>
                                <td bgcolor="#F1F1ED" colspan="7">
                                    <asp:Label ID="lblBasicPrice" runat="server" Text="Basic Price at which Business Can be Obtained :"></asp:Label>
                                </td>
                                <td bgcolor="#F1F1ED" colspan="3">
                                    <asp:TextBox ID="txtBasicPrice" runat="server" Height="18px" Width="309px" MaxLength="50"
                                        TabIndex="52"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <%--      <tr>
                                <td bgcolor="#F1F1ED" colspan="7">
                                    <asp:Label ID="lblPymt" runat="server" Text="Payment Terms of Customer"></asp:Label>
                                </td>
                                <td bgcolor="#F1F1ED" colspan="3">
                                    <asp:TextBox ID="txtPymt" runat="server" Height="18px" Width="309px" MaxLength="50"
                                        TabIndex="53"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <%-- <tr>
                                <td colspan="2">
                                    Competitors Product Sample
                                </td>
                                <td colspan="3">
                                    <asp:RadioButtonList ID="rbcust0" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow"
                                        TabIndex="55">
                                        <asp:ListItem Selected="True" Text="Available" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Not Available" Value="1"></asp:ListItem>
                                    </asp:RadioButtonList>
                                    <asp:TextBox ID="txtpsample" runat="server" Height="18px" MaxLength="50" TabIndex="54"
                                        Width="150px" Visible="false"></asp:TextBox>
                                </td>
                               <%-- <td>
                                    Select Option
                                </td>
                                <td>
                                    <%--<asp:RadioButtonList ID="rbcust0" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow"
                                        TabIndex="55">
                                        <asp:ListItem Selected="True" Text="Available" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Not Available" Value="1"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td colspan="3">
                                    Qty
                                </td>
                                <td>
                                    <asp:TextBox ID="txtQty" runat="server" Height="18px" MaxLength="56" ONKEYPRESS="return isNumberKey(event)"
                                        TabIndex="56" Width="50px"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <%-- <tr style="display: none">
                                <td colspan="2">
                                    <asp:Label ID="lblbasic" runat="server" Text="Price At Which Business can be Obtained(Per Kg/Ltr.)"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtbasic" runat="server" Height="18px" Width="150px"></asp:TextBox>
                                </td>
                                <td colspan="3">
                                    <asp:Label ID="lblCust3" runat="server" Text=" Basic Price/Selling Price"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:RadioButtonList ID="rbcust3" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                        <asp:ListItem Selected="True" Text="Basic " Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Selling " Value="1"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>--%>
                            <%--   <tr>
                                <td bgcolor="#F1F1ED" colspan="10">
                                    Shade/Finish Referance
                                </td>
                            </tr>--%>
                            <%--<tr>
                                <td bgcolor="#F1F1ED" colspan="3">
                                    Standard Shade Panel Given & Signed by Customer :
                                </td>
                                <td bgcolor="#F1F1ED" colspan="7">
                                    <asp:RadioButtonList ID="rbcust1" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow"
                                        Enabled="False" TabIndex="57">
                                        <asp:ListItem Text="Available" Value="0" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Not Available" Value="1"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>--%>
                            <%--  <tr>
                                <td bgcolor="#F1F1ED" colspan="3">
                                    Time Frame for Development:
                                </td>
                                <td bgcolor="#F1F1ED" colspan="7">
                                    <asp:TextBox ID="txttime" runat="server" Height="18px" Width="531px" MaxLength="15"
                                        TabIndex="58"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <%--<tr>
                                <td bgcolor="#F1F1ED" colspan="3">
                                    <asp:Label ID="lblSampleQty" runat="server" Text="Sample Qty Required For Trail (Standard 1- Ltr.)for Extra Qty of Sample Pls Specify"></asp:Label>
                                </td>
                                <td bgcolor="#F1F1ED" colspan="7">
                                    <asp:TextBox ID="txtSampleQty" runat="server" Height="18px" Width="531px" MaxLength="50"
                                        TabIndex="59"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <%-- <tr>
                                <td bgcolor="#F1F1ED" colspan="3">
                                    Suggest Final Name of the Product
                                </td>
                                <td bgcolor="#F1F1ED" colspan="7">
                                    <asp:TextBox ID="txtSuggest" runat="server" Height="18px" Width="531px" MaxLength="50"
                                        TabIndex="60"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <%--        <tr>
                                <td bgcolor="#F1F1ED" colspan="3">
                                    Additional Information(If Any):
                                </td>
                                <td bgcolor="#F1F1ED" colspan="7">
                                    <asp:TextBox ID="txtaddition" runat="server" Height="18px" Width="531px" MaxLength="100"
                                        TabIndex="61"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <%-- <tr>
                                <td colspan="3">
                                    Requested By BDM Name
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtreques" runat="server" Height="18px" Width="150px" MaxLength="50"
                                        TabIndex="62"></asp:TextBox>
                                </td>
                                <td colspan="3">
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                            </tr>--%>
                            <%--     <tr style="display:none">
                                <td colspan="3">
                                    &nbsp;</td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtprodname" runat="server" Height="18px" Width="150px" MaxLength="50"
                                        TabIndex="63"></asp:TextBox>
                                </td>
                                <td colspan="3">
                                    <asp:Button ID="btnstaus" runat="server" Height="20px" OnClick="btnstaus_Click" Style="float: right;"
                                        Text="!" Width="17px" Visible="false" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtstatus" runat="server" Height="18px" Width="150px" MaxLength="50"
                                        TabIndex="64"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <%--      <tr>
                                <td colspan="3">
                                    Approved By (Marketing Head Name)</td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtrecomm" runat="server" Height="18px" MaxLength="25" TabIndex="65" Width="150px"></asp:TextBox>
                                </td>
                                <td colspan="3">
                                    Remarks for Refusal
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRefusal1" runat="server" Height="18px" Width="350px" MaxLength="50"
                                        TabIndex="66"></asp:TextBox>
                                </td>
                            </tr>--%>
                        </table>
                    </asp:View>
                    <asp:View ID="View2" runat="server">
                        <table align="center" style="border-color: #000080; width: 98%; height: 333px; font-family: Arial; font-size: small;"
                            frame="box">
                            <%--  <tr>
                                <td colspan="6" align="center" style="background-image: url('images/icons_bg.gif');
                                    font-size: medium; font-weight: bold;">
                                    SDR&nbsp; Page 2 : Feasibility Assessment</td>
                            </tr>--%>
                            <%-- <tr>
                                <td>
                                    Doc No.
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFeasDoc" runat="server" Height="18px" MaxLength="6" Width="70px"
                                      TabIndex="1" ReadOnly="True"></asp:TextBox>
                                </td>
                                <td>
                                    Revision No.
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFeasRev" runat="server" Height="18px" MaxLength="6" Width="70px"
                                        TabIndex="2" ReadOnly="True"></asp:TextBox>
                                </td>
                                <td>
                                    W.E.F
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFeasDate" runat="server" Height="18px" MaxLength="20" Width="80px"
                                        TabIndex="3" ReadOnly="True"></asp:TextBox>
                                    &nbsp;
                                </td>
                            </tr>--%>
                            <%--     <tr>
                                <td>
                                    Draft No.
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDraft" runat="server" Height="18px" ReadOnly="True" Width="70px" TabIndex="4"></asp:TextBox>
                                </td>
                                <td>
                                    Draft Date
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtDraftDt" runat="server" Height="18px" ToolTip="Enter Date in dd/mm/yyyy Format"
                                        Width="70px" MaxLength="5"></asp:TextBox>
                                    <asp:MaskedEditExtender ID="MaskedEditExtender1" runat="server" Mask="99/99/9999"
                                        MaskType="Date" TargetControlID="txtdate1" />
                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                        PopupButtonID="ImgDate5" TargetControlID="txtdate1">
                                    </asp:CalendarExtender>
                                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/btn_calendar.gif" />
                                </td>
                            </tr>--%>
                            <%--      <tr>
                                <td>
                                    Development No.
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSDR2" runat="server" ReadOnly="true" Width="70px" TabIndex="6" ></asp:TextBox>
                                </td>
                                <td>Date</td>
                                <td>
                                    <asp:TextBox ID="txtSDR2Date" runat="server" ReadOnly="true" Width="70px" MaxLength="7" ></asp:TextBox>
                                </td>
                            </tr>--%>
                            <%-- <tr>
                           <td>Customer</td><td><asp:TextBox ID="txtCust2" runat="server" ReadOnly="true" Width="300px" Rows="8" ></asp:TextBox></td>
                            </tr>
                            <tr  style="display:none">
                                <td>
                                    Is the design a new concept to the company?</td>
                                <td colspan="5">
                                    <asp:TextBox ID="txtprod" runat="server" Height="18px" Width="480px" MaxLength="60" TabIndex="9"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <%--    <tr style="display:none">
                                <td bgcolor="#F1F1ED">
                                    Are new product used in this product?</td>
                                <td bgcolor="#F1F1ED">
                                    <asp:RadioButtonList ID="rbminor" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                        <asp:ListItem Selected="True" Text="Yes" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="1"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td colspan="2" bgcolor="#F1F1ED">
                                    Are new types of tooling needed for producing this product ?</td>
                                <td bgcolor="#F1F1ED" colspan="2">
                                    <asp:TextBox ID="txtprodcode" runat="server" Height="18px" Width="100px" MaxLength="50"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <%-- <tr style="display:none">
                                <td bgcolor="#F1F1ED" colspan="6">
                                    New Product
                                </td>
                            </tr>--%>
                            <%-- <tr style="display:none">
                                <td>
                                    Are all special charcteristics identified in the specification?</td>
                                <td colspan="5">
                                    <asp:TextBox ID="txtdpperiod" runat="server" Height="18px" Width="480px" MaxLength="100"></asp:TextBox>
                                </td>
                            </tr>--%>

                            <%-- <tr style="display:none">
                                <td>
                                    Are any standards/test specification required from customer ? Following are some standards: production Validation,Continous Confermance,Reliability testing,Material testing, Process Standards,Performance Standards,Appearance Standards,Lab Procedures,Goverment Standards</td>
                                <td colspan="5">
                                    <asp:TextBox ID="txtdpcost" runat="server" Height="18px" Width="480px" MaxLength="80"></asp:TextBox>
                                </td>
                            </tr>--%>

                            <%--<tr style="display: none">
                                <td>
                                    Are additional resources (both people &amp; equipments) needed for this design or additional training required ?</td>
                                <td colspan="5">
                                    <asp:TextBox ID="txtsample" runat="server" Height="18px" Width="480px" MaxLength="80"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <%--  <tr style="display:none">
                                <td>
                                    Do we need to develop special test equipments? Do we require customer approval for these equipments ?</td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtRegular" runat="server" Height="18px" Width="480px" MaxLength="50"></asp:TextBox>
                                </td>
                                <td>
                                    Are design changes pending from customer at the customer end ?</td>
                                <td>
                                    <asp:TextBox ID="txtCostSheet" runat="server" Height="18px" Width="480px" MaxLength="50"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <%--  <tr style="display:none">
                                <td>
                                    Is first sample needed ?</td>
                                <td colspan="5">
                                    <asp:TextBox ID="txtSampleSize" runat="server" Height="18px" Width="480px" MaxLength="50"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <%--  <tr style="display:none">
                                <td bgcolor="#F1F1ED" colspan="6">
                                    Additional Resources Needed
                                </td>
                            </tr>--%>
                            <%--  <tr style="display:none">
                                <td>
                                    Any other specific requirements ?</td>
                                <td colspan="5">
                                    <asp:TextBox ID="txtfordp" runat="server" Height="18px" Width="480px" MaxLength="80"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <%--<tr style="display:none">
                                <td>
                                    Have we faced quality /warrenty problems in similar product ?</td>
                                <td colspan="5">
                                    <asp:TextBox ID="txtforreg" runat="server" Height="18px" Width="480px" MaxLength="15"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <%--  <tr style="display: none">
                                <td>Is the product sensitive to customer`s plant process or assembly ?</td>
                                <td colspan="5">
                                    <asp:TextBox ID="txtfortest" runat="server" Height="18px" Width="480px" MaxLength="15"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <%--  <tr style="display: none">
                                <td>Are unique in process &amp; final inspection required ? Can we develop these?</td>
                                <td colspan="5">
                                    <asp:TextBox ID="txtEstimated" runat="server" Height="18px" Width="480px" MaxLength="50"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <%-- <tr style="display: none">
                                <td>Will any new or existing processes or capacity enhancement required to reach current customer requirement ?</td>
                                <td colspan="5" style="display: none">
                                    <asp:TextBox ID="txtremark" runat="server" Height="18px" Width="480px" MaxLength="100"></asp:TextBox>
                                </td>
                            </tr>--%>

                            <%--          <tr>
                                <td colspan="5">
                                    <asp:GridView ID="sg1" runat="server" Width="100%" AutoGenerateColumns="False" OnRowCommand="sg1_RowCommand" OnRowDataBound="sg1_RowDataBound" Style="font-size: smaller;" Height="200px">
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    A
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btnadd" runat="server" CommandName="Add" ImageAlign="Middle"
                                                        ImageUrl="~/images/Btn_addn.png" Width="20px" ToolTip="Insert Item" />
                                                </ItemTemplate>
                                                <ItemStyle Width="11px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    D
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btnrmv" runat="server" CommandName="Rmv" ImageUrl="~/images/Btn_remn.png"
                                                        Width="20px" ImageAlign="Middle" ToolTip="Remove Item" />
                                                </ItemTemplate>
                                                <ItemStyle Width="11px" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="srno" HeaderText="Srno" ReadOnly="True">
                                                <HeaderStyle Width="100px" />
                                                <ItemStyle Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Code" HeaderText="Code" ReadOnly="True">
                                                <HeaderStyle Width="100px" />
                                                <ItemStyle Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="name" HeaderText="Name" ReadOnly="True">
                                                <HeaderStyle Width="3000px" />
                                                <ItemStyle Width="3000px" />
                                            </asp:BoundField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    Yes / No
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtYes" runat="server" Text='<%#Eval("yes") %>' oncontextmenu="return false;" Style="text-align: left" MaxLength="6" Width="200px"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    Remarks
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtRemarks" runat="server" Text='<%#Eval("Remarks") %>' oncontextmenu="return false;" Style="text-align: left" MaxLength="50" Width="500px"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle BackColor="#1797c0" ForeColor="White" Height="20px" CssClass="GridviewScrollHeader"
                                            Font-Bold="True" />
                                    </asp:GridView>
                                </td>
                            </tr>--%>

                            <%-- <tr style="display: none">
                                <td>Head R&amp;D
                                </td>
                                <td>
                                    <asp:TextBox ID="txthead" runat="server" Height="18px" Width="100px" MaxLength="15"></asp:TextBox>
                                </td>
                                <td>Sig/Date
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtsign" runat="server" Height="18px" ToolTip="Enter Date in dd/mm/yyyy Format"
                                        Width="70px"></asp:TextBox>
                                    <asp:CalendarExtender ID="CE1" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                        PopupButtonID="ImgDate1" TargetControlID="txtsign">
                                    </asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="MEE1" runat="server" Mask="99/99/9999" MaskType="Date"
                                        TargetControlID="txtsign" />
                                    <asp:ImageButton ID="ImgDate1" runat="server" ImageUrl="~/images/btn_calendar.gif" />
                                </td>
                            </tr>--%>
                            <%--<tr>
                                <td bgcolor="#F1F1ED" colspan="6">Conclusions
                                </td>
                            </tr>--%>
                            <%-- <tr>
                                <td bgcolor="#F1F1ED" colspan="6">1. CFT Members:
                                </td>
                            </tr>--%>
                            <%--  <tr>
                                <td>&nbsp;</td>
                                <td colspan="5">
                                    <asp:TextBox ID="txti" runat="server" Height="18px" Width="300px" MaxLength="50"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <%--<tr>
                                <td>&nbsp;</td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtii" runat="server" Height="18px" Width="300px" MaxLength="50"></asp:TextBox>
                                </td>
                                <td colspan="2" style="display: none">Approval of Additional Capital Cost
                                </td>
                                <td style="display: none">
                                    <asp:TextBox ID="txtAppAdd" runat="server" Height="18px" Width="100px" MaxLength="50"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <%--<tr>
                                <td bgcolor="#F1F1ED" colspan="6">1. Project Feasible:
                                </td>
                            </tr>--%>
                            <%-- <tr>
                                <td>&nbsp;</td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtiii" runat="server" Height="18px" Width="300px" MaxLength="50"></asp:TextBox>
                                </td>
                                <td colspan="2" style="display: none">M.D.Signature Date
                                </td>
                                <td style="display: none">
                                    <asp:TextBox ID="txtmddate" runat="server" Height="18px" Width="100px" MaxLength="20"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtvchdate_CalendarExtender" runat="server"
                                        Enabled="True" TargetControlID="txtmddate" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="Maskedit1" runat="server" Mask="99/99/9999"
                                        MaskType="Date" TargetControlID="txtmddate" />
                                    <%-- <cc1:MaskedEditExtender ID="MEE3" runat="server" Mask="99/99/9999" MaskType="Date"
                                        TargetControlID="txtmddate" />
                                </td>
                            </tr>--%>
                            <%-- <tr>
                                <td>&nbsp;</td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtiiii" runat="server" Height="18px" Width="300px" MaxLength="50"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <%--    <tr>
                                <td bgcolor="#F1F1ED">Condition for Acceptance /Rejection
                                </td>
                                <td bgcolor="#F1F1ED" colspan="5">
                                    <asp:TextBox ID="txtfeedback" runat="server" Height="18px" Width="300px" MaxLength="50"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <%--  <tr>
                                <td bgcolor="#F1F1ED">3  Costing / Quotation Required  
                                </td>
                                <td bgcolor="#F1F1ED" colspan="5">
                                    <asp:TextBox ID="txtclosed" runat="server" Height="18px" Width="300px" MaxLength="50"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <%-- <tr>
                                <td bgcolor="#F1F1ED"></td>
                                <td bgcolor="#F1F1ED" colspan="5">
                                    <asp:TextBox ID="txtclosed2" runat="server" Height="18px" Width="300px" MaxLength="15"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <%-- <tr>
                                <td>Approval From R&amp;D Head Date &amp; Time
                                </td>
                                <td>
                                    <asp:TextBox ID="txtApproval" runat="server" Height="18px" Width="150px" MaxLength="50"
                                        TabIndex="67"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <%--<tr>
                                <td>Remarks for Refusal
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRefusal2" runat="server" Height="18px" MaxLength="50" TabIndex="68" Width="350px"></asp:TextBox>
                                </td>
                            </tr>--%>

                            <%-- <tr style="display: none">
                                <td bgcolor="#F1F1ED">Head Marketing SIgn/Date
                                </td>
                                <td bgcolor="#F1F1ED" colspan="5">
                                    <asp:TextBox ID="txthmdate" runat="server" Height="18px" Width="100px" ToolTip="Enter Date in dd/mm/yyyy Format"
                                        MaxLength="20"></asp:TextBox>
                                    <asp:CalendarExtender ID="CE2" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                        PopupButtonID="ImgDate2" TargetControlID="txthmdate">
                                    </asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="MEE2" runat="server" Mask="99/99/9999" MaskType="Date"
                                        TargetControlID="txthmdate" />
                                    <asp:ImageButton ID="ImgDate2" runat="server" ImageUrl="~/images/btn_calendar.gif" />
                                </td>
                            </tr>--%>
                        </table>
                    </asp:View>
                </asp:MultiView>
            </td>
        </tr>
    </table>


    <asp:Button ID="btnhideF" runat="server" OnClick="btnhideF_Click" Style="display: none" />
    <asp:Button ID="btnhideF_s" runat="server" OnClick="btnhideF_S_Click" Style="display: none" />
    <asp:HiddenField ID="doc_nf" runat="server" />
    <asp:HiddenField ID="doc_df" runat="server" />
    <asp:HiddenField ID="doc_vty" runat="server" />
    <asp:HiddenField ID="doc_addl" runat="server" />
    <asp:HiddenField ID="edmode" runat="server" />
    <asp:HiddenField ID="hffield" runat="server" />
    <asp:HiddenField ID="hf1" runat="server" />
    <asp:HiddenField ID="lbledmode" runat="server" />
    <asp:HiddenField ID="lblname" runat="server" />
    <asp:HiddenField ID="HFOLDDT" runat="server" />
    <asp:HiddenField ID="HFOPT" runat="server" />
    <asp:HiddenField ID="hf2" runat="server" />
            <asp:HiddenField ID="TabName" runat="server" />

    <%--  --%>
</asp:Content>
<%--<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphHead">
    </asp:Content>--%>