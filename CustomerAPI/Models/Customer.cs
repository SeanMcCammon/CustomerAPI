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
        [Required]
        [RegularExpression(@"[A-Z]{2}[-][0-9]{6}", ErrorMessage = "Format of Policy is Incorrect. Required to be in format XX-999999.")]
        public string Policy { get; set; }
        [EmailAddress]
        public string EMail { get; set; }

        public DateTime DOB { get; set; }
    }
}
