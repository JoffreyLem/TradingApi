using System.Collections.Generic;
using ApiTrading.Helper;
using Modele;
using Newtonsoft.Json;

namespace ApiTrading.Modele.DTO.Response
{
    public class CandleListDto : ResponseModel
    {
        [JsonProperty(Order = 3)]
        public List<Candle> Data { get; set; }

        public CandleListDto(int statusCode, string message, List<Candle> data) : base(statusCode, message)
        {
            Data = data;
        }


        public override string ToString()
        {
            var settings = new JsonSerializerSettings()
            {
                ContractResolver = new OrderedContractResolver()
            };
   
            return JsonConvert.SerializeObject(this, Formatting.Indented,settings);
        }


    }
}