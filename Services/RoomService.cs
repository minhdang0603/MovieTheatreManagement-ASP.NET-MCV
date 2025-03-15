using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Models;
using Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
	public class RoomService : IRoomService
	{
		private readonly ApplicationDbContext _context;

		public RoomService(ApplicationDbContext context)
		{
			_context = context;
		}
		public void AddRoom(Room room, List<int> seatTypePerRow)
		{
			room = AddSeats(room, seatTypePerRow);
			_context.Rooms.Add(room);
		}

		public void RemoveRoom(Room room) => _context.Rooms.Remove(room);

		public Room GetRoomById(int id)
		{
			return _context.Rooms
				.Include(r => r.Seats)
				.FirstOrDefault(r => r.RoomId == id);
		}

		public List<Room> GetRoomList()
		{
			return _context.Rooms
				.ToList();
		}

		public void UpdateRoom(Room room, List<int> seatTypePerRow)
		{
			var objFromDb = _context.Rooms
				.Include(r => r.Seats)
				.FirstOrDefault(r => r.RoomId == room.RoomId);
			if (objFromDb == null)
				throw new Exception("Room not found");

			objFromDb.Name = room.Name;
			objFromDb.TotalColumns = room.TotalColumns;
			objFromDb.TotalRows = room.TotalRows;
			objFromDb.Seats = new List<Seat>();
			objFromDb = AddSeats(objFromDb, seatTypePerRow);
			_context.Rooms.Update(objFromDb);
		}

		private Room AddSeats(Room room, List<int> seatTypePerRow)
		{
			room.Seats = new List<Seat>();
			for (int row = 0; row < room.TotalRows; row++)
			{
				int seatTypeId = (seatTypePerRow.Count > row) ? seatTypePerRow[row] : 1;
				char seatRow = (char)('A' + row);
				for (int column = 0; column < room.TotalColumns; column++)
				{
					room.Seats.Add(new Seat
					{
						SeatRow = seatRow.ToString(),
						SeatColumn = column,
						RoomId = room.RoomId,
						TypeId = seatTypeId
					});
				}
			}
			return room;
		}
	}
}
