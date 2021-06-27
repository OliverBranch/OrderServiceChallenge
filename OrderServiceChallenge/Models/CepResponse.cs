using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OrderServiceChallenge.Models
{
    public class CepResponse
    {
        [JsonProperty("cep")]
        public string Cep { get; set; }
        [JsonProperty("logradouro")]
        public string Street { get; set; }
        [JsonProperty("complemento")]
        public string Complement { get; set; }
        [JsonProperty("bairro")]
        public string District { get; set; }
        [JsonProperty("localidade")]
        public string Local { get; set; }
        [JsonProperty("uf")]
        public string Uf { get; set; }
        [JsonProperty("unidade")]
        public string Unity { get; set; }
        [JsonProperty("ibge")]
        public string Ibge { get; set; }
        [JsonProperty("gia")]
        public string Gia { get; set; }
        [JsonProperty("ddd")]
        public string DDD { get; set; }
    }
}
