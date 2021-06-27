using System.Threading.Tasks;
using Refit;

namespace OrderServiceChallenge.Models.Interfaces
{
    public interface ICepApiService
    {
        [Get("/ws/{cep}/json")]
        Task<CepResponse> GetAddressAsync(string cep);

    }
}
