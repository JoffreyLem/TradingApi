using System.Collections.Generic;
using ApiTrading.Helper;
using Modele;
using Newtonsoft.Json;

namespace ApiTrading.Modele.DTO.Response
{
    public class CandleListResponse
    {
        public CandleListResponse(List<Candle> data)
        {
            Data = data;
        }

        public CandleListResponse()
        {
        }

        [JsonProperty(Order = 3)] public List<Candle> Data { get; set; }


        public override string ToString()
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new OrderedContractResolver()
            };

            return JsonConvert.SerializeObject(this, Formatting.Indented, settings);
        }
    }
}