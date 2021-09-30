using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBooking.Models
{
    public class BookingModel
    {
        [Key]
        public int BookingId { get; set; }
        [Required]
        public DateTime DateForm { get; set; }
        [Required]
        public DateTime DateTo { get; set; }
        [Required]
        public int RoomCount { get; set; }
        [Required]
        public bool IsActive { get; set; } = false;
        public int HotelId { get; set; }
        public HotelModel Hotel { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public List<RoomBookedModel> BookedRooms { get; set; }
    }
}
