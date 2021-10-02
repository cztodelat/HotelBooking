using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace HotelBooking.ViewModels
{
    //Имя классов ViewModel должно начинаться с имени View к которому ViewModel привязываеться
    public class RegisterViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        [EmailAddress] //Email addres validation attribiute
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)] //Hide characteres
        public string Password { get; set; }

        [DataType(DataType.Password)] //Hide characteres
        [Display(Name = "Confirm password")]
        [Compare("Password",
            ErrorMessage = "Password and confirmation password do not match.")] //Compare two passwords
        public string ConfirmPassword { get; set; }

        [Required]
        public IFormFile Photo { get; set; } //This type we set becouse this file that is uploaded to the server can be accesed thow ModelBinding
    }
}
