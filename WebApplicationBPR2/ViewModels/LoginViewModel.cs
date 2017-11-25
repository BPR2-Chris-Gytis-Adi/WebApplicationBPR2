using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationBPR2.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "User name")]
        [MinLength(5)]
        public string UserName { get; set; }

        [Required(ErrorMessage ="At least 6 characters; must contain: literals(Caps and non-caps), numerics and nonliteral(eg. %$^&)")]
        [DataType(DataType.Password)]
        [MinLength(6)]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}
