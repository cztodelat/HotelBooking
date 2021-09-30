using HotelBooking.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBooking.DataAccess
{
    public class SqlHotelRepository : Repository<HotelModel>, IHotelRepository
    {
        public SqlHotelRepository(AppDbContext context) : base(context)
        {}
        public IEnumerable<HotelModel> GetAllHotels()
        {
            IEnumerable <HotelModel> hotels = AppContext.Hotels.Include(h => h.Owner).Include(h => h.Country);
            return hotels;
        }
        public IEnumerable<HotelModel> GetUserHotels(string id)
        {
            IEnumerable <HotelModel> hotels = AppContext.Hotels
                                                     .Include(h => h.Owner)
                                                     .Include(h => h.Country)
                                                     .Where(x => x.Owner.Id == id);
            return hotels;
        }
        public HotelModel GetHotelWithAllRelations(int id)
        {
            HotelModel hotel = AppContext.Hotels.Include(h => h.Rooms)
                                             .ThenInclude(r => r.RoomStatus)
                                             .Include(h => h.Country)
                                             .Include(h => h.Owner)
                                             .Include(h => h.HotelBoookings)
                                             .ThenInclude(hb => hb.BookedRooms).FirstOrDefault(h => h.HotelId == id);
            return hotel;
        }
        public HotelModel UpdateHotel(HotelModel updatedHotel)
        {
            var hotel = AppContext.Hotels.Attach(updatedHotel);
            hotel.State = EntityState.Modified;
            context.SaveChanges();

            return updatedHotel;
        }

        public AppDbContext AppContext { get { return context as AppDbContext; } }
    }
}
