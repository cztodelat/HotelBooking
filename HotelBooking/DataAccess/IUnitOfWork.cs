using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBooking.DataAccess
{
    public interface IUnitOfWork : IDisposable
    {
        IBookingRepository Bookings { get; }
        IHotelRepository Hotels { get; }
        IRoomRepository Rooms { get; }
        ICountryRepository Countries { get; }
        void Save();
    }
}
