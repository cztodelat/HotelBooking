using HotelBooking.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBooking.DataAccess
{
    //TODO Урать возможность удалять Users 
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        public DbSet<HotelModel> Hotels { get; set; }
        public DbSet<BookingModel> Bookings { get; set; }
        public DbSet<CountryModel> Countries { get; set; }
        public DbSet<RoomBookedModel> RoomsBooked { get; set; }
        public DbSet<RoomModel> Rooms { get; set; }
        public DbSet<RoomStatusModel> RoomStatuses { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Не забыть добавить поведение при удалении
            base.OnModelCreating(builder);

            builder.Entity<RoomStatusModel>().HasData(
                    new RoomStatusModel() { RoomStatusId = 1, StatusName = RoomStatusEnum.Free },
                    new RoomStatusModel() { RoomStatusId = 2, StatusName = RoomStatusEnum.Occupied }
                );

            //Init Countries Hotels One-To-Many relation 

            builder.Entity<HotelModel>()
                   .HasOne(hotel => hotel.Country)
                   .WithMany(country => country.Hotels)
                   .HasForeignKey(hotel => hotel.CountryId);


            //Init User Hotels One-To-Many relation

            builder.Entity<HotelModel>()
                   .HasOne(hotel => hotel.Owner)
                   .WithMany(user => user.Hotels)
                   .HasForeignKey(hotel => hotel.OwnerId);


            //Init Bookings Many-To-Many relation with Hotels and AppUsers

            builder.Entity<BookingModel>()
                   .HasKey(t => new { t.BookingId });

            builder.Entity<BookingModel>()
                   .HasOne(booking => booking.User)
                   .WithMany(user => user.UserBookings)
                   .HasForeignKey(booking => booking.UserId);

            builder.Entity<BookingModel>()
                   .HasOne(booking => booking.Hotel)
                   .WithMany(hotel => hotel.HotelBoookings)
                   .HasForeignKey(booking => booking.HotelId);



            //Init Rooms Hotels One-To-Many relation 

            builder.Entity<RoomModel>()
                   .HasOne(room => room.Hotel)
                   .WithMany(hotel => hotel.Rooms)
                   .HasForeignKey(room => room.HotelId);

            //Init Rooms RoomStatus One-To-Many relation 

            builder.Entity<RoomModel>()
                   .HasOne(room => room.RoomStatus)
                   .WithMany(roomStatus => roomStatus.Rooms)
                   .HasForeignKey(room => room.StatusId);

            //Init RoomBooked Many-To-Many relation with Bookings and Rooms

            builder.Entity<RoomBookedModel>()
                   .HasKey(t => new { t.RoomBookedId });

            builder.Entity<RoomBookedModel>()
                   .HasOne(bookedRoom => bookedRoom.Room)
                   .WithMany(room => room.BookedRooms)
                   .HasForeignKey(bookedRoom => bookedRoom.RoomId);

            builder.Entity<RoomBookedModel>()
                  .HasOne(bookedRoom => bookedRoom.Booking)
                  .WithMany(booking => booking.BookedRooms)
                  .HasForeignKey(bookedRoom => bookedRoom.BookingId).OnDelete(DeleteBehavior.NoAction);


        }
    }
}
