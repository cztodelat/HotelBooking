using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBooking.Models
{
    public class HotelModel
    {
     
        [Key]
        public int HotelId { get; set; }
        
        [Required]
        public string HotelName { get; set; }

        [Required]
        [MaxLength(450)]
        public string HotelCity { get; set; }
        
        [Required]
        public string HotelDescription { get; set; }
        
        [Required]
        [MaxLength(450)]
        public string HotelAddress { get; set; }
        
        [Required]
        public string PhotoPath { get; set; }

        public int CountryId { get; set; }
        public CountryModel Country { get; set; }

        public string OwnerId { get; set; }
        public ApplicationUser Owner { get; set; }
        public List<BookingModel> HotelBoookings { get; set; }
        public List<RoomModel> Rooms { get; set; }
    }
}
