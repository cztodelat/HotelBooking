using HotelBooking.Models;
using System.Collections.Generic;


namespace HotelBooking.DataAccess
{
    public interface IBookingRepository : IRepository<BookingModel>
    {
        BookingModel UpdateBooking(BookingModel model);
        IEnumerable<BookingModel> GetUserBookings(string userId);
        BookingModel GetBooking(int bookingId);
        RoomBookedModel AddRoomBooked(RoomBookedModel roomBookedModel);
    }
}
