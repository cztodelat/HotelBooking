using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBooking.Models
{
    public class RoomModel
    {
        [Key]
        public int RoomId { get; set; }
       
        [Required]
        public decimal RoomPrice { get; set; }
        
        [Required]
        public int Floor { get; set; }
        
        [Required]
        public int RoomNumber { get; set; }
        public string Description { get; set; }

        public int HotelId { get; set; }
        public HotelModel Hotel { get; set; }

        public int StatusId { get; set; }
        public RoomStatusModel RoomStatus { get; set; }

        public List<RoomBookedModel> BookedRooms { get; set; }
    }
}
