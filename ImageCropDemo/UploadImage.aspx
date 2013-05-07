<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadImage.aspx.cs" Inherits="ImageCropDemo.UploadImage" %>

<!DOCTYPE html>

<html>
<head runat="server">
	<title>Upload/Crop Image - Webforms</title>
	<link href="/css/jquery.Jcrop.css" rel="stylesheet" type="text/css" />
	<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.3/jquery.min.js"></script>
	<script type="text/javascript" src="/scripts/jquery.Jcrop.min.js"></script>
	<script type="text/javascript">
		$(document).ready(function() {
			$('#imgCrop').Jcrop({
				onSelect: storeCoords
			});
		});
 
		function storeCoords(c) {
			$('#XCoordinate').val(c.x);
			$('#YCoordinate').val(c.y);
			$('#Width').val(c.w);
			$('#Height').val(c.h);
		};
	</script>
</head>
<body>
	<form id="form1" runat="server">
	<div>
		<h1>Upload/Crop Image - Webforms</h1>
		
		<asp:Panel ID="pnlUpload" runat="server">
			<asp:FileUpload ID="Upload" runat="server" />
			<br />
			<asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click" Text="Upload" />
			<asp:Label ID="lblError" runat="server" Visible="false" />
		</asp:Panel>
		<asp:Panel ID="pnlCrop" runat="server" Visible="false">
			<asp:Image ID="imgCrop" runat="server" />
			<br />
			<asp:HiddenField ID="XCoordinate" runat="server" />
			<asp:HiddenField ID="YCoordinate" runat="server" />
			<asp:HiddenField ID="Width" runat="server" />
			<asp:HiddenField ID="Height" runat="server" />
			<asp:Button ID="btnCrop" runat="server" Text="Crop" OnClick="btnCrop_Click" />	
		</asp:Panel>
		<asp:Panel ID="pnlCropped" runat="server" Visible="false">
			<asp:Image ID="imgCropped" runat="server" />
		</asp:Panel>
	</div>
	</form>
</body>
</html>
