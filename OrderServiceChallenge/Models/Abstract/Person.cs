using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OrderServiceChallenge.Models
{
    public abstract class Person
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} Required")]
        [StringLength(60, MinimumLength = 3, ErrorMessage ="{0} size should be {2} and {1}")]
        public string Name { get; set; }
    }
}
