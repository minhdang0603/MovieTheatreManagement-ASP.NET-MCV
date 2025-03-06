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

namespace MovieTheatreManagement.Controllers
{
	public class GenresController : Controller
	{
		private readonly IGenreService _genreService;

		public GenresController(IGenreService genreService)
		{
			_genreService = genreService;
		}

		// GET: Genres
		public IActionResult Index()
		{
			var genres = _genreService.GetGenreList();
			return View(genres);
		}	

		// GET: Genres/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Genres/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create([Bind("GenreId,GenreName")] Genre genre)
		{
			if (!ModelState.IsValid)
			{
				return View(genre);
			}

			_genreService.AddGenre(genre);
			TempData["success"] = "Genre added successfully";
			return RedirectToAction(nameof(Index));
		}

		// GET: Genres/Edit/5
		public IActionResult Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var genre = _genreService.GetGenreById(id.Value);
			if (genre == null)
			{
				return NotFound();
			}
			return View(genre);
		}

		// POST: Genres/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost, ActionName("Edit")]
		[ValidateAntiForgeryToken]
		public IActionResult EditPost(int id, [Bind("GenreId,GenreName")] Genre genre)
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
				var updateResult = _genreService.UpdateGenre(genre);
				if (!updateResult)
				{
					return NotFound();
				}
			}
			catch (DbUpdateConcurrencyException)
			{
				throw;
			}
			TempData["success"] = "Genre updated successfully";
			return RedirectToAction(nameof(Index));
		}

		// GET: Genres/Delete/5
		public IActionResult Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var genre = _genreService.GetGenreById(id.Value);
			if (genre == null)
			{
				return NotFound();
			}

			return View(genre);
		}

		// POST: Genres/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public IActionResult DeletePost(int id)
		{
			var genre = _genreService.GetGenreById(id);
			if (genre != null)
			{
				_genreService.RemoveGenre(genre);
			}
			TempData["success"] = "Genre deleted successfully";
			return RedirectToAction(nameof(Index));
		}
	}
}
