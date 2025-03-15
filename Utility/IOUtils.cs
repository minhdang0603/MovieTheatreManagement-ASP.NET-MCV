using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
	public static class IOUtils
	{
		public static string UploadImage(string webRootPath, IFormFile file, string imageUrl)
		{
			string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
			string movieImagePath = Path.Combine(webRootPath, @"images\movies");

			if (!Directory.Exists(movieImagePath))
			{
				Directory.CreateDirectory(movieImagePath);
			}

			if (!string.IsNullOrEmpty(imageUrl))
			{
				// Delete the existing file
				var imagePath = Path.Combine(webRootPath, imageUrl.TrimStart('\\'));

				if (System.IO.File.Exists(imagePath))
				{
					System.IO.File.Delete(imagePath);
				}
			}

			using (var fileStream = new FileStream(Path.Combine(movieImagePath, fileName), FileMode.Create))
			{
				file.CopyTo(fileStream);
			}

			return @"\images\movies\" + fileName;
		}
	}
}
