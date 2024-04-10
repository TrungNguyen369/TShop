using System.ComponentModel.DataAnnotations;

namespace TShop.ViewModels
{
    public class UserVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "*")]
        [MaxLength(50, ErrorMessage = "Character length exceeds the limit")]
        public string Name { get; set; }

        [EmailAddress(ErrorMessage = "Not in correct format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "*")]
        [MaxLength(50, ErrorMessage = "Character length exceeds the limit")]
        public string Password { get; set; }

        [Required(ErrorMessage = "*")]
        [MaxLength(24, ErrorMessage = "*")]
        [RegularExpression(@"0[0789]\d{8}", ErrorMessage = "Not in correct format")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "*")]
        public DateTime BirthDay { get; set; }

        [Required(ErrorMessage = "*")]
        public string Address { get; set;}

        public bool Sex { get; set;} = true;
    }
}
