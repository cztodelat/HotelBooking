using HotelBooking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBooking.DataAccess
{
    public class SqlCountryRepository : Repository<CountryModel>, ICountryRepository
    {
        public SqlCountryRepository(AppDbContext context) : base(context)
        {}
    }
}
