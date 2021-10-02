using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBooking.Models
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(450)]
        public string Name { get; set; }
        [MaxLength(450)]
        public string Surname { get; set; }
        [Required]
        public string PhotoPath { get; set; }
        public List<HotelModel> Hotels { get; set; }
        public List<BookingModel> UserBookings { get; set; }
    }
}
