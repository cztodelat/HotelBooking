using HotelBooking.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace HotelBooking.DataAccess
{
    public class SqlRoomRepository : Repository<RoomModel>, IRoomRepository
    {
        public SqlRoomRepository(AppDbContext context) : base(context)
        {}

        public IEnumerable<RoomModel> GetHotelRooms(int hotelId)
        {
            IEnumerable<RoomModel> rooms = AppContext.Rooms.Include(r => r.RoomStatus).Where(r => r.HotelId == hotelId);
            return rooms;
        }
        public RoomStatusModel GetRoomStatus(RoomStatusEnum roomStatus)
        {
            return AppContext.RoomStatuses.FirstOrDefault(s => s.StatusName == roomStatus);
        }

        public RoomModel UpdateRoomStatus(int statusId, int roomId)
        {
            RoomModel room = AppContext.Rooms.Find(roomId);
            if (room != null)
            {
                room.StatusId = statusId;
                context.SaveChanges();
            }
            return room;
        }

        public AppDbContext AppContext { get { return context as AppDbContext; } }
    }
}
