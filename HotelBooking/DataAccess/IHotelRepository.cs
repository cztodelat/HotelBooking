using System.Collections.Generic;
using HotelBooking.Models;

namespace HotelBooking.DataAccess
{
    public interface IHotelRepository : IRepository<HotelModel>
    {
        IEnumerable<HotelModel> GetAllHotels();
        IEnumerable<HotelModel> GetUserHotels(string id);
        HotelModel GetHotelWithAllRelations(int id);
        HotelModel UpdateHotel(HotelModel updatedHotel);
    }
}
