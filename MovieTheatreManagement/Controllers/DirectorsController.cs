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
using Humanizer.Localisation;
using Microsoft.Extensions.Hosting;

namespace MovieTheatreManagement.Controllers
{
	public class DirectorsController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;

		public DirectorsController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		// GET: Directors
		public IActionResult Index()
		{
			var directors = _unitOfWork.Director.GetDirectorList();
			return View(directors);
		}

		// GET: Directors/Edit/5
		public IActionResult Upsert(int? id)
		{
			if (id is null or 0)
			{
				return View(new Director());
			}

			var director = _unitOfWork.Director.GetDirectorById(id.Value);
			if (director == null)
			{
				return NotFound();
			}
			return View(director);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Upsert(int id, [Bind("DirectorId,DirectorName")] Director director)
		{
			if (id != director.DirectorId)
			{
				return NotFound();
			}

			if (!ModelState.IsValid)
			{
				return View(director);
			}

			try
			{
				if (id == 0)
				{
					_unitOfWork.Director.AddDirector(director);
					TempData["success"] = "Director added successfully";
				}
				else
				{
					_unitOfWork.Director.UpdateDirector(director);
					TempData["success"] = "Director updated successfully";
				}
				_unitOfWork.Save();
			}
			catch (DbUpdateConcurrencyException)
			{
				throw;
			}
			return RedirectToAction(nameof(Index));
		}

		[HttpDelete]
		public IActionResult Delete(int? id)
		{
			if (id is null or 0)
			{
				return NotFound();
			}

			var director = _unitOfWork.Director.GetDirectorById(id.Value);

			if (director is null)
			{
				return NotFound();
			}

			_unitOfWork.Director.RemoveDirector(director);
			_unitOfWork.Save();
			TempData["success"] = "Delete successful";
			return Json(new { });
		}
	}
}
