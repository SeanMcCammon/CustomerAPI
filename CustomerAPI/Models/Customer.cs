using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerAPI.Models
{
    public class Customer
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string Policy { get; set; }
        [EmailAddress]
        public string EMail { get; set; }

        public DateTime DOB { get; set; }
    }
}
