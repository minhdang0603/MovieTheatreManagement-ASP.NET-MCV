using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using Models;
using Models.ViewModels;
using Services.IService;
using System.Runtime.CompilerServices;
using Utility;

namespace MovieTheatreManagement.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = SD.Role_Admin + ", " + SD.Role_Employee)]
	public class RoomsController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;

		public RoomsController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index()
		{
			var rooms = _unitOfWork.Room.GetRoomList();
			return View(rooms);
		}

		public IActionResult Upsert(int? id)
		{
			var roomVM = new RoomVM()
			{
				Room = new Room(),
				SeatTypeList = _unitOfWork.SeatType.GetSeatTypeList().Select(st => new SelectListItem()
				{
					Text = st.TypeName,
					Value = st.TypeId.ToString()
				}),
				SeatTypePerRow = new List<int>()
			};

			if (id is null or 0)
			{
				return View(roomVM);
			}

			roomVM.Room = _unitOfWork.Room.GetRoomById(id.Value);
			roomVM.SeatTypePerRow = roomVM.Room.Seats
				.GroupBy(s => s.SeatRow)
				.OrderBy(g => g.Key)
				.Select(g => g.First().TypeId)
				.ToList();

			if (roomVM.Room == null)
			{
				return NotFound();
			}

			return View(roomVM);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Upsert(int id, RoomVM roomVM)
		{
			if (id != roomVM.Room.RoomId)
			{
				return NotFound();
			}

			if (!ModelState.IsValid)
			{
				roomVM.SeatTypeList = _unitOfWork.SeatType.GetSeatTypeList().Select(st => new SelectListItem()
				{
					Text = st.TypeName,
					Value = st.TypeId.ToString()
				});
				return View(roomVM);
			}
			try
			{
				if (roomVM.Room.RoomId == 0)
				{
					_unitOfWork.Room.AddRoom(roomVM.Room, roomVM.SeatTypePerRow);
					TempData["success"] = "Room added successfully";
				}
				else
				{
					_unitOfWork.Room.UpdateRoom(roomVM.Room, roomVM.SeatTypePerRow);
					TempData["success"] = "Room updated successfully";
				}
				_unitOfWork.Save();
			}
			catch (Exception)
			{
				TempData["error"] = "Error when updating room";
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

			var movie = _unitOfWork.Room.GetRoomById(id.Value);

			if (movie is null)
			{
				return NotFound();
			}

			_unitOfWork.Room.RemoveRoom(movie);
			_unitOfWork.Save();
			TempData["success"] = "Delete successful";
			return Json(new { });
		}
	}
}
