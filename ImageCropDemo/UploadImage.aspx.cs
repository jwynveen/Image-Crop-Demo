using System;
using System.Web;
using System.Web.UI;
using SaveResponseStatus=ImageCropDemo.UploadHelper.SaveResponseStatus;

namespace ImageCropDemo
{
	public partial class UploadImage : Page
	{
		private const string ImagePath = "images/webforms";

		protected void btnUpload_Click(object sender, EventArgs e)
		{
			var response = UploadHelper.SaveFile(ImagePath, new HttpPostedFileWrapper(Upload.PostedFile));
			switch (response.Status)
			{
				case SaveResponseStatus.InvalidFileType:
					lblError.Text = "Cannot accept files of this type.";
					lblError.Visible = true;
					break;
				case SaveResponseStatus.UploadError:
					lblError.Text = "File could not be uploaded. " + response.Message;
					lblError.Visible = true;
					break;
				case SaveResponseStatus.Success:
					pnlUpload.Visible = false;
					pnlCrop.Visible = true;
					Session["WorkingImage"] = response.Filename;
					imgCrop.ImageUrl = response.FileAndPath;
					break;
			}
		}

		protected void btnCrop_Click(object sender, EventArgs e)
		{
			var imageName = Session["WorkingImage"].ToString();
			var w = Convert.ToInt32(Width.Value);
			var h = Convert.ToInt32(Height.Value);
			var x = Convert.ToInt32(XCoordinate.Value);
			var y = Convert.ToInt32(YCoordinate.Value);

			var croppedImagePath = UploadHelper.CropAndSave(ImagePath, imageName, w, h, x, y);
			pnlCrop.Visible = false;
			pnlCropped.Visible = true;
			imgCropped.ImageUrl = croppedImagePath;
		}
	}
}