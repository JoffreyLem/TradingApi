namespace ApiTrading.Modele.DTO.Response
{
    using System.Collections.Generic;
    using global::Modele;
    using Helper;
    using Newtonsoft.Json;

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