using System.ComponentModel.DataAnnotations;

namespace rgproj.ViewModels
{
    public class SignUpVM
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords don't match.")]
        [Display(Name = "Confirm Password")]

        public string? ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]

        public string? Location { get; set; }

        [Required]
        public string? Role { get; set; }
    }
}
