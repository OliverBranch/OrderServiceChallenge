using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace OrderServiceChallenge.Models
{
    public class Employee : Person
    {
        [Required(ErrorMessage = "{0} Required")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "A CPF must be 11 digits long")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:###-###-###-##}")]
        public string CPF { get; set; }

        public Employee()
        {

        }

        public Employee(string name, string cpf)
        {
            Name = name;
            CPF = cpf;
        }

    }
}
