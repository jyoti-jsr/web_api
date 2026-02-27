using System;
using System.ComponentModel.DataAnnotations;

namespace WebApp_Curd.Models
{
    public class Registration
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string? Email { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be atleast 6 charater long.")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Confirm password is required")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string? ConfirmPassword { get; set; }

        // 1. using query string
        //public static ValueTask<Registration?> BindAsync(HttpContext context)
        //{
        //    var email = context.Request.Query["email"];
        //    var password = context.Request.Query["password"];
        //    var confirmPassword = context.Request.Query["confirmPassword"];

        //    if (!string.IsNullOrEmpty(email) &&
        //        !string.IsNullOrEmpty(password) &&
        //        !string.IsNullOrEmpty(confirmPassword))
        //    {
        //        return ValueTask.FromResult<Registration?>(
        //            new Registration
        //            {
        //                Email = email,
        //                Password = password,
        //                ConfirmPassword = confirmPassword
        //            });
        //    }

        //    return ValueTask.FromResult<Registration?>(null);
        //}
    }
}
