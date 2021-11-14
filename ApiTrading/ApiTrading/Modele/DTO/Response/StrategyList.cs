using System.Collections.Generic;

namespace ApiTrading.Modele.DTO.Response
{

    public class StrategyResponse : ResponseModel
    {
        public List<StrategyList> StrategyLists { get; set; }

        public StrategyResponse(List<StrategyList> strategyLists)
        {
            StrategyLists = strategyLists;
        }

        public StrategyResponse()
        {
        }

        public StrategyResponse(int statusCode, string message) : base(statusCode, message)
        {
        }

        public StrategyResponse(int statusCode, string message, List<StrategyList> strategyLists) : base(statusCode, message)
        {
            StrategyLists = strategyLists;
        }
    }
    
    public class StrategyList
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}