using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace OrderServiceChallenge.Models
{
    public class Employee : Person
    {
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
