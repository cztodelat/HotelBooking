using HotelBooking.Models;
using System.Collections.Generic;


namespace HotelBooking.DataAccess
{
    public interface IRoomRepository : IRepository<RoomModel>
    {
        IEnumerable<RoomModel> GetHotelRooms(int hotelId);
        RoomModel UpdateRoomStatus(int statusId, int roomId);
        RoomStatusModel GetRoomStatus(RoomStatusEnum roomStatus);
    }
}
