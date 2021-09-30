using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBooking.Models
{
    public class RoomStatusModel
    {
        [Key]
        public int RoomStatusId { get; set; }
        public RoomStatusEnum StatusName { get; set; }
        public bool IsRoomOccupied { get; set; } = false;
        public List<RoomModel> Rooms { get; set; }
    }
}
