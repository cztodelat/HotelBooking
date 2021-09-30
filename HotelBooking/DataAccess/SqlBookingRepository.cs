using HotelBooking.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace HotelBooking.DataAccess
{
    public class SqlBookingRepository : Repository<BookingModel>, IBookingRepository
    {
        public SqlBookingRepository(AppDbContext context) : base(context)
        {}

        public IEnumerable<BookingModel> GetUserBookings(string userId)
        {
            var hotels = AppContext.Bookings.Include(c => c.Hotel).Include(c => c.BookedRooms).ThenInclude(r => r.Room).ThenInclude(r => r.RoomStatus);

            return hotels.Where(x => x.UserId == userId);
        }

        public BookingModel GetBooking(int bookingId)
        {
            BookingModel booking = AppContext.Bookings.Include(b => b.BookedRooms)
                                                   .ThenInclude(br => br.Room)
                                                   .ThenInclude(r => r.RoomStatus).FirstOrDefault(x => x.BookingId == bookingId);


            return booking;
        }

        public BookingModel UpdateBooking(BookingModel model)
        {
            var booking = AppContext.Bookings.Attach(model);
            booking.State = EntityState.Modified;
            context.SaveChanges();
            return model;
        }

        public RoomBookedModel AddRoomBooked(RoomBookedModel roomBookedModel)
        {
            AppContext.RoomsBooked.Add(roomBookedModel);
            context.SaveChanges();
            return roomBookedModel;
        }

        public AppDbContext AppContext { get { return context as AppDbContext; }  }
    }
}
