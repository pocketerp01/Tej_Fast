<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="Dak" Title="" CodeFile="Dak.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style1 {
            font-family: Arial, Helvetica, sans-serif;
            font-weight: 700;
            color: #474646;
            font-size: 12px;
            width: 68px;
        }
    </style>
    <script type="text/javascript">
        function openfileDialog() {
            $("#Attch").click();
        }
        function submitFile() {
            $("#<%= btnAtt.ClientID%>").click();
        };
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper">
        <section class="content-header">
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Task Management"></asp:Label>
                    </td>
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
                                <label id="lblno" runat="server" class="col-sm-3 control-label" title="lbl1">Entry_No</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtvchnum" runat="server" ReadOnly="true" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>
                            </div>


                            <div class="form-group">
                                <label id="lbldt" runat="server" class="col-sm-3 control-label" title="lbl1">Entry_Date</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtvchdate" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtvchdate_CalendarExtender" runat="server"
                                        Enabled="True" TargetControlID="txtvchdate"
                                        Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="MaskedEditExtender1" runat="server" Mask="99/99/9999"
                                        MaskType="Date" TargetControlID="txtvchdate" />
                                </div>
                            </div>


                            <div class="form-group">
                                <label id="lblid" runat="server" class="col-sm-1 control-label" title="lbl1">User_ID</label>
                                <div class="col-sm-2">
                                    <asp:ImageButton ID="imguserid" runat="server" ImageUrl="~/tej-base/images/Btn_addn.png"
                                        Style="float: right" Width="18px" Height="18px" ToolTip="Created User's" OnClick="imguserid_Click" />
                                    <%--   </div>
                                 <div class="col-sm-1">--%>
                                    <asp:ImageButton ID="imguserid1" runat="server" ImageUrl="~/tej-base/images/Btn_addn.png"
                                        Style="float: right" Width="18px" Height="18px" ToolTip="ERP User's" OnClick="imguserid1_Click" />
                                </div>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtuserid" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="lblsub" runat="server" class="col-sm-3 control-label" title="lbl1">Subject</label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtsubject" runat="server" Width="100%" MaxLength="150" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="lblcc" runat="server" class="col-sm-3 control-label" title="lbl1">CC</label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtemailcc" runat="server" MaxLength="50" Width="100%" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">

                            <%--      <div class="form-group">     
                           <label id="Label2" runat="server" class="col-sm-3 control-label" title="lbl1">CC:</label>
                                 <div class="col-sm-9">                               
                                    <asp:TextBox ID="TextBox1" runat="server"  Width="100%" CssClass="textboxStyle"></asp:TextBox>
                             </div>
                        </div>--%>
                            <div class="form-group">
                                <label id="lblentby" runat="server" class="col-sm-4 control-label" title="lbl1">Ent_By</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtentby" ReadOnly="true" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="lblentdt" runat="server" class="col-sm-2 control-label" title="lbl1">Ent_Date</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtentdt" runat="server" Width="100%" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>


                            <div class="form-group">
                                <label id="Label1" runat="server" class="col-sm-4 control-label" title="lbl1">Task_Completion_Date:</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txttskdate" runat="server" Width="100%" CssClass="form-control" ToolTip="Enter Last Date by which task shoud be Completed"></asp:TextBox>
                                    <asp:CalendarExtender ID="txttskdate_CalendarExtender" runat="server"
                                        Enabled="True" TargetControlID="txttskdate"
                                        Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="Maskedit1" runat="server" Mask="99/99/9999"
                                        MaskType="Date" TargetControlID="txttskdate" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="lblpriority" runat="server" class="col-sm-2 control-label" title="lbl1">Priority</label>
                                <div class="col-sm-3">
                                    <asp:DropDownList ID="ddl1" runat="server" Width="100%" CssClass="form-control">
                                        <asp:ListItem Enabled="true" Text="Medium" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="High" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Low" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-12">
                                </div>
                            </div>


                            <div class="form-group">
                                <label id="lblrmndr" runat="server" class="col-sm-4 control-label" title="lbl1">Reminder_Days </label>
                                <%-- <div class="col-sm-2">                          
                            <span style="font-size: 9px">(Keep it 0 if You want Daily Reminders)</span>   
                                      </div>--%>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtDays" Width="100%" runat="server" MaxLength="10" placeholder="Keep it 0 if You want Daily Reminders" CssClass="form-control" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                </div>
                            </div>


                            <%--     <div  class="form-group">
                                   <div class="col-sm-4">    
                                        <span style="font-size: 10px">(Keep it 0 if You want Daily Reminders)</span>   
                                        </div>
                                      <div class="col-sm-8">     
                                  </div> </div>--%>
                            <div class="form-group">
                                <asp:FileUpload ID="Attch" runat="server" Visible="true" onchange="submitFile()" />

                                <asp:TextBox ID="txtAttch" runat="server" MaxLength="100" placeholder="Path Upto 100 Char" ReadOnly="true" Visible="false"></asp:TextBox>

                                <asp:Label ID="Label27" runat="server" Text=" Please Link Correct File upto 3MB Size ." Visible="false"></asp:Label>

                                <asp:Button ID="btnAtt" runat="server" Text="Attachment" OnClick="btnAtt_Click" Width="134px" Style="display: none" />

                                <asp:Label ID="lblShow" runat="server"></asp:Label>
                                <asp:Label ID="lblUpload" runat="server" Style="display: none"></asp:Label>

                                <asp:ImageButton ID="btnView1" runat="server" ImageUrl="~/tej-base/images/preview-file.png" OnClick="btnView1_Click" Visible="false" />
                                <asp:ImageButton ID="btnDwnld1" runat="server" ImageUrl="~/tej-base/images/Save.png" OnClick="btnDwnld1_Click" Visible="false" />
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-12">
                    <div>
                        <div class="box-body">

                            <%--<div class="form-group">     
                           <label id="Label1" runat="server" class="col-sm-3 control-label" title="lbl1">CC:</label>
                                 <div class="col-sm-9">                               
                                    <asp:TextBox ID="TextBox1" runat="server"  Width="100%" CssClass="textboxStyle"></asp:TextBox>
                             </div>
                        </div>--%>

                            <div class="form-group">
                                <label id="lblmsg" runat="server" class="col-sm-1 control-label" title="lbl1">Message:</label>
                                <%-- <label id="Label1" runat="server" class="col-sm-1 control-label" title="lbl1">Task Date:</label>      
                                  <div class="col-sm-1">     
                            <asp:TextBox ID="txttskdate" runat="server" Width="70" CssClass="textboxStyle"></asp:TextBox>
                                 </div>--%>

                                <%--   <label id="lblpriority" runat="server" class="col-sm-1 control-label" title="lbl1">Priority</label> 

                                  <div class="col-sm-2">     
                            <asp:DropDownList ID="ddl1" runat="server">
                                <asp:ListItem Enabled="true" Text="Medium" Value="0"></asp:ListItem>
                                <asp:ListItem Text="High" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Low" Value="2"></asp:ListItem>
                            </asp:DropDownList>
                                      </div>--%>

                                <%--                               <label id="lblrmndr" runat="server" class="col-sm-3 control-label" title="lbl1">Reminder Days </label>                           
                            <span style="font-size: 9px">(Keep it 0 if You want Daily Reminders)</span>
                            <div class="col-sm-3">                                    
                                    <asp:TextBox ID="txtDays" runat="server"></asp:TextBox>
                                 </div>
                             </div>--%>


                                <div class="form-group">
                                    <div class="col-sm-12">
                                        <asp:TextBox ID="txtmsg" runat="server" MaxLength="200" TextMode="MultiLine" Width="100%" Height="270px" CssClass="form-control"></asp:TextBox>
                                    </div>
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
    <asp:HiddenField ID="doc_nf" runat="server" />
    <asp:HiddenField ID="doc_df" runat="server" />
    <asp:HiddenField ID="doc_vty" runat="server" />

</asp:Content>
