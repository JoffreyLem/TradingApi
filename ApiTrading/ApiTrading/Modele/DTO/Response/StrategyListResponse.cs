using System.Collections.Generic;

namespace ApiTrading.Modele.DTO.Response
{
    public class StrategyResponse
    {
        public StrategyResponse()
        {
            StrategyLists = new List<StrategyList>();
        }


        public StrategyResponse(List<StrategyList> strategyLists)
        {
            StrategyLists = strategyLists;
        }

        public List<StrategyList> StrategyLists { get; set; }
    }

    public class StrategyList
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}