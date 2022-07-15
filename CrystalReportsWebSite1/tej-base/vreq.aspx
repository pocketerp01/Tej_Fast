<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="vreq" CodeFile="vreq.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

	<script src="../tej-base/Scripts/jquery-ui-sliderAccess.js" type="text/javascript"></script>
	<script src="../tej-base/Scripts/jquery-ui-timepicker-addon.js" type="text/javascript"></script>
	<link href="../tej-base/css/jquery-ui.css" rel="stylesheet" />
	<link href="../tej-base/css/jquery-ui-timepicker-addon.css" rel="stylesheet" />

	<style type="text/css">
		.style7 {
			height: 24px;
		}
	</style>

	<script type="text/javascript">
		$(document).ready(function () {
			$("#ContentPlaceHolder1_txtvtime").timepicker({
				timeFormat: "hh:mm tt",
				showOn: "button",
				buttonImage: "../tej-base/css/images/time.png",
				buttonImageOnly: true,
				showAnim: "fold"
			});
		});
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
	<div class="content-wrapper">
		<section class="content-header">
			<table style="width: 100%">
				<tr>
					
					<td style="float: left;">
						<button id="btnnew" accesskey="N" class="btn btn-info" style="width: 100px;" runat="server" onserverclick="btnnew_Click"><u>N</u>ew</button>
						<button id="btnedit" accesskey="i" class="btn btn-info" style="width: 100px;" runat="server" onserverclick="btnedit_Click">Ed<u>i</u>t</button>
						<button id="btnsave" accesskey="S" class="btn btn-info" style="width: 100px;" runat="server" onserverclick="btnsave_Click"><u>S</u>ave</button>
						<button id="btnprint" accesskey="P" class="btn btn-info" style="width: 100px;" runat="server" onserverclick="btnprint_Click"><u>P</u>rint</button>
						<button id="btndel" accesskey="l" class="btn btn-info" style="width: 100px;" runat="server" onserverclick="btndelete_Click">De<u>l</u>ete</button>
						<button id="btnlist" accesskey="t" class="btn btn-info" style="width: 100px;" runat="server" onserverclick="btnlist_Click">Lis<u>t</u></button>
						<button type="submit" id="btncancel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="c"><u>C</u>ancel</button>

						<button id="btnexit" runat="server" class="btn btn-info" style="width: 100px;" onserverclick="btnexit_Click">E<u>x</u>it</button>
					</td>
                    <td>
						<asp:Label ID="lblheader" runat="server" Text="Visitor Requisition" Font-Bold="True" Font-Size="X-Large"></asp:Label></td>

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
								<div class="col-sm-8">
									<asp:TextBox ID="txtdocno" runat="server" placeholder="Entry No" Width="100%" CssClass="form-control" ReadOnly="true" Height="32px"></asp:TextBox>
								</div>

							</div>
							<div class="form-group">
								<label id="tdvname" class="col-sm-4 control-label">Visitor Name</label>
								<div class="col-sm-8">
									<asp:TextBox ID="txtvname" runat="server" placeholder="Visitor Name" Width="100%" CssClass="form-control" Height="32px"></asp:TextBox>
								</div>
							</div>

						</div>
					</div>
				</div>
				<div class="col-md-6">
					<div>
						<div class="box-body">
							<div class="form-group">
								<label id="tddate" class="col-sm-4 control-label">Date</label>
								<div class="col-sm-8">
									<asp:TextBox ID="txtdate" runat="server" placeholder="Entry Date" Width="100%" CssClass="form-control" Height="32px"></asp:TextBox>
									<asp:CalendarExtender ID="txtdate_CalendarExtender" runat="server"
										Enabled="True" TargetControlID="txtdate"
										Format="dd/MM/yyyy">
									</asp:CalendarExtender>
									<asp:MaskedEditExtender ID="Maskedit1" runat="server" Mask="99/99/9999"
										MaskType="Date" TargetControlID="txtdate" />
								</div>
							</div>

							<div class="form-group">
								<label id="Mobile" runat="server" class="col-sm-4 control-label">Mobile No.</label>
								<%--<div class="col-sm-2">
									<span style="color: red">*</span>
								</div>--%>
								<div class="col-sm-8">
									<asp:TextBox ID="txtmobile" onkeypress="return isDecimalKey(event)" runat="server" Width="100%" MaxLength="10" Height="32px"></asp:TextBox>
								</div>
							</div>


						</div>
					</div>
				</div>

				<div class="col-md-12">
					<div>
						<div class="box-body">
							<div class="form-group">

								<label id="Company" runat="server" class="col-sm-2 control-label">Company/Individual</label>
								<%-- <div class="col-sm-1">
									<span style="color: red">*</span>
								</div>--%>
								<div class="col-sm-10">
									<asp:TextBox ID="txtcomp" runat="server"
										placeholder="Company" MaxLength="80" Width="100%" Height="32px"></asp:TextBox>
								</div>
							</div>

							<div class="form-group">
								<label id="Location" runat="server" class="col-sm-2 control-label">Address</label>
								<%--<div class="col-sm-1">
									<span style="color: red">*</span>
								</div>--%>
								<div class="col-sm-10">
									<asp:TextBox ID="txtloc" runat="server"
										placeholder="" MaxLength="80" Width="100%" Height="32px"></asp:TextBox>
								</div>
							</div>

							<div class="form-group">
								<label id="PurposeOfVisit" runat="server" class="col-sm-2 control-label">Purpose_Of_Visit</label>
								<div class="col-sm-10">
									<asp:TextBox ID="txtrmk" runat="server"
										placeholder="" MaxLength="80" Width="100%" Height="32px"></asp:TextBox>
								</div>
							</div>

							<div class="form-group">
								<label id="Department" runat="server" class="col-sm-2 control-label">Department</label>
								<div class="col-sm-10">
									<asp:TextBox ID="txtdept" runat="server"
										placeholder="" MaxLength="80" Width="100%" Height="32px"></asp:TextBox>
								</div>
							</div>

							<div class="form-group">
								<label id="Designation" runat="server" class="col-sm-2 control-label">Designation</label>
								<div class="col-sm-10">
									<asp:TextBox ID="txtdesig" runat="server"
										placeholder="" MaxLength="80" Width="100%" Height="32px"></asp:TextBox>
								</div>
							</div>


						</div>
					</div>
				</div>

				<div class="col-md-6">
					<div>
						<div class="box-body">
						 <%--   <div class="form-group">
								<label id="dateofvisit" runat="server" class="col-sm-3 control-label">Date_Of_Visit</label>
								<div class="col-sm-9">
									<asp:TextBox ID="txtvdate" runat="server" placeholder="" Width="100%" Height="32px"></asp:TextBox>
									<asp:CalendarExtender ID="txtvdate_CalendarExtender" runat="server" Enabled="True" TargetControlID="txtVdate" Format="dd/MM/yyyy">
									</asp:CalendarExtender>
									<asp:MaskedEditExtender ID="MaskedEdit2" runat="server" Mask="99/99/9999" MaskType="Date" TargetControlID="txtVdate" />
								</div>

							</div>--%>

							<div class="form-group">
								<label id="PreparedBy" runat="server" class="col-sm-3 control-label">Prepared_By</label>
								<div class="col-sm-3">
									<asp:TextBox ID="txtpre" runat="server"
										placeholder="" Width="100%" Height="32px" ReadOnly="true" CssClass="form-control"></asp:TextBox>
								</div>
									<label id="Visitortype" runat="server" class="col-sm-2 control-label">Visitor Type</label>
								<div class="col-sm-4">
									<asp:DropDownList ID="ddvtype" runat="server" TabIndex="11" Width="100%" Height="32px">
									</asp:DropDownList>
								</div>
							</div>

							<div class="form-group">
								<label id="Inform" runat="server" class="col-sm-2 control-label">To_Meet</label>
								<%--<div class="col-sm-2">
									<span style="color: red">*</span>
								</div>--%>
								<div class="col-sm-1">
									<asp:ImageButton ID="btnemp" runat="server" Height="23px" Width="26px"
										ImageAlign="left" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" ToolTip="Select Inform"
										OnClick="btndept_Click" />
								</div>
								<div class="col-sm-9">
									<asp:TextBox ID="txtempid" runat="server" Width="100%" placeholder="" ReadOnly="true" Height="28px"></asp:TextBox>
								</div>
							</div>


							<div class="form-group">
								<label id="Name1" runat="server" class="col-sm-3 control-label">Name</label>
								<div class="col-sm-9">
									<asp:TextBox ID="txtname" runat="server" Width="100%" placeholder="" ReadOnly="true" Height="28px"></asp:TextBox>
								</div>
							</div>

						 <%--   <div class="form-group">
							
								  <label id="Label1" runat="server" class="col-sm-3 control-label">Whom_To_Meet</label>
								 <div class="col-sm-3">
									<asp:TextBox ID="txtwhom_meet" runat="server" placeholder="" Width="100%" Height="32px" MaxLength="15" CssClass="form-control"></asp:TextBox>
								</div>
							</div>--%>

						</div>
					</div>

				</div>

				<div class="col-md-6">
					<div>
						<div class="box-body">
							<div class="form-group">
								<label id="ExpectedTime" runat="server" class="col-sm-3 control-label">Expected Time</label>
								<div class="col-sm-3">
									<asp:TextBox ID="txtvtime" runat="server" type="text" placeholder="" Width="80px" Height="32px" ReadOnly="true"></asp:TextBox>
									<%--<input name="txtvtime" type="text" id="Text1"  placeholder="Expected Time" runat="server" readonly="readonly" tabindex="-1"/></td>                                                           --%>
								</div>

								 <label id="dateofvisit" runat="server" class="col-sm-3 control-label">Date_Of_Visit</label>
								<div class="col-sm-3">
									<asp:TextBox ID="txtvdate" runat="server" placeholder="" Width="100%" Height="32px"></asp:TextBox>
									<asp:CalendarExtender ID="txtvdate_CalendarExtender" runat="server" Enabled="True" TargetControlID="txtVdate" Format="dd/MM/yyyy">
									</asp:CalendarExtender>
									<asp:MaskedEditExtender ID="MaskedEdit2" runat="server" Mask="99/99/9999" MaskType="Date" TargetControlID="txtVdate" />
								</div>
							</div>


							<div class="form-group">
								<label id="Approvedby" runat="server" class="col-sm-3 control-label">App_By/App_Dt</label>
								<%--<div class="col-sm-1">
									<asp:ImageButton ID="btnapp" runat="server" Height="23px" Width="26px" ToolTip="Select Approver Name"
										ImageAlign="left" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;"
										OnClick="btnapp_Click" />
								</div>--%>
								<div class="col-sm-9">
									<asp:TextBox ID="txtapp" runat="server"
										placeholder="" Width="100%" Height="32px" ReadOnly="true" CssClass="form-control"></asp:TextBox>
								</div>
							</div>

							<div class="form-group">
								<label id="Modifiedby" runat="server" class="col-sm-3 control-label">Mod_By/Mod_Dt</label>
								<%--<div class="col-sm-1">
									<asp:ImageButton ID="btnmodi" runat="server" Height="23px" Width="26px" ToolTip="Select Modifier Name"
										ImageAlign="left" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;"
										OnClick="btnmodi_Click" />
								</div>--%>
								<div class="col-sm-9">
									<asp:TextBox ID="txtedit" runat="server"
										placeholder="" Width="100%" Height="32px" ReadOnly="true" CssClass="form-control"></asp:TextBox>
								</div>
							</div>

						</div>
					</div>
				</div>
			</div>
		</section>

	</div>


	<asp:HiddenField ID="hf1" runat="server" />
	 <asp:HiddenField ID="hf2" runat="server" />
	<asp:HiddenField ID="hfbtnmode" runat="server" />
	<asp:HiddenField ID="hfedmode" runat="server" />
	<asp:HiddenField ID="hffielddt" runat="server" />
	<asp:HiddenField ID="doc_nf" runat="server" />
	<asp:HiddenField ID="doc_df" runat="server" />
	<%--<input type="button" ID="btnhideF" runat="server" onserverclick="btnhideF_Click"  style="display:none" />
<input type="button" ID="btnhideF_S" runat="server"  onserverclick="btnhideF_S_Click" style="display:none" />--%>
	<asp:Button ID="btnhideF" runat="server" OnClick="btnhideF_Click" Style="display: none" />
	<asp:Button ID="btnhideF_s" runat="server" OnClick="btnhideF_S_Click" Style="display: none" />
	<asp:Button ID="btnOKTarget" runat="server" Text="!" OnClick="btnOKTarget_Click" Style="display: none;" />
	<asp:Button ID="btnCancelTarget" runat="server" Text="!" OnClick="btnCancelTarget_Click" Style="display: none;" />
	<%--<cr:crystalreportviewer ID="CRV1" runat="server" AutoDataBind="true" style="display:none;" />--%>
</asp:Content>
