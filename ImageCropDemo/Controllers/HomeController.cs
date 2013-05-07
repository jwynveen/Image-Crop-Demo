using System.Web;
using System.Web.Mvc;
using SaveResponseStatus = ImageCropDemo.UploadHelper.SaveResponseStatus;

namespace ImageCropDemo.Controllers
{
	public class HomeController : Controller
	{
		private const string ImagePath = "images/mvc";
		public ActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public ActionResult UploadImage()
		{
			return View();
		}
		[HttpPost]
		public JsonResult UploadImage(HttpPostedFileBase upload)
		{
			var response = UploadHelper.SaveFile(ImagePath, upload);
			string message = null, filename = null;
			switch (response.Status)
			{
				case SaveResponseStatus.InvalidFileType:
					message = "Cannot accept files of this type.";
					break;
				case SaveResponseStatus.UploadError:
					message = "File could not be uploaded. " + response.Message;
					break;
				case SaveResponseStatus.Success:
					filename = response.FileAndPath;
					break;
			}
			return Json(string.IsNullOrEmpty(message)
							? (object) new {filename}
							: new {errorMessage = message});
		}

		[HttpPost]
		public JsonResult CropImage(int? xCoordinate, int? yCoordinate, int? width, int? height, string filename)
		{
			if (!xCoordinate.HasValue || !yCoordinate.HasValue || !width.HasValue || !height.HasValue)
				return Json(new {errorMessage = "Fail!"});
			var imageName = filename.Replace(ImagePath, "").Trim('/');

			var croppedImagePath = UploadHelper.CropAndSave(ImagePath, imageName, width.Value, height.Value, xCoordinate.Value, yCoordinate.Value);
			
			return Json(new {filename = croppedImagePath});
		}
	}
}
