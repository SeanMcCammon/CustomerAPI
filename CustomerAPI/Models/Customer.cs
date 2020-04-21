using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;

namespace CustomerAPI.Models
{
    public class Customer
    {
        [Key]
        public long Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "The Surname is required to be between 3 - 50 characters in length . ")]
        public string Surname { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "The First Name is required to be between 3 - 50 characters in length. ")]
        public string FirstName { get; set; }
        public string Policy { get; set; }
        [EmailAddress]
        public string EMail { get; set; }

        public DateTime DOB { get; set; }
    }
}
