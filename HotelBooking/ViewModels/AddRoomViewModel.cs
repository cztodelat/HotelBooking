using HotelBooking.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;


namespace HotelBooking.ViewModels
{
    public class AddRoomViewModel
    {
        [Required(ErrorMessage = "The \"Room price\" field is required.")]
        [Remote(action: "IsPriceValid", controller: "Profile")]
        public decimal RoomPrice { get; set; }

        [Required]
        public int Floor { get; set; }

        [Required(ErrorMessage = "The \"Room number\" field is required.")]
        public int RoomNumber { get; set; }
        [Required(ErrorMessage = "The \"Status\" field is required.")]
        public RoomStatusEnum RoomStatus { get; set; }
        public string Description { get; set; }

        public int HotelId { get; set; }
    }
}
