using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Refit;
using OrderServiceChallenge.Models.Interfaces;

namespace OrderServiceChallenge.Models
{
    public class Company : LegalPerson
    {

        public Company()
        {

        }

        public Task<CepResponse> Address()
        {
            try
            {
            var address = RestService.For<ICepApiService>("http://viacep.com.br").GetAddressAsync(CEP);
                return address;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool AddressIsValid()
        {
            var address = RestService.For<ICepApiService>("http://viacep.com.br").GetAddressAsync(CEP);

            if (address.Result != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
