using HotelBooking.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBooking.ViewModels
{
    public class BookViewModel
    {
        public int HotelId { get; set; }

        [Remote(action: "IsDateFromValid", controller: "Home")]
        [DataType(DataType.Date)]
        public DateTime DateFrom { get; set; }

        [Remote(action: "IsDateToValid", controller: "Home")]
        [DataType(DataType.Date)]
        public DateTime DateTo { get; set; }
        public int RoomCount { get => Rooms.Where(r => r.RoomStatus.IsRoomOccupied == true).ToList().Count; }
        public List<RoomModel> Rooms { get; set; }
    }
}
