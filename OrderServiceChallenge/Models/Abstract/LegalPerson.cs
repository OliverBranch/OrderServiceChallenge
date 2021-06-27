using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OrderServiceChallenge.Models
{
    public abstract class LegalPerson : Person
    {


        [Required(ErrorMessage = "{0} Required")]
        [DisplayFormat(DataFormatString = "{0:000\\.000\\.000-00}", ApplyFormatInEditMode = true)]
        [StringLength(14, MinimumLength = 14, ErrorMessage = "A CNPJ must be 14 digits long")]
        public string CNPJ { get; set; }

        [Required(ErrorMessage = "{0} Required")]
        [Display(Name = "Postal Code")]
        public string CEP { get; set; }
    }
}
