using System;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Web;
using SD = System.Drawing;

namespace ImageCropDemo
{
	public class UploadHelper
	{
		public static SaveResponse SaveFile(string path, HttpPostedFileBase file)
		{
			var physicalPath = HttpContext.Current.Request.MapPath("~" + FormatPath(path));
			var response = new SaveResponse(SaveResponseStatus.InvalidFileType){Path = path};
			var fileOk = false;

			if (file != null)
			{
				response.Filename = file.FileName;
				var extension = Path.GetExtension(response.Filename);
				if (extension != null)
				{
					var fileExtension = extension.ToLower();
					var allowedExtensions = new[] { ".png", ".jpeg", ".jpg", ".gif" };
					fileOk = allowedExtensions.Any(t => fileExtension == t);
				}
			}

			if (fileOk)
			{
				try
				{
					file.SaveAs(physicalPath + response.Filename);
					response.Status = SaveResponseStatus.Success;
				}
				catch (Exception ex)
				{
					response.Status = SaveResponseStatus.UploadError;
					response.Message = ex.Message;
				}
			}
			return response;
		}
		public enum SaveResponseStatus
		{
			InvalidFileType,
			UploadError,
			Success
		}
		public class SaveResponse
		{
			public SaveResponse(SaveResponseStatus status)
			{
				Status = status;
			}
			public SaveResponseStatus Status { get; set; }
			public string Message { get; set; }
			public string Filename { get; set; }
			public string Path { get; set; }
			public string FileAndPath { get { return FormatPath(Path) + Filename; }}
		}

		public static string CropAndSave(string sourcePath, string imageName, int width, int height, int x, int y, string prefix = "crop")
		{
			var physicalPath = HttpContext.Current.Request.MapPath("~/" + FormatPath(sourcePath));
			var cropImage = Crop(physicalPath + imageName, width, height, x, y);
			using (var ms = new MemoryStream(cropImage, 0, cropImage.Length))
			{
				ms.Write(cropImage, 0, cropImage.Length);
				using (var croppedImage = SD.Image.FromStream(ms, true))
				{
					var saveTo = physicalPath + prefix + imageName;
					croppedImage.Save(saveTo, croppedImage.RawFormat);
					return FormatPath(sourcePath) + prefix + imageName;
				}
			}
		}
		protected static byte[] Crop(string img, int width, int height, int x, int y)
		{
			using (var originalImage = SD.Image.FromFile(img))
			{
				using (var bmp = new SD.Bitmap(width, height))
				{
					bmp.SetResolution(originalImage.HorizontalResolution, originalImage.VerticalResolution);
					using (var graphic = SD.Graphics.FromImage(bmp))
					{
						graphic.SmoothingMode = SmoothingMode.AntiAlias;
						graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
						graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
						graphic.DrawImage(originalImage, new SD.Rectangle(0, 0, width, height), x, y, width, height,
										SD.GraphicsUnit.Pixel);
						var ms = new MemoryStream();
						bmp.Save(ms, originalImage.RawFormat);
						return ms.GetBuffer();
					}
				}
			}
		}
		private static string FormatPath(string path)
		{
			return "/" + path.Trim('/') + "/";
		}
	}
}