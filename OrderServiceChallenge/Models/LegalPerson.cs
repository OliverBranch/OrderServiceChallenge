using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OrderServiceChallenge.Models
{
    public abstract class LegalPerson: Person
    {
        //[Display(Name = "CNPJ Company")]
        [DisplayFormat(DataFormatString = "{0:000\\.000\\.000-00}", ApplyFormatInEditMode = true)]
        [Range(10000000000000, 99999999999999, ErrorMessage = "Digite um valor válido de CNPJ (14 digitos)")]
        public string CNPJ { get; set; }
        [Range(10000000, 99999999, ErrorMessage = "Digite um valor válido de CEP (8 digitos)")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#####-###}")]
        public string CEP { get; set; }
    }
}
