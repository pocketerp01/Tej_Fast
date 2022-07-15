<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="Dak1_task" Title="Tejaxo" CodeFile="Dak1_task.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style1 {
            font-family: Arial, Helvetica, sans-serif;
            font-weight: 700;
            color: #474646;
            font-size: 12px;
            width: 68px;
        }

        .auto-style2 {
            font-family: Arial, Helvetica, sans-serif;
            font-weight: 700;
            color: #474646;
            font-size: 12px;
            width: 485px;
        }

        .auto-style3 {
            width: 148px;
        }

        .auto-style4 {
            font-family: Arial, Helvetica, sans-serif;
            font-weight: 700;
            color: #474646;
            font-size: 12px;
            width: 148px;
        }

        .auto-style5 {
            width: 485px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper">
        <section class="content-header">
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Task Action Taken"></asp:Label>
                    </td>
                    <td style="text-align: right">
                        <button id="btnnew" runat="server" style="width: 100px;" accesskey="N" onserverclick="btnnew_ServerClick"
                            class="btn btn-info">
                            <u>N</u>ew</button>
                        <button id="btnedit" runat="server" style="width: 100px;" accesskey="i" class="btn btn-info" onserverclick="btnedit_ServerClick">
                            Ed<u>i</u>t</button>
                        <button id="btnsave" runat="server" style="width: 100px;" accesskey="S" class="btn btn-info" onserverclick="btnsave_ServerClick">
                            <u>S</u>ave</button>
                        <button id="btndel" runat="server" style="width: 100px;" accesskey="l" class="btn btn-info" onserverclick="btndel_ServerClick">
                            De<u>l</u>ete</button>
                        <asp:Button ID="btnext" runat="server" Style="width: 100px;" Text="Exit" class="btn btn-info" OnClick="btnext_Click" />
                    </td>
                </tr>
            </table>
        </section>

        <section class="content">
            <div class="row">
                <div class="col-md-6" style="background-color: white">
                    <div>
                        <div class="form-group">
                            <label id="lblno" runat="server" class="col-md-2 control-label" title="lbl1">Entry No</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtvchnew" runat="server" ReadOnly="true" CssClass="form-control"
                                    onblur="Change(this, event)" onfocus="Change(this, event)" Width="100%"></asp:TextBox>
                            </div>
                            <label id="lbldt" runat="server" class="col-md-2 control-label" title="lbl1">Entry Date</label>
                            <div class="col-md-3">
                                <asp:TextBox ID="txtdate" runat="server" Width="100%" CssClass="form-control" onblur="Change(this, event)"
                                    onfocus="Change(this, event)"></asp:TextBox>
                                <asp:CalendarExtender ID="txtdate_CalendarExtender" runat="server"
                                    Enabled="True" TargetControlID="txtdate"
                                    Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                                <asp:MaskedEditExtender ID="MaskedEditExtender1" runat="server" Mask="99/99/9999"
                                    MaskType="Date" TargetControlID="txtdate" />
                            </div>
                        </div>

                        <div class="form-group">
                            <label id="Label1" runat="server" class="col-md-2 control-label" title="lbl1">Assign By</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtAssign" runat="server" ReadOnly="true" CssClass="form-control"
                                    onblur="Change(this, event)" onfocus="Change(this, event)" Width="100%"></asp:TextBox>
                            </div>

                            <label id="Label3" runat="server" class="col-md-2 control-label" title="lbl1">Assign Dt</label>
                            <div class="col-md-3">
                                <asp:TextBox ID="txtAssignDt" runat="server" Width="100%" CssClass="form-control"
                                    onblur="Change(this, event)" ReadOnly="true" onfocus="Change(this, event)"></asp:TextBox>
                            </div>
                        </div>





                    </div>
                </div>



                <div class="row">
                    <div class="col-md-6" style="background-color: white">
                        <div>
                            <div class="form-group">
                                <label id="Label4" runat="server" class="col-md-3 control-label" title="lbl1">Action Date</label>
                                <div class="col-md-9">
                                    <asp:TextBox ID="txtredate" runat="server" Width="95%" CssClass="form-control" onblur="Change(this, event)" onfocus="Change(this, event)"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtredate_CalendarExtender" runat="server"
                                        Enabled="True" TargetControlID="txtredate"
                                        Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="MaskedEditExtender2" runat="server" Mask="99/99/9999"
                                        MaskType="Date" TargetControlID="txtredate" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label2" runat="server" class="col-md-3 control-label" title="lbl1">Action Taken</label>
                                <div class="col-md-9">
                                    <asp:TextBox ID="txtreason" MaxLength="200" runat="server" Width="95%" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>


                        </div>

                    </div>
                </div>





                <%--next div--%>
                <div class="col-md-12">
                    <div>
                        <div class="box-body">

                            <div class="form-group">
                                <label id="lbltskno" runat="server" class="col-sm-1 control-label" title="lbl1">Task No</label>

                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtvchnum" runat="server" ReadOnly="true" CssClass="form-control"
                                        onblur="Change(this, event)" onfocus="Change(this, event)" Width="100%"></asp:TextBox>
                                </div>
                                <label id="Label5" runat="server" class="col-sm-2 control-label" title="lbl1">Task_Assign_Date</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtvchdate" runat="server" Width="100%" CssClass="form-control"
                                        ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="lbluserid" runat="server" class="col-sm-1 control-label" title="lbl1">User ID:</label>
                                <div class="col-sm-11">
                                    <asp:TextBox ID="txtuserid" runat="server" Width="100%" CssClass="form-control" ReadOnly="true"
                                        onblur="Change(this, event)" onfocus="Change(this, event)"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="lblsub" runat="server" class="col-sm-1 control-label" title="lbl1">Subject:</label>
                                <div class="col-sm-11">
                                    <asp:TextBox ID="txtsubject" runat="server" ReadOnly="true" Width="100%" CssClass="form-control"
                                        onblur="Change(this, event)" onfocus="Change(this, event)"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="lblcc" runat="server" class="col-sm-1 control-label" title="lbl1">CC:</label>
                                <div class="col-sm-11">
                                    <asp:TextBox ID="txtemailcc" runat="server" ReadOnly="true" Width="100%" CssClass="form-control"
                                        onblur="Change(this, event)" onfocus="Change(this, event)"></asp:TextBox>
                                </div>
                            </div>


                            <div class="form-group">
                                <label id="lblmesg" runat="server" class="col-sm-3 control-label" title="lbl1">Message:</label>
                                <label id="Label9" runat="server" class="col-sm-2 control-label" title="lbl1">Task_to_be_completed_date</label>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txttskdate" runat="server" Width="100%" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                </div>
                                <label id="Label6" runat="server" class="col-sm-1 control-label" title="lbl1">Priority</label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtdrop" ReadOnly="true" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <%--<asp:DropDownList ID="ddl1" runat="server" >
<asp:ListItem Enabled="true" Text="Medium" Value="0"></asp:ListItem>
<asp:ListItem Text="High" Value="1"></asp:ListItem>
</asp:DropDownList>--%>
                            <div class="form-group">
                                <div class="col-sm-12">
                                    <asp:TextBox ID="txtmsg" runat="server" ReadOnly="true" TextMode="MultiLine" Height="220px"
                                        Width="100%" CssClass="form-control" onblur="Change(this, event)" onfocus="Change(this, event)"></asp:TextBox>
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
    <asp:HiddenField ID="hffromdt" runat="server" />
    <asp:HiddenField ID="hftodt" runat="server" />
    <asp:HiddenField ID="hf1" runat="server" />
    <asp:HiddenField ID="hffirst" runat="server" />
</asp:Content>
