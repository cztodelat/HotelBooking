using HotelBooking.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace HotelBooking.ViewModels
{
    public class AddHotelViewModel
    {
        [Required(ErrorMessage = "The \"Hotel name\" field is required.")]
        public string HotelName { get; set; }
        
        [Required(ErrorMessage = "The \"Country\" field is required.")]
        [Remote(action: "IsHotelCountryValid", controller: "Profile")]
        public int HotelCountryId { get; set; }
        
        [Required(ErrorMessage = "The \"Hotel city\" field is required.")]
        [MaxLength(450)]
        public string HotelCity { get; set; }
        
        [Required(ErrorMessage = "The \"Hotel description\" field is required.")]
        public string HotelDescription { get; set; }
        
        [Required(ErrorMessage = "The \"Hotel address\" field is required.")]
        [MaxLength(450)]
        public string HotelAddress { get; set; }
        
        [Required]
        public IFormFile Photo { get; set; } //This type we set becouse this file that is uploaded to the server can be accesed thow ModelBinding

        public IEnumerable<CountryModel> Countries { get; set; }
    }
}
