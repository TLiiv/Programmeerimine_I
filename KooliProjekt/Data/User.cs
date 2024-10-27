using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Data
{
    public class User
    {
        public Guid UserId { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Username is required.")]
        [MinLength(4, ErrorMessage = "Username must be at least 4 characters long.")]
        [MaxLength(40, ErrorMessage = "Username cannot exceed 40 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Username can only contain alphanumeric characters.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,50}$",
         ErrorMessage = "Characters are not allowed.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,50}$",
         ErrorMessage = "Characters are not allowed.")]
        public string LastName { get; set; }
        
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Password is required.")]
        //[StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        //ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public ICollection<UserBets> UserBets { get; set; } = new List<UserBets>();
        public bool IsAdmin { get; set; }
    }
}
