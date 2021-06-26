﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OrderServiceChallenge.Models
{
    public abstract class LegalPerson: Person
    {
        public string CNPJ { get; set; }
        public string CEP { get; set; }
    }
}
