using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBooking.Models
{
    public class RoomBookedModel
    {
        [Key]
        public int RoomBookedId { get; set; }

        public int BookingId { get; set; }
        public BookingModel Booking { get; set; }
        public int RoomId { get; set; }
        public RoomModel Room { get; set; }
    }
}
