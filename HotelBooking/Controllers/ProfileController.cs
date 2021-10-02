using HotelBooking.Models;
using HotelBooking.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.IO;

using System;
using System.Collections.Generic;
using System.Linq;
using HotelBooking.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Diagnostics;

namespace HotelBooking.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUnitOfWork unitOfWork;

        //This type helps us to get physical path to wwwroot folder 
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly ILogger<ProfileController> logger;
        private ApplicationUser user = null;
        public ProfileController(UserManager<ApplicationUser> userManager,
                                 IUnitOfWork unitOfWork,
                                 IWebHostEnvironment hostingEnvironment,
                                 ILogger<ProfileController> logger)
        {
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
            this.hostingEnvironment = hostingEnvironment;
            this.logger = logger;
        }

        [Route("Profile")]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            user ??= await GetCurrentUserAsync();
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Bookings()
        {
            user ??= await GetCurrentUserAsync();
            var bookings = unitOfWork.Bookings.GetUserBookings(user.Id);
            return View(bookings);
        }

        [Authorize]
        public async Task<IActionResult> Book(BookViewModel model)
        {
            user ??= await GetCurrentUserAsync();
            RoomStatusModel status = unitOfWork.Rooms.GetRoomStatus(RoomStatusEnum.Occupied);
            //Сначала создаём BookingModel где заполняем всю информацию. Добавляем в БД
            BookingModel booking = new BookingModel()
            {
                HotelId = model.HotelId,
                UserId = user.Id,
                DateForm = model.DateFrom,
                DateTo = model.DateTo,
                RoomCount = model.RoomCount,
                IsActive = true
            };

            booking = unitOfWork.Bookings.Add(booking);
            unitOfWork.Save();

            //Создаём RoomBokedModel заполняем его информацией сохраняем в БД
            foreach (RoomModel room in model.Rooms)
            {
                if (room.RoomStatus.IsRoomOccupied == true)
                {
                    RoomBookedModel roomBooked = new RoomBookedModel()
                    {
                        BookingId = booking.BookingId,
                        RoomId = room.RoomId
                    };

                    unitOfWork.Bookings.AddRoomBooked(roomBooked);

                    //Меняем StatusId у комнат которые заняты

                    RoomModel roomModel = unitOfWork.Rooms.UpdateRoomStatus(status.RoomStatusId, room.RoomId);
                }

            }


            return RedirectToAction("Bookings", "Profile");
        }

        public IActionResult CancelBooking(int bookingId)
        {
            //При отмене бронирования, мы изменяем статус бронирования, а так же изменяем статус у комнат
            BookingModel booking = unitOfWork.Bookings.GetBooking(bookingId);
            RoomStatusModel status = unitOfWork.Rooms.GetRoomStatus(RoomStatusEnum.Free);


            if (booking == null)
            {
                Response.StatusCode = 404;
                ViewData["ErrorMessage"] = "Sory but this booking can not be found. \n" +
                    "Try again please!";
                return View("NotFound");
            }

            booking.IsActive = false;
            unitOfWork.Bookings.UpdateBooking(booking);

            foreach (var roomBooked in booking.BookedRooms)
            {
                unitOfWork.Rooms.UpdateRoomStatus(status.RoomStatusId, roomBooked.Room.RoomId);
            }


            return RedirectToAction("Bookings", "Profile");
        }

        [HttpGet]
        public async Task<IActionResult> MyOffers()
        {
            user ??= await GetCurrentUserAsync();

            IEnumerable<HotelModel> hotels = unitOfWork.Hotels.GetUserHotels(user.Id);

            return View(hotels);
        }

        [HttpGet]
        public IActionResult Payments()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddHotel()
        {
            AddHotelViewModel hotelViewModel = new AddHotelViewModel();
            hotelViewModel.Countries = unitOfWork.Countries.GetAll(
                                                            orderBy: countries => countries.OrderBy(country => country.CountryName));
            return View(hotelViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddHotel(AddHotelViewModel model)
        {
            user ??= await GetCurrentUserAsync();

            if (ModelState.IsValid)
            {
                string uniqueFileName = ProccessUploadedFile(model);

                HotelModel hotel = new HotelModel()
                {
                    HotelName = model.HotelName,
                    HotelCity = model.HotelCity,
                    HotelAddress = model.HotelAddress,
                    HotelDescription = model.HotelDescription,
                    PhotoPath = uniqueFileName,
                    CountryId = model.HotelCountryId,
                    Owner = user
                };

                unitOfWork.Hotels.Add(hotel);
                unitOfWork.Save();

                return RedirectToAction("Index", "Home");
            }

            model.Countries = unitOfWork.Countries.GetAll(
                                                    orderBy: countries => countries.OrderBy(country => country.CountryName));
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> MyOfferDetails(int hotelId)
        {
            HotelModel hotel = unitOfWork.Hotels.GetHotelWithAllRelations(hotelId);
            user ??= await GetCurrentUserAsync();

            if (hotel == null || hotel.OwnerId != user.Id)
            {
                string owner = hotel == null ? " hotel doesn't exist" : hotel.Owner.Id;

                logger.LogWarning($"The hotel that user tryed to acces is not found or user didn't have permissions. " +
                    $"UserID = {user.Id}\nOwnerID={owner}");
                Response.StatusCode = 404;
                return View("HotelNotFound", hotelId);
            }

            OwnerHotelDetailsViewModel model = new OwnerHotelDetailsViewModel()
            {
                Hotel = hotel,
                Rooms = unitOfWork.Rooms.GetHotelRooms(hotelId)
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult AddRoom(AddRoomViewModel model)
        {
            if (ModelState.IsValid)
            {
                int statusId = unitOfWork.Rooms.GetRoomStatus(model.RoomStatus).RoomStatusId;

                RoomModel room = new RoomModel()
                {
                    Description = model.Description,
                    Floor = model.Floor,
                    HotelId = model.HotelId,
                    RoomNumber = model.RoomNumber,
                    RoomPrice = model.RoomPrice,
                    StatusId = statusId,
                };

                unitOfWork.Rooms.Add(room);
                unitOfWork.Save();

            }

            return RedirectToAction("MyOfferDetails", "Profile", new { hotelId = model.HotelId });
        }

        [HttpGet]
        public async Task<IActionResult> HotelEdit(int hotelId)
        {
            HotelModel hotel = unitOfWork.Hotels.GetHotelWithAllRelations(hotelId);
            user ??= await GetCurrentUserAsync();

            if (hotel == null || hotel.OwnerId != user.Id)
            {
                string owner = hotel == null ? " hotel doesn't exist" : hotel.Owner.Id;


                logger.LogWarning($"The hotel that user tryed to acces is not found or user didn't have permissions. " +
                    $"UserID = {user.Id}\nOwnerID={owner}");
                Response.StatusCode = 404;
                return View("HotelNotFound", hotelId);
            }

            HotelEditViewModel model = new HotelEditViewModel()
            {
                HotelId = hotel.HotelId,
                PhotoPath = hotel.PhotoPath,
                HotelAddress = hotel.HotelAddress,
                HotelCity = hotel.HotelCity,
                HotelDescription = hotel.HotelDescription,
                HotelName = hotel.HotelName,
                Countries = unitOfWork.Countries.GetAll(
                                                    orderBy: countries => countries.OrderBy(country => country.CountryName)),
                HotelCountryId = hotel.CountryId
        };

            return View(model);
        }

        [HttpPost]
        public IActionResult HotelEdit(HotelEditViewModel model) {

            if (ModelState.IsValid)
            {
                HotelModel updatedHotel = unitOfWork.Hotels.GetHotelWithAllRelations(model.HotelId);

                updatedHotel.HotelName = model.HotelName;
                updatedHotel.CountryId = model.HotelCountryId;
                updatedHotel.HotelCity = model.HotelCity;
                updatedHotel.HotelAddress = model.HotelAddress;
                updatedHotel.HotelDescription = model.HotelDescription;

                if (model.Photo != null)
                {
                    if (model.PhotoPath != null)
                    {
                        string filePath = Path.Combine(hostingEnvironment.WebRootPath, "img\\hotel", model.PhotoPath);
                        System.IO.File.Delete(filePath);
                    }

                    string uniqueFileName = ProccessUploadedFile(model);
                    updatedHotel.PhotoPath = uniqueFileName;
                }

                unitOfWork.Hotels.UpdateHotel(updatedHotel);
                return RedirectToAction("Index","Home");
            }

            return View(model);
        }

        private async Task<ApplicationUser> GetCurrentUserAsync() { 
            return await userManager.GetUserAsync(HttpContext.User); 
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult IsPriceValid(decimal roomPrice)
        {

            if (roomPrice <= 0)
            {
                return Json("The price can't be equal or less than 0!");
            }
            else
            {
                return Json(true);
            }

        }

        private string ProccessUploadedFile(AddHotelViewModel model)
        {
            string uniqueFileName = null;

            if (model.Photo != null)
            {

                string uploadsFolder = Path.Combine(/*path to wwwroot folder*/
                                                     hostingEnvironment.WebRootPath, "img\\hotel"); //Gat path wwwroot/img
                uniqueFileName = Guid.NewGuid() + "_" + model.Photo.FileName; //unique img file
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                try
                {
                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {
                        //Copy the photo to our server, into wwwroot/img folder
                        model.Photo.CopyTo(fs);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError("Something went wrong when photo was coping to the server.");
                    logger.LogError(ex.ToString());
                }
                
            }

            return uniqueFileName;
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult IsHotelCountryValid(int hotelCountryId)
        {
            if (!unitOfWork.Countries.GetAll().Any(x => x.CountryId == hotelCountryId))
            {
                return Json("The \"Country\" field is required.");
            }

            return Json(true);
        }

        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}
