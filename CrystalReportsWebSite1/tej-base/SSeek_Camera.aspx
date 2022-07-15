<%@ Page Language="C#" AutoEventWireup="true" Inherits="SSeek_Camera" CodeFile="SSeek_Camera.aspx.cs" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta charset="utf-8" />
	<meta http-equiv="X-UA-Compatible" content="IE=edge" />
	<title>Finsys</title>
	<meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport" />

	<script src="../tej-base/Scripts/jquery-1.11.1.min.js" type="text/javascript"></script>
	<script src="../tej-base/Scripts/jquery-ui.min.js" type="text/javascript"></script>
	<link href="../tej-base/Styles/vip_vrm.css" rel="stylesheet" type="text/css" />
	<link rel="stylesheet" href="../tej-base/bootstrap/css/bootstrap.min.css" />

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
				context.drawImage(video, 0, 0, 200, 200);
				setTimeout(function () {
					var canvas = $("#canvas");
					var imgStr = canvas[0].toDataURL('image/png');
					imgStr = imgStr.replace('data:image/png;base64,', '');
					$('#hfImage').val(imgStr);
					$('#empImage').attr('src', 'data:image/png;base64,' + imgStr);
					$('#imgData').val(imgStr);
					document.getElementById("<%= btnhide.ClientID %>").click();
				}, 100);
			});

		}

		function closePopup(o) { $(o, window.parent.document).trigger("click"), parent.$.colorbox.close() }
	</script>
</head>
<body>
	<form id="form1" runat="server">
		<asp:ScriptManager ID="scr1" runat="server" EnableCdn="true"></asp:ScriptManager>
		<table style="width: 100%;">
			<tr style="text-align: center">
				<td>
					<video id="video" height="190" style="max-width: 180px"></video>
				</td>
				<td>
					<asp:Image ID="empImage" runat="server" Width="200px" Height="100px" />
				</td>
			</tr>
			<tr style="text-align: center">
				<td colspan="2">
					<input id="btncp1" type="button" class="btn btn-info" value="Open Camera" title="Open Camera" />
					<input id="btncp2" type="button" class="btn btn-info" value="Capture Image" title="Capture Image" /></td>
				<td>
			</tr>
		</table>
		<asp:HiddenField ID="imgData" runat="server" />
		<input id="hfImage" type="hidden" value="" runat="server" />
		<asp:HiddenField ID="HiddenField1" runat="server" />
		<asp:Button ID="btnhide" runat="server" OnClick="btnhide_Click" Style="display: none" />
		<canvas id="canvas" height="190"></canvas>
	</form>
</body>

</html>
