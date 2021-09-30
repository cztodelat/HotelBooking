using HotelBooking.DataAccess;
using HotelBooking.Models;
using HotelBooking.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBooking.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public HomeController(
            IUnitOfWork unitOfWork
            )
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<HotelModel> hotels = unitOfWork.Hotels.GetAllHotels();

            return View(hotels);
        }

        [HttpGet]
        public IActionResult Details(int hotelId = 1)
        {
            HotelModel hotel = unitOfWork.Hotels.GetHotelWithAllRelations(hotelId);

            if (hotel == null)
            {
                Response.StatusCode = 404;
                return View("HotelNotFound", hotelId);
            }

            RoomStatusModel roomStatus = unitOfWork.Rooms.GetRoomStatus(RoomStatusEnum.Free);
            //После того как добавиться новый маркер, будем делать выборку по этому маркеру и дате. Те бронирования которые не активны, в них не должны попасть комнаты которые забронированы сейчас но при этом были забронированы в прошлом 

            IEnumerable<BookingModel> bookingModel = hotel.HotelBoookings.Where(b => b.DateTo < DateTime.Now && b.IsActive);

            foreach (var booking in bookingModel)
            {
                booking.IsActive = false;
                unitOfWork.Bookings.UpdateBooking(booking);

                foreach (var roomBooked in booking.BookedRooms)
                {
                    unitOfWork.Rooms.UpdateRoomStatus(roomStatus.RoomStatusId, roomBooked.Room.RoomId);
                }
            }

            return View(hotel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        //ЗАМЕТКА имя свойства и аргумента должны быть идентичны 
        [AcceptVerbs("Get", "Post")]
        public IActionResult IsDateToValid(DateTime dateTo)
        {
            if (dateTo < DateTime.Now)
            {
                return Json("Please select valid date!");
            }
            else
            {
                return Json(true);
            }
        }

        //ЗАМЕТКА имя свойства и аргумента должны быть идентичны 
        [AcceptVerbs("Get", "Post")]
        public IActionResult IsDateFromValid(DateTime dateFrom)
        {
            DateTime date = dateFrom.AddHours(23).AddMinutes(59);
            if (date < DateTime.Now)
            {
                return Json("Please select valid date!");
            }
            else
            {
                return Json(true);
            }
        }

        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}
