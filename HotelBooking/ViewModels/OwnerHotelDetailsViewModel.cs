using HotelBooking.Models;
using System.Collections.Generic;

namespace HotelBooking.ViewModels
{
    public class OwnerHotelDetailsViewModel
    {
        public HotelModel Hotel { get; set; }
        public IEnumerable<RoomModel> Rooms { get; set; }
    }
}