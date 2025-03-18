using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess.Data;
using Models;
using Services.IService;
using Services;
using System.IO;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authorization;
using Utility;

namespace MovieTheatreManagement.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = SD.Role_Admin + ", " + SD.Role_Employee)]
	public class GenresController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;

		public GenresController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index()
		{
			var genres = _unitOfWork.Genre.GetGenreList();
			return View(genres);
		}

		public IActionResult Upsert(int? id)
		{
			if (id is null or 0)
			{
				return View(new Genre());
			}

			var genre = _unitOfWork.Genre.GetGenreById(id.Value);
			if (genre == null)
			{
				return NotFound();
			}
			return View(genre);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Upsert(int id, [Bind("GenreId,GenreName")] Genre genre)
		{
			if (id != genre.GenreId)
			{
				return NotFound();
			}

			if (!ModelState.IsValid)
			{
				return View(genre);
			}

			try
			{
				if (id == 0)
				{
					_unitOfWork.Genre.AddGenre(genre);
					TempData["success"] = "Genre added successfully";
				}
				else
				{
					_unitOfWork.Genre.UpdateGenre(genre);
					TempData["success"] = "Genre updated successfully";
				}
				_unitOfWork.Save();
			}
			catch (DbUpdateConcurrencyException)
			{
				throw;
			}
			return RedirectToAction(nameof(Index));
		}

		#region API CALLS
		[HttpDelete]
		public IActionResult Delete(int? id)
		{
			if (id is null or 0)
			{
				return NotFound();
			}

			var genre = _unitOfWork.Genre.GetGenreById(id.Value);

			if (genre is null)
			{
				return NotFound();
			}

			_unitOfWork.Genre.RemoveGenre(genre);
			_unitOfWork.Save();
			TempData["success"] = "Delete successful";
			return Json(new { });
		}
		#endregion
	}
}
