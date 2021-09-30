using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBooking.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext context;

        public UnitOfWork(AppDbContext context)
        {
            this.context = context;

            Bookings = new SqlBookingRepository(context);
            Hotels = new SqlHotelRepository(context);
            Rooms = new SqlRoomRepository(context);
            Countries = new SqlCountryRepository(context);
        }
        public IBookingRepository Bookings { get; private set; }

        public IHotelRepository Hotels { get; private set; }

        public IRoomRepository Rooms { get; private set; }

        public ICountryRepository Countries { get; private set; }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
