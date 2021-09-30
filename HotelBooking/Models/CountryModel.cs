using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBooking.Models
{
    public class CountryModel
    {
        [Key]
        public int CountryId { get; set; }
        
        [Required]
        [MaxLength(450)]
        public string CountryName { get; set; }
        
        [MaxLength(40)]
        public string CountryCurrency { get; set; }
        public List<HotelModel> Hotels { get; set; }
    }
}
