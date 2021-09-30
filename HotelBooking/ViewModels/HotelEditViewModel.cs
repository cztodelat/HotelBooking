using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBooking.ViewModels
{
    public class HotelEditViewModel : AddHotelViewModel
    {
        public int HotelId { get; set; }
        public string PhotoPath { get; set; }
    }
}
