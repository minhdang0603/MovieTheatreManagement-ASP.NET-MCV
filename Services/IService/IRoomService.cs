using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IService
{
	public interface IRoomService
	{
		void AddRoom(Room room, List<int> seatTypePerRow);
		void RemoveRoom(Room room);
		Room GetRoomById(int id);
		List<Room> GetRoomList();
		void UpdateRoom(Room room, List<int> seatTypePerRow);
		List<Seat> GetSeatsByIds(List<int> selectedSeatIds);
	}
}
